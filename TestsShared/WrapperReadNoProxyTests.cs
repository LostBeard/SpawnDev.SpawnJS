using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Runtime.InteropServices.JavaScript;

namespace TestsShared
{
    /// <summary>
    /// Reading a wrapper out of Javascript must not create a JSObject proxy.<br/>
    /// <br/>
    /// A proxy is the most expensive thing in the library - a GC handle, a proxy-table entry and an
    /// enumerable Symbol tag stamped permanently on the Javascript object, measured at 21us against 1.3us
    /// for the slot path. Writing a descriptor stopped creating them; reading did not, so every
    /// JS.Get&lt;T&gt; and every wrapper returned from a call still paid for one.
    /// <br/>
    /// Every test here asserts an ABSENCE, which is the dangerous shape: an instrument that cannot see a
    /// proxy reports the same zero as an instrument watching a path that genuinely creates none. So the
    /// first two tests prove each instrument can detect a proxy that IS created, and only then is a zero
    /// from it worth anything.
    /// </summary>
    public class WrapperReadNoProxyTests(SpawnJSRuntime JS)
    {
        /// <summary>Counted proxy constructions since the marker was taken.</summary>
        static long ProxyCount()
            => SpawnJSRuntime.CallCounts.TryGetValue("handle:fromJSObject", out var n) ? n : 0;

        /// <summary>Enumerable Symbol keys on a global, which is what the .Net runtime tags a proxy with.</summary>
        int SymbolKeysOn(string global) => JS.Call<int>("eval",
            $"Object.getOwnPropertySymbols(globalThis.{global})" +
            $".filter(s => Object.getOwnPropertyDescriptor(globalThis.{global}, s).enumerable).length");

        /// <summary>
        /// THE GUARD FOR EVERY OTHER TEST IN THIS FILE. The proxy counter has to move when a proxy is
        /// genuinely created, or a zero from it means nothing and every assertion below is vacuous.
        /// </summary>
        [SpawnJSTest]
        public async Task ProxyCounterDetectsAProxyTest()
        {
            var counting = SpawnJSRuntime.CountCalls;
            SpawnJSRuntime.CountCalls = true;
            try
            {
                var before = ProxyCount();
                using (var target = JS.New("Object"))
                using (var handle = new SpawnJSHandle(target.JSObject))
                {
                    // constructing a handle FROM a JSObject is the proxy path by definition
                }
                if (ProxyCount() <= before)
                    throw new Exception(
                        "the proxy counter did not move when a proxy was deliberately created - it cannot " +
                        "detect one, so every no-proxy assertion in this file would pass for the wrong reason");
            }
            finally
            {
                SpawnJSRuntime.CountCalls = counting;
            }
        }

        /// <summary>
        /// THE GUARD FOR THE SYMBOL TESTS. The runtime must actually tag a proxied object with an
        /// enumerable Symbol, or counting Symbols proves nothing about whether a proxy was made.
        /// </summary>
        [SpawnJSTest]
        public async Task SymbolProbeDetectsAProxyTest()
        {
            JS.CallVoid("eval", "globalThis.__proxyTagProbe = { tag: 'x' }");
            try
            {
                if (SymbolKeysOn("__proxyTagProbe") != 0)
                    throw new Exception("a freshly created Javascript object already carried an enumerable Symbol");
                // deliberately materialise a proxy for it
                using (var proxied = JS.Get<JSObject>("__proxyTagProbe"))
                {
                    if (proxied == null) throw new Exception("reading the probe as a JSObject returned null");
                }
                if (SymbolKeysOn("__proxyTagProbe") == 0)
                    throw new Exception(
                        "proxying an object did NOT tag it with an enumerable Symbol - so counting Symbols " +
                        "cannot tell a proxied object from a clean one, and the tests that rely on it have no teeth");
            }
            finally
            {
                JS.CallVoid("eval", "delete globalThis.__proxyTagProbe");
            }
        }

