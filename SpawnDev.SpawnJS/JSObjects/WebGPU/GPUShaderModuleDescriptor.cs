// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Describes the parameters for creating a GPUShaderModule, which is used to compile and manage shader code in WebGPU.<br/>
    /// https://www.w3.org/TR/webgpu/#shader-module-creation
    /// </summary>
    public class GPUShaderModuleDescriptor : GPUObjectDescriptorBase
    {
        /// <summary>
        /// A string representing the WGSL source code for the shader module.
        /// </summary>
        public string Code { get; set; }
    }
}
