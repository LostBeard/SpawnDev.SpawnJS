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
