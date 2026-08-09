
using System.Text.Json.Serialization;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/InputEvent/InputEvent#options
    /// </summary>
    public class InputEventOptions : EventOptions
    {
        /// <summary>
        /// A string specifying the type of change for editable content such as, for example, inserting, deleting, or formatting text.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InputType { get; set; }
        /// <summary>
        /// A string containing characters to insert. This may be an empty string if the change doesn't insert text (such as when deleting characters, for example).
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Data { get; set; }
        /// <summary>
        /// A boolean indicating that the event is part of a composition session, meaning it is after a compositionstart event but before a compositionend event. The default is false.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsComposing { get; set; }
    }
}
