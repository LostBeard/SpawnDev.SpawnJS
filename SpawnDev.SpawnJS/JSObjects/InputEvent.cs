// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    // https://developer.mozilla.org/en-US/docs/Web/API/InputEvent
    // TODO - finish
    /// <summary>
    /// The InputEvent interface represents an event notifying the user to input text.
    /// </summary>
    public class InputEvent : UIEvent
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public InputEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public InputEvent(string type, InputEventOptions? options = null) : base(options == null ? JS.New(nameof(InputEvent), type) : JS.New(nameof(InputEvent), type, options)) { }

    }
}