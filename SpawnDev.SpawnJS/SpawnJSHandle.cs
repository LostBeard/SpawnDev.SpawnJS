using SpawnDev.SpawnJS.Extensions;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Holds a Javascript object automatically managing the JSObject lifetime.
    /// </summary>
    public sealed partial class SpawnJSHandle : IDisposable
    {
        /// <summary>
        /// SpawnJSRuntime
        /// </summary>
        public SpawnJSRuntime JS => SpawnJSRuntime.Instance ?? throw new InvalidOperationException("SpawnJSRuntime has not been created.");
        /// <summary>
        /// Explicit converion with JSObject
        /// </summary>
        public static explicit operator JSObject(SpawnJSHandle value) => value == null ? null! : value.JSObject!;
        /// <summary>
        /// Explicit converion with JSObject
        /// </summary>
        public static explicit operator SpawnJSHandle(JSObject value) => value == null ? null! : new SpawnJSHandle(value);
        /// <summary>
        /// Reference counts of every Javascript object currently held by a SpawnJSHandle, keyed on the
        /// JSObject proxy id (see Ptr). The underlying JSObject is only disposed when the last handle
        /// holding that proxy releases it, so disposing one handle never kills a sibling's.<br/>
        /// Add and release are O(1). This is hot path: every JS ➡️ .Net marshal creates and releases a
        /// volatile handle, so a scan per dispose would be quadratic in the number of live handles.<br/>
        /// Not synchronized on purpose. The .Net WASM runtime is single threaded and the GC finalizer
        /// runs on that same thread, so there is no concurrent access. If a threaded runtime lands, this
        /// is the one place that needs a lock.
        /// </summary>
        static readonly Dictionary<long, int> PtrRefCounts = new Dictionary<long, int>();
        /// <summary>
        /// The number of distinct Javascript objects currently held by SpawnJSHandles. Diagnostics only.
        /// </summary>
        public static int LiveObjectCount => PtrRefCounts.Count;
        /// <summary>
        /// Takes a reference on a JSObject proxy id
        /// </summary>
        static void PtrAddRef(long ptr)
        {
            if (ptr < 0) return;
            PtrRefCounts[ptr] = PtrRefCounts.TryGetValue(ptr, out var count) ? count + 1 : 1;
        }
        /// <summary>
        /// Releases a reference on a JSObject proxy id. Returns true if it was the last one.
        /// </summary>
        static bool PtrRelease(long ptr)
        {
            if (ptr < 0 || !PtrRefCounts.TryGetValue(ptr, out var count)) return false;
            if (count > 1)
            {
                PtrRefCounts[ptr] = count - 1;
                return false;
            }
            PtrRefCounts.Remove(ptr);
            return true;
        }
        /// <summary>
        /// JSObject. Do NOT dispose this JSObject. Dispose this JSObjectHandle when no longer needed.
        /// </summary>
        public JSObject? JSObject
        {
            get
            {
                UpdateHandleCheck();
                return _jsObject;
            }
        }
        /// <summary>
        /// The JSObject this handle points at, throwing if there is not one.<br/>
        /// Every Reflect entry point takes a non-nullable JSObject, so passing the nullable
        /// <see cref="JSObject"/> straight in only defers the failure: the null travels into the interop
        /// boundary and surfaces as a NullReferenceException with no indication of which handle was bad.
        /// This fails at the call instead, naming the cause - the handle is disposed, or its value is not
        /// an object (a string, a number or null on the Javascript side).
        /// </summary>
        public JSObject JSObjectRequired => JSObject
            ?? throw new InvalidOperationException(IsDisposed
                ? $"{nameof(SpawnJSHandle)} is disposed"
                : $"{nameof(SpawnJSHandle)} does not hold a Javascript object (JSType '{JSType}')");

        /// <summary>
        /// This will be a JSObject or a Javascript value type
        /// </summary>
        public object? JSValue
        {
            get
            {
                UpdateHandleCheck();
                return _jsValue;
            }
        }
        /// <summary>
        /// Returns true if disposed
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// The current JSObject that points to the Javscript object this handle is attached to
        /// </summary>
        private JSObject? _jsObject;
        /// <summary>
        /// The Javascript Array we use to store our JSObject for retrieval when needed
        /// </summary>
        /// <summary>
        /// One Javascript object that holds the value of EVERY owned handle, each under its own key.<br/>
        /// Each handle used to construct its own single element Array to park its value in. That is a
        /// Javascript object construction, a boundary crossing and a JSObject proxy per handle - paid on
        /// every handle that owns its storage, which is every handle returned from a typed read. One
        /// shared object costs that once for the whole process.<br/>
        /// Keys are never reused. The allocator only ever increments, so a disposed handle's key can never
        /// be handed to a new handle and resurrect a stale value.<br/>
        /// Not synchronized, for the same reason PtrRefCounts is not: the .Net WASM runtime is single
        /// threaded and the finalizer runs on that same thread.
        /// </summary>
        static JSObject? _store;
        /// <summary>
        /// The shared slot table, which lives in Javascript. It is the SAME table
        /// <see cref="SlotInterop"/> allocates from, so a value created JS-side and a value parked by a
        /// handle are addressed the same way and either can be handed to the other.
        /// </summary>
        // Slot keys are allocated Javascript side and cross as doubles, never as Int64: a boxed Int64
        // cannot cross as an Any-marshalled key ("ToJSNotImplemented, System.Int64"), and a Javascript
        // number IS a double. Exact integers up to 2^53 is far more slots than a process will allocate.
        static JSObject Store => _store ??= Reflect.GetJSObject(JSHost.GlobalThis, "__sjsSlots")!;
        /// <summary>
        /// How many slots are currently held in the shared store. Diagnostics only - it should track the
        /// number of live owning handles, so a number that only ever climbs means slots are being leaked.
        /// </summary>
        public static long LiveSlotCount => _liveSlots;
        static long _liveSlots;
        /// <summary>
        /// True when this handle owns a slot in the shared store and must release it on dispose
        /// </summary>
        private bool _ownsSlot;
        private JSObject? _ownedParent;
        /// <summary>
        /// This holds the parent store when we don't own it
        /// </summary>
        private SpawnJSHandle? _unownedParent = null;
        /// <summary>
        /// The JSObject that currently stores the value . May be owned or unowned by this handle. See Volatile property.<br/>
        /// Use with caution. Do not dispose. 
        /// </summary>
        public JSObject JSParent => _ownedParent ?? _unownedParent?.JSObject!;
        /// <summary>
        /// Holds the key used to retrieve the data in JSParent
        /// </summary>
        public object JSKey { get; private set; }
        /// <summary>
        /// The id of the last JSObject (can change).<br/>
        /// Assigning maintains the shared reference count, so a handle that re-acquires a fresh proxy
        /// releases the old id and takes a reference on the new one.
        /// </summary>
        internal long Ptr
        {
            get => _ptr;
            private set
            {
                if (_ptr == value) return;
                // the old proxy is either already disposed (that is why we re-acquired) or was never
                // resolved, so there is nothing to dispose here - only the count to release.
                PtrRelease(_ptr);
                _ptr = value;
                PtrAddRef(_ptr);
            }
        }
        long _ptr = -1;
        /// <summary>
        /// Checks if the data needs to be read from the store
        /// </summary>
        void UpdateHandleCheck()
        {
            if (!IsDisposed && (_jsObject?.IsDisposed == true || !Resolved))
            {
                if (!Resolved || IsObject)
                {
                    try
                    {
                        var tmpJSO = Reflect.GetJSObject(JSParent, JSKey)!;
                        IsObject = true;
                        Resolved = true;
                        _jsObject = tmpJSO;
                        _jsValue = tmpJSO;
                        Ptr = _jsObject?.GetId() ?? -1;
                    }
                    catch (JSException)
                    {
                        // Expected, not exceptional: the value is not a Javascript object, so the runtime
                        // refuses to marshal it as a JSObject ("JSObject proxy of number is not supported").
                        // A string, a number, a boolean, null or undefined all land here. Resolved stays
                        // false, so the block below reads it as a plain value through GetObject instead -
                        // that fallback IS the handling.
                        //
                        // Only JSException is caught, which is the one the runtime raises for this. Anything
                        // else - a disposed parent, a null reference - is a real fault and propagates rather
                        // than being silently turned into an unresolved handle.
                    }
                }
                if (!Resolved && !IsObject)
                {
                    IsObject = false;
                    Resolved = true;
                    _jsValue = Reflect.GetObject(JSParent, JSKey)!;
                    _jsObject = null;
                    Ptr = -1;
                }
            }
        }
        /// <summary>
        /// Returns true if the underlying type is undefined
        /// </summary>
        public bool IsUndefined => JSType == "undefined";
        object? _jsValue = null;
        /// <summary>
        /// Returns true if the data has been resolved. JSValue has been resolved to a JSObject, null, or the JS data.
        /// </summary>
        public bool Resolved { get; private set; } = false;
        /// <summary>
        /// Returns true if this handle represents an object
        /// </summary>
        public bool IsObject { get; private set; }
        /// <summary>
        /// Returns true if this handle acesses teh JS data via an unsafe parent
        /// </summary>
        public bool Volatile { get; private set; }
        /// <summary>
        /// New instance
        /// </summary>
        public SpawnJSHandle(JSObject jsObject)
        {
            if (SpawnJSRuntime.CountCalls) SpawnJSRuntime.CountCall("handle:fromJSObject");
            if (jsObject == null) throw new NullReferenceException(nameof(jsObject));
            if (jsObject.IsDisposed) throw new ObjectDisposedException(nameof(jsObject));
            _jsObject = jsObject;
            _jsValue = jsObject;
            IsObject = true;
            // its own slot key in the shared store. Never reused, so a disposed handle's key can
            // never be handed out again and resurrect a stale value.
            // one crossing: allocate the slot with the object already in it
            JSKey = SlotInterop.AllocValue(_jsObject);
            Resolved = true;
            Ptr = _jsObject?.GetId() ?? -1;
            // store the JSObject in a JS array we own so that if the JSObject gets Dispsoed elsewhere we can get a fresh JSObject
            _ownedParent = Store;
            _ownsSlot = true;
            _liveSlots++;
        }
        /// <summary>
        /// Adopts a slot that Javascript already holds a value in.<br/>
        /// No JSObject proxy is created. The proxy is what costs - measured at 21us to create an object
        /// through the proxy path against 1.3us to create it in Javascript and take its slot - and most
        /// intermediate objects (a descriptor being assembled, an argument array) never need one at all.
        /// If something does ask for <see cref="JSObject"/>, UpdateHandleCheck materialises it then.
        /// </summary>
        internal SpawnJSHandle(double slot)
        {
            JSKey = slot;
            _ownedParent = Store;
            _ownsSlot = true;
            _liveSlots++;
            // deliberately NOT Resolved: nothing has asked for a proxy, so do not make one
        }
        /// <summary>
        /// Creates a volatile Javascript handle from a parent and its key
        /// </summary>
        public SpawnJSHandle(SpawnJSHandle jsParent, object jsKey, bool unsafeUse = false)
        {
            if (SpawnJSRuntime.CountCalls) SpawnJSRuntime.CountCall("handle:fromParentKey");
            if (jsParent == null) throw new NullReferenceException(nameof(jsParent));
            if (jsParent.IsDisposed) throw new ObjectDisposedException(nameof(jsParent));
            Volatile = unsafeUse;
            if (unsafeUse)
            {
                // use the unsafe storage (faster, but possibly shares the parent)
                JSKey = jsKey;
                _unownedParent = jsParent;
            }
            else
            {
                // copy into our own storage
                // its own slot key in the shared store. Never reused, so a disposed handle's key can
                // never be handed out again and resurrect a stale value.
                JSKey = SlotInterop.AllocEmpty();
                // store the JSObject in a JS array we own so that if the JSObject gets Dispsoed elsewhere we can get a fresh JSObject
                _ownedParent = Store;
                _ownsSlot = true;
                _liveSlots++;
                JS.CopyProperty(jsParent.JSObject!, jsKey, _ownedParent, JSKey);
            }
        }
        /// <summary>
        ///  Copies the value this JSHandle represents to the destination
        /// </summary>
        /// <param name="destHandle"></param>
        /// <param name="destKey"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public void CopyTo(SpawnJSHandle destHandle, object destKey)
        {
            if (destHandle == null) throw new NullReferenceException(nameof(destHandle));
            if (destHandle.IsDisposed) throw new ObjectDisposedException(nameof(destHandle));
            JS.CopyProperty(JSParent, JSKey, destHandle.JSObject!, destKey);
        }
        /// <summary>
        /// Returns the Javascript type info
        /// </summary>
        public string JSType
        {
            get
            {
                if (_jsType == null) GetTypeAndConstructorNames();
                return _jsType!;
            }
        }
        string? _jsType = null;

        string[]? _constructorNames;
        /// <summary>
        /// Javascript object protoype chain, most derived first.<br/>
        /// Empty for null and undefined. The first entry is the value's real constructor name, which is
        /// not the same thing as <see cref="JSClass"/> - Object.prototype.toString reports "Error" for
        /// every Error subclass, so it cannot identify a derived type.
        /// </summary>
        public string[] ConstructorNames
        {
            get
            {
                if (_constructorNames == null) GetTypeAndConstructorNames();
                return _constructorNames!;
            }
        }
        /// <summary>
        /// Reads typeof and the prototype chain in one call and caches both
        /// </summary>
        void GetTypeAndConstructorNames()
        {
            var info = JS.GetPropertyTypeAndConstructorNames(JSParent, JSKey);
            _jsType = info.Length > 0 ? info[0] : "";
            _constructorNames = info.Length > 1 ? info[1..] : System.Array.Empty<string>();
        }
        /// <summary>
        /// Returns the Javascript type info
        /// </summary>
        public string JSClass
        {
            get
            {
                if (_jsClass == null) GetTypeInfo();
                return _jsClass!;
            }
        }
        string? _jsClass = null;
        void GetTypeInfo()
        {
            var typeInfo = JS.GetPropertyTypeInfo(JSParent, JSKey) ?? "";
            var parts = typeInfo.Split(" ");
            if (parts.Length == 2)
            {
                _jsType = parts[0];
                _jsClass = parts[1];
            }
            else
            {
                _jsClass = "";
                _jsType = "";
            }
        }
        /// <summary>
        /// Dispose resources
        /// </summary>
        void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            IsDisposed = true;
            // release our reference on the Javascript object. lastReference is true only if no other
            // SpawnJSHandle still points at this proxy, so we never dispose one a sibling is still using.
            var lastReference = PtrRelease(_ptr);
            _ptr = -1;
            // The store is shared and outlives every handle, so it is never disposed - only this
            // handle's slot in it is removed. Leaving the slot behind would keep the Javascript value
            // alive for the life of the process.
            if (_ownsSlot)
            {
                _ownsSlot = false;
                _liveSlots--;
                SlotInterop.Free(Convert.ToDouble(JSKey));
            }
            // _unownedParent belongs to whoever handed it to us - that is what Volatile means.
            // Disposing it here would tear down the caller's handle out from under them.
            _unownedParent = null;
            if (lastReference && _jsObject?.IsDisposed == false)
            {
                _jsObject.Dispose();
            }
        }
        /// <summary>
        /// Clone the handle
        /// </summary>
        public SpawnJSHandle Clone()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(SpawnJSHandle));
            if (JSObject == null) throw new Exception("Cannot clone volatile SpawnJSHandle");
            return new SpawnJSHandle(JSObject);
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
        ~SpawnJSHandle()
        {
            Dispose(false);
        }
    }
}