        /// <summary>
        /// The headline: reading a wrapper creates no proxy. This is the path JS.Get&lt;Window&gt; takes.
        /// </summary>
        [SpawnJSTest]
        public async Task WrapperReadCreatesNoProxyTest()
        {
            JS.CallVoid("eval", "globalThis.__wrapperReadProbe = { tag: 'read me' }");
            var counting = SpawnJSRuntime.CountCalls;
            SpawnJSRuntime.CountCalls = true;
            try
            {
                var before = ProxyCount();
                using (var wrapper = JS.Get<SpawnJSObject>("__wrapperReadProbe"))
                {
                    if (wrapper == null) throw new Exception("reading the probe as a wrapper returned null");
                    // and it must be the REAL object, not an empty handle that happens to cost nothing
                    var tag = wrapper.JSRef!.Get<string>("tag");
                    if (tag != "read me") throw new Exception($"the wrapper read back tag '{tag}'");
                }
                var created = ProxyCount() - before;
                if (created != 0)
                    throw new Exception($"reading a wrapper created {created} JSObject proxy(s)");
                if (SymbolKeysOn("__wrapperReadProbe") != 0)
                    throw new Exception("the Javascript object was tagged with an enumerable Symbol, so it was proxied");
            }
            finally
            {
                SpawnJSRuntime.CountCalls = counting;
                JS.CallVoid("eval", "delete globalThis.__wrapperReadProbe");
            }
        }

        /// <summary>
        /// A wrapper read from a live browser API, rather than a probe object we made. Location is the
        /// archetype of the mechanically ported wrapper.
        /// </summary>
        [SpawnJSTest]
        public async Task PortedWrapperReadCreatesNoProxyTest()
        {
            HostCapabilities.RequireBrowser();
            var counting = SpawnJSRuntime.CountCalls;
            SpawnJSRuntime.CountCalls = true;
            try
            {
                var before = ProxyCount();
                using (var location = JS.Get<Location>("location"))
                {
                    if (string.IsNullOrEmpty(location.Href)) throw new Exception("Location.Href was empty");
                }
                var created = ProxyCount() - before;
                if (created != 0)
                    throw new Exception($"reading Location created {created} JSObject proxy(s)");
            }
            finally
            {
                SpawnJSRuntime.CountCalls = counting;
            }
        }

        /// <summary>
        /// A wrapper RETURNED FROM A CALL, which is the other read path - the value arrives in a slot from
        /// the call buffer rather than being read from a parent by name.
        /// </summary>
        [SpawnJSTest]
        public async Task WrapperReturnedFromCallCreatesNoProxyTest()
        {
            JS.CallVoid("eval", "globalThis.__makeProbeObject = function () { return { made: true }; }");
            var counting = SpawnJSRuntime.CountCalls;
            SpawnJSRuntime.CountCalls = true;
            try
            {
                var before = ProxyCount();
                using (var made = JS.Call<SpawnJSObject>("__makeProbeObject"))
                {
                    if (made == null) throw new Exception("the call returned null");
                    if (!made.JSRef!.Get<bool>("made")) throw new Exception("the returned object was not the one the function made");
                }
                var created = ProxyCount() - before;
                if (created != 0)
                    throw new Exception($"a wrapper returned from a call created {created} JSObject proxy(s)");
            }
            finally
            {
                SpawnJSRuntime.CountCalls = counting;
                JS.CallVoid("eval", "delete globalThis.__makeProbeObject");
            }
        }

        /// <summary>
        /// Reading null and undefined must produce a null wrapper and, just as importantly, must not leak
        /// a slot - the read allocates none for them, and a mistake here would be invisible except as a
        /// store that only ever grows.
        /// </summary>
        [SpawnJSTest]
        public async Task ReadingNullAndUndefinedLeaksNoSlotTest()
        {
            JS.CallVoid("eval", "globalThis.__nullProbe = null");
            try
            {
                var before = SpawnJSHandle.LiveSlotCount;
                for (var i = 0; i < 20; i++)
                {
                    var fromNull = JS.Get<SpawnJSObject>("__nullProbe");
                    if (fromNull != null) throw new Exception("reading a null value produced a non null wrapper");
                    var fromMissing = JS.Get<SpawnJSObject>("__thisPropertyDoesNotExist");
                    if (fromMissing != null) throw new Exception("reading an undefined value produced a non null wrapper");
                }
                var leaked = SpawnJSHandle.LiveSlotCount - before;
                if (leaked != 0)
                    throw new Exception($"reading null and undefined 40 times leaked {leaked} slot(s)");
            }
            finally
            {
                JS.CallVoid("eval", "delete globalThis.__nullProbe");
            }
        }

