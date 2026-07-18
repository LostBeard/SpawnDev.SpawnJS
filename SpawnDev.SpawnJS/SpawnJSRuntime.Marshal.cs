using SpawnDev.SpawnJS.Marshallers;
using System.Collections.Concurrent;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSRuntime
    {
        #region Marshal
        ConcurrentDictionary<Type, JSMarshaller> _typeMarshallerCache = new ConcurrentDictionary<Type, JSMarshaller>();
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
        internal object? JSToNet(Type type, JSObject jsParent, object jsKey)
        {
            var marshaller = GetMarshaller(type);
            return marshaller.JSToNet(type, jsParent, jsKey, this);
        }
        internal void NetToJS(JSObject jsParent, object jsKey, object? obj)
        {
            var type = obj?.GetType();
            var marshaller = GetMarshaller(type);
            marshaller.NetToJS(type, jsParent, jsKey, obj, this);
        }
        internal JSObject? NetArrayToJSArray(object?[]? args)
        {
            if (args == null) return null;
            var ret = NewJSArray();
            for (var i = 0; i < args.Length; i++)
            {
                var item = args[i];
                var itemType = item?.GetType();
                var marshaller = GetMarshaller(itemType);
                marshaller.NetToJS(itemType, ret, i, item, this);
            }
            return ret;
        }
        #endregion
        #region Sync NetRun
        internal T NetRun<T>(string cmd, object?[]? args = null) => (T)NetRun(typeof(T), cmd, args)!;
        internal object? NetRun(Type type, string cmd, object?[]? args = null)
        {
            args ??= new object?[0];
            using var jsArgs = NetArrayToJSArray(args);
            // _netToJSCall always returns the [ret] wrapper array, so the result is never null
            using var ret = NetToJSCall(cmd, jsArgs)!;
            var marshaller = GetMarshaller(type);
            var netRet = marshaller.JSToNet(type, ret, 0, this);
            return netRet;
        }
        internal void NetRunVoid(string cmd, object?[]? args = null)
        {
            args ??= new object?[0];
            using var jsArgs = NetArrayToJSArray(args);
            NetToJSCallVoid(cmd, jsArgs);
        }
        #endregion
        #region Async NetRun
        internal async Task<T> NetRunAsync<T>(string cmd, object?[]? args = null) => (T)(await NetRunAsync(typeof(T), cmd, args))!;
        internal async Task<object?> NetRunAsync(Type type, string cmd, object?[]? args = null)
        {
            args ??= new object?[0];
            using var jsArgs = NetArrayToJSArray(args);
            // _netToJSCallAsync always resolves to the [ret] wrapper array, so the result is never null
            using var ret = (await NetToJSCallAsync(cmd, jsArgs))!;
            var marshaller = GetMarshaller(type);
            var netRet = marshaller.JSToNet(type, ret, 0, this);
            return netRet;
        }
        internal async Task NetRunVoidAsync(string cmd, object?[]? args = null)
        {
            args ??= new object?[0];
            using var jsArgs = NetArrayToJSArray(args);
            await NetToJSCallVoidAsync(cmd, jsArgs);
        }
        #endregion

        #region NetToJS calls
        internal void NetToJSCallVoid(string cmd, JSObject? args)
            => Reflect.ApplyVoid(_netToJSCall, SpawnJSInterop, new object?[] { cmd, args });

        internal JSObject? NetToJSCall(string cmd, JSObject? args)
            => Reflect.ApplyJSObject(_netToJSCall, SpawnJSInterop, new object?[] { cmd, args });

        internal Task<JSObject?> NetToJSCallAsync(string cmd, JSObject? args)
            => Reflect.ApplyJSObjectAsync(_netToJSCallAsync, SpawnJSInterop, new object?[] { cmd, args });

        internal Task NetToJSCallVoidAsync(string cmd, JSObject? args)
            => Reflect.ApplyVoidAsync(_netToJSCallAsync, SpawnJSInterop, new object?[] { cmd, args });
        #endregion
    }
}
