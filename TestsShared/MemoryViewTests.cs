using SpawnDev.SpawnJS;

namespace TestsShared
{
    /// <summary>
    /// What Microsoft's [JSMarshalAs&lt;MemoryView&gt;] actually produces on the Javascript side.<br/>
    /// Reflect.Set has ArraySegment&lt;byte&gt; and Span&lt;byte&gt; overloads marshalled that way, reached
    /// publicly through SpawnJSHandle.SetPropertyArraySegmentByte / SetPropertySpanByte. Whether Javascript
    /// receives a real TypedArray or a distinct MemoryView object decides whether that path can feed a
    /// browser API directly (writable.write, queue.writeBuffer, postMessage all require a TypedArray) or
    /// whether a copy is needed first. It was never measured, so these tests measure it rather than
    /// assuming either answer.
    /// </summary>
    public class MemoryViewTests(SpawnJSRuntime JS)
    {
        const string Key = "_spawnjs_memoryview_test";

        /// <summary>
        /// Writes an ArraySegment through the MemoryView marshalled setter and reports exactly what
        /// Javascript ended up holding: its prototype chain, typeof, and whether the usual TypedArray and
        /// MemoryView members are present. This test records the answer; it does not presume one.
        /// </summary>
        [SpawnJSTest]
        public async Task ArraySegmentMarshalsAsWhatOnTheJavascriptSideTest()
        {
            var data = new byte[] { 1, 2, 3, 4 };
            JS.JSHandle.SetPropertyArraySegmentByte(Key, new ArraySegment<byte>(data));
            try
            {
                using var handle = new SpawnJSHandle(JS.JSHandle, Key, true);
                var chain = handle.ConstructorNames;
                var jsType = handle.JSType;

                var hasSlice = !JS.IsUndefined($"{Key}.slice");
                var hasCopyTo = !JS.IsUndefined($"{Key}.copyTo");
                var hasSet = !JS.IsUndefined($"{Key}.set");
                var hasByteLength = !JS.IsUndefined($"{Key}.byteLength");
                var hasBuffer = !JS.IsUndefined($"{Key}.buffer");
                var hasLength = !JS.IsUndefined($"{Key}.length");

                Console.WriteLine($"MemoryView marshalling: typeof={jsType} chain=[{string.Join(",", chain)}] " +
                    $"slice={hasSlice} copyTo={hasCopyTo} set={hasSet} byteLength={hasByteLength} buffer={hasBuffer} length={hasLength}");

                if (chain.Length == 0) throw new Exception($"An ArraySegment marshalled to typeof {jsType} with no prototype chain - nothing was written");
            }
            finally
            {
                JS.Delete(Key);
            }
        }

        /// <summary>
        /// The question that actually matters: is it a TypedArray?<br/>
        /// A real TypedArray can be handed straight to a browser API. Anything else has to be copied first,
        /// which is the copy the whole zero copy path exists to avoid. Asserts the measured answer so a
        /// future runtime change to this marshalling is caught rather than silently altering the cost.
        /// </summary>
        [SpawnJSTest]
        public async Task ArraySegmentIsNotATypedArrayOnTheJavascriptSideTest()
        {
            var data = new byte[] { 9, 8, 7 };
            JS.JSHandle.SetPropertyArraySegmentByte(Key, new ArraySegment<byte>(data));
            try
            {
                using var handle = new SpawnJSHandle(JS.JSHandle, Key, true);
                var chain = handle.ConstructorNames;
                var isTypedArray = System.Array.IndexOf(chain, "TypedArray") >= 0
                                || System.Array.IndexOf(chain, "Uint8Array") >= 0;

                if (isTypedArray)
                {
                    // if this ever fires, MemoryView now yields a real TypedArray and the extra copy
                    // needed to reach a browser API can be dropped - a genuine win, not a failure
                    throw new Exception($"MemoryView now marshals as a TypedArray (chain [{string.Join(",", chain)}]). " +
                        "It previously did not. The copy to reach browser APIs can be removed.");
                }
                if (chain.Length == 0) throw new Exception("Nothing was written");
            }
            finally
            {
                JS.Delete(Key);
            }
        }

        /// <summary>
        /// A MemoryView that has been written to a Javascript property cannot be brought back into .Net as
        /// a JSObject: the runtime refuses with "NotImplementedException: ArraySegment".<br/>
        /// So the path is effectively one way. Javascript can be told about the memory, but .Net cannot
        /// then take a handle to the resulting object and pass it around, which rules MemoryView out as a
        /// general purpose way of handing bulk data to a wrapper - any read of a property THROUGH it
        /// (even byteLength) has to resolve the parent to a proxy first and hits the same wall.<br/>
        /// Pinned as an assertion so that if a future runtime starts supporting it, this fails and tells us
        /// the restriction has lifted.
        /// </summary>
        [SpawnJSTest]
        public async Task MemoryViewCannotBeTakenBackAsAJSObjectTest()
        {
            var data = new byte[] { 100, 101, 102, 103 };
            JS.JSHandle.SetPropertyArraySegmentByte(Key, new ArraySegment<byte>(data));
            try
            {
                using var handle = new SpawnJSHandle(JS.JSHandle, Key, true);
                try
                {
                    _ = handle.GetPropertyAsInt32("byteLength");
                }
                catch (Exception ex) when (ex.Message.Contains("ArraySegment"))
                {
                    return; // measured behaviour - the runtime will not proxy it
                }
                throw new Exception("A MemoryView can now be resolved to a JSObject proxy. The runtime restriction has lifted - MemoryView may now be usable for two way bulk data.");
            }
            finally
            {
                JS.Delete(Key);
            }
        }
    }
}
