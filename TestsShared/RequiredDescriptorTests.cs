using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;

namespace TestsShared
{
    /// <summary>
    /// Proof that the `required` modifier applied to the WebIDL descriptor types actually WORKS on the
    /// REAL types, rather than on a stand-in POCO.<br/>
    /// <br/>
    /// The point being tested is not the language feature - it is that marking these properties required
    /// did not break either of the two things consumers do with them:<br/>
    /// 1. CONSTRUCT one the way existing consuming code already does. Every method here is written in the
    ///    exact shape found in SpawnDev.ILGPU / SpawnScene / SpawnDev.VoxelEngine today, so if `required`
    ///    made that shape illegal, this file would not COMPILE. That is the source-compatibility check.<br/>
    /// 2. MARSHAL one to Javascript and read it back. `required` is a compile-time contract, but the
    ///    marshaller builds objects reflectively via Activator.CreateInstance, which never runs that
    ///    check - so a read had to be confirmed to still work, not assumed.<br/>
    /// <br/>
    /// Descriptors needing a live GPUDevice (GPURenderPassColorAttachment needs a real GPUTextureView)
    /// cannot be round tripped in a plain test host. Those are covered by the compile check only, and
    /// that limit is stated here rather than glossed over.
    /// </summary>
    public class RequiredDescriptorTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// GPUVertexAttribute is the ideal real subject: `Format` is required, and every member is a
        /// scalar or enum, so the whole descriptor marshals with no GPU device involved.<br/>
        /// The construction below is the shape used by real rendering code.
        /// </summary>
        [SpawnJSTest]
        public async Task RequiredVertexAttributeRoundTripsTest()
        {
            var attribute = new GPUVertexAttribute
            {
                Format = GPUVertexFormat.Float32x3,
                Offset = 12,
                ShaderLocation = 1,
            };

            JS.Set("__reqVertexAttribute", attribute);

            // assert Javascript holds the real values, so a failure below is unambiguous
            var format = JS.Get<string>("__reqVertexAttribute.format");
            if (format != "float32x3") throw new Exception($"format crossed as '{format}'");
            var offset = JS.Get<double>("__reqVertexAttribute.offset");
            if (offset != 12) throw new Exception($"offset crossed as {offset}");
            var location = JS.Get<double>("__reqVertexAttribute.shaderLocation");
            if (location != 1) throw new Exception($"shaderLocation crossed as {location}");

            // and read the whole descriptor back into .Net - this is the path `required` could have broken
            var back = JS.Get<GPUVertexAttribute>("__reqVertexAttribute");
            if (back == null) throw new Exception("descriptor read back as null");
            // EnumString<T> exposes the parsed value as Enum (String carries the Javascript spelling)
            if (back.Format?.Enum != GPUVertexFormat.Float32x3)
                throw new Exception($"required member read back as '{back.Format?.String}'");
            if (back.Offset != 12) throw new Exception($"offset read back as {back.Offset}");
        }

        /// <summary>
        /// GPUBindGroupEntry.Resource is required. This is the single most constructed descriptor in
        /// SpawnDev.ILGPU - it builds one per kernel binding on every dispatch - and the shape here is
        /// copied from WebGPUComputeShader.
        /// </summary>
        [SpawnJSTest]
        public async Task RequiredBindGroupEntryConstructsAndMarshalsTest()
        {
            // GPUBufferBinding without a real GPUBuffer still exercises the descriptor walk; the buffer
            // property simply crosses as null, which is what a marshalled absent object looks like
            var entry = new GPUBindGroupEntry
            {
                Binding = 3,
                Resource = new GPUBufferBinding { Offset = 0, Size = 256 },
            };

            JS.Set("__reqBindGroupEntry", entry);

            var binding = JS.Get<double>("__reqBindGroupEntry.binding");
            if (binding != 3) throw new Exception($"binding crossed as {binding}");
            var size = JS.Get<double>("__reqBindGroupEntry.resource.size");
            if (size != 256) throw new Exception($"nested required resource crossed as size {size}");
        }

        /// <summary>
        /// GPUColorTargetState.Format is required, and is constructed exactly this way in
        /// WebGPUCanvasRenderer.
        /// </summary>
        [SpawnJSTest]
        public async Task RequiredColorTargetStateRoundTripsTest()
        {
            var target = new GPUColorTargetState { Format = GPUTextureFormat.bgra8unorm };
            JS.Set("__reqColorTarget", target);

            var format = JS.Get<string>("__reqColorTarget.format");
            if (format != "bgra8unorm") throw new Exception($"format crossed as '{format}'");

            var back = JS.Get<GPUColorTargetState>("__reqColorTarget");
            if (back?.Format?.Enum != GPUTextureFormat.bgra8unorm)
                throw new Exception($"read back as '{back?.Format?.String}'");
        }

        /// <summary>
        /// The crypto params hierarchy is the part that could not be expressed by `required` alone.
        /// A fixed-name type must still be constructible with NO arguments and must carry its algorithm
        /// name to Javascript - if the [SetsRequiredMembers] constructor were wrong, either this would
        /// not compile or "name" would arrive missing.
        /// </summary>
        [SpawnJSTest]
        public async Task FixedNameCryptoParamsCarriesItsAlgorithmNameTest()
        {
            var aes = new AesGcmParams { TagLength = 128 };
            JS.Set("__reqAesGcm", aes);

            var name = JS.Get<string>("__reqAesGcm.name");
            if (name != "AES-GCM") throw new Exception($"algorithm name crossed as '{name}' - the fixed name did not survive");
            var tagLength = JS.Get<double>("__reqAesGcm.tagLength");
            if (tagLength != 128) throw new Exception($"tagLength crossed as {tagLength}");
        }

        /// <summary>
        /// And a type whose OTHER required members are supplied through the constructor. This is the case
        /// a parameterless [SetsRequiredMembers] constructor would have silently broken: it switches off
        /// required checking for the whole type, so salt would have stopped being enforced while the
        /// constructor claimed to set it.
        /// </summary>
        [SpawnJSTest]
        public async Task ConstructorSuppliedRequiredMembersCrossTest()
        {
            using var salt = new Uint8Array(new byte[] { 1, 2, 3, 4 });
            var pbkdf2 = new Pbkdf2Params(salt) { Iterations = 1000, Hash = "SHA-256" };
            JS.Set("__reqPbkdf2", pbkdf2);

            var name = JS.Get<string>("__reqPbkdf2.name");
            if (name != "PBKDF2") throw new Exception($"algorithm name crossed as '{name}'");
            var iterations = JS.Get<double>("__reqPbkdf2.iterations");
            if (iterations != 1000) throw new Exception($"iterations crossed as {iterations}");
            var saltLength = JS.Get<double>("__reqPbkdf2.salt.length");
            if (saltLength != 4) throw new Exception($"constructor supplied salt crossed with length {saltLength}");
        }
    }
}
