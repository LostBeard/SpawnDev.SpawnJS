using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A generic queueing strategy
    /// </summary>
    public class QueueingStrategy : SpawnJSObject
    {
        ///<inheritdoc/>
        public QueueingStrategy(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// A non-negative integer. This defines the total number of chunks that can be contained in the internal queue before backpressure is applied.
        /// </summary>
        public virtual int HighWaterMark => JSRef!.Get<int>("highWaterMark");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        public virtual int Size(object chunk) => JSRef!.Call<int>("size", chunk);
    }
}
