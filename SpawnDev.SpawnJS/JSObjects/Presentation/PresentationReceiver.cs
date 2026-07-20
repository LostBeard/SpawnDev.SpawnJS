// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The PresentationReceiver interface of the Presentation API provides a way for a receiving browsing context to access the controlling browsing contexts and communicate with them.
    /// </summary>
    public class PresentationReceiver : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public PresentationReceiver(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Returns a Promise that resolves with a PresentationConnectionList object containing a list of incoming presentation connections.
        /// </summary>
        public Task<PresentationConnectionList> ConnectionList => JSRef!.GetAsync<PresentationConnectionList>("connectionList");
    }
}
