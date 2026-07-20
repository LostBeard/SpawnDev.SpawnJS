// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/BackgroundFetchUpdateUIEvent/updateUI#icons
    /// https://developer.mozilla.org/en-US/docs/Web/API/BackgroundFetchManager/fetch#icons
    /// </summary>
    public class IconInfo
    {
        /// <summary>
        /// A string representing a URL to the icon file.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Src { get; set; }
        /// <summary>
        /// A string representing the sizes of the image, expressed using the same syntax as the sizes attribute of the &lt;link> element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Sizes { get; set; }
        /// <summary>
        /// A string representing the MIME type of the icon.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; set; }
        /// <summary>
        /// A string representing the accessible name of the icon.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Label { get; set; }
    }
}
