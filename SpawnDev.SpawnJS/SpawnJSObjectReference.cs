using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// SpawnJSObject wraps a JSObject and enables interop using SpawnJSRuntime Marshallers
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform("browser")]
    public partial class SpawnJSObjectReference : IDisposable
    {
        /// <summary>
        /// Explciit conversion with JSObject
        /// </summary>
        public static explicit operator JSObject(SpawnJSObjectReference value) => value == null ? null! : value.JSObject;
        /// <summary>
        /// Explciit conversion with JSObject
        /// </summary>
        public static explicit operator SpawnJSObjectReference(JSObject value) => value == null ? null! : new SpawnJSObjectReference(value);
        /// <summary>
        /// SpawnJSRuntime
        /// </summary>
        public SpawnJSRuntime JS => SpawnJSRuntime.Instance ?? throw new InvalidOperationException("SpawnJSRuntime has not been created.");
        /// <summary>
        /// Returns true if JSObject is null or disposed
        /// </summary>
        public bool IsDisposed => JSHandle.IsDisposed != false;
        /// <summary>
        /// If true, the JSObject will be disposed when this is disposed.<br/>
        /// Note: SpawnJSObjectReference auto-disposes via a finalizer.
        /// </summary>
        public bool OwnsJSObject { get; set; } = true;
        /// <summary>
        /// JSObjectHandle holds the JSObject with auto-reacquistion
        /// </summary>
        public SpawnJSHandle JSHandle { get; private set; }
        /// <summary>
        /// JSObject that points to the JAvascript data
        /// </summary>
        public JSObject JSObject => JSHandle.JSObjectRequired;
        /// <summary>
        /// Create a new instance of SpawnJSObject to wrap a JSObject
        /// </summary>
        /// <param name="jsObject"></param>
        public SpawnJSObjectReference(JSObject jsObject)
        {
            JSHandle = new SpawnJSHandle(jsObject);
        }
        /// <summary>
        /// Create a new instance around a handle that already holds the Javascript value, TAKING
        /// OWNERSHIP of it - the handle is disposed with this reference.<br/>
        /// This is the path a read takes now. Constructing from a <see cref="JSObject"/> forces the value
        /// to be resolved to a runtime proxy first, which is the most expensive operation in the library;
        /// a handle can address the value through the slot table instead and never make one.
        /// </summary>
        /// <param name="jsHandle">The handle to take ownership of</param>
        public SpawnJSObjectReference(SpawnJSHandle jsHandle)
        {
            JSHandle = jsHandle ?? throw new ArgumentNullException(nameof(jsHandle));
        }
        /// <summary>
        /// Dispose resources
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            if (OwnsJSObject) JSHandle?.Dispose();
        }
        /// <summary>
        /// Release the JSObject, and other resources
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Finalizer
        /// </summary>
        ~SpawnJSObjectReference()
        {
            Dispose(false);
        }
        /// <summary>
        /// Marshalls this to Javascript and then returns as type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks>
        /// The handle is passed, not <see cref="JSObject"/>. Passing the JSObject forced the value to be
        /// resolved to a runtime proxy purely to hand it back across a boundary it never had to cross;
        /// the handle marshaller assigns slot to slot instead. Every JSRefAs, JSRefCopy and JSRefMove
        /// funnels through here.
        /// </remarks>
        public T As<T>() => JS.NetRun<T>("returnMe", new object[] { JSHandle });
    }
}
