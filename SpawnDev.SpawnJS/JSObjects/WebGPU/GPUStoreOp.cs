// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
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
