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
        internal object? JSToNet(Type type, SpawnJSHandle jsParent, object jsKey)
        {
            var marshaller = GetMarshaller(type);
            return marshaller.JSToNet(type, jsParent, jsKey);
        }
        internal void NetToJS(SpawnJSHandle jsParent, object jsKey, object? obj)
        {
            var type = obj?.GetType();
            var marshaller = GetMarshaller(type);
            marshaller.NetToJS(type, jsParent, jsKey, obj);
        }
        internal SpawnJSHandle? NetArrayToJSArray(object?[]? args)
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
            using var jsArgs = NetArrayToJSArray(args);
            // _netToJSCall always returns the [ret] wrapper array, so the result is never null
            using var ret = NetToJSCall(cmd, jsArgs)!;
            var marshaller = GetMarshaller(type);
            var netRet = marshaller.JSToNet(type, ret, 0);
            if (netRet != null && netRet.GetType() != type)
                throw new Exception($"{nameof(SpawnJSRuntime)}.NetRun expected {type.Name} got {netRet.GetType().Name}");
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
            var netRet = marshaller.JSToNet(type, ret, 0);
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
        internal void NetToJSCallVoid(string cmd, SpawnJSHandle? args)
            => Reflect.ApplyVoid(_netToJSCall.JSObject, SpawnJSInterop.JSObject, new object?[] { cmd, args?.JSObject });

        internal SpawnJSHandle? NetToJSCall(string cmd, SpawnJSHandle? args)
            => (SpawnJSHandle?)Reflect.ApplyJSObject(_netToJSCall.JSObject, SpawnJSInterop.JSObject, new object?[] { cmd, args?.JSObject })!;

        internal async Task<SpawnJSHandle?> NetToJSCallAsync(string cmd, SpawnJSHandle? args)
            => (SpawnJSHandle?)(await Reflect.ApplyJSObjectAsync(_netToJSCallAsync.JSObject, SpawnJSInterop.JSObject, new object?[] { cmd, args?.JSObject }))!;

        internal Task NetToJSCallVoidAsync(string cmd, SpawnJSHandle? args)
            => Reflect.ApplyVoidAsync(_netToJSCallAsync.JSObject, SpawnJSInterop.JSObject, new object?[] { cmd, args?.JSObject });
        #endregion
    }
}
