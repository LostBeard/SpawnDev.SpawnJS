
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ContentVisibilityAutoStateChangeEvent interface is the event object for the contentvisibilityautostatechange event, which fires on any element with content-visibility: auto set on it when it starts or stops being relevant to the user and skipping its contents.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ContentVisibilityAutoStateChangeEvent
    /// </summary>
    public class ContentVisibilityAutoStateChangeEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public ContentVisibilityAutoStateChangeEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public ContentVisibilityAutoStateChangeEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(ContentVisibilityAutoStateChangeEvent), type) : JS.New(nameof(ContentVisibilityAutoStateChangeEvent), type, options)) { }
        /// <summary>
        /// Returns true if the user agent is skipping the element's rendering, or false otherwise.
        /// </summary>
        public bool Skipped => JSRef!.Get<bool>("skipped");
    }
}
