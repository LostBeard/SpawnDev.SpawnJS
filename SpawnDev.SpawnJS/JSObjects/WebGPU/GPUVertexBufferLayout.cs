// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Represents the expected layout of a vertex buffer used in the pipeline. 
    /// https://www.w3.org/TR/webgpu/#dictdef-gpuvertexbufferlayout
    /// </summary>
    public class GPUVertexBufferLayout
    {
        /// <summary>
        /// An array defining the layout of the vertex attributes within each element.
        /// </summary>
        public IEnumerable<GPUVertexAttribute>? Attributes { get; set; }

        /// <summary>
        /// The byte offset between the start of consecutive vertex attributes. 
        /// </summary>
        public GPUSize64 ArrayStride { get; set; }

        /// <summary>
        /// A GPUVertexStepMode describing how vertex attributes are advanced during rendering. 
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EnumString<GPUVertexStepMode>? StepMode { get; set; }
    }
}
