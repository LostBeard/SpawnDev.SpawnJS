// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The RTCPeerConnectionEvent interface represents an event that is fired when an RTCPeerConnection receives an ICE candidate.
    /// </summary>
    public class RTCPeerConnectionEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public RTCPeerConnectionEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The RTCIceCandidate that was received.
        /// </summary>
        public RTCIceCandidate Candidate => JSRef!.Get<RTCIceCandidate>("candidate");
    }
}
