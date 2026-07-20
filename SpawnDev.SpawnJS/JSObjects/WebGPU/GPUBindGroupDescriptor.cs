// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#dictdef-gpubindgroupdescriptor
    /// </summary>
    public class GPUBindGroupDescriptor : GPUObjectDescriptorBase
    {
        /// <summary>
        /// The GPUBindGroupLayout the entries of this bind group will conform to.
        /// </summary>
        [JsonPropertyName("layout")]
        public GPUBindGroupLayout Layout { get; set; }

        /// <summary>
        /// A list of entries describing the resources to expose to the shader for each binding described by the layout.
        /// </summary>
        [JsonPropertyName("entries")]
        public IEnumerable<GPUBindGroupEntry> Entries { get; set; }
    }
}
