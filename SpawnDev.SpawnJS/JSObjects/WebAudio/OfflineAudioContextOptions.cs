// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Options for the OfflineAudioContext constructor.
    /// </summary>
    public class OfflineAudioContextOptions
    {
        /// <summary>
        /// An integer specifying the number of channels for this context.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? NumberOfChannels { get; set; }

        /// <summary>
        /// An integer specifying the length of the buffer to be created, in sample-frames.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Length { get; set; }

        /// <summary>
        /// A floating-point number specifying the sample-rate, in samples per second, of the audio data.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? SampleRate { get; set; }
    }
}
