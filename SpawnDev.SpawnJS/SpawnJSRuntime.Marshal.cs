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
        internal JSMarshaller GetMarshaller(Type? type)
        {
            JSMarshaller? marshaller = null;
            if (type == null && _nullTypeMarshaller != null)
            {
                marshaller = _nullTypeMarshaller;
            }
            else if (type == null || !_typeMarshallerCache.TryGetValue(type, out marshaller))
            {
                var length = Marshallers.Count;
                for (var i = length - 1; i >= 0; i--)
                {
                    marshaller = Marshallers[i];
                    if (marshaller.CanMarshal(type))
                    {
                        var typeMarshaller = marshaller.GetMarshaller(type);
                        if (typeMarshaller == null) continue;
                        if (type == null)
                        {
                            _nullTypeMarshaller = marshaller;
                        }
                        else
                        {
                            _typeMarshallerCache.TryAdd(type, marshaller);
                        }
                        break;
                    }
                }
            }
            if (marshaller == null) throw new Exception($"GetMarshaller failed: {type?.Name}");
            if (Verbose) Console.WriteLine($"<< GetMarshaller: {type?.Name} {marshaller.GetType().Name}");
            return marshaller;
        }
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
            using var jsArgs = MarshallNetArrayToJSArray(args);
            // _netToJSCall always returns the [ret] wrapper array, so the result is never null
            using var ret = NetToJSCall(cmd, jsArgs)!;
            var netRet = MarshallJSToNet(type, ret, 0);
            if (netRet != null && netRet.GetType() != type)
                throw new Exception($"{nameof(SpawnJSRuntime)}.NetRun expected {type.Name} got {netRet.GetType().Name}");
            return netRet;
        }
        internal void NetRunVoid(string cmd, object?[]? args = null)
        {
            args ??= new object?[0];
            using var jsArgs = MarshallNetArrayToJSArray(args);
            NetToJSCallVoid(cmd, jsArgs);
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
        private void NetToJSCallVoid(string cmd, SpawnJSHandle? args)
            => Reflect.ApplyVoid(_netToJSCall.JSObject, SpawnJSInterop.JSObject, new object?[] { cmd, args?.JSObject });

        private SpawnJSHandle? NetToJSCall(string cmd, SpawnJSHandle? args)
            => (SpawnJSHandle?)Reflect.ApplyJSObject(_netToJSCall.JSObject, SpawnJSInterop.JSObject, new object?[] { cmd, args?.JSObject })!;

        private async Task<SpawnJSHandle?> NetToJSCallAsync(string cmd, SpawnJSHandle? args)
            => (SpawnJSHandle?)(await Reflect.ApplyJSObjectAsync(_netToJSCallAsync.JSObject, SpawnJSInterop.JSObject, new object?[] { cmd, args?.JSObject }))!;

        private Task NetToJSCallVoidAsync(string cmd, SpawnJSHandle? args)
            => Reflect.ApplyVoidAsync(_netToJSCallAsync.JSObject, SpawnJSInterop.JSObject, new object?[] { cmd, args?.JSObject });
        #endregion
    }
}
