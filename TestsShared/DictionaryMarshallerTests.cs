using SpawnDev.SpawnJS;
using System.Text.Json.Serialization;

namespace TestsShared
{
    /// <summary>
    /// Dictionaries crossing as plain Javascript objects.<br/>
    /// The web platform calls this a record - <c>GPUProgrammableStage.constants</c> is one. Before
    /// DictionaryMarshaller existed a Dictionary matched ObjectMarshaller, which reflected over the
    /// dictionary's own members instead of its contents, and SpawnDev.ILGPU could not create a WebGPU
    /// compute pipeline at all. String keys cross verbatim; non-string keys (int, enum, ...) stringify to
    /// their invariant form on write and parse back on read, since Javascript object keys are always
    /// strings - which is what SpawnDev.ILGPU's WebGL <c>Dictionary&lt;int, int[]&gt;</c> stride map needs.
    /// </summary>
    public class DictionaryMarshallerTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// Mirrors the shape that actually failed: a descriptor holding a record, where the record is
        /// null because this kernel declares no override constants.
        /// </summary>
        public class StageLike
        {
            [JsonPropertyName("entryPoint")]
            public string? EntryPoint { get; set; }
            [JsonPropertyName("constants")]
            public Dictionary<string, object>? Constants { get; set; }
        }

        /// <summary>
        /// The contents must cross, keyed by the dictionary's own keys - not the dictionary's members.
        /// Reading a value back by key is what proves Javascript received a record.
        /// </summary>
        [SpawnJSTest]
        public async Task DictionaryCrossesAsJavascriptObjectTest()
        {
            JS.Set("__dictTest", new Dictionary<string, object>
            {
                ["workgroupSize"] = 64,
                ["label"] = "main",
                ["enabled"] = true,
            });

            var size = JS.Get<double>("__dictTest.workgroupSize");
            if (size != 64) throw new Exception($"workgroupSize read back as {size}");
            var label = JS.Get<string>("__dictTest.label");
            if (label != "main") throw new Exception($"label read back as '{label}'");
            var enabled = JS.Get<bool>("__dictTest.enabled");
            if (!enabled) throw new Exception("enabled read back as false");
        }

        /// <summary>
        /// A dictionary must NOT cross as a description of itself. Count/Keys/Comparer appearing on the
        /// Javascript side is the exact old behaviour, and it is what made the failure so hard to place.
        /// </summary>
        [SpawnJSTest]
        public async Task DictionaryDoesNotLeakItsOwnMembersTest()
        {
            JS.Set("__dictMembers", new Dictionary<string, object> { ["a"] = 1 });
            foreach (var leaked in new[] { "Count", "Keys", "Values", "Comparer" })
            {
                if (!JS.IsUndefined($"__dictMembers.{leaked}"))
                    throw new Exception($"dictionary leaked its own member '{leaked}' to javascript");
            }
        }

        /// <summary>
        /// A record must carry NO enumerable Symbol keys.<br/>
        /// This is not academic. WebIDL converts a <c>record&lt;USVString, V&gt;</c> by enumerating every
        /// own key including Symbols and converting each to a string, and converting a Symbol to a string
        /// throws. WebGPU rejected the whole compute pipeline with "Failed to read the 'constants'
        /// property ... Cannot convert a Symbol value to a string" for exactly this reason, so any object
        /// SpawnJS hands to a record-typed web API has to be clean.
        /// </summary>
        [SpawnJSTest]
        public async Task RecordHasNoEnumerableSymbolKeysTest()
        {
            JS.Set("__dictSymbols", new Dictionary<string, object> { ["a"] = 1 });

            var enumerableSymbols = JS.Call<int>("eval",
                "Object.getOwnPropertySymbols(globalThis.__dictSymbols)" +
                ".filter(s => Object.getOwnPropertyDescriptor(globalThis.__dictSymbols, s).enumerable).length");
            if (enumerableSymbols != 0)
                throw new Exception($"record carries {enumerableSymbols} enumerable Symbol key(s) - a record-typed web API will reject it");
        }

        /// <summary>
        /// Round trip, so the read direction is covered too and not just the write.
        /// </summary>
        [SpawnJSTest]
        public async Task DictionaryRoundTripsTest()
        {
            JS.Set("__dictRoundTrip", new Dictionary<string, double>
            {
                ["x"] = 1.5,
                ["y"] = 2.5,
            });

            var back = JS.Get<Dictionary<string, double>>("__dictRoundTrip");
            if (back == null) throw new Exception("round trip returned null");
            if (back.Count != 2) throw new Exception($"expected 2 entries, got {back.Count}");
            if (back["x"] != 1.5 || back["y"] != 2.5)
                throw new Exception($"values came back as x={back.GetValueOrDefault("x")}, y={back.GetValueOrDefault("y")}");
        }

