// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#enumdef-gpustenciloperation
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GPUStencilOperation
    {
        /// <summary>
        /// keep
        /// </summary>
        [JsonPropertyName("keep")]
        Keep,
        /// <summary>
        /// zero
        /// </summary>
        [JsonPropertyName("zero")]
        Zero,
        /// <summary>
        /// replace
        /// </summary>
        [JsonPropertyName("replace")]
        Replace,
        /// <summary>
        /// invert
        /// </summary>
        [JsonPropertyName("invert")]
        Invert,
        /// <summary>
        /// increment-clamp
        /// </summary>
        [JsonPropertyName("increment-clamp")]
        IncrementClamp,
        /// <summary>
        /// decrement-clamp
        /// </summary>
        [JsonPropertyName("decrement-clamp")]
        DecrementClamp,
        /// <summary>
        /// increment-wrap
        /// </summary>
        [JsonPropertyName("increment-wrap")]
        IncrementWrap,
        /// <summary>
        /// decrement-wrap
        /// </summary>
        [JsonPropertyName("decrement-wrap")]
        DecrementWrap,
    }
}
