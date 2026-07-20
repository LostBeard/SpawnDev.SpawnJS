// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#enumdef-gpuaddressmode
    /// </summary>
    public enum GPUAddressMode
    {
        /// <summary>
        /// Texture coordinates are clamped between 0.0 and 1.0, inclusive.
        /// </summary>
        [JsonPropertyName("clamp-to-edge")]
        ClampToEdge,
        /// <summary>
        /// Texture coordinates wrap to the other side of the texture.
        /// </summary>
        [JsonPropertyName("repeat")]
        Repeat,
        /// <summary>
        /// Texture coordinates wrap to the other side of the texture, but the texture is flipped when the integer part of the coordinate is odd.
        /// </summary>
        [JsonPropertyName("mirror-repeat")]
        MirrorRepeat,
    }
}