        /// <summary>
        /// The wrapper must OWN its reference: it has to outlive the handle the marshaller was given, which
        /// is disposed the moment the marshal returns. If it borrowed that handle instead of taking its
        /// own, the wrapper would read a freed slot - and reading a freed slot returns undefined rather
        /// than throwing, so the failure would be silently wrong data.
        /// </summary>
        [SpawnJSTest]
        public async Task WrapperOutlivesTheMarshalledHandleTest()
        {
            JS.CallVoid("eval", "globalThis.__ownershipProbe = { value: 42 }");
            try
            {
                using var wrapper = JS.Get<SpawnJSObject>("__ownershipProbe");
                if (wrapper == null) throw new Exception("reading the probe returned null");
                // force a good deal of unrelated handle traffic, which would reuse or free storage the
                // wrapper was relying on if it had not taken its own
                for (var i = 0; i < 50; i++)
                {
                    using var churn = JS.New("Object");
                    churn.Set("i", i);
                }
                var value = wrapper.JSRef!.Get<int>("value");
                if (value != 42) throw new Exception($"the wrapper read back {value} after unrelated handle traffic, expected 42");
            }
            finally
            {
                JS.CallVoid("eval", "delete globalThis.__ownershipProbe");
            }
        }

        /// <summary>
        /// Two wrappers read from the same Javascript value hold INDEPENDENT references. Disposing one must
        /// not disturb the other - the failure mode the shared slot table has to rule out, and the same
        /// class of bug as the JSObject interning quirk this design exists to escape.
        /// </summary>
        [SpawnJSTest]
        public async Task IndependentReadsDoNotShareALifetimeTest()
        {
            JS.CallVoid("eval", "globalThis.__sharedProbe = { value: 7 }");
            try
            {
                var first = JS.Get<SpawnJSObject>("__sharedProbe")!;
                using var second = JS.Get<SpawnJSObject>("__sharedProbe")!;
                if (Equals(first.JSRef!.JSHandle.JSKey, second.JSRef!.JSHandle.JSKey))
                    throw new Exception("two independent reads share one slot, so disposing either would break the other");
                first.Dispose();
                var value = second.JSRef!.Get<int>("value");
                if (value != 7) throw new Exception($"the second wrapper read back {value} after the first was disposed, expected 7");
            }
            finally
            {
                JS.CallVoid("eval", "delete globalThis.__sharedProbe");
            }
        }

        /// <summary>
        /// JSRefAs routes through As&lt;T&gt;, which used to hand its JSObject across purely to get it back
        /// as another type. It must still produce a working wrapper, and now without a proxy.
        /// </summary>
        [SpawnJSTest]
        public async Task JSRefAsCreatesNoProxyTest()
        {
            JS.CallVoid("eval", "globalThis.__asProbe = { value: 'converted' }");
            var counting = SpawnJSRuntime.CountCalls;
            SpawnJSRuntime.CountCalls = true;
            try
            {
                using var wrapper = JS.Get<SpawnJSObject>("__asProbe")!;
                var before = ProxyCount();
                using var asOther = wrapper.JSRefAs<SpawnJSObject>();
                var created = ProxyCount() - before;
                var value = asOther.JSRef!.Get<string>("value");
                if (value != "converted") throw new Exception($"JSRefAs produced a wrapper that read back '{value}'");
                if (created != 0) throw new Exception($"JSRefAs created {created} JSObject proxy(s)");
            }
            finally
            {
                SpawnJSRuntime.CountCalls = counting;
                JS.CallVoid("eval", "delete globalThis.__asProbe");
            }
        }

        /// <summary>
        /// Reading an ARRAY creates no proxy. This path takes a handle to the Javascript array before
        /// reading a single element, so it proxied once per array read regardless of length.
        /// </summary>
        [SpawnJSTest]
        public async Task ArrayReadCreatesNoProxyTest()
        {
            JS.CallVoid("eval", "globalThis.__arrayProbe = [3, 5, 7, 11]");
            var counting = SpawnJSRuntime.CountCalls;
            SpawnJSRuntime.CountCalls = true;
            try
            {
                var before = ProxyCount();
                var values = JS.Get<int[]>("__arrayProbe");
                var created = ProxyCount() - before;
                if (values == null || values.Length != 4) throw new Exception($"read back {values?.Length.ToString() ?? "null"} elements, expected 4");
                if (values[0] != 3 || values[3] != 11) throw new Exception($"read back [{string.Join(",", values)}]");
                if (created != 0) throw new Exception($"reading an array created {created} JSObject proxy(s)");
                if (SymbolKeysOn("__arrayProbe") != 0) throw new Exception("the array was tagged with an enumerable Symbol, so it was proxied");
            }
            finally
            {
                SpawnJSRuntime.CountCalls = counting;
                JS.CallVoid("eval", "delete globalThis.__arrayProbe");
            }
        }

