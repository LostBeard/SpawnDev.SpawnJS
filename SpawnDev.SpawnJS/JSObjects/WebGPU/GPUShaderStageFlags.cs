// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// GPUShaderStage contains the following flags, which describe which shader stages a corresponding GPUBindGroupEntry 
    /// for this GPUBindGroupLayoutEntry will be visible to.
    /// https://www.w3.org/TR/webgpu/#typedefdef-gpushaderstageflags
    /// </summary>
    [Flags]
    public enum GPUShaderStageFlags : uint
    {
        /// <summary>
        /// The bind group entry will be accessible to vertex shaders.
        /// </summary>
        VERTEX = 0x1,
        /// <summary>
        /// The bind group entry will be accessible to fragment shaders.
        /// </summary>
        FRAGMENT = 0x2,
        /// <summary>
        /// The bind group entry will be accessible to compute shaders.
        /// </summary>
        COMPUTE = 0x4,
    }
}
