namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// An extendable class that represents a JS object
    /// </summary>
    public class SpawnJSObject : IDisposable
    {
        /// <summary>
        /// Returns true if SpawnJSObjectReference is null or disposed
        /// </summary>
        public bool IsWrapperDisposed => JSRef?.IsDisposed != false;
        /// <summary>
        /// SpawnJSRuntime
        /// </summary>
        protected static SpawnJSRuntime JS => SpawnJSRuntime.Instance ?? throw new InvalidOperationException("SpawnJSRuntime has not been created.");
        /// <summary>
        /// The underlying SpawnJSObjectReference
        /// </summary>
        public SpawnJSObjectReference? JSRef { get; private set; }
        /// <summary>
        /// Create a new instance of SpawnJSObject
        /// </summary>
        /// <param name="jsRef"></param>
        public SpawnJSObject(SpawnJSObjectReference jsRef)
        {
            JSRef = jsRef;
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            JSRef?.Dispose();
        }
    }
}
