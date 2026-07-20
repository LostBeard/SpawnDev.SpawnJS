// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// GPUBlendOperation defines the algorithm used to combine source and destination blend factors:<br/>
    /// https://www.w3.org/TR/webgpu/#enumdef-gpublendoperation
    /// </summary>
    public enum GPUBlendOperation
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("add")]
        Add,
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("subtract")]
        Subtract,
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("reverse-subtract")]
        ReverseSubtract,
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("min")]
        Min,
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("max")]
        Max,
    }
}
