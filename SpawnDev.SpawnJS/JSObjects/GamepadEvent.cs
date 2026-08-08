// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The GamepadEvent interface of the Gamepad API contains references to gamepads connected to the system, which is what the gamepad events gamepadconnected and gamepaddisconnected are fired in response to.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/GamepadEvent
    /// </summary>
    public class GamepadEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public GamepadEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public GamepadEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(GamepadEvent), type) : JS.New(nameof(GamepadEvent), type, options)) { }
        /// <summary>
        /// Returns a Gamepad object, providing access to the associated gamepad data for the event fired.
        /// </summary>
        public Gamepad Gamepad => JSRef!.Get<Gamepad>("gamepad");
    }
}
