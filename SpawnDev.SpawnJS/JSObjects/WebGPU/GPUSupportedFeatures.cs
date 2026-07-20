// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The GPUSupportedFeatures interface of the WebGPU API is a Set-like object that describes additional functionality supported by a GPUAdapter.
    /// https://www.w3.org/TR/webgpu/#gpusupportedfeatures
    /// </summary>
    public class GPUSupportedFeatures : Set<string>
    {
        /// <inheritdoc />
        public GPUSupportedFeatures(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
