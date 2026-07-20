// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#dictdef-gpublendcomponent
    /// </summary>
    public class GPUBlendComponent
    {
        /// <summary>
        /// Defines the GPUBlendOperation used to calculate the values written to the target attachment components.<br/>
        /// defaulting to "add"
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EnumString<GPUBlendOperation>? Operation { get; set; }
        /// <summary>
        /// Defines the GPUBlendFactor operation to be performed on values from the fragment shader.<br/>
        /// defaulting to "one"
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EnumString<GPUBlendFactor>? SrcFactor { get; set; }
        /// <summary>
        /// Defines the GPUBlendFactor operation to be performed on values from the target attachment.<br/>
        /// defaulting to "zero"
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EnumString<GPUBlendFactor>? DstFactor { get; set; }
    }
}
