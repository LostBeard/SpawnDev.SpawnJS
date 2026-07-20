// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://html.spec.whatwg.org/multipage/canvas.html#predefinedcolorspace
    /// </summary>
    public enum PredefinedColorSpace
    {
        /// <summary>
        /// srgb
        /// </summary>
        [JsonPropertyName("srgb")]
        Srgb,
        /// <summary>
        /// display-p3
        /// </summary>
        [JsonPropertyName("display-p3")]
        DisplayP3,
    }
}
