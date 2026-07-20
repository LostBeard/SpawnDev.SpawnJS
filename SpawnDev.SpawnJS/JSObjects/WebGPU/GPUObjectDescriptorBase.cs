// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// An object descriptor holds the information needed to create an object, which is typically done via one of the create* methods of GPUDevice.
    /// https://www.w3.org/TR/webgpu/#dictdef-gpuobjectdescriptorbase
    /// </summary>
    public abstract class GPUObjectDescriptorBase
    {
        /// <summary>
        /// A string providing a label that can be used to identify the object, for example in GPUError messages or console warnings.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("label")]
        public string Label { get; set; } = "";
    }
}