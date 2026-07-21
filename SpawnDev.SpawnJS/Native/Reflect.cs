using System.Runtime.InteropServices.JavaScript;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Typed bindings to the Javascript globalThis.Reflect object used for get/set/apply/construct/deleteProperty interop
    /// </summary>
    public static partial class Reflect
    {

        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// Returns the awaited result as a JSObject, or null if it was null or undefined.
        /// Use this only where the Javascript really returns a promise - a synchronous binding would marshal the promise itself as a value instead of awaiting it.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        public static partial Task<JSObject?> ApplyJSObjectAsync(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        #region Reflect.apply
        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// The promise it returns is awaited and its result discarded.
        /// Use this only where the Javascript really returns a promise - a synchronous binding would marshal the promise itself as a value instead of awaiting it.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        public static partial Task ApplyVoidAsync(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// The result is discarded.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        public static partial void ApplyVoid(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// Returns the result as a JSObject, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        public static partial JSObject? ApplyJSObject(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// Returns the result marshalled to its .Net equivalent.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        [return: JSMarshalAs<Any>]
        public static partial object? ApplyObject(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// Returns the result as a string, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        public static partial string? ApplyString(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// Returns the result as a byte array, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        public static partial byte[]? ApplyByteArray(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// Returns the result as an int.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        public static partial int ApplyInt32(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// Returns the result as a double.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        public static partial double ApplyDouble(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        public static partial bool ApplyBoolean(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// Returns the result as an int, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        public static partial int? ApplyInt32Nullable(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// Returns the result as a double, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        public static partial double? ApplyDoubleNullable(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);

        /// <summary>
        /// Calls <c>target</c> as a function through <c>Reflect.apply</c>, with <c>thisArg</c> as its <c>this</c> and <c>args</c> as its argument array.
        /// Returns the result as a bool, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.apply")]
        public static partial bool? ApplyBooleanNullable(JSObject target, JSObject? thisArg, [JSMarshalAs<Any>] object args);
        #endregion

        #region Reflect.get
        /// <summary>
        /// Reads <c>target[key]</c> through <c>Reflect.get</c>.
        /// Returns the result as a JSObject, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.get")]
        public static partial JSObject? GetJSObject(JSObject target, [JSMarshalAs<Any>] object key);

        /// <summary>
        /// Reads <c>target[key]</c> through <c>Reflect.get</c>.
        /// Returns the result marshalled to its .Net equivalent.
        /// </summary>
        [JSImport("globalThis.Reflect.get")]
        [return: JSMarshalAs<Any>]
        public static partial object? GetObject(JSObject target, [JSMarshalAs<Any>] object key);

        /// <summary>
        /// Reads <c>target[key]</c> through <c>Reflect.get</c>.
        /// Returns the result as a string, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.get")]
        public static partial string? GetString(JSObject target, [JSMarshalAs<Any>] object key);

        /// <summary>
        /// Reads <c>target[key]</c> through <c>Reflect.get</c>.
        /// Returns the result as a byte array, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.get")]
        public static partial byte[]? GetByteArray(JSObject target, [JSMarshalAs<Any>] object key);

        /// <summary>
        /// Reads <c>target[key]</c> through <c>Reflect.get</c>.
        /// Returns the result as an int.
        /// </summary>
        [JSImport("globalThis.Reflect.get")]
        public static partial int GetInt32(JSObject target, [JSMarshalAs<Any>] object key);

        /// <summary>
        /// Reads <c>target[key]</c> through <c>Reflect.get</c>.
        /// Returns the result as a double.
        /// </summary>
        [JSImport("globalThis.Reflect.get")]
        public static partial double GetDouble(JSObject target, [JSMarshalAs<Any>] object key);

        /// <summary>
        /// Reads <c>target[key]</c> through <c>Reflect.get</c>.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("globalThis.Reflect.get")]
        public static partial bool GetBoolean(JSObject target, [JSMarshalAs<Any>] object key);

        /// <summary>
        /// Reads <c>target[key]</c> through <c>Reflect.get</c>.
        /// Returns the result as an int, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.get")]
        public static partial int? GetInt32Nullable(JSObject target, [JSMarshalAs<Any>] object key);

        /// <summary>
        /// Reads <c>target[key]</c> through <c>Reflect.get</c>.
        /// Returns the result as a double, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.get")]
        public static partial double? GetDoubleNullable(JSObject target, [JSMarshalAs<Any>] object key);

        /// <summary>
        /// Reads <c>target[key]</c> through <c>Reflect.get</c>.
        /// Returns the result as a bool, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.get")]
        public static partial bool? GetBooleanNullable(JSObject target, [JSMarshalAs<Any>] object key);
        #endregion

        #region Reflect.construct
        /// <summary>
        /// Constructs a new instance of <c>target</c> through <c>Reflect.construct</c>, passing <c>args</c> as the argument array.
        /// Returns the result as a JSObject, or null if it was null or undefined.
        /// </summary>
        [JSImport("globalThis.Reflect.construct")]
        public static partial JSObject? ConstructJSObject(JSObject target, [JSMarshalAs<Any>] object args);
        #endregion

        #region Reflect.deleteProperty
        /// <summary>
        /// Deletes <c>target[key]</c> through <c>Reflect.deleteProperty</c>. Returns true if the delete was accepted.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("globalThis.Reflect.deleteProperty")]
        public static partial bool DeletePropertyVoid(JSObject target, [JSMarshalAs<Any>] object key);
        #endregion

        #region Reflect.set
        /// <summary>
        /// Writes <c>target[key]</c> through <c>Reflect.set</c>. Returns true if the write was accepted.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("globalThis.Reflect.set")]
        public static partial bool SetObject(JSObject target, [JSMarshalAs<Any>] object key, [JSMarshalAs<Any>] object? value);

        /// <summary>
        /// Writes <c>target[key]</c> through <c>Reflect.set</c>. Returns true if the write was accepted.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("globalThis.Reflect.set")]
        public static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, JSObject? value);

        /// <summary>
        /// Writes <c>target[key]</c> through <c>Reflect.set</c>. Returns true if the write was accepted.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("globalThis.Reflect.set")]
        public static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, [JSMarshalAs<MemoryView>] ArraySegment<byte> value);

        /// <summary>
        /// Writes <c>target[key]</c> through <c>Reflect.set</c>. Returns true if the write was accepted.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("globalThis.Reflect.set")]
        public static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, [JSMarshalAs<MemoryView>] Span<byte> value);

        /// <summary>
        /// Writes <c>target[key]</c> through <c>Reflect.set</c>. Returns true if the write was accepted.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("globalThis.Reflect.set")]
        public static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, double value);

        /// <summary>
        /// Writes <c>target[key]</c> through <c>Reflect.set</c>. Returns true if the write was accepted.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("globalThis.Reflect.set")]
        public static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, int value);

        /// <summary>
        /// Writes <c>target[key]</c> through <c>Reflect.set</c>. Returns true if the write was accepted.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("globalThis.Reflect.set")]
        public static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, bool value);

        /// <summary>
        /// Writes <c>target[key]</c> through <c>Reflect.set</c>. Returns true if the write was accepted.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("globalThis.Reflect.set")]
        public static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, string? value);

        /// <summary>
        /// Writes <c>target[key]</c> through <c>Reflect.set</c>. Returns true if the write was accepted.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("globalThis.Reflect.set")]
        public static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, byte[]? value);

        /// <summary>
        /// Writes <c>target[key]</c> through <c>Reflect.set</c>. Returns true if the write was accepted.
        /// Returns the result as a bool.
        /// </summary>
        /// <summary>
        /// Publishes the JS ➡️ .Net entry point. Its signature is all primitives - the command, and the
        /// region of jsToNetBuffer holding the arguments - so an inbound call marshals no object at all.
        /// It used to be <c>Func&lt;string, JSObject, JSObject&gt;</c>, which cost a runtime proxy for the
        /// argument array on the way in and another for the result on the way out.
        /// </summary>
        [JSImport("globalThis.Reflect.set")]
        public static partial bool Set(JSObject target, [JSMarshalAs<Any>] object key, [JSMarshalAs<JSType.Function<JSType.String, JSType.Number, JSType.Number, JSType.Boolean>>] Func<string, double, double, bool> value);


        /// <summary>
        /// Returns strings like: '[object String]'
        /// </summary>
        /// <summary>
        /// Returns the Javascript internal class tag of <c>target</c>, for example "[object Array]". Note this reports the BASE tag for built in subclasses, so it cannot identify a derived type.
        /// </summary>
        [JSImport("Object.prototype.toString.call")]
        public static partial string TypeOf(JSObject target);

        /// <summary>
        /// The Reflect.has() static method is like the in operator, but as a function.
        /// </summary>
        /// <summary>
        /// Returns true if <c>key</c> is a property of <c>target</c> or anywhere on its prototype chain, using the Javascript <c>in</c> operator semantics of <c>Reflect.has</c>.
        /// Returns the result as a bool.
        /// </summary>
        [JSImport("Reflect.has")]
        public static partial bool Has(JSObject target, [JSMarshalAs<Any>] object key);


        // 
        #endregion
    }
}