        /// <summary>
        /// The WebGPU case exactly: a null record on a descriptor. This is the one that broke ILGPU, and
        /// it broke while the value was null - so null is the case that matters most here.
        /// </summary>
        [SpawnJSTest]
        public async Task NullDictionaryOnObjectDoesNotThrowTest()
        {
            JS.Set("__dictStage", new StageLike { EntryPoint = "main", Constants = null });

            var entryPoint = JS.Get<string>("__dictStage.entryPoint");
            if (entryPoint != "main") throw new Exception($"entryPoint read back as '{entryPoint}'");
        }

        /// <summary>
        /// And a populated record on that same descriptor shape, which is what a kernel with override
        /// constants actually sends.
        /// </summary>
        [SpawnJSTest]
        public async Task PopulatedDictionaryOnObjectCrossesTest()
        {
            JS.Set("__dictStage2", new StageLike
            {
                EntryPoint = "main",
                Constants = new Dictionary<string, object> { ["wgSize"] = 256 },
            });

            var wgSize = JS.Get<double>("__dictStage2.constants.wgSize");
            if (wgSize != 256) throw new Exception($"nested constant read back as {wgSize}");
        }

        /// <summary>
        /// The exact SpawnDev.ILGPU WebGL shape: an int-keyed <c>Dictionary&lt;int, int[]&gt;</c> stride map
        /// on a dispatch message. Javascript object keys are always strings, so the int keys must cross as
        /// their invariant string form (<c>{"0":[...],"7":[...]}</c>) - the same coercion <c>obj[0]=x</c>
        /// performs. Before this was supported the int-keyed dictionary failed CanMarshal, fell through to
        /// IEnumerableMarshaller&lt;string&gt;, and threw InvalidCastException - which killed nearly the
        /// entire WebGL test lane.
        /// </summary>
        [SpawnJSTest]
        public async Task IntKeyedDictionaryCrossesAsJavascriptObjectTest()
        {
            JS.Set("__dictInt", new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3 },
                [7] = new[] { 40, 50 },
            });

            // Keys coerced to strings on the Javascript side, exactly like a JS object.
            var firstStride = JS.Get<int>("__dictInt.0.1");
            if (firstStride != 2) throw new Exception($"__dictInt['0'][1] read back as {firstStride}");
            var secondStride = JS.Get<int>("__dictInt.7.0");
            if (secondStride != 40) throw new Exception($"__dictInt['7'][0] read back as {secondStride}");
        }

        /// <summary>
        /// Round trip of the int-keyed dictionary, so the read direction parses the Javascript string keys
        /// back into <c>int</c> - not just the write direction.
        /// </summary>
        [SpawnJSTest]
        public async Task IntKeyedDictionaryRoundTripsTest()
        {
            JS.Set("__dictIntRoundTrip", new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2 },
                [7] = new[] { 40 },
            });

            var back = JS.Get<Dictionary<int, int[]>>("__dictIntRoundTrip");
            if (back == null) throw new Exception("round trip returned null");
            if (back.Count != 2) throw new Exception($"expected 2 entries, got {back.Count}");
            if (!back.ContainsKey(0) || !back.ContainsKey(7))
                throw new Exception($"keys came back as [{string.Join(",", back.Keys)}] - expected 0 and 7");
            if (back[0].Length != 2 || back[0][1] != 2 || back[7][0] != 40)
                throw new Exception("int-keyed values did not round trip");
        }

        enum StrideAxis { X, Y, Z }

        /// <summary>
        /// An enum-keyed dictionary round trips too - enum keys stringify by name on write and parse back
        /// by name on read, so any non-string key type crosses, not only <c>int</c>.
        /// </summary>
        [SpawnJSTest]
        public async Task EnumKeyedDictionaryRoundTripsTest()
        {
            JS.Set("__dictEnum", new Dictionary<StrideAxis, double>
            {
                [StrideAxis.X] = 1.0,
                [StrideAxis.Z] = 3.0,
            });

            var z = JS.Get<double>("__dictEnum.Z");
            if (z != 3.0) throw new Exception($"__dictEnum['Z'] read back as {z}");

            var back = JS.Get<Dictionary<StrideAxis, double>>("__dictEnum");
            if (back == null) throw new Exception("enum round trip returned null");
            if (back.Count != 2 || back[StrideAxis.X] != 1.0 || back[StrideAxis.Z] != 3.0)
                throw new Exception("enum-keyed dictionary did not round trip");
        }
    }
}
