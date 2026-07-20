// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The IDBCursorWithValue interface of the IndexedDB API represents a cursor for traversing or iterating over multiple records in a database. It is the same as the IDBCursor, except that it includes the value property.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/IDBCursorWithValue
    /// </summary>
    public class IDBCursorWithValue<TKey, TPrimaryKey, TValue> : IDBCursor<TKey, TPrimaryKey, TValue>
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public IDBCursorWithValue(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns the value of the current cursor.
        /// </summary>
        public TValue Value => JSRef!.Get<TValue>("value");
        /// <summary>
        /// Returns the value of the current cursor.
        /// </summary>
        public TValueAlt ValueAs<TValueAlt>() => JSRef!.Get<TValueAlt>("value");
    }
}
