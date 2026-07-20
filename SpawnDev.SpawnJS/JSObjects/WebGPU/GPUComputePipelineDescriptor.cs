// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#dictdef-gpucomputepipelinedescriptor
    /// </summary>
    public class GPUComputePipelineDescriptor : GPUPipelineDescriptorBase
    {
        /// <summary>
        /// Describes the compute shader entry point of the pipeline.
        /// </summary>
        [JsonPropertyName("compute")]
        public GPUProgrammableStage Compute { get; set; }
    }
}
