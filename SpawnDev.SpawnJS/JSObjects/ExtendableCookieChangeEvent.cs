// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The cookiechange event of the ServiceWorkerGlobalScope interface is fired when a cookie change occurs that matches the service worker's cookie change subscription list.<br/>
    /// This event is not cancelable and does not bubble.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ExtendableCookieChangeEvent
    /// </summary>
    public class ExtendableCookieChangeEvent : ExtendableEvent
    {
        /// <inheritdoc/>
        public ExtendableCookieChangeEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns an array containing the changed cookies.
        /// </summary>
        public Cookie[] Changed => JSRef!.Get<Cookie[]>("changed");
        /// <summary>
        /// Returns an array containing the deleted cookies.
        /// </summary>
        public Cookie[] Deleted => JSRef!.Get<Cookie[]>("deleted");
    }
}
