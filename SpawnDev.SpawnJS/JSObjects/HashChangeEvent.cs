
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The HashChangeEvent interface represents events that fire when the fragment identifier of the URL has changed.
    /// </summary>
    public class HashChangeEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public HashChangeEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public HashChangeEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(HashChangeEvent), type) : JS.New(nameof(HashChangeEvent), type, options)) { }
        /// <summary>
        /// The new URL to which the window is navigating.
        /// </summary>
        public string NewURL => JSRef!.Get<string>("newURL");
        /// <summary>
        /// The previous URL from which the window is navigating.
        /// </summary>
        public string OldURL => JSRef!.Get<string>("oldURL");
    }
}
