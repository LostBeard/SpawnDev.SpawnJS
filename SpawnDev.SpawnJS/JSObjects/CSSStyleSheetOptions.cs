using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// An object specifying options for the new stylesheet.
    /// </summary>
    public class CSSStyleSheetOptions
    {
        /// <summary>
        /// A string containing the baseURL used to resolve relative URLs in the stylesheet.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BaseURL { get; set; }
        /// <summary>
        /// A MediaList containing a list of media rules, or a string containing a single rule.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MediaList? Media { get; set; }
        /// <summary>
        /// A Boolean indicating whether the stylesheet is disabled. False by default.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Disabled { get; set; }
    }
}
