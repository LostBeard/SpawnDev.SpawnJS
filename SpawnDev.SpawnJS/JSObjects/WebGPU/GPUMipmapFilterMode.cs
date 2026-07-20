// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#enumdef-gpumipmapfiltermode
    /// </summary>
    public enum GPUMipmapFilterMode
    {
        /// <summary>
        /// Return the value of the texel nearest to the texture coordinates.
        /// </summary>
        [JsonPropertyName("nearest")]
        Nearest,
        /// <summary>
        /// Select two texels in each dimension and return a linear interpolation between their values.
        /// </summary>
        [JsonPropertyName("linear")]
        Linear,
    }
}