        /// <summary>
        /// Reading a RECORD back as a dictionary creates no proxy. Enumerating its keys used to resolve one
        /// for the record itself, which also tagged it - so the read path reintroduced the very Symbol
        /// hazard the write path had been cleared of.
        /// </summary>
        [SpawnJSTest]
        public async Task DictionaryReadCreatesNoProxyTest()
        {
            JS.CallVoid("eval", "globalThis.__recordProbe = { a: 1, b: 2 }");
            var counting = SpawnJSRuntime.CountCalls;
            SpawnJSRuntime.CountCalls = true;
            try
            {
                var before = ProxyCount();
                var map = JS.Get<Dictionary<string, int>>("__recordProbe");
                var created = ProxyCount() - before;
                if (map == null || map.Count != 2) throw new Exception($"read back {map?.Count.ToString() ?? "null"} entries, expected 2");
                if (map["a"] != 1 || map["b"] != 2) throw new Exception("the dictionary read back the wrong values");
                if (created != 0) throw new Exception($"reading a record created {created} JSObject proxy(s)");
                if (SymbolKeysOn("__recordProbe") != 0) throw new Exception("the record was tagged with an enumerable Symbol, so it was proxied");
            }
            finally
            {
                SpawnJSRuntime.CountCalls = counting;
                JS.CallVoid("eval", "delete globalThis.__recordProbe");
            }
        }

        /// <summary>
        /// A NULL record must still read back as null, not as a present-but-empty dictionary. The slot key
        /// read returns null and an empty array for two different situations, and collapsing them would
        /// silently turn absence into emptiness - which no assertion about proxies would ever catch.
        /// </summary>
        [SpawnJSTest]
        public async Task NullRecordReadsBackAsNullNotEmptyTest()
        {
            JS.CallVoid("eval", "globalThis.__nullRecord = null; globalThis.__emptyRecord = {}");
            try
            {
                var fromNull = JS.Get<Dictionary<string, int>>("__nullRecord");
                if (fromNull != null) throw new Exception($"a null record read back as a dictionary of {fromNull.Count} entries, expected null");
                var fromEmpty = JS.Get<Dictionary<string, int>>("__emptyRecord");
                if (fromEmpty == null) throw new Exception("an empty record read back as null, expected an empty dictionary");
                if (fromEmpty.Count != 0) throw new Exception($"an empty record read back {fromEmpty.Count} entries");
            }
            finally
            {
                JS.CallVoid("eval", "delete globalThis.__nullRecord; delete globalThis.__emptyRecord");
            }
        }

        /// <summary>
        /// Writing a byte array onto a held object creates no proxy for the PARENT. The bytes themselves
        /// have to cross; the object being written into never did.
        /// </summary>
        [SpawnJSTest]
        public async Task ByteArrayWriteCreatesNoProxyTest()
        {
            using var target = JS.New("Object");
            JS.Set("__byteTarget", target);
            var counting = SpawnJSRuntime.CountCalls;
            SpawnJSRuntime.CountCalls = true;
            try
            {
                var before = ProxyCount();
                target.Set("bytes", new byte[] { 1, 2, 3, 4 });
                var created = ProxyCount() - before;
                var length = JS.Call<int>("eval", "globalThis.__byteTarget.bytes.length");
                if (length != 4) throw new Exception($"the byte array arrived with length {length}, expected 4");
                var first = JS.Call<int>("eval", "globalThis.__byteTarget.bytes[0]");
                var last = JS.Call<int>("eval", "globalThis.__byteTarget.bytes[3]");
                if (first != 1 || last != 4) throw new Exception($"the bytes arrived as [{first}...{last}]");
                if (created != 0) throw new Exception($"writing a byte array created {created} JSObject proxy(s)");
            }
            finally
            {
                SpawnJSRuntime.CountCalls = counting;
                JS.CallVoid("eval", "delete globalThis.__byteTarget");
            }
        }

        /// <summary>
        /// A value that is not a reference cannot be owned by a handle, and the read falls back rather than
        /// inventing a meaning for it - so it fails exactly as it always did instead of handing back a
        /// wrapper around a number whose every property reads undefined.
        /// </summary>
        [SpawnJSTest]
        public async Task ReadingAWrapperFromANumberStillFailsTest()
        {
            JS.CallVoid("eval", "globalThis.__numberProbe = 5");
            try
            {
                var threw = false;
                try
                {
                    using var wrapper = JS.Get<SpawnJSObject>("__numberProbe");
                }
                catch
                {
                    threw = true;
                }
                if (!threw)
                    throw new Exception("reading a number as a wrapper succeeded - it must fail rather than produce a wrapper around a primitive");
            }
            finally
            {
                JS.CallVoid("eval", "delete globalThis.__numberProbe");
            }
        }
    }
}
