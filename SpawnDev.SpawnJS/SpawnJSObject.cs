namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// An extendable class that represents a JS object.<br/>
    /// This is the SpawnJS equivalent of SpawnDev.BlazorJS's JSObject and carries the same member
    /// surface (JSRef, JSRefMove, JSRefCopy, JSRefAs, JSRefIs, JSEquals, IsWrapperDisposed) so wrapper
    /// types port across with no change to their bodies.
    /// </summary>
    public class SpawnJSObject : IDisposable
    {
        /// <summary>
        /// SpawnJSRuntime
        /// </summary>
        protected static SpawnJSRuntime JS => SpawnJSRuntime.Instance ?? throw new InvalidOperationException("SpawnJSRuntime has not been created.");
        /// <summary>
        /// The underlying SpawnJSObjectReference
        /// </summary>
        public SpawnJSObjectReference? JSRef { get; private set; }
        /// <summary>
        /// Returns true if this wrapper has been disposed
        /// </summary>
        public bool IsWrapperDisposed { get; private set; } = false;
        /// <summary>
        /// Create a new instance of SpawnJSObject
        /// </summary>
        public SpawnJSObject(SpawnJSObjectReference jsRef) => FromReference(jsRef);
        /// <summary>
        /// Called when JSRef is going to be set. base.FromReference(_ref) must be called.<br/>
        /// Override to run post deserialization initialization, such as attaching to events.
        /// </summary>
        protected virtual void FromReference(SpawnJSObjectReference _ref)
        {
            if (IsWrapperDisposed) throw new Exception("SpawnJSObject.FromReference error: object already disposed.");
            if (JSRef != null) throw new Exception("SpawnJSObject.FromReference error: _ref object already set.");
            JSRef = _ref;
        }
        /// <summary>
        /// Returns this object as type T and disposes this wrapper.<br/>
        /// If type T is a SpawnJSObject the JSRef is moved instead of copied.
        /// </summary>
        public T JSRefMove<T>()
        {
            if (IsWrapperDisposed) throw new ObjectDisposedException(nameof(JSRef));
            var _ref = JSRef;
            JSRef = null;
            T ret;
            if (typeof(SpawnJSObject).IsAssignableFrom(typeof(T)))
            {
                ret = (T)Activator.CreateInstance(typeof(T), _ref)!;
            }
            else
            {
                ret = _ref!.As<T>();
            }
            Dispose();
            return ret;
        }
        /// <summary>
        /// Returns this object's SpawnJSObjectReference and disposes this wrapper
        /// </summary>
        public SpawnJSObjectReference? JSRefMove()
        {
            if (IsWrapperDisposed) throw new ObjectDisposedException(nameof(JSRef));
            var _ref = JSRef;
            JSRef = null;
            Dispose();
            return _ref;
        }
        /// <summary>
        /// Returns this SpawnJSObject as type T
        /// </summary>
        public T JSRefAs<T>()
        {
            if (IsWrapperDisposed) throw new ObjectDisposedException(nameof(JSRef));
            return JSRef!.As<T>();
        }
        /// <summary>
        /// Returns this SpawnJSObject as type T. Synonym for JSRefAs&lt;T&gt;
        /// </summary>
        public T JSRefCopy<T>()
        {
            if (IsWrapperDisposed) throw new ObjectDisposedException(nameof(JSRef));
            return JSRef!.As<T>();
        }
        /// <summary>
        /// Returns a copy of this object's SpawnJSObjectReference
        /// </summary>
        public SpawnJSObjectReference JSRefCopy()
        {
            if (IsWrapperDisposed) throw new ObjectDisposedException(nameof(JSRef));
            return JSRef!.As<SpawnJSObjectReference>();
        }
        /// <summary>
        /// Returns true if the referenced Javascript object's constructor.name == constructorName
        /// </summary>
        public bool JSRefIs(string constructorName)
        {
            if (IsWrapperDisposed) throw new ObjectDisposedException(nameof(JSRef));
            return JSRef?.ConstructorName() == constructorName;
        }
        /// <summary>
        /// Returns true if the referenced Javascript object's constructor.name == typeof(T).Name
        /// </summary>
        public bool JSRefIs<T>() => JSRefIs(typeof(T).Name);
        /// <summary>
        /// If this object's constructor.name == constructorName, sets value to this object as type T and returns true
        /// </summary>
        /// <param name="constructorName">The constructor.name to test for</param>
        /// <param name="value">Set if the constructor.name matches</param>
        /// <param name="moveJSRef">If true, moves the JSRef (disposing this wrapper) instead of copying it</param>
        public bool JSRefIs<T>(string constructorName, out T value, bool moveJSRef = false)
        {
            if (IsWrapperDisposed) throw new ObjectDisposedException(nameof(JSRef));
            if (JSRef?.ConstructorName() != constructorName)
            {
                value = default!;
                return false;
            }
            value = moveJSRef ? JSRefMove<T>() : JSRefCopy<T>();
            return true;
        }
        /// <summary>
        /// If this object's constructor.name == typeof(T).Name, sets value to this object as type T and returns true
        /// </summary>
        public bool JSRefIs<T>(out T value, bool moveJSRef = false) => JSRefIs(typeof(T).Name, out value, moveJSRef);
        /// <summary>
        /// Compare this SpawnJSObject to obj2 using Javascript equality.<br/>
        /// Returns full ? this === obj2 : this == obj2
        /// </summary>
        public bool JSEquals(object? obj2, bool full = false)
        {
            if (obj2 == null) return false;
            return JS.ObjectEquals(this, obj2, full);
        }
        /// <summary>
        /// Dispose resources
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (IsWrapperDisposed) return;
            IsWrapperDisposed = true;
            JSRef?.Dispose();
            JSRef = null;
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsWrapperDisposed) return;
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Finalizer
        /// </summary>
        ~SpawnJSObject()
        {
            Dispose(false);
        }
    }
}
