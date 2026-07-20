using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A GPUSampler encodes transformations and filtering information that can be used in a shader to interpret texture resource data.
    /// https://www.w3.org/TR/webgpu/#gpusampler
    /// </summary>
    public class GPUSampler : GPUObjectBase
    {
        /// <inheritdoc />
        public GPUSampler(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
