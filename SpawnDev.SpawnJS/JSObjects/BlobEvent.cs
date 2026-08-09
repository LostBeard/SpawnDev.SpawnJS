
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The BlobEvent interface represents events associated with a Blob. These blobs are typically, but not necessarily, associated with media content.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/BlobEvent
    /// </summary>
    public class BlobEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public BlobEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public BlobEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(BlobEvent), type) : JS.New(nameof(BlobEvent), type, options)) { }
        /// <summary>
        /// A Blob representing the data associated with the event. The event was fired on the EventTarget because of something happening on that specific Blob.
        /// </summary>
        public Blob Data => JSRef!.Get<Blob>("data");
        /// <summary>
        /// A DOMHighResTimeStamp indicating the difference between the timestamp of the first chunk in data and the timestamp of the first chunk in the first BlobEvent produced by this recorder. Note that the timecode in the first produced BlobEvent does not need to be zero.
        /// </summary>
        /// <returns></returns>
        public double Timecode => JSRef!.Get<double>("timecode");
    }
}
