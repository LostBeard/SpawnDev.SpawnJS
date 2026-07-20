// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Result of AudioDecoder.isConfigSupported
    /// </summary>
    public class AudioDecoderSupport
    {
        /// <summary>
        /// If the configuration is supported.
        /// </summary>
        [JsonPropertyName("supported")]
        public bool Supported { get; set; }

        /// <summary>
        /// The configuration that is supported.
        /// </summary>
        [JsonPropertyName("config")]
        public AudioDecoderConfig Config { get; set; }
    }
}
