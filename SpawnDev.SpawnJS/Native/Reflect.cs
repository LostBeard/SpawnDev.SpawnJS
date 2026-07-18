using System.Runtime.InteropServices.JavaScript;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Typed bindings to the Javascript globalThis.Reflect object used for get/set/apply/construct/deleteProperty interop
    /// </summary>
    internal static partial class Reflect
    {

        [JSImport("globalThis.Reflect.apply")]
        internal static partial Task<JSObject?> ApplyJSObjectAsync(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        #region Reflect.apply
        [JSImport("globalThis.Reflect.apply")]
        internal static partial Task ApplyVoidAsync(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        [JSImport("globalThis.Reflect.apply")]
        internal static partial void ApplyVoid(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        [JSImport("globalThis.Reflect.apply")]
        internal static partial JSObject? ApplyJSObject(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        [JSImport("globalThis.Reflect.apply")]
        [return: JSMarshalAs<Any>]
        internal static partial object? ApplyObject(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        [JSImport("globalThis.Reflect.apply")]
        internal static partial string? ApplyString(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        [JSImport("globalThis.Reflect.apply")]
        internal static partial byte[]? ApplyByteArray(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        [JSImport("globalThis.Reflect.apply")]
        internal static partial int ApplyInt32(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        [JSImport("globalThis.Reflect.apply")]
        internal static partial double ApplyDouble(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        [JSImport("globalThis.Reflect.apply")]
        internal static partial bool ApplyBoolean(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        [JSImport("globalThis.Reflect.apply")]
        internal static partial int? ApplyInt32Nullable(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        [JSImport("globalThis.Reflect.apply")]
        internal static partial double? ApplyDoubleNullable(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        [JSImport("globalThis.Reflect.apply")]
        internal static partial bool? ApplyBooleanNullable(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);
        #endregion

        #region Reflect.get
        [JSImport("globalThis.Reflect.get")]
        internal static partial JSObject? GetJSObject(JSObject target, [JSMarshalAs<Any>] object key);

        [JSImport("globalThis.Reflect.get")]
        [return: JSMarshalAs<Any>]
        internal static partial object? GetObject(JSObject target, [JSMarshalAs<Any>] object key);

        [JSImport("globalThis.Reflect.get")]
        internal static partial string? GetString(JSObject target, [JSMarshalAs<Any>] object key);

        [JSImport("globalThis.Reflect.get")]
        internal static partial byte[]? GetByteArray(JSObject target, [JSMarshalAs<Any>] object key);

        [JSImport("globalThis.Reflect.get")]
        internal static partial int GetInt32(JSObject target, [JSMarshalAs<Any>] object key);

        [JSImport("globalThis.Reflect.get")]
        internal static partial double GetDouble(JSObject target, [JSMarshalAs<Any>] object key);

        [JSImport("globalThis.Reflect.get")]
        internal static partial bool GetBoolean(JSObject target, [JSMarshalAs<Any>] object key);

        [JSImport("globalThis.Reflect.get")]
        internal static partial int? GetInt32Nullable(JSObject target, [JSMarshalAs<Any>] object key);

        [JSImport("globalThis.Reflect.get")]
        internal static partial double? GetDoubleNullable(JSObject target, [JSMarshalAs<Any>] object key);

        [JSImport("globalThis.Reflect.get")]
        internal static partial bool? GetBooleanNullable(JSObject target, [JSMarshalAs<Any>] object key);
        #endregion

        #region Reflect.construct
        [JSImport("globalThis.Reflect.construct")]
        internal static partial JSObject? ConstructJSObject(JSObject target, [JSMarshalAs<Any>] object args);
        #endregion

        #region Reflect.deleteProperty
        [JSImport("globalThis.Reflect.deleteProperty")]
        internal static partial bool DeletePropertyVoid(JSObject target, [JSMarshalAs<Any>] object key);
        #endregion

        #region Reflect.set
        [JSImport("globalThis.Reflect.set")]
        internal static partial bool SetObject(JSObject target, [JSMarshalAs<Any>] object key, [JSMarshalAs<Any>] object? value);

        [JSImport("globalThis.Reflect.set")]
        internal static partial bool SetJSObject(JSObject target, [JSMarshalAs<Any>] object key, JSObject? value);

        [JSImport("globalThis.Reflect.set")]
        internal static partial bool SetArraySegmentByte(JSObject target, [JSMarshalAs<Any>] object key, [JSMarshalAs<MemoryView>] ArraySegment<byte> value);

        [JSImport("globalThis.Reflect.set")]
        internal static partial bool SetSpanByte(JSObject target, [JSMarshalAs<Any>] object key, [JSMarshalAs<MemoryView>] Span<byte> value);

        [JSImport("globalThis.Reflect.set")]
        internal static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, double value);

        [JSImport("globalThis.Reflect.set")]
        internal static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, int value);

        [JSImport("globalThis.Reflect.set")]
        internal static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, bool value);

        [JSImport("globalThis.Reflect.set")]
        internal static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, string? value);

        [JSImport("globalThis.Reflect.set")]
        internal static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, byte[]? value);

        [JSImport("globalThis.Reflect.set")]
        internal static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, [JSMarshalAs<JSType.Function<JSType.String, JSType.Object, JSType.Object>>] Func<string, JSObject, JSObject> value);

        // 
        #endregion
    }
}
