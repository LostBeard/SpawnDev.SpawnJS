
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/CookieChangeEvent/changed
    /// </summary>
    public class CookieChangeEvent : Event
    {
        /// <inheritdoc/>
        public CookieChangeEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public CookieChangeEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(CookieChangeEvent), type) : JS.New(nameof(CookieChangeEvent), type, options)) { }
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
