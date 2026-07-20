// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The BackgroundFetchEvent interface of the Background Fetch API is the event type for background fetch events dispatched on the service worker global scope.<br/>
    /// It is the event type passed to backgroundfetchclick event and backgroundfetchabort event.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/BackgroundFetchEvent
    /// </summary>
    public class BackgroundFetchEvent : ExtendableEvent
    {
        /// <inheritdoc/>
        public BackgroundFetchEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns the BackgroundFetchRegistration that the event was initialized to.
        /// </summary>
        public BackgroundFetchRegistration Registration => JSRef!.Get<BackgroundFetchRegistration>("registration");
    }
}
