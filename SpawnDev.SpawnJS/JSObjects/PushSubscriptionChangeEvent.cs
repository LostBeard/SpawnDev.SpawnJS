// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// (MDN does not know about this event)
    /// https://www.w3.org/TR/push-api/#dom-pushsubscriptionchangeevent
    /// </summary>
    public class PushSubscriptionChangeEvent : ExtendableEvent
    {
        ///<inheritdoc/>
        public PushSubscriptionChangeEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The newSubscription attribute, when getting, returns the value it was initialized to.
        /// </summary>
        public PushSubscription? NewSubscription => JSRef!.Get<PushSubscription?>("newSubscription");
        /// <summary>
        /// The oldSubscription attribute, when getting, returns the value it was initialized to.
        /// </summary>
        public PushSubscription? OldSubscription => JSRef!.Get<PushSubscription?>("oldSubscription");
    }
}
