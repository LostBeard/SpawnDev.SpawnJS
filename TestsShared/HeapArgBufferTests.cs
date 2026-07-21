using SpawnDev.SpawnJS;

namespace TestsShared
{
    /// <summary>
    /// PROBE: an argument buffer in .Net's OWN memory, read by Javascript through the runtime's own HEAP
    /// TypedArray views.<br/>
    /// <br/>
    /// The premise: Javascript can view the WebAssembly linear memory directly, so a buffer there is free
    /// to BOTH sides - .Net writes are plain array stores, Javascript reads are element reads off
    /// HEAPF64 - and an N argument call costs one crossing instead of N+1.<br/>
    /// <br/>
    /// These establish the premise HOLDS before any transport is built on it.
    /// </summary>
    public class HeapArgBufferTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// The design reads through HEAPF64 and HEAPU8, so confirm the runtime actually publishes them
        /// rather than relying on what Emscripten normally exports. If this ever fails, the reads have to
        /// build their own views over the buffer instead - still TypedArrays, still platform endian.
        /// </summary>
        [SpawnJSTest]
        public async Task RuntimePublishesTheHeapViewsWeReadThroughTest()
        {
            var names = SlotInterop.HeapViewNames();
            Console.WriteLine($"HEAP VIEWS: {names}");
            foreach (var required in new[] { "HEAPU8", "HEAPF64" })
            {
                if (!names.Split(',').Contains(required))
                    throw new Exception($"the runtime does not publish {required} - available: '{names}'");
            }
        }

        /// <summary>
        /// The headline: .Net writes five values with NO crossings, one call delivers them, and Javascript
        /// reads back exactly what .Net wrote.<br/>
        /// The values are deliberately not symmetric under byte reversal, so a byte order problem could
        /// not pass by coincidence.
        /// </summary>
        [SpawnJSTest]
        public async Task JavascriptReadsArgumentsFromDotnetMemoryTest()
        {
            using var buffer = new HeapArgBuffer(64);
            buffer.Bind();
            double[] args = { 1.5, 2.25, 3.125, 1234567.891011, -0.0009765625 };
            for (var i = 0; i < args.Length; i++) buffer.Write(i, args[i]);

            var sum = SlotInterop.HeapSum(0, args.Length);
            var expected = 0d;
            foreach (var a in args) expected += a;
            if (sum != expected)
                throw new Exception($"Javascript read {sum} from .Net memory, expected {expected}");
        }

        /// <summary>
        /// Byte order is not merely assumed to be right - 1.0 byte reversed is a denormal near zero, so
        /// reading back exactly 1.0 proves the two sides agree. Reading through the runtime's HEAP views
        /// means there is no byte order option anywhere in the path, which is why this passes without any
        /// flag being passed.
        /// </summary>
        [SpawnJSTest]
        public async Task ByteOrderAgreesWithoutAnyFlagTest()
        {
            using var buffer = new HeapArgBuffer(64);
            buffer.Bind();
            buffer.Write(0, 1.0);
            var one = SlotInterop.HeapSum(0, 1);
            if (one != 1.0)
                throw new Exception($"Javascript read {one} for 1.0 - a byte reversed 1.0 is a denormal near zero, so the two sides disagree on byte order");
        }

        /// <summary>
        /// The value region must be 8 byte aligned, because HEAPF64 is indexed in ELEMENTS - a misaligned
        /// address would read the wrong element silently rather than fail. Bind() asserts it; this
        /// confirms the assertion holds for a real pinned .Net array rather than only being checked.
        /// </summary>
        [SpawnJSTest]
        public async Task PinnedValueRegionIsEightByteAlignedTest()
        {
            using var buffer = new HeapArgBuffer(64);
            if (buffer.ValueAddress % 8 != 0)
                throw new Exception($"the pinned double[] landed at {buffer.ValueAddress}, which is not 8 byte aligned");
            buffer.Bind();
        }

        /// <summary>
        /// A heterogeneous argument list: tags in one region, values in another, parallel by index. This
        /// is why the layout is structure of arrays - interleaving a tag byte with an eight byte payload
        /// gives a stride of 9 and breaks the alignment HEAPF64 indexing needs.
        /// </summary>
        [SpawnJSTest]
        public async Task TaggedArgumentsCarryTypeAndPayloadTest()
        {
            using var buffer = new HeapArgBuffer(64);
            buffer.Bind();
            buffer.WriteTagged(0, HeapArgBuffer.TagNumber, 1.5);
            buffer.WriteTagged(1, HeapArgBuffer.TagBoolean, 1);
            buffer.WriteTagged(2, HeapArgBuffer.TagNumber, 0.5);

            var sum = SlotInterop.HeapTaggedSum(buffer.TagOffsetFromValues, 0, 3);
            if (sum != 3.0)
                throw new Exception($"tagged arguments summed to {sum}, expected 3.0");
            if (buffer.ReadTag(1) != HeapArgBuffer.TagBoolean)
                throw new Exception("the tag byte did not survive the round trip");
        }

