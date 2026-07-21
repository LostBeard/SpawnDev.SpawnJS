using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;

namespace TestsShared
{
    /// <summary>
    /// HeapView: pin .Net memory and hand Javascript a real TypedArray over those exact bytes.<br/>
    /// SpawnDev.BlazorJS did this by JSON serializing a {_heapViewInfo:{...}} descriptor that a Javascript
    /// hook recognised by property name. SpawnJS constructs the view directly from an address and a length,
    /// so nothing is serialized on the zero copy path.<br/>
    /// A view built over the wrong memory, or sized wrongly, still looks like a TypedArray - so every test
    /// here reads or writes actual bytes across the boundary rather than checking shape.
    /// </summary>
    public class HeapViewTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// The heap must be reachable WITHOUT any Blazor global.<br/>
        /// The port carried BlazorJS's module-name probe ("Module" for .Net 8 and earlier,
        /// "Blazor.runtime.Module" for .Net 9 and up) and threw "Unsupported .Net version. Module not
        /// found." on any host that is neither - which is every SpawnJS host, since SpawnJS has no Blazor
        /// dependency by design. These three entry points are the ones that went through it.
        /// </summary>
        [SpawnJSTest]
        public async Task HeapIsReachableWithoutBlazorGlobalsTest()
        {
            var size = HeapView.GetHeapBufferSize();
            if (size <= 0) throw new Exception($"heap buffer size reported as {size}");

            using var buffer = HeapView.GetHeapBuffer();
            if (buffer.ByteLength != size)
                throw new Exception($"heap buffer byteLength {buffer.ByteLength} does not match reported size {size}");

            using var heap = HeapView.GetHeap();
            if (heap.Length != size)
                throw new Exception($"heap view length {heap.Length} does not match heap size {size}");

            // and confirm the reason the old path failed is real, not incidental: neither Blazor global
            // that the ported probe looked for exists in this host
            if (!JS.IsUndefined("Blazor"))
                throw new Exception("this host DOES have a Blazor global, so this test is not proving host independence");
        }

        /// <summary>
        /// `new Uint8Array(byte[])` goes .Net array -> HeapView -> ArrayBuffer, which is the path the
        /// Blazor module probe broke. It is the most ordinary thing a consumer can write, so it gets its
        /// own regression test rather than being covered only indirectly.
        /// </summary>
        [SpawnJSTest]
        public async Task TypedArrayFromDotnetArrayRoundTripsTest()
        {
            var source = new byte[] { 9, 8, 7, 6, 5 };
            using var array = new Uint8Array(source);

            if (array.Length != source.Length)
                throw new Exception($"length crossed as {array.Length}, expected {source.Length}");

            // read the bytes back out of Javascript - a wrong-memory view is still a well formed array
            var back = array.ReadBytes();
            for (var i = 0; i < source.Length; i++)
                if (back[i] != source[i])
                    throw new Exception($"byte {i} crossed as {back[i]}, expected {source[i]}");
        }

        /// <summary>
        /// The view Javascript receives must be a genuine Uint8Array, not a wrapper around a descriptor
        /// object. This is exactly what the old JSON descriptor mechanism would produce here, since SpawnJS
        /// has no hook for it - so this test is what tells the two apart.
        /// </summary>
        [SpawnJSTest]
        public async Task ViewIsARealTypedArrayTest()
        {
            using var heap = new HeapView<byte>(new byte[] { 1, 2, 3, 4 });
            using var view = heap.As<Uint8Array>();

            var chain = view.JSRef!.ConstructorNames();
            if (System.Array.IndexOf(chain, "Uint8Array") < 0)
                throw new Exception($"The view is a [{string.Join(",", chain)}], expected a real Uint8Array");
            if (view.Length != 4) throw new Exception($"The view has length {view.Length}, expected 4");

            // and it is a view over the .Net heap itself, not over some other buffer that merely holds a
            // copy - the view's backing buffer must be the very same object the runtime hands out
            using var wasmMemory = JS.WasmMemoryBuffer();
            using var viewBuffer = view.JSRef!.Get<SpawnJSObjectReference>("buffer");
            if (!JS.ObjectEquals(viewBuffer.JSObject, wasmMemory.JSObject, true))
                throw new Exception("The view's buffer is not the WebAssembly heap, so it is not a view over .Net memory");
        }

