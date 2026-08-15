
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ErrorEvent interface represents events providing information related to errors in scripts or in files.
    /// </summary>
    public class ErrorEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public ErrorEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public ErrorEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(ErrorEvent), type) : JS.New(nameof(ErrorEvent), type, options)) { }
        /// <summary>
        /// A string containing a human-readable error message describing the problem. Lacking a crossorigin setting reduces error logging.
        /// </summary>
        public string Message => JSRef!.Get<string>("message");
        /// <summary>
        /// A string containing the name of the script file in which the error occurred.
        /// </summary>
        public string Filename => JSRef!.Get<string>("filename");
        /// <summary>
        /// An integer containing the line number of the script file on which the error occurred.
        /// </summary>
        public int LineNO => JSRef!.Get<int>("lineno");
        /// <summary>
        /// An integer containing the column number of the script file on which the error occurred.
        /// </summary>
        public int ColNO => JSRef!.Get<int>("colno");
        /// <summary>
        /// A JavaScript Object that is concerned by the event.
        /// </summary>
        public SpawnJSObject? Error => JSRef!.Get<SpawnJSObject?>("error");
    }
}
