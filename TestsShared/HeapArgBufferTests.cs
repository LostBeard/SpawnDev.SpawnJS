using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.Toolbox;

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
        /// marshaller involved.<br/>
        /// <br/>
        /// Pinning is HeapViewString's job - it already pins the string and exposes the address of its
        /// first character, so this uses that rather than taking its own GCHandle. Strings being pinnable
        /// is not an open question; the library has supported it since BlazorJS.
        /// </summary>
        [SpawnJSTest]
        public async Task JavascriptReadsAPinnedDotnetStringTest()
        {
            foreach (var text in new[] { "hello", "a", new string('x', 5000) })
            {
                using var view = HeapView.Create(text);
                if (view.Length != text.Length)
                    throw new Exception($"HeapViewString reported {view.Length} chars for a {text.Length} char string");
                if (view.ByteLength != text.Length * 2)
                    throw new Exception($"HeapViewString reported {view.ByteLength} bytes, expected {text.Length * 2} for UTF-16");

                var read = SlotInterop.ReadUtf16(view.Address, view.Length);
                if (read != text)
                    throw new Exception($"Javascript read '{Trim(read)}' (length {read.Length}) for a {text.Length} char string '{Trim(text)}'");
            }
        }

        /// <summary>
        /// Text that is not ASCII - the case that proves the data really is read as UTF-16 rather than
        /// working by accident because every character fitted in a byte. Includes a surrogate pair, which
        /// is two chars in .Net and must stay two.
        /// </summary>
        [SpawnJSTest]
        public async Task PinnedStringReadHandlesNonAsciiTest()
        {
            foreach (var text in new[] { "café", "日本語", "vulcan \U0001F596", "éèêë" })
            {
                using var view = HeapView.Create(text);
                var read = SlotInterop.ReadUtf16(view.Address, view.Length);
                if (read != text)
                    throw new Exception($"non-ASCII round trip failed: read '{Trim(read)}' expected '{Trim(text)}'");
            }
        }

        /// <summary>
        /// HeapViewString can pin a WINDOW into a string - an offset and a length - so a substring can be
        /// handed over without allocating one. Worth covering because the offset is applied in CHARACTERS
        /// and scaled by the two byte element size; getting that wrong would read from the wrong place.
        /// </summary>
        [SpawnJSTest]
        public async Task PinnedStringWindowReadsTheRightSubstringTest()
        {
            const string text = "abcdefghij";
            using var view = HeapView.Create(text, 3, 4);
            if (view.Length != 4)
                throw new Exception($"the window reported {view.Length} chars, expected 4");

            var read = SlotInterop.ReadUtf16(view.Address, view.Length);
            if (read != "defg")
                throw new Exception($"the window read '{read}', expected 'defg' - the character offset is not being scaled correctly");
        }

        /// <summary>
        /// The library's own conversion, rather than a probe binding: HeapViewString hands Javascript a
        /// string primitive through TextDecoder over the pinned region. This is the shipped path, so it is
        /// the one worth guarding.
        /// </summary>
        [SpawnJSTest]
        public async Task HeapViewStringConvertsToAJavascriptStringTest()
        {
            const string text = "vulcan salute \U0001F596 café";
            using var view = HeapView.Create(text);
            using var native = view.AsNativeView();
            var roundTripped = JS.Call<string>("String", native);
            if (roundTripped != text)
                throw new Exception($"AsNativeView produced '{Trim(roundTripped)}', expected '{Trim(text)}'");
        }

        static string Trim(string s) => s.Length <= 40 ? s : s[..40] + "...";

        /// <summary>
        /// The INTERLEAVED frame - one padded slot per argument, the shape the runtime's own marshaller
        /// uses. Values must land where Javascript expects them given a stride of 16 bytes.<br/>
        /// Deliberately uses values that differ per index, so a stride mistake shows up as the wrong sum
        /// rather than passing because every slot held the same thing.
        /// </summary>
        [SpawnJSTest]
        public async Task InterleavedFrameReadsEveryValueTest()
        {
            using var frame = new HeapArgFrame(64);
            frame.BindProbe();
            double[] args = { 1.5, 2.25, 3.125, 1234567.891011, -0.0009765625 };
            for (var i = 0; i < args.Length; i++) frame.Write(i, args[i]);

            var sum = SlotInterop.FrameSum(args.Length);
            var expected = 0d;
            foreach (var a in args) expected += a;
            if (sum != expected)
                throw new Exception($"the interleaved frame summed to {sum}, expected {expected} - check the stride");
        }

        /// <summary>
        /// The tag byte lives INSIDE the slot, so it shares memory with the padding next to the value.
        /// Writing a tag must not disturb the value it belongs to, and writing a value must not clear the
        /// tag - which is the failure mode interleaving introduces and structure-of-arrays cannot have.
        /// </summary>
        [SpawnJSTest]
        public async Task InterleavedTagAndValueDoNotCorruptEachOtherTest()
        {
            using var frame = new HeapArgFrame(64);
            frame.BindProbe();
            // write the tag FIRST, then the value, so an overlapping value write would clear the tag
            frame.WriteTaggedByte(0, HeapArgFrame.TagBoolean, 1);
            frame.Write(0, 7.5);
            if (frame.ReadTagByte(0) != HeapArgFrame.TagBoolean)
                throw new Exception("writing a value cleared the tag of the same slot");
            if (frame.Read(0) != 7.5)
                throw new Exception($"the value read back as {frame.Read(0)}, expected 7.5");

            // and the other order - a tag write must not touch the value
            frame.WriteTaggedByte(1, HeapArgFrame.TagNumber, 9.25);
            if (frame.Read(1) != 9.25)
                throw new Exception($"the tagged value read back as {frame.Read(1)}, expected 9.25");

            // a tag on one slot must not bleed into its neighbours
            frame.Write(2, 4.0);
            frame.WriteTaggedByte(1, HeapArgFrame.TagSlot, 9.25);
            if (frame.Read(2) != 4.0)
                throw new Exception("a tag write bled into the next slot");
            if (frame.ReadTagByte(0) != HeapArgFrame.TagBoolean)
                throw new Exception("a tag write bled into the previous slot");
        }

        /// <summary>
        /// The interleaved frame's tagged read, end to end through Javascript - including a slot tag,
        /// which Javascript resolves against the slot table.
        /// </summary>
        [SpawnJSTest]
        public async Task InterleavedFrameTaggedSumResolvesSlotsTest()
        {
            using var frame = new HeapArgFrame(64);
            frame.BindProbe();
            var slot = JS.Call<double>("eval", "globalThis.__sjsAlloc(40)");
            try
            {
                frame.WriteTaggedByte(0, HeapArgFrame.TagSlot, slot);
                frame.WriteTaggedByte(1, HeapArgFrame.TagNumber, 2);
                var sum = SlotInterop.FrameTaggedSum(2);
                if (sum != 42)
                    throw new Exception($"the interleaved tagged frame summed to {sum}, expected 42");
            }
            finally { SlotInterop.Free(slot); }
        }

        /// <summary>
        /// Every slot's value must be 8 byte aligned, which is the whole reason the stride is padded.
        /// Checked across many slots rather than only the first, because a stride that is not a multiple
        /// of 8 misaligns progressively - slot 0 would pass and slot 1 would not.
        /// </summary>
        [SpawnJSTest]
        public async Task EverySlotValueIsEightByteAlignedTest()
        {
            using var frame = new HeapArgFrame(64);
            if (HeapArgFrame.Stride % 8 != 0)
                throw new Exception($"a stride of {HeapArgFrame.Stride} is not a multiple of 8, so values drift out of alignment");
            for (var i = 0; i < 64; i++)
            {
                var slotAddress = frame.Address + i * HeapArgFrame.Stride;
                if (slotAddress % 8 != 0)
                    throw new Exception($"slot {i} is at {slotAddress}, which is not 8 byte aligned");
            }
        }

        /// <summary>
        /// REGRESSION. Binding a probe frame must not disturb the LIVE TRANSPORT.<br/>
        /// <br/>
        /// They shared one global address at first, so binding any probe silently redirected every
        /// transport call to read the probe's memory instead of the runtime's. Nothing threw - arguments
        /// simply came from the wrong place, and a call read whatever happened to be there.<br/>
        /// <br/>
        /// The whole suite passed anyway, because the tests that bind a frame only make calls that take
        /// the slot fast path and never reach the generic dispatcher. The benchmark caught it, using a
        /// dotted path. This closes that gap: bind a probe, then make a call that MUST go through the
        /// transport, and check the answer.
        /// </summary>
        [SpawnJSTest]
        public async Task BindingAProbeFrameDoesNotDisturbTheTransportTest()
        {
            JS.CallVoid("eval", "globalThis.__transportProbe = { value: 4242, nested: { deep: 7 } }");
            try
            {
                // a dotted path defeats every fast path, so this is the generic dispatcher and its
                // arguments travel through the transport frame
                if (JS.Get<int>("__transportProbe.value") != 4242)
                    throw new Exception("the transport was already wrong before any probe was bound");

                using var probe = new HeapArgFrame(64);
                probe.BindProbe();
                probe.WriteTagged(0, HeapArgFrame.TagNumber, 999);

                var afterBind = JS.Get<int>("__transportProbe.value");
                if (afterBind != 4242)
                    throw new Exception($"after binding a probe frame the transport read {afterBind}, expected 4242 - the probe took over the transport's frame address");

                // and a call whose result is an OBJECT, which comes back as a slot through the frame
                using var nested = JS.Get<SpawnJSObject>("__transportProbe.nested");
                if (nested == null || nested.JSRef!.Get<int>("deep") != 7)
                    throw new Exception("an object result did not survive a probe frame being bound");
            }
            finally
            {
                JS.CallVoid("eval", "delete globalThis.__transportProbe");
            }
        }

        /// <summary>
        /// The transport itself, end to end, over every argument shape it can carry: a number, a boolean,
        /// null, a string (interned to a slot) and an object (already a slot).<br/>
        /// Runs the SAME assertions with the frame off and on, so the two transports are held to one
        /// standard rather than the new one being trusted because it is new.
        /// </summary>
        [SpawnJSTest]
        public async Task TransportCarriesEveryArgumentShapeTest()
        {
            var wasFrame = SpawnJSRuntime.UseArgFrame;
            JS.CallVoid("eval",
                "globalThis.__shapeProbe = function (n, b, s, o, x) {" +
                "  return `${typeof n}:${n}|${typeof b}:${b}|${typeof s}:${s}|${o === null ? 'null' : typeof o.tag}|${x === null ? 'null' : typeof x}`;" +
                "}");
            JS.CallVoid("eval", "globalThis.__shapeObj = { tag: 'obj' }");
            try
            {
                using var obj = JS.Get<SpawnJSObject>("__shapeObj")!;
                foreach (var useFrame in new[] { false, true })
                {
                    SpawnJSRuntime.UseArgFrame = useFrame;
                    var which = useFrame ? "frame" : "buffer";
                    var got = JS.Call<string>("__shapeProbe", 42.5, true, "hello", obj, null);
                    const string expected = "number:42.5|boolean:true|string:hello|string|null";
                    if (got != expected)
                        throw new Exception($"[{which}] carried '{got}', expected '{expected}'");
                }
            }
            finally
            {
                SpawnJSRuntime.UseArgFrame = wasFrame;
                JS.CallVoid("eval", "delete globalThis.__shapeProbe; delete globalThis.__shapeObj");
            }
        }

        /// <summary>
        /// A repeated string argument is interned to a slot, so the second use costs what a number costs.
        /// The value must still arrive intact - an intern table that returned the wrong slot would be a
        /// silent data corruption, so this checks the string itself, not just the cache behaviour.
        /// </summary>
        [SpawnJSTest]
        public async Task InternedStringArgumentsStayCorrectTest()
        {
            JS.CallVoid("eval", "globalThis.__internProbe = function (s) { return s + '!'; }");
            try
            {
                var texts = new[] { "alpha", "beta", "alpha", "gamma", "beta", "alpha" };
                foreach (var text in texts)
                {
                    var got = JS.Call<string>("__internProbe", text);
                    if (got != text + "!")
                        throw new Exception($"an interned string argument arrived as '{got}', expected '{text}!'");
                }
                // a string past the intern length limit must still work, on the ordinary path
                var longText = new string('z', SpawnDev.SpawnJS.Marshallers.StringMarshaller.InternMaxLength + 10);
                if (JS.Call<string>("__internProbe", longText) != longText + "!")
                    throw new Exception("a string too long to intern did not survive the ordinary path");
            }
            finally
            {
                JS.CallVoid("eval", "delete globalThis.__internProbe");
            }
        }

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
