// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The contentdelete event of the ServiceWorkerGlobalScope interface is fired when an item is removed from the indexed content via the user agent.<br/>
    /// This event is not cancelable and does not bubble.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ServiceWorkerGlobalScope/contentdelete_event
    /// </summary>
    public class ContentIndexEvent : ExtendableEvent
    {
        /// <inheritdoc/>
        public ContentIndexEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// A string which identifies the deleted content index via it's id.
        /// </summary>
        public string Id => JSRef!.Get<string>("id");
    }
}
