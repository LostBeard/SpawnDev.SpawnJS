using SpawnDev.SpawnJS;
using System.Runtime.InteropServices;

namespace TestsShared
{
    /// <summary>
    /// Reaching the WebAssembly linear memory from Javascript.<br/>
    /// This is the foundation the zero copy path is built on: a .Net array is pinned, its address taken,
    /// and Javascript is handed a TypedArray over those exact bytes so bulk data never crosses the
    /// boundary. Getting the buffer by the wrong route does not throw - it produces a view onto the wrong
    /// memory - so it has to be proven by writing bytes on one side and reading them on the other.
    /// </summary>
    public class WasmMemoryTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// The runtime exposes the heap under different shapes depending on version. Record which one is
        /// actually in use so a future runtime change that silently moves it is visible.
        /// </summary>
        [SpawnJSTest]
        public async Task WasmMemoryBufferIsReachableTest()
        {
            var source = JS.WasmMemoryBufferSource();
            if (string.IsNullOrEmpty(source))
                throw new Exception("The WebAssembly memory buffer could not be reached by any known runtime shape");
            // on the record in every run, so a runtime update that quietly moves the heap is visible in
            // the log rather than only when a zero copy view starts reading the wrong bytes
            Console.WriteLine($"WASM memory buffer reached via: {source}");

            using var buffer = JS.WasmMemoryBuffer();
            if (buffer?.JSObject == null) throw new Exception($"WasmMemoryBuffer returned nothing (source '{source}')");

            var constructorNames = buffer.ConstructorNames;
            if (System.Array.IndexOf(constructorNames, "ArrayBuffer") < 0 && System.Array.IndexOf(constructorNames, "SharedArrayBuffer") < 0)
                throw new Exception($"The memory buffer is a [{string.Join(",", constructorNames)}], expected an ArrayBuffer or SharedArrayBuffer (source '{source}')");

            var byteLength = buffer.GetPropertyAsInt32("byteLength");
            if (byteLength <= 0) throw new Exception($"The memory buffer reports byteLength {byteLength} (source '{source}')");
        }

        /// <summary>
        /// The real proof: pin a .Net array, build a Javascript view over its address, and confirm
        /// Javascript sees the bytes .Net wrote. A view onto the wrong memory passes every structural
        /// check above and only fails here.
        /// </summary>
        [SpawnJSTest]
        public async Task JavascriptSeesPinnedDotnetBytesTest()
        {
            var data = new byte[] { 11, 22, 33, 44, 55, 66, 77, 88 };
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                var address = handle.AddrOfPinnedObject().ToInt64();
                using var memory = JS.WasmMemoryBuffer();
                using var view = JS.New("Uint8Array", memory.JSObject, (double)address, (double)data.Length);

                var length = view.Get<int>("length");
                if (length != data.Length) throw new Exception($"The view has length {length}, expected {data.Length}");

                for (var i = 0; i < data.Length; i++)
                {
                    var seen = view.Get<int>(i);
                    if (seen != data[i])
                        throw new Exception($"Javascript read {seen} at index {i}, .Net wrote {data[i]} - the view is over the wrong memory");
                }
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Zero copy has to mean zero copy in BOTH directions: a write from Javascript must land in the
        /// .Net array itself, not in a detached copy of it.
        /// </summary>
        [SpawnJSTest]
        public async Task JavascriptWritesLandInTheDotnetArrayTest()
        {
            var data = new byte[4];
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                var address = handle.AddrOfPinnedObject().ToInt64();
                using var memory = JS.WasmMemoryBuffer();
                using var view = JS.New("Uint8Array", memory.JSObject, (double)address, (double)data.Length);

                view.Set(0, 200);
                view.Set(1, 201);
                view.Set(2, 202);
                view.Set(3, 203);

                if (data[0] != 200 || data[1] != 201 || data[2] != 202 || data[3] != 203)
                    throw new Exception($".Net array holds [{string.Join(",", data)}] after Javascript wrote [200,201,202,203]; the view is a copy, not a view");
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
