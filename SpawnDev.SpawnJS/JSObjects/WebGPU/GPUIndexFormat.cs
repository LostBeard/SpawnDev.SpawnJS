
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
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
