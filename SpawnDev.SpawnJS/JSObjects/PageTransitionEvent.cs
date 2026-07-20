// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The PageTransitionEvent event object is available inside handler functions for the pageshow and pagehide events, fired when a document is being loaded or unloaded.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/PageTransitionEvent
    /// </summary>
    public class PageTransitionEvent : Event
    {
        ///<inheritdoc/>
        public PageTransitionEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Indicates if the document is loading from a cache.
        /// </summary>
        public bool Persisted => JSRef!.Get<bool>("persisted");
    }
}
