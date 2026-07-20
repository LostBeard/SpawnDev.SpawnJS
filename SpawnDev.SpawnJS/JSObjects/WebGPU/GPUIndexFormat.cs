// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#enumdef-gpuindexformat
    /// </summary>
    public enum GPUIndexFormat
    {
        /// <summary>
        /// Uint16
        /// </summary>
        [JsonPropertyName("uint16")]
        Uint16,
        /// <summary>
        /// Uint32
        /// </summary>
        [JsonPropertyName("uint32")]
        Uint32,
    }
}
