// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The PresentationConnectionCloseEvent interface of the Presentation API is fired on a PresentationConnection when it is closed.
    /// </summary>
    public class PresentationConnectionCloseEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public PresentationConnectionCloseEvent(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Returns a human-readable message that explains why the connection closed.
        /// </summary>
        public string Message => JSRef!.Get<string>("message");

        /// <summary>
        /// Returns the reason why the connection closed.
        /// </summary>
        public string Reason => JSRef!.Get<string>("reason");
    }
}
