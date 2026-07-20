// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The shape of the wave produced by the node. 
    /// </summary>
    public enum OscillatorType
    {
        /// <summary>
        /// sine
        /// </summary>
        [JsonPropertyName("sine")]
        Sine,
        /// <summary>
        /// square
        /// </summary>
        [JsonPropertyName("square")]
        Square,
        /// <summary>
        /// sawtooth
        /// </summary>
        [JsonPropertyName("sawtooth")]
        Sawtooth,
        /// <summary>
        /// triangle
        /// </summary>
        [JsonPropertyName("triangle")]
        Triangle,
        /// <summary>
        /// custom
        /// </summary>
        [JsonPropertyName("custom")]
        Custom,
    }
}
