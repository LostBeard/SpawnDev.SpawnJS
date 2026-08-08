// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using System.Text.Json.Serialization;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/KeyboardEvent#options
    /// </summary>
    public class KeyboardEventOptions : EventOptions
    {
        /// <summary>
        /// A string, defaulting to "", that sets the value of KeyboardEvent.key.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Key { get; set; }
        /// <summary>
        /// A string, defaulting to "", that sets the value of KeyboardEvent.code.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Code { get; set; }
        /// <summary>
        /// A number, defaulting to 0, that sets the value of KeyboardEvent.location.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Location { get; set; }
        /// <summary>
        /// A boolean value, defaulting to false, that sets the value of KeyboardEvent.repeat.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Repeat { get; set; }
        /// <summary>
        /// A boolean value, defaulting to false, that sets the value of KeyboardEvent.isComposing.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsComposing { get; set; }
        /// <summary>
        /// A number, defaulting to 0, that sets the value of the deprecated KeyboardEvent.charCode.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CharCode { get; set; }
        /// <summary>
        /// A number, defaulting to 0, that sets the value of the deprecated KeyboardEvent.keyCode.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? KeyCode { get; set; }
        /// <summary>
        /// A number, defaulting to 0, that sets the value of the deprecated UIEvent.which.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Which { get; set; }
        /// <summary>
        /// A boolean value, defaulting to false, that sets the value of KeyboardEvent.ctrlKey.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CtrlKey { get; set; }
        /// <summary>
        /// A boolean value, defaulting to false, that sets the value of KeyboardEvent.shiftKey.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ShiftKey { get; set; }
        /// <summary>
        /// A boolean value, defaulting to false, that sets the value of KeyboardEvent.altKey.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AltKey { get; set; }
        /// <summary>
        /// A boolean value, defaulting to false, that sets the value of KeyboardEvent.metaKey.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? MetaKey { get; set; }
    }
}
