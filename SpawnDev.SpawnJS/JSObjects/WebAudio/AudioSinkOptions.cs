// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// An object representing different options for a sink ID. Currently this takes a single property, type, with a value of "none". Setting this parameter causes the audio to be processed without being played through any audio output device. This is a useful option to minimize power consumption when you don't need playback along with processing.
    /// </summary>
    public class AudioSinkOptions
    {
        /// <summary>
        /// Currently only supports being set to none
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; set; }
    }
}
