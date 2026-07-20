// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The RTCErrorEvent interface represents an error that occurred while using the WebRTC API.
    /// </summary>
    public class RTCErrorEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public RTCErrorEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The error that occurred.
        /// </summary>
        public RTCError Error => JSRef!.Get<RTCError>("error");
    }
}
