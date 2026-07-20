// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ColorSelectionResult dictionary of the EyeDropper API provides the result of an eye dropper selection.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ColorSelectionResult
    /// </summary>
    public class ColorSelectionResult
    {
        /// <summary>
        /// A string representing the selected color, in hexadecimal sRGB format (#RRGGBB).
        /// </summary>
        [JsonPropertyName("sRGBHex")]
        public string SRGBHex { get; set; } = "";
    }
}
