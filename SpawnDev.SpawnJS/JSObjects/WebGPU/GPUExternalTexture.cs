using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A GPUExternalTexture is a sampleable 2D texture wrapping an external video frame. 
    /// It is an immutable snapshot; its contents may not change over time, either from inside WebGPU
    /// (it is only sampleable) or from outside WebGPU (e.g. due to video frame advancement).
    /// https://www.w3.org/TR/webgpu/#gpuexternaltexture
    /// </summary>
    public class GPUExternalTexture : GPUObjectBase
    {
        /// <inheritdoc />
        public GPUExternalTexture(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
