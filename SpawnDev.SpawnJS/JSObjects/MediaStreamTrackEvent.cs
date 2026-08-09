
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The MediaStreamTrackEvent interface of the Media Capture and Streams API represents events which indicate that a MediaStream has had tracks added to or removed from the stream through calls to Media Capture and Streams API methods. These events are sent to the stream when these changes occur.
    /// </summary>
    public class MediaStreamTrackEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public MediaStreamTrackEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public MediaStreamTrackEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(MediaStreamTrackEvent), type) : JS.New(nameof(MediaStreamTrackEvent), type, options)) { }
        /// <summary>
        /// Returns a MediaStreamTrack object representing the track associated with the event.
        /// </summary>
        public MediaStreamTrack Track => JSRef!.Get<MediaStreamTrack>("track");
    }
}
