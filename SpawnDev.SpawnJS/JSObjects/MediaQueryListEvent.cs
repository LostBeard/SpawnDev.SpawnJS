
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The MediaQueryListEvent object stores information on the changes that have occurred to a MediaQueryList object.
    /// </summary>
    public class MediaQueryListEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public MediaQueryListEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public MediaQueryListEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(MediaQueryListEvent), type) : JS.New(nameof(MediaQueryListEvent), type, options)) { }
        /// <summary>
        /// A boolean value that returns true if the document currently matches the media query list, or false if not.
        /// </summary>
        public bool Matches => JSRef!.Get<bool>("matches");
        /// <summary>
        /// A string representing the serialized media query.
        /// </summary>
        public string Media => JSRef!.Get<string>("media");
    }
}
