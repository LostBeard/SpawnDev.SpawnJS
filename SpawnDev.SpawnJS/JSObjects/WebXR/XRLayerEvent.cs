
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The XRLayerEvent interface of the WebXR Device API is the event type for events related to a change of state of an XRLayer object. These events occur, for example, when the layer needs to be redrawn.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRLayerEvent
    /// </summary>
    public class XRLayerEvent : Event
    {
        /// <inheritdoc/>
        public XRLayerEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The XRLayer which generated the event.
        /// </summary>
        public XRLayer Layer => JSRef!.Get<XRLayer>("layer");
    }
}
