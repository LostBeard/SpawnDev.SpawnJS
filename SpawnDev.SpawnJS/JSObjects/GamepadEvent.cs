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
        /// Returns a Gamepad object, providing access to the associated gamepad data for the event fired.
        /// </summary>
        public Gamepad Gamepad => JSRef!.Get<Gamepad>("gamepad");
    }
}
