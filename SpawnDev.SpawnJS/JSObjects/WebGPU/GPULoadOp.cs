
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
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
