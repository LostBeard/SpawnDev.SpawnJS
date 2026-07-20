// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Power preference options for GPUAdapter request.
    /// </summary>
    public enum GPUPowerPreference
    {
        /// <summary>
        /// Low power usage is preferred.
        /// </summary>
        [JsonPropertyName("low-power")]
        LowPower,
        /// <summary>
        /// High performance is preferred.
        /// </summary>
        [JsonPropertyName("high-performance")]
        HighPerformance,
    }
}
