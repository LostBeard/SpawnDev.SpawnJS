// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Custom queueing strategy
    /// </summary>
    public class CustomQueuingStrategy : QueueingStrategy
    {
        FuncCallback<SpawnJSObject, int>? _sizeCallback = null;
        ///<inheritdoc/>
        public CustomQueuingStrategy(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The ByteLengthQueuingStrategy() constructor creates and returns a ByteLengthQueuingStrategy object instance.
        /// </summary>
        public CustomQueuingStrategy(int highWaterMark, Func<SpawnJSObject, int> size) : base(JS.New("Object"))
        {
            JSRef!.Set("highWaterMark", highWaterMark);
            _sizeCallback = new FuncCallback<SpawnJSObject, int>(size);
            JSRef!.Set("size", _sizeCallback);
        }
        /// <summary>
        /// A non-negative integer. This defines the total number of chunks that can be contained in the internal queue before backpressure is applied.
        /// </summary>
        public override int HighWaterMark => JSRef!.Get<int>("highWaterMark");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        public override int Size(object chunk) => JSRef!.Call<int>("size", chunk);
        ///<inheritdoc/>
        override protected void Dispose(bool disposing)
        {
            _sizeCallback?.Dispose();
            _sizeCallback = null;
            base.Dispose(disposing);
        }
    }
}
