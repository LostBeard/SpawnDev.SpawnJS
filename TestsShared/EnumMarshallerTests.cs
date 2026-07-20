using SpawnDev.SpawnJS;
using System.Text.Json.Serialization;

namespace TestsShared
{
    /// <summary>
    /// Enums crossing to Javascript and back.<br/>
    /// A .Net enum is numeric on the wire; the exception is one marked
    /// <see cref="JsonStringEnumConverter"/>, which crosses as its Javascript string. Before
    /// EnumMarshaller existed an enum matched only the default marshaller and arrived as something no
    /// web API would accept as a number - which is how SpawnDev.ILGPU failed to create a single WebGPU
    /// buffer, reported as "Value is not of type 'unsigned long'".
    /// </summary>
    public class EnumMarshallerTests(SpawnJSRuntime JS)
    {
        [Flags]
        public enum TestFlags : uint
        {
            None = 0,
            CopySrc = 0x0004,
            CopyDst = 0x0008,
            Storage = 0x0080,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum TestChunkType
        {
            [JsonPropertyName("key")]
            Key,
            [JsonPropertyName("delta")]
            Delta,
        }

        public class FlagsHolder
        {
            [JsonPropertyName("usage")]
            public TestFlags Usage { get; set; }
            [JsonPropertyName("type")]
            public TestChunkType Type { get; set; }
            [JsonPropertyName("optional")]
            public TestFlags? Optional { get; set; }
        }

        /// <summary>
        /// A numeric enum must be readable from Javascript as a number. Reading it back as a double is
        /// the check that matters: had it crossed as a string or an opaque box, this read would not
        /// produce the value, which is precisely what a web API sees when it rejects the argument.
        /// </summary>
        [SpawnJSTest]
        public async Task NumericEnumCrossesAsNumberTest()
        {
            const TestFlags expected = TestFlags.CopySrc | TestFlags.Storage;
            JS.Set("__enumTestUsage", expected);
            var asNumber = JS.Get<double>("__enumTestUsage");
            if (asNumber != (double)(uint)expected)
                throw new Exception($"expected {(uint)expected} in javascript, read back {asNumber}");
        }

        /// <summary>
        /// And it must survive the round trip as the enum type.<br/>
        /// The numeric read comes first deliberately. Reading the enum back on its own proves nothing:
        /// that passes even with no enum marshaller at all, because the value can travel as an opaque
        /// .Net box that Javascript can hold but not use. Asserting Javascript holds the NUMBER is what
        /// makes this a test rather than an identity check.
        /// </summary>
        [SpawnJSTest]
        public async Task NumericEnumRoundTripsTest()
        {
            const TestFlags expected = TestFlags.CopyDst | TestFlags.Storage;
            JS.Set("__enumTestRoundTrip", expected);
            var asNumber = JS.Get<double>("__enumTestRoundTrip");
            if (asNumber != (double)(uint)expected)
                throw new Exception($"javascript holds {asNumber}, not the number {(uint)expected}");
            var back = JS.Get<TestFlags>("__enumTestRoundTrip");
            if (back != expected) throw new Exception($"expected {expected}, got {back}");
        }

        /// <summary>
        /// An enum marked as a string enum crosses as its Javascript spelling, which is the
        /// JsonPropertyName ("delta") and not the .Net member name ("Delta").
        /// </summary>
        [SpawnJSTest]
        public async Task StringEnumCrossesAsJavascriptNameTest()
        {
            JS.Set("__enumTestChunk", TestChunkType.Delta);
            var raw = JS.Get<string>("__enumTestChunk");
            if (raw != "delta") throw new Exception($"expected 'delta', got '{raw}'");
            var back = JS.Get<TestChunkType>("__enumTestChunk");
            if (back != TestChunkType.Delta) throw new Exception($"round trip gave {back}");
        }

        /// <summary>
        /// Enums nested in an object are the shape that actually failed - a descriptor handed to a web
        /// API, where every property is marshalled on its own.
        /// </summary>
        [SpawnJSTest]
        public async Task EnumPropertiesOnObjectCrossCorrectlyTest()
        {
            JS.Set("__enumTestDescriptor", new FlagsHolder
            {
                Usage = TestFlags.CopySrc | TestFlags.CopyDst,
                Type = TestChunkType.Key,
                Optional = null,
            });

            var usage = JS.Get<double>("__enumTestDescriptor.usage");
            if (usage != (double)(uint)(TestFlags.CopySrc | TestFlags.CopyDst))
                throw new Exception($"nested usage read back as {usage}");
            var type = JS.Get<string>("__enumTestDescriptor.type");
            if (type != "key") throw new Exception($"nested type read back as '{type}'");
        }
    }
}