        /// <summary>
        /// Javascript must see the bytes .Net wrote, through the public HeapView API.
        /// </summary>
        [SpawnJSTest]
        public async Task JavascriptReadsDotnetBytesTest()
        {
            var data = new byte[] { 10, 20, 30, 40, 50 };
            using var heap = new HeapView<byte>(data);
            using var view = heap.As<Uint8Array>();

            for (var i = 0; i < data.Length; i++)
            {
                var seen = view.JSRef!.Get<int>(i);
                if (seen != data[i]) throw new Exception($"Javascript read {seen} at {i}, .Net wrote {data[i]}");
            }
        }

        /// <summary>
        /// The other direction: a Javascript write must land in the .Net array itself. If it does not, this
        /// is a copy wearing a view's clothes.
        /// </summary>
        [SpawnJSTest]
        public async Task JavascriptWritesReachTheDotnetArrayTest()
        {
            var data = new byte[3];
            using (var heap = new HeapView<byte>(data))
            using (var view = heap.As<Uint8Array>())
            {
                view.JSRef!.Set(0, 111);
                view.JSRef!.Set(1, 122);
                view.JSRef!.Set(2, 133);
            }
            if (data[0] != 111 || data[1] != 122 || data[2] != 133)
                throw new Exception($".Net array holds [{string.Join(",", data)}] after Javascript wrote [111,122,133] - the view was a copy");
        }

        /// <summary>
        /// A non-byte element type, so the element-size arithmetic is exercised rather than the trivial
        /// one-byte-per-element case that hides every sizing mistake.
        /// </summary>
        [SpawnJSTest]
        public async Task FloatViewRoundTripsTest()
        {
            var data = new float[] { 1.5f, -2.25f, 3.75f };
            using var heap = new HeapView<float>(data);
            using var view = heap.As<Float32Array>();

            if (view.Length != 3) throw new Exception($"Float32Array length {view.Length}, expected 3");
            for (var i = 0; i < data.Length; i++)
            {
                var seen = view.JSRef!.Get<float>(i);
                if (Math.Abs(seen - data[i]) > 0.0001f) throw new Exception($"Javascript read {seen} at {i}, expected {data[i]}");
            }

            view.JSRef!.Set(1, -9.5f);
            if (Math.Abs(data[1] - (-9.5f)) > 0.0001f)
                throw new Exception($".Net float[1] is {data[1]} after a Javascript write of -9.5");
        }

        /// <summary>
        /// The cross type view: a HeapView&lt;double&gt; read as bytes. The byte length must be sized by the
        /// TARGET view's element size, not the source's - sizing it by the source builds a view 8x too long,
        /// which looks fine until its tail is touched. This exact bug has shipped before.
        /// </summary>
        [SpawnJSTest]
        public async Task CrossTypeViewIsSizedByTargetElementTest()
        {
            var data = new double[] { 1.0, 2.0, 3.0 };
            using var heap = new HeapView<double>(data);
            using var view = heap.As<Uint8Array>();

            var expectedBytes = data.Length * sizeof(double);
            if (view.Length != expectedBytes)
                throw new Exception($"Cross type byte view has length {view.Length}, expected {expectedBytes} - sized by the wrong element size");

            // touch the very last byte: an oversized view only fails here
            var last = view.JSRef!.Get<int>(expectedBytes - 1);
            if (last < 0 || last > 255) throw new Exception($"Last byte read as {last}");
        }

        /// <summary>
        /// A DataView over the same memory, since it is constructed in bytes rather than elements and so
        /// takes a different path through the view builder.
        /// </summary>
        [SpawnJSTest]
        public async Task DataViewOverPinnedMemoryTest()
        {
            var data = new byte[] { 0, 0, 0, 0 };
            using var heap = new HeapView<byte>(data);
            using var view = heap.AsDataView();

            var chain = view.JSRef!.ConstructorNames();
            if (System.Array.IndexOf(chain, "DataView") < 0)
                throw new Exception($"AsDataView produced a [{string.Join(",", chain)}]");

            view.JSRef!.CallVoid("setUint8", 2, 200);
            if (data[2] != 200) throw new Exception($".Net byte[2] is {data[2]} after DataView.setUint8(2, 200)");
        }
    }
}
