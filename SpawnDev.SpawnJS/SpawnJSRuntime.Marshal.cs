using SpawnDev.SpawnJS.Marshallers;
using System.Collections.Concurrent;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSRuntime
    {
        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string CopyProperty(JSObject srcParent, object? srcKey, JSObject destParent, object? destKey)
        {
            if (srcParent == null || srcParent.IsDisposed || srcParent == null) throw new Exception("CopyProperty invalid JSObject");
            return NetRun<string>("copyProperty", new object[] { srcParent, srcKey!, destParent!, destKey! });
        }
        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string MoveProperty(JSObject srcParent, object? srcKey, JSObject destParent, object? destKey)
        {
            if (srcParent == null || srcParent.IsDisposed || srcParent == null) throw new Exception("MoveProperty invalid JSObject");
            return NetRun<string>("moveProperty", new object[] { srcParent, srcKey!, destParent!, destKey! });
        }
        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string CopyProperty(SpawnJSHandle srcParent, object? srcKey, SpawnJSHandle destParent, object? destKey)
        {
            if (srcParent == null || srcParent.IsDisposed || srcParent.JSObject == null) throw new Exception("CopyProperty invalid SpawnJSHandle");
            return NetRun<string>("copyProperty", new object[] { srcParent.JSObject, srcKey!, destParent.JSObject!, destKey! });
        }
        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string MoveProperty(SpawnJSHandle srcParent, object? srcKey, SpawnJSHandle destParent, object? destKey)
        {
            if (srcParent == null || srcParent.IsDisposed || srcParent.JSObject == null) throw new Exception("MoveProperty invalid SpawnJSHandle");
            return NetRun<string>("moveProperty", new object[] { srcParent.JSObject, srcKey!, destParent.JSObject!, destKey! });
        }

        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string[] GetPropertyConstructorNames(SpawnJSHandle jsParent, object? jsKey)
        {
            if (jsParent == null || jsParent.IsDisposed || jsParent.JSObject == null) throw new Exception("ObjectPrototypeToStringCall invalid SpawnJSHandle");
            return NetRun<string[]>("getPropertyConstructorNames", new object[] { jsParent.JSObject, jsKey! });
        }

        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string[] GetConstructorNames(SpawnJSHandle jsHandle)
        {
            if (jsHandle == null || jsHandle.IsDisposed || jsHandle.JSParent == null) throw new Exception("ObjectPrototypeToStringCall invalid SpawnJSHandle");
            return NetRun<string[]>("getConstructorNames", new object[] { jsHandle.JSParent });
        }

        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string[] GetPropertyConstructorNames(JSObject jsParent, object? jsKey)
        {
            if (jsParent == null || jsParent.IsDisposed) throw new Exception("ObjectPrototypeToStringCall invalid SpawnJSHandle");
            return NetRun<string[]>("getPropertyConstructorNames", new object[] { jsParent, jsKey! });
        }

        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string[] GetConstructorNames(JSObject jsHandle)
        {
            if (jsHandle == null || jsHandle.IsDisposed || jsHandle == null) throw new Exception("ObjectPrototypeToStringCall invalid SpawnJSHandle");
            return NetRun<string[]>("getConstructorNames", new object[] { jsHandle,  });
        }

        /// <summary>
        /// The WebAssembly linear memory ArrayBuffer the .Net heap lives in.<br/>
        /// This is what a zero copy view is built over: pin a .Net array, take its address, and hand
        /// Javascript a TypedArray onto these bytes so the data is never copied across the boundary.
        /// </summary>
        public SpawnJSHandle WasmMemoryBuffer() => NetRun<SpawnJSHandle>("wasmMemoryBuffer");
        /// <summary>
        /// Which runtime shape the WebAssembly memory buffer was reached through, or an empty string if it
        /// could not be reached. Diagnostics: the runtime exposes the heap differently across versions,
        /// and reaching it the wrong way yields a view onto the wrong bytes rather than an error.
        /// </summary>
        public string WasmMemoryBufferSource() => NetRun<string>("wasmMemoryBufferSource");
        /// <summary>
        /// Returns the Javascript typeof followed by the value's prototype chain constructor names, most
        /// derived first: ["object", "TypeError", "Error", "Object"].<br/>
        /// One call, because identifying a Javascript value needs both and a marshaller should not pay two
        /// round trips to find out what it is reading.
        /// </summary>
        public string[] GetPropertyTypeAndConstructorNames(JSObject jsParent, object? jsKey)
        {
            if (jsParent == null || jsParent.IsDisposed) throw new Exception("GetPropertyTypeAndConstructorNames invalid JSObject");
            return NetRun<string[]>("getPropertyTypeAndConstructorNames", new object[] { jsParent, jsKey! });
        }
        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string GetPropertyTypeInfo(SpawnJSHandle jsParent, object? jsKey)
        {
            if (jsParent == null || jsParent.IsDisposed || jsParent.JSObject == null) throw new Exception("ObjectPrototypeToStringCall invalid SpawnJSHandle");
            return NetRun<string>("getPropertyTypeInfo", new object[] { jsParent.JSObject, jsKey! });
        }
        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string GetPropertyTypeInfo(JSObject jsParent, object? jsKey)
        {
            if (jsParent == null || jsParent.IsDisposed) throw new Exception("ObjectPrototypeToStringCall invalid SpawnJSHandle");
            return NetRun<string>("getPropertyTypeInfo", new object[] { jsParent, jsKey! });
        }
        /// <summary>
        /// Compares two values using Javascript equality.<br/>
        /// full == true uses strict equality (===), otherwise loose equality (==)
        /// </summary>
        public bool ObjectEquals(object? obj1, object? obj2, bool full = false)
            => NetRun<bool>("objectEquals", new object?[] { obj1, obj2, full });
        #region Marshal
        ConcurrentDictionary <Type, JSMarshaller> _typeMarshallerCache = new ConcurrentDictionary<Type, JSMarshaller>();
        JSMarshaller? _nullTypeMarshaller = null;
        /// <summary>
        /// Returns the marshaller that will be used for the given .Net type.<br/>
        /// Marshallers are matched in reverse registration order, so a later registration wins, and the
        /// match is cached per type. A marshaller may hand back an instance specialised to the type, in
        /// which case that specialisation is what gets cached and returned.<br/>
        /// Public because the marshaller registry is part of the API - anyone adding a marshaller needs to
        /// be able to see which one a type actually resolves to.
        /// </summary>
        public JSMarshaller GetMarshaller(Type? type)
        {
            JSMarshaller? marshaller = null;
            if (type == null && _nullTypeMarshaller != null)
            {
                marshaller = _nullTypeMarshaller;
            }
            else if (type == null || !_typeMarshallerCache.TryGetValue(type, out marshaller))
            {
                // reset, because TryGetValue writes null on a miss and the loop below may decline every
                // candidate. Leaving a declined candidate in `marshaller` would defeat the null check.
                marshaller = null;
                var length = Marshallers.Count;
                for (var i = length - 1; i >= 0; i--)
                {
                    var candidate = Marshallers[i];
                    if (!candidate.CanMarshal(type)) continue;
                    // GetMarshaller lets a marshaller hand back a per-type specialization (UnionMarshaller
                    // returns one bound to the concrete Union<...> arms). Cache and use THAT, not the
                    // generic candidate - otherwise the specialization hook does nothing.
                    var typeMarshaller = candidate.GetMarshaller(type);
                    if (typeMarshaller == null) continue;
                    marshaller = typeMarshaller;
                    if (type == null)
                    {
                        _nullTypeMarshaller = typeMarshaller;
                    }
                    else
                    {
                        _typeMarshallerCache.TryAdd(type, typeMarshaller);
                    }
                    break;
                }
            }
            if (marshaller == null) throw new Exception($"GetMarshaller failed: {type?.Name}");
            if (Verbose) Console.WriteLine($"<< GetMarshaller: {type?.Name} {marshaller.GetType().Name}");
            return marshaller;
        }
        /// <summary>
        /// Reusable argument arrays, one per call depth.<br/>
        /// Every .Net to Javascript call used to construct a fresh Javascript Array to carry its
        /// arguments - a boundary crossing, a Javascript object, a JSObject proxy and a handle slot, per
        /// call. The arrays are now kept and refilled.<br/>
        /// Indexed by call DEPTH rather than shared outright, because marshalling an argument can itself
        /// make a call (a marshaller reading a property, say). A single shared array would be refilled by
        /// the nested call before the outer one had fired. Depth gives each nesting level its own array,
        /// and nesting is only ever a handful deep.<br/>
        /// Sync only. An async call has not finished when it returns, so its array cannot be handed back
        /// yet and the depth counter would unwind in the wrong order.
        /// </summary>
        readonly List<SpawnJSHandle> _argsArrays = new List<SpawnJSHandle>();
        int _callDepth;

        /// <summary>
        /// Fills and returns the argument array for the current call depth. Pair every call with
        /// <see cref="ReleaseArgsArray"/> in a finally.
        /// </summary>
        SpawnJSHandle RentArgsArray(object?[] args)
        {
            var depth = _callDepth++;
            while (_argsArrays.Count <= depth) _argsArrays.Add(NewJSArray());
            var array = _argsArrays[depth];
            // Set length first so anything left over from a longer previous call at this depth is dropped.
            // Javascript truncates the array on a shorter length, and grows it with holes on a longer one,
            // which the fill below then writes over.
            Reflect.Set(array.JSObjectRequired, "length", args.Length);
            for (var i = 0; i < args.Length; i++)
            {
                var item = args[i];
                var itemType = item?.GetType();
                GetMarshaller(itemType).NetToJS(itemType, array, i, item);
            }
            return array;
        }

        /// <summary>
        /// Gives the current depth's argument array back for reuse
        /// </summary>
        void ReleaseArgsArray() => _callDepth--;

        /// <summary>
        /// Marshall object?[]? args to a Javascript Array
        /// </summary>
        internal SpawnJSHandle? MarshallNetArrayToJSArray(object?[]? args)
        {
            if (args == null) return null;
            var ret = NewJSArray();
            for (var i = 0; i < args.Length; i++)
            {
                var item = args[i];
                var itemType = item?.GetType();
                var marshaller = GetMarshaller(itemType);
                marshaller.NetToJS(itemType, ret, i, item);
            }
            return ret;
        }
        /// <summary>
        /// Writes obj to jsParent[jsKey]
        /// </summary>
        internal void MarshallNetToJS(SpawnJSHandle jsParent, object jsKey, object? obj)
        {
            var type = obj?.GetType();
            var marshaller = GetMarshaller(type);
            marshaller.NetToJS(type, jsParent, jsKey, obj);
        }
        /// <summary>
        /// Reads type from jsParent[jsKey]
        /// </summary>
        internal object? MarshallJSToNet(Type type, SpawnJSHandle jsParent, object jsKey)
        {
            var marshaller = GetMarshaller(type);
            using var jsHandle = new SpawnJSHandle(jsParent, jsKey, true);
            return marshaller.JSToNet(type, jsHandle);
        }
        /// <summary>
        /// Reads type from jsParent[jsKey]
        /// </summary>
        internal object? MarshallJSToNet(Type type, SpawnJSHandle jsHandle)
        {
            var marshaller = GetMarshaller(type);
            return marshaller.JSToNet(type, jsHandle);
        }
        #endregion
        #region Sync NetRun
        internal T NetRun<T>(string cmd, object?[]? args = null)
        {
            var ret = NetRun(typeof(T), cmd, args)!;
            return (T)ret;
        }
        internal object? NetRun(Type type, string cmd, object?[]? args = null)
        {
            args ??= new object?[0];
            // the depth this call will use; the same key names its slot in the return store, so a nested
            // call (which runs at depth + 1) can never overwrite a result this one has not read yet
            var retKey = (double)_callDepth;
            var jsArgs = RentArgsArray(args);
            try
            {
                NetToJSCall(cmd, jsArgs, retKey);
                var netRet = MarshallJSToNet(type, _retStore!, retKey);
                if (netRet != null && netRet.GetType() != type)
                    throw new Exception($"{nameof(SpawnJSRuntime)}.NetRun expected {type.Name} got {netRet.GetType().Name}");
                return netRet;
            }
            finally
            {
                ReleaseArgsArray();
            }
        }
        internal void NetRunVoid(string cmd, object?[]? args = null)
        {
            args ??= new object?[0];
            var jsArgs = RentArgsArray(args);
            try
            {
                NetToJSCallVoid(cmd, jsArgs);
            }
            finally
            {
                ReleaseArgsArray();
            }
        }
        #endregion
        #region Async NetRun
        internal async Task<T> NetRunAsync<T>(string cmd, object?[]? args = null) => (T)(await NetRunAsync(typeof(T), cmd, args))!;
        internal async Task<object?> NetRunAsync(Type type, string cmd, object?[]? args = null)
        {
            args ??= new object?[0];
            using var jsArgs = MarshallNetArrayToJSArray(args);
            // _netToJSCallAsync always resolves to the [ret] wrapper array, so the result is never null
            using var ret = (await NetToJSCallAsync(cmd, jsArgs))!;
            var netRet = MarshallJSToNet(type, ret, 0);
            return netRet;
        }
        internal async Task NetRunVoidAsync(string cmd, object?[]? args = null)
        {
            args ??= new object?[0];
            using var jsArgs = MarshallNetArrayToJSArray(args);
            await NetToJSCallVoidAsync(cmd, jsArgs);
        }
        #endregion
        #region NetToJS calls
        /// <summary>
        /// Runs a synchronous command and discards its result. The -1 return key tells the Javascript side
        /// not to hold a reference to a value nobody is going to read.
        /// </summary>
        private void NetToJSCallVoid(string cmd, SpawnJSHandle? args)
            => Reflect.ApplyVoid(_netToJSCall.JSObjectRequired, SpawnJSInterop.JSObjectRequired, new object?[] { cmd, args?.JSObjectRequired, -1d });

        /// <summary>
        /// Runs a synchronous command and parks its result in the return store under retKey, rather than
        /// returning it wrapped in a freshly allocated array. The caller reads the slot through a volatile
        /// handle, so the round trip allocates nothing.
        /// </summary>
        private void NetToJSCall(string cmd, SpawnJSHandle? args, double retKey)
            => Reflect.ApplyVoid(_netToJSCall.JSObjectRequired, SpawnJSInterop.JSObjectRequired, new object?[] { cmd, args?.JSObjectRequired, retKey });

        private async Task<SpawnJSHandle?> NetToJSCallAsync(string cmd, SpawnJSHandle? args)
            => (SpawnJSHandle?)(await Reflect.ApplyJSObjectAsync(_netToJSCallAsync.JSObjectRequired, SpawnJSInterop.JSObjectRequired, new object?[] { cmd, args?.JSObjectRequired }))!;

        private Task NetToJSCallVoidAsync(string cmd, SpawnJSHandle? args)
            => Reflect.ApplyVoidAsync(_netToJSCallAsync.JSObjectRequired, SpawnJSInterop.JSObjectRequired, new object?[] { cmd, args?.JSObjectRequired });
        #endregion
    }
}
