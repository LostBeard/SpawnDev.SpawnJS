
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#enumdef-gpucullmode
    /// </summary>
    public enum GPUCullMode
    {
        /// <summary>
        /// No polygons are discarded.
        /// </summary>
        [JsonPropertyName("none")]
        None,
        /// <summary>
        /// Front-facing polygons are discarded.
        /// </summary>
        [JsonPropertyName("front")]
        Front,
        /// <summary>
        /// Back-facing polygons are discarded.
        /// </summary>
        [JsonPropertyName("back")]
        Back,
    }
}
