// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The USBConnectionEvent interface of the WebUSB API is the event type passed to USB connect and disconnect events when the user agent detects that a new USB device has been connected or disconnected.
    /// </summary>
    public class USBConnectionEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public USBConnectionEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns a USBDevice object representing the current device.
        /// </summary>
        public USBDevice Device => JSRef!.Get<USBDevice>("device");
    }
}
