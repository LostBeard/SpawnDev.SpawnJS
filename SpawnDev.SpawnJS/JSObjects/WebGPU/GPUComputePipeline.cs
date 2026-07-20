// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#gpucomputepipeline
    /// </summary>
    public class GPUComputePipeline : GPUObjectBase
    {
        /// <inheritdoc/>
        public GPUComputePipeline(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Gets a GPUBindGroupLayout that is compatible with the GPUPipelineBase’s GPUBindGroupLayout at index.
        /// </summary>
        /// <param name="index">Index into the pipeline layout’s [[bindGroupLayouts]] sequence.</param>
        /// <returns>GPUBindGroupLayout</returns>
        public GPUBindGroupLayout GetBindGroupLayout(int index) => JSRef!.Call<GPUBindGroupLayout>("getBindGroupLayout", index);
    }
}