        /// <summary>
        /// A slot tagged argument resolves against the slot table Javascript side, so an object reference
        /// costs exactly what a number costs to deliver - eight bytes and no crossing. This is the case
        /// that matters for a GPU dispatch, which is mostly numbers and object references.
        /// </summary>
        [SpawnJSTest]
        public async Task SlotTaggedArgumentResolvesThroughTheSlotTableTest()
        {
            using var buffer = new HeapArgBuffer(64);
            buffer.Bind();
            // take a real slot the same way the library does, holding a value the sum can verify
            var slot = JS.Call<double>("eval", "globalThis.__sjsAlloc(40)");
            try
            {
                buffer.WriteTagged(0, HeapArgBuffer.TagSlot, slot);
                buffer.WriteTagged(1, HeapArgBuffer.TagNumber, 2);

                var sum = SlotInterop.HeapTaggedSum(buffer.TagOffsetFromValues, 0, 2);
                if (sum != 42)
                    throw new Exception($"a slot tagged argument summed to {sum}, expected 42 - the slot did not resolve");
            }
            finally
            {
                SlotInterop.Free(slot);
            }
        }

        /// <summary>
        /// A .Net STRING read straight out of .Net memory, with nothing copied .Net side and no string
        /// marshaller involved: pin it, hand Javascript the address of its first character, and let
        /// HEAPU16 index it.<br/>
        /// <br/>
        /// This confirms two things that were assumptions until now: that a string can be pinned at all,
        /// and that AddrOfPinnedObject on one gives the FIRST CHARACTER rather than the object header. If
        /// it gave the header, the text would come back shifted by the header size and be garbage - so a
        /// correct round trip is the proof.
        /// </summary>
        [SpawnJSTest]
        public async Task JavascriptReadsAPinnedDotnetStringTest()
        {
            foreach (var text in new[] { "hello", "", "a", new string('x', 5000) })
            {
                var handle = System.Runtime.InteropServices.GCHandle.Alloc(text, System.Runtime.InteropServices.GCHandleType.Pinned);
                try
                {
                    var address = handle.AddrOfPinnedObject().ToInt64();
                    var read = SlotInterop.ReadUtf16(address, text.Length);
                    if (read != text)
                        throw new Exception($"Javascript read '{Trim(read)}' (length {read.Length}) for a {text.Length} char string '{Trim(text)}'");
                }
                finally { handle.Free(); }
            }
        }

        /// <summary>
        /// The same, for text that is not ASCII - which is the case that proves the data really is being
        /// read as UTF-16 rather than accidentally working because every character happened to fit in a
        /// byte. Includes a surrogate pair, which is two chars in .Net and must stay two.
        /// </summary>
        [SpawnJSTest]
        public async Task PinnedStringReadHandlesNonAsciiTest()
        {
            foreach (var text in new[] { "café", "日本語", "vulcan \U0001F596", "éèêë" })
            {
                var handle = System.Runtime.InteropServices.GCHandle.Alloc(text, System.Runtime.InteropServices.GCHandleType.Pinned);
                try
                {
                    var read = SlotInterop.ReadUtf16(handle.AddrOfPinnedObject().ToInt64(), text.Length);
                    if (read != text)
                        throw new Exception($"non-ASCII round trip failed: read '{Trim(read)}' expected '{Trim(text)}'");
                }
                finally { handle.Free(); }
            }
        }

        static string Trim(string s) => s.Length <= 40 ? s : s[..40] + "...";

        /// <summary>
        /// The buffer must survive a garbage collection. It is pinned through HeapView's pinned GCHandle,
        /// so the collector cannot move it - but a view over memory the collector COULD move would read
        /// whatever now occupies that address, silently.
        /// </summary>
        [SpawnJSTest]
        public async Task BufferSurvivesCollectionTest()
        {
            using var buffer = new HeapArgBuffer(64);
            buffer.Bind();
            buffer.Write(0, 987.654);

            for (var i = 0; i < 200; i++) _ = new byte[8192];
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var value = SlotInterop.HeapSum(0, 1);
            if (value != 987.654)
                throw new Exception($"after a collection Javascript read {value}, expected 987.654 - the buffer moved");
        }

        /// <summary>
        /// Many writes then one read - the shape that matters. .Net paid ZERO crossings for 512 arguments
        /// and one for the call.
        /// </summary>
        [SpawnJSTest]
        public async Task ManyArgumentsCostOneCrossingTest()
        {
            using var buffer = new HeapArgBuffer(1024);
            buffer.Bind();
            const int count = 512;
            var expected = 0d;
            for (var i = 0; i < count; i++)
            {
                var v = i * 0.25;
                buffer.Write(i, v);
                expected += v;
            }
            var sum = SlotInterop.HeapSum(0, count);
            if (sum != expected)
                throw new Exception($"{count} arguments summed to {sum}, expected {expected}");
        }
    }
}
