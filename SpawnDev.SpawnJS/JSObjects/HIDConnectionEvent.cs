// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The HIDConnectionEvent interface of the WebHID API represents HID connection events, and is the event type passed to connect and disconnect event handlers when an input report is received.
    /// </summary>
    public class HIDConnectionEvent : Event
    {
        /// <summary>
        /// Creates a new instance of <see cref="HIDConnectionEvent"/>.
        /// </summary>
        /// <param name="_ref"></param>
        public HIDConnectionEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns the HIDDevice instance representing the device associated with the connection event.
        /// </summary>
        public HIDDevice Device => JSRef!.Get<HIDDevice>("device");
    }
}
