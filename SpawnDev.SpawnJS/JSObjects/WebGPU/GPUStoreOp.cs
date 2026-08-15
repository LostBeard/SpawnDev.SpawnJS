
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#enumdef-gpustoreop
    /// </summary>
    public enum GPUStoreOp
    {
        /// <summary>
        /// Load
        /// </summary>
        [JsonPropertyName("store")]
        Store,
        /// <summary>
        /// Clear
        /// </summary>
        [JsonPropertyName("discard")]
        Discard,
    }
}
