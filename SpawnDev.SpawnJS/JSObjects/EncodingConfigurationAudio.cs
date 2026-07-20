// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Configuration object for an audio media source. 
    /// </summary>
    public class EncodingConfigurationAudio
    {
        /// <summary>
        /// String containing a valid audio MIME type, and (optionally) a codecs parameter.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ContentType { get; set; }
        /// <summary>
        /// The number of channels used by the audio track
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Channels { get; set; }
        /// <summary>
        /// The number of bits used to encode one second of the audio file.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? Bitrate { get; set; }
        /// <summary>
        /// The number of audio samples making up one second of the audio file.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? Samplerate { get; set; }
    }
}
