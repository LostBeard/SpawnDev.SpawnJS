using SpawnDev.SpawnJS.Extensions;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Holds a Javascript object automatically managing the JSObejct lifetime.
    /// </summary>
    public sealed partial class SpawnJSHandle : IDisposable
    {
        /// <summary>
        /// Explicit converion with JSObject
        /// </summary>
        public static explicit operator JSObject(SpawnJSHandle value) => value.JSObject;
        /// <summary>
        /// Explicit converion with JSObject
        /// </summary>
        public static explicit operator SpawnJSHandle(JSObject value) => value == null ? null! : new SpawnJSHandle(value);
        /// <summary>
        /// All undisposed JSObjectHandles
        /// </summary>
        static List<SpawnJSHandle> Handles { get; } = new List<SpawnJSHandle>();
        /// <summary>
        /// JSObject. Do NOT dispose this JSObject. Dispose this JSObjectHandle when no longer needed.
        /// </summary>
        public JSObject JSObject
        {
            get
            {
                if (!IsDisposed && _jsObject.IsDisposed)
                {
                    // Pull a fresh proxy instance pointing to the same JS object out of our JS array
                    _jsObject = Reflect.GetJSObject(_store, 0)!;
                    Ptr = _jsObject.GetId();
                }
                return _jsObject;
            }
        }
        /// <summary>
        /// Returns true if disposed
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// The current JSObject that poitns to the Javscript object this handle is attached to
        /// </summary>
        private JSObject _jsObject;
        /// <summary>
        /// The Javascript Array we use to store our JSObject for retrieval when needed
        /// </summary>
        private JSObject _store;
        /// <summary>
        /// The id of the last JSObject (can change)
        /// </summary>
        internal long Ptr { get; private set; }
        /// <summary>
        /// New instance
        /// </summary>
        /// <param name="jsObject"></param>
        public SpawnJSHandle(JSObject jsObject)
        {
            if (jsObject == null) throw new NullReferenceException(nameof(jsObject));
            if (jsObject.IsDisposed) throw new ObjectDisposedException(nameof(jsObject));
            _jsObject = jsObject;
            Ptr = _jsObject.GetId();
            // store the JSObject in a JS array we own so that if the JSObject gets Dispsoed elsewhere we can get a fresh JSObject
            _store = JSHost.GlobalThis.InvokePropertyConstructor("Array")!;
            Reflect.Set(_store, 0, _jsObject);
            Handles.Add(this);
        }
        /// <summary>
        /// Dispose resources
        /// </summary>
        /// <param name="disposing"></param>
        void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            IsDisposed = true;
            var currentPTr = Ptr;
            Ptr = 0;
            Handles.Remove(this);
            _store.Dispose();
            if (!_jsObject.IsDisposed)
            {
                // we don't want to dispose handles others may be using so check how many handles are still alive and only dispose if we are the last one
                var alive = Handles.Any(o => o.Ptr == currentPTr);
                if (!alive)
                {
                    _jsObject.Dispose();
                }
            }
        }
        /// <summary>
        /// Clone the handle
        /// </summary>
        /// <returns></returns>
        public SpawnJSHandle Clone() => !IsDisposed ? new SpawnJSHandle(JSObject) : throw new ObjectDisposedException(nameof(SpawnJSHandle));
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
        ~SpawnJSHandle()
        {
            Dispose(false);
        }
    }
}
