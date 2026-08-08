// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The IDBVersionChangeEvent interface of the IndexedDB API indicates that the version of the database has changed, as the result of an onupgradeneeded event handler function.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/IDBVersionChangeEvent
    /// </summary>
    public class IDBVersionChangeEvent : Event<IDBOpenDBRequest>
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public IDBVersionChangeEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public IDBVersionChangeEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(IDBVersionChangeEvent), type) : JS.New(nameof(IDBVersionChangeEvent), type, options)) { }
        /// <summary>
        /// The oldVersion read-only property of the IDBVersionChangeEvent interface returns the old version number of the database.<br/>
        /// A number containing a 64-bit integer.
        /// </summary>
        public long OldVersion => JSRef!.Get<long>("oldVersion");
        /// <summary>
        /// The newVersion read-only property of the IDBVersionChangeEvent interface returns the new version number of the database.<br/>
        /// A number that is a 64-bit integer or null if the database is being deleted.
        /// </summary>
        public long? NewVersion => JSRef!.Get<long?>("newVersion");
    }
}
