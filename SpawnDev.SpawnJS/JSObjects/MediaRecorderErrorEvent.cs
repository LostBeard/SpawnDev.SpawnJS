
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The MediaRecorderErrorEvent interface represents errors which occur during recording.
    /// </summary>
    public class MediaRecorderErrorEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public MediaRecorderErrorEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public MediaRecorderErrorEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(MediaRecorderErrorEvent), type) : JS.New(nameof(MediaRecorderErrorEvent), type, options)) { }
    }
}
