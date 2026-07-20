// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The PresentationConnectionAvailableEvent interface of the Presentation API is fired on a PresentationRequest when a connection is available.
    /// </summary>
    public class PresentationConnectionAvailableEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public PresentationConnectionAvailableEvent(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Returns the PresentationConnection instance that is now available.
        /// </summary>
        public PresentationConnection Connection => JSRef!.Get<PresentationConnection>("connection");
    }
}
