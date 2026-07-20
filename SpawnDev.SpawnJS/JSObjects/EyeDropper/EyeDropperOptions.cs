// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The EyeDropperOptions dictionary of the EyeDropper API provides options for the EyeDropper.open() method.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/EyeDropper/open
    /// </summary>
    public class EyeDropperOptions
    {
        /// <summary>
        /// An AbortSignal. The open() method will be aborted when the signal's AbortController.abort() method is called.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("signal")]
        public AbortSignal? Signal { get; set; }
    }
}
