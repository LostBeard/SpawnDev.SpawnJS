// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#enumdef-gpuloadop
    /// </summary>
    public enum GPULoadOp
    {
        /// <summary>
        /// Load
        /// </summary>
        [JsonPropertyName("load")]
        Load,
        /// <summary>
        /// Clear
        /// </summary>
        [JsonPropertyName("clear")]
        Clear,
    }
}
