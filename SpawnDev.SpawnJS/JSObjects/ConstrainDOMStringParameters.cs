// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/mediacapture-streams/#dom-constraindomstringparameters
    /// </summary>
    public class ConstrainDOMStringParameters
    {
        /// <summary>
        /// A string specifying a specific, required, value the property must have to be considered acceptable.
        /// </summary>
        [JsonPropertyName("exact")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Exact { get; set; }
        /// <summary>
        /// A string specifying an ideal value for the property. If possible, this value will be used, but if it's not possible, the user agent will use the closest possible match.
        /// </summary>
        [JsonPropertyName("ideal")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Ideal { get; set; }
    }
}
