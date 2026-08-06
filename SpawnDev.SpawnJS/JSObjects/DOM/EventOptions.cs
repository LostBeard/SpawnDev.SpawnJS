// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using System.Text.Json.Serialization;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Event constructor options
    /// https://developer.mozilla.org/en-US/docs/Web/API/Event/Event#options
    /// </summary>
    public class EventOptions
    {
        /// <summary>
        /// A boolean value indicating whether the event bubbles. The default is false.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Bubbles { get; set; }
        /// <summary>
        /// A boolean value indicating whether the event can be cancelled. The default is false.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Cancelable { get; set; }
        /// <summary>
        /// A boolean value indicating whether the event will trigger listeners outside of a shadow root (see Event.composed for more details). The default is false.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Composed { get; set; }
    }
}
