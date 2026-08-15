
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The HIDConnectionEvent interface of the WebHID API represents HID connection events, and is the event type passed to connect and disconnect event handlers when an input report is received.
    /// </summary>
    public class HIDConnectionEvent : Event
    {
        /// <summary>
        /// Creates a new instance of <see cref="HIDConnectionEvent"/>.
        /// </summary>
        /// <param name="_ref"></param>
        public HIDConnectionEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public HIDConnectionEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(HIDConnectionEvent), type) : JS.New(nameof(HIDConnectionEvent), type, options)) { }
        /// <summary>
        /// Returns the HIDDevice instance representing the device associated with the connection event.
        /// </summary>
        public HIDDevice Device => JSRef!.Get<HIDDevice>("device");
    }
}
