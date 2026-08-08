// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The MediaEncryptedEvent interface of the Encrypted Media Extensions API contains the information associated with an encrypted event sent to a HTMLMediaElement when some initialization data is encountered in the media.
    /// </summary>
    public class MediaEncryptedEvent : Event
    {
        /// <summary>
        /// Creates a new instance of <see cref="MediaEncryptedEvent"/>.
        /// </summary>
        /// <param name="_ref"></param>
        public MediaEncryptedEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public MediaEncryptedEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(MediaEncryptedEvent), type) : JS.New(nameof(MediaEncryptedEvent), type, options)) { }
        /// <summary>
        /// Returns an ArrayBuffer containing the initialization data found. If there is no initialization data associated with the format, it returns null.
        /// </summary>
        public ArrayBuffer? InitData => JSRef!.Get<ArrayBuffer?>("initData");
        /// <summary>
        /// Returns a case-sensitive string with the type of the format of the initialization data found.
        /// </summary>
        public string InitDataType => JSRef!.Get<string>("initDataType");
    }
}
