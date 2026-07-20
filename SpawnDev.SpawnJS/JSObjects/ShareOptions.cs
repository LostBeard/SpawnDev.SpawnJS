// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ShareOptions dictionary represents the options for a navigator.share() call.
    /// </summary>
    public class ShareOptions
    {
        /// <summary>
        ///  A string representing a URL to be shared.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Url { get; set; }
        /// <summary>
        /// A string representing text to be shared.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Text { get; set; }
        /// <summary>
        ///  A string representing the title to be shared.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Title { get; set; }
        /// <summary>
        /// An array of File objects representing files to be shared.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public File[]? Files { get; set; }
    }
}
