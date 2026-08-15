
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ClipboardEvent interface of the Clipboard API represents events providing information related to modification of the clipboard, that is cut, copy, and paste events.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ClipboardEvent
    /// </summary>
    public class ClipboardEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public ClipboardEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public ClipboardEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(ClipboardEvent), type) : JS.New(nameof(ClipboardEvent), type, options)) { }
        /// <summary>
        /// A DataTransfer object containing the data affected by the user-initiated cut, copy, or paste operation, along with its MIME type.
        /// </summary>
        public DataTransfer ClipboardData => JSRef!.Get<DataTransfer>("clipboardData");
    }
}
