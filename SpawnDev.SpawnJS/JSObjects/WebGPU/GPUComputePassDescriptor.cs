// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#dictdef-gpucomputepassdescriptor
    /// </summary>
    public class GPUComputePassDescriptor : GPUObjectDescriptorBase
    {
        /// <summary>
        /// Defines which timestamp values will be written for this pass, and where to write them to.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("timestampWrites")]
        public GPUComputePassTimestampWrites? TimestampWrites { get; set; }
    }
}
