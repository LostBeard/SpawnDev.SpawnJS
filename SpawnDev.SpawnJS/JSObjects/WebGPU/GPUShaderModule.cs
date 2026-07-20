// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The GPUShaderModule interface of the WebGPU API represents an internal shader module object, 
    /// a container for WGSL shader code that can be submitted to the GPU for execution by a pipeline.
    /// A GPUShaderModule object instance is created using GPUDevice.createShaderModule().
    /// https://developer.mozilla.org/en-US/docs/Web/API/GPUShaderModule
    /// </summary>
    public class GPUShaderModule : GPUObjectBase
    {
        /// <inheritdoc/>
        public GPUShaderModule(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<GPUCompilationInfo> GetCompilationInfo() => JSRef!.CallAsync<GPUCompilationInfo>("getCompilationInfo");
    }
}
