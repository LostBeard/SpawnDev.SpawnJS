// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Serial port parity options<br/>
    /// https://wicg.github.io/serial/#paritytype-enum
    /// </summary>
    public enum ParityType
    {
        /// <summary>
        /// No parity bit is used. This is the default value.
        /// </summary>
        [JsonPropertyName("none")]
        None,
        /// <summary>
        /// An even parity bit is used. This means that the number of 1 bits in the data must be even.
        /// </summary>
        [JsonPropertyName("even")]
        Even,
        /// <summary>
        /// An odd parity bit is used. This means that the number of 1 bits in the data must be odd.
        /// </summary>
        [JsonPropertyName("odd")]
        Odd,
    }
}
