using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Returned from an Iterator or AsyncIterator Next call
    /// </summary>
    public class IteratorResult : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public IteratorResult(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// A boolean that's false if the iterator was able to produce the next value in the sequence. (This is equivalent to not specifying the done property altogether.)<br/>
        /// Has the value true if the iterator has completed its sequence. In this case, value optionally specifies the return value of the iterator.
        /// </summary>
        public bool Done => JSRef!.Get<bool>("done");
        /// <summary>
        /// Any JavaScript value returned by the iterator. Can be omitted when done is true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetValue<T>() => JSRef!.Get<T>("value");
    }
    /// <summary>
    /// Returned from an Iterator or AsyncIterator Next call
    /// </summary>
    public class IteratorResult<TValue> : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public IteratorResult(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// A boolean that's false if the iterator was able to produce the next value in the sequence. (This is equivalent to not specifying the done property altogether.)<br/>
        /// Has the value true if the iterator has completed its sequence. In this case, value optionally specifies the return value of the iterator.
        /// </summary>
        public bool Done => JSRef!.Get<bool>("done");
        /// <summary>
        /// Any JavaScript value returned by the iterator. Can be omitted when done is true.
        /// </summary>
        public TValue Value => JSRef!.Get<TValue>("value");
    }
}
