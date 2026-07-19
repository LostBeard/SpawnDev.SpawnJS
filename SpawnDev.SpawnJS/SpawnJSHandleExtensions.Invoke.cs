using SpawnDev.SpawnJS.Native;
using System.Text.Json;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Low level SpawnJSHandle extension methods for invoking the SpawnJSHandle or its properties.
    /// Prefer SpawnJSObjectReference / SpawnJSObject for normal use - these are for when you must work at the SpawnJSHandle level.
    /// </summary>
    public static partial class SpawnJSHandleExtensions
    {
        #region Invoke
        /// <summary>
        /// Invoke the SpawnJSHandle as a constructor (new) - pass the arguments as an array.
        /// </summary>
        public static SpawnJSHandle InvokeConstructorApply(this SpawnJSHandle _this, object?[] args) => (SpawnJSHandle)Reflect.ConstructJSObject(_this.JSObject, args)!;
        /// <summary>
        /// Invoke the SpawnJSHandle as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokeConstructor(this SpawnJSHandle _this) => _this.InvokeConstructorApply(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokeConstructor(this SpawnJSHandle _this, object? arg1) => _this.InvokeConstructorApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokeConstructor(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeConstructorApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokeConstructor(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokeConstructor(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokeConstructor(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokeConstructor(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokeConstructor(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokeConstructor(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokeConstructor(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokeConstructor(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle - pass the arguments as an array.
        /// </summary>
        public static void InvokeVoidApply(this SpawnJSHandle _this, object?[] args) => Reflect.ApplyVoid(_this.JSObject, null, args);
        /// <summary>
        /// Invoke the SpawnJSHandle
        /// </summary>
        public static void InvokeVoid(this SpawnJSHandle _this) => _this.InvokeVoidApply(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle
        /// </summary>
        public static void InvokeVoid(this SpawnJSHandle _this, object? arg1) => _this.InvokeVoidApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle
        /// </summary>
        public static void InvokeVoid(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeVoidApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle
        /// </summary>
        public static void InvokeVoid(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle
        /// </summary>
        public static void InvokeVoid(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle
        /// </summary>
        public static void InvokeVoid(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle
        /// </summary>
        public static void InvokeVoid(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle
        /// </summary>
        public static void InvokeVoid(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle
        /// </summary>
        public static void InvokeVoid(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle
        /// </summary>
        public static void InvokeVoid(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle
        /// </summary>
        public static void InvokeVoid(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as SpawnJSHandle - pass the arguments as an array.
        /// </summary>
        public static SpawnJSHandle? InvokeJSObjectApply(this SpawnJSHandle _this, object?[] args) => (SpawnJSHandle)Reflect.ApplyJSObject(_this.JSObject, null!, args)!;
        /// <summary>
        /// Invoke the SpawnJSHandle and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokeJSObject(this SpawnJSHandle _this) => _this.InvokeJSObjectApply(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokeJSObject(this SpawnJSHandle _this, object? arg1) => _this.InvokeJSObjectApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokeJSObject(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokeJSObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokeJSObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokeJSObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokeJSObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokeJSObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokeJSObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokeJSObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokeJSObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as object - pass the arguments as an array.
        /// </summary>
        public static object? InvokeObjectApply(this SpawnJSHandle _this, object?[] args) => Reflect.ApplyObject(_this.JSObject, null, args);
        /// <summary>
        /// Invoke the SpawnJSHandle and return as object
        /// </summary>
        public static object? InvokeObject(this SpawnJSHandle _this) => _this.InvokeObjectApply(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as object
        /// </summary>
        public static object? InvokeObject(this SpawnJSHandle _this, object? arg1) => _this.InvokeObjectApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as object
        /// </summary>
        public static object? InvokeObject(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeObjectApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as object
        /// </summary>
        public static object? InvokeObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as object
        /// </summary>
        public static object? InvokeObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as object
        /// </summary>
        public static object? InvokeObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as object
        /// </summary>
        public static object? InvokeObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as object
        /// </summary>
        public static object? InvokeObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as object
        /// </summary>
        public static object? InvokeObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as object
        /// </summary>
        public static object? InvokeObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as object
        /// </summary>
        public static object? InvokeObject(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as byte[] - pass the arguments as an array.
        /// </summary>
        public static byte[]? InvokeByteArrayApply(this SpawnJSHandle _this, object?[] args) => Reflect.ApplyByteArray(_this.JSObject, null, args);
        /// <summary>
        /// Invoke the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this SpawnJSHandle _this) => _this.InvokeByteArrayApply(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this SpawnJSHandle _this, object? arg1) => _this.InvokeByteArrayApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as string - pass the arguments as an array.
        /// </summary>
        public static string? InvokeStringApply(this SpawnJSHandle _this, object?[] args) => Reflect.ApplyString(_this.JSObject, null, args);
        /// <summary>
        /// Invoke the SpawnJSHandle and return as string
        /// </summary>
        public static string? InvokeString(this SpawnJSHandle _this) => _this.InvokeStringApply(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as string
        /// </summary>
        public static string? InvokeString(this SpawnJSHandle _this, object? arg1) => _this.InvokeStringApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as string
        /// </summary>
        public static string? InvokeString(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeStringApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as string
        /// </summary>
        public static string? InvokeString(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as string
        /// </summary>
        public static string? InvokeString(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as string
        /// </summary>
        public static string? InvokeString(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as string
        /// </summary>
        public static string? InvokeString(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as string
        /// </summary>
        public static string? InvokeString(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as string
        /// </summary>
        public static string? InvokeString(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as string
        /// </summary>
        public static string? InvokeString(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as string
        /// </summary>
        public static string? InvokeString(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as int - pass the arguments as an array.
        /// </summary>
        public static int InvokeInt32Apply(this SpawnJSHandle _this, object?[] args) => Reflect.ApplyInt32(_this.JSObject, null, args);
        /// <summary>
        /// Invoke the SpawnJSHandle and return as int
        /// </summary>
        public static int InvokeInt32(this SpawnJSHandle _this) => _this.InvokeInt32Apply(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as int
        /// </summary>
        public static int InvokeInt32(this SpawnJSHandle _this, object? arg1) => _this.InvokeInt32Apply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as int
        /// </summary>
        public static int InvokeInt32(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeInt32Apply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as int
        /// </summary>
        public static int InvokeInt32(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as int
        /// </summary>
        public static int InvokeInt32(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as int
        /// </summary>
        public static int InvokeInt32(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as int
        /// </summary>
        public static int InvokeInt32(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as int
        /// </summary>
        public static int InvokeInt32(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as int
        /// </summary>
        public static int InvokeInt32(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as int
        /// </summary>
        public static int InvokeInt32(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as int
        /// </summary>
        public static int InvokeInt32(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as double - pass the arguments as an array.
        /// </summary>
        public static double InvokeDoubleApply(this SpawnJSHandle _this, object?[] args) => Reflect.ApplyDouble(_this.JSObject, null, args);
        /// <summary>
        /// Invoke the SpawnJSHandle and return as double
        /// </summary>
        public static double InvokeDouble(this SpawnJSHandle _this) => _this.InvokeDoubleApply(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as double
        /// </summary>
        public static double InvokeDouble(this SpawnJSHandle _this, object? arg1) => _this.InvokeDoubleApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as double
        /// </summary>
        public static double InvokeDouble(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeDoubleApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as double
        /// </summary>
        public static double InvokeDouble(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as double
        /// </summary>
        public static double InvokeDouble(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as double
        /// </summary>
        public static double InvokeDouble(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as double
        /// </summary>
        public static double InvokeDouble(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as double
        /// </summary>
        public static double InvokeDouble(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as double
        /// </summary>
        public static double InvokeDouble(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as double
        /// </summary>
        public static double InvokeDouble(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as double
        /// </summary>
        public static double InvokeDouble(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as bool - pass the arguments as an array.
        /// </summary>
        public static bool InvokeBooleanApply(this SpawnJSHandle _this, object?[] args) => Reflect.ApplyBoolean(_this.JSObject, null, args);
        /// <summary>
        /// Invoke the SpawnJSHandle and return as bool
        /// </summary>
        public static bool InvokeBoolean(this SpawnJSHandle _this) => _this.InvokeBooleanApply(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as bool
        /// </summary>
        public static bool InvokeBoolean(this SpawnJSHandle _this, object? arg1) => _this.InvokeBooleanApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as bool
        /// </summary>
        public static bool InvokeBoolean(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeBooleanApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as bool
        /// </summary>
        public static bool InvokeBoolean(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as bool
        /// </summary>
        public static bool InvokeBoolean(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as bool
        /// </summary>
        public static bool InvokeBoolean(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as bool
        /// </summary>
        public static bool InvokeBoolean(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as bool
        /// </summary>
        public static bool InvokeBoolean(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as bool
        /// </summary>
        public static bool InvokeBoolean(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as bool
        /// </summary>
        public static bool InvokeBoolean(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as bool
        /// </summary>
        public static bool InvokeBoolean(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable int - pass the arguments as an array.
        /// </summary>
        public static int? InvokeInt32NullableApply(this SpawnJSHandle _this, object?[] args) => Reflect.ApplyInt32Nullable(_this.JSObject, null, args);
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this SpawnJSHandle _this) => _this.InvokeInt32NullableApply(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this SpawnJSHandle _this, object? arg1) => _this.InvokeInt32NullableApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable double - pass the arguments as an array.
        /// </summary>
        public static double? InvokeDoubleNullableApply(this SpawnJSHandle _this, object?[] args) => Reflect.ApplyDoubleNullable(_this.JSObject, null, args);
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this SpawnJSHandle _this) => _this.InvokeDoubleNullableApply(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this SpawnJSHandle _this, object? arg1) => _this.InvokeDoubleNullableApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable bool - pass the arguments as an array.
        /// </summary>
        public static bool? InvokeBooleanNullableApply(this SpawnJSHandle _this, object?[] args) => Reflect.ApplyBooleanNullable(_this.JSObject, null, args);
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this SpawnJSHandle _this) => _this.InvokeBooleanNullableApply(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this SpawnJSHandle _this, object? arg1) => _this.InvokeBooleanNullableApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization - pass the arguments as an array.
        /// </summary>
        public static T InvokeJsonApply<T>(this SpawnJSHandle _this, object?[] args)
        {
            using var jsObject = Reflect.ApplyJSObject(_this.JSObject, null, args);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json)!;
        }
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this) => _this.InvokeJsonApply<T>(new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, object? arg1) => _this.InvokeJsonApply<T>(new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, object? arg1, object? arg2) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization (with JsonSerializerOptions) - pass the arguments as an array.
        /// </summary>
        public static T InvokeJsonApply<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, object?[] args)
        {
            using var jsObject = Reflect.ApplyJSObject(_this.JSObject, null, args);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
        }
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, object? arg1) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the SpawnJSHandle and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        #endregion
        #region InvokeProperty
        /// <summary>
        /// Invoke the property as a constructor (new) - pass the arguments as an array.
        /// </summary>
        public static SpawnJSHandle InvokePropertyConstructorApply(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            var jsObj = (SpawnJSHandle)Reflect.ConstructJSObject(property!.JSObject, args)!;
            return jsObj;
        }
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokePropertyConstructor(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyConstructorApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokePropertyConstructor(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokePropertyConstructor(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokePropertyConstructor(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokePropertyConstructor(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokePropertyConstructor(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokePropertyConstructor(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokePropertyConstructor(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokePropertyConstructor(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokePropertyConstructor(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static SpawnJSHandle InvokePropertyConstructor(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        public static void InvokePropertyVoidApply(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            Reflect.ApplyVoid(property!.JSObject, _this.JSObject, args);
        }
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyVoidApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property asynchronously - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task InvokePropertyVoidApplyAsync(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            await Reflect.ApplyVoidAsync(property!.JSObject, _this.JSObject, args);
        }
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as object - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object? InvokePropertyObjectApply(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            return Reflect.ApplyObject(property!.JSObject, _this.JSObject, args);
        }
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyObjectApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as SpawnJSHandle - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static SpawnJSHandle? InvokePropertyJSHandleApply(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            return (SpawnJSHandle)Reflect.ApplyJSObject(property!.JSObject, _this.JSObject, args)!;
        }
        /// <summary>
        /// Invoke the property and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokePropertyJSHandle(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyJSHandleApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokePropertyJSHandle(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyJSHandleApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokePropertyJSHandle(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyJSHandleApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokePropertyJSHandle(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyJSHandleApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokePropertyJSHandle(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyJSHandleApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokePropertyJSHandle(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyJSHandleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokePropertyJSHandle(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyJSHandleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokePropertyJSHandle(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyJSHandleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokePropertyJSHandle(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyJSHandleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokePropertyJSHandle(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyJSHandleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as SpawnJSHandle
        /// </summary>
        public static SpawnJSHandle? InvokePropertyJSHandle(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyJSHandleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property asynchronously and return as SpawnJSHandle - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<SpawnJSHandle> InvokePropertyJSHandleApplyAsync(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            return (SpawnJSHandle)(await Reflect.ApplyJSObjectAsync(property!.JSObject, _this.JSObject, args))!;
        }
        /// <summary>
        /// Invoke the property asynchronously and return as SpawnJSHandle
        /// </summary>
        public static Task<SpawnJSHandle> InvokePropertyJSHandleAsync(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyJSHandleApplyAsync(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property asynchronously and return as SpawnJSHandle
        /// </summary>
        public static Task<SpawnJSHandle> InvokePropertyJSHandleAsync(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyJSHandleApplyAsync(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property asynchronously and return as SpawnJSHandle
        /// </summary>
        public static Task<SpawnJSHandle> InvokePropertyJSHandleAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyJSHandleApplyAsync(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property asynchronously and return as SpawnJSHandle
        /// </summary>
        public static Task<SpawnJSHandle> InvokePropertyJSHandleAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyJSHandleApplyAsync(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property asynchronously and return as SpawnJSHandle
        /// </summary>
        public static Task<SpawnJSHandle> InvokePropertyJSHandleAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyJSHandleApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property asynchronously and return as SpawnJSHandle
        /// </summary>
        public static Task<SpawnJSHandle> InvokePropertyJSHandleAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyJSHandleApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property asynchronously and return as SpawnJSHandle
        /// </summary>
        public static Task<SpawnJSHandle> InvokePropertyJSHandleAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyJSHandleApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property asynchronously and return as SpawnJSHandle
        /// </summary>
        public static Task<SpawnJSHandle> InvokePropertyJSHandleAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyJSHandleApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property asynchronously and return as SpawnJSHandle
        /// </summary>
        public static Task<SpawnJSHandle> InvokePropertyJSHandleAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyJSHandleApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property asynchronously and return as SpawnJSHandle
        /// </summary>
        public static Task<SpawnJSHandle> InvokePropertyJSHandleAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyJSHandleApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property asynchronously and return as SpawnJSHandle
        /// </summary>
        public static Task<SpawnJSHandle> InvokePropertyJSHandleAsync(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyJSHandleApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as byte[] - pass the arguments as an array.
        /// </summary>
        public static byte[]? InvokePropertyByteArrayApply(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            return Reflect.ApplyByteArray(property!.JSObject, _this.JSObject, args);
        }
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as string - pass the arguments as an array.
        /// </summary>
        public static string? InvokePropertyStringApply(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            return Reflect.ApplyString(property!.JSObject, _this.JSObject, args);
        }
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyStringApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as int - pass the arguments as an array.
        /// </summary>
        public static int InvokePropertyInt32Apply(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            return Reflect.ApplyInt32(property!.JSObject, _this.JSObject, args);
        }
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyInt32Apply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as double - pass the arguments as an array.
        /// </summary>
        public static double InvokePropertyDoubleApply(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            return Reflect.ApplyDouble(property!.JSObject, _this.JSObject, args);
        }
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyDoubleApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as bool - pass the arguments as an array.
        /// </summary>
        public static bool InvokePropertyBooleanApply(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            return Reflect.ApplyBoolean(property!.JSObject, _this.JSObject, args);
        }
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyBooleanApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as nullable int - pass the arguments as an array.
        /// </summary>
        public static int? InvokePropertyInt32NullableApply(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            return Reflect.ApplyInt32Nullable(property!.JSObject, _this.JSObject, args);
        }
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as nullable double - pass the arguments as an array.
        /// </summary>
        public static double? InvokePropertyDoubleNullableApply(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            return Reflect.ApplyDoubleNullable(property!.JSObject, _this.JSObject, args);
        }
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as nullable bool - pass the arguments as an array.
        /// </summary>
        public static bool? InvokePropertyBooleanNullableApply(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            return Reflect.ApplyBooleanNullable(property!.JSObject, _this.JSObject, args);
        }
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization - pass the arguments as an array.
        /// </summary>
        public static T InvokePropertyJsonApply<T>(this SpawnJSHandle _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            using var jsObject = Reflect.ApplyJSObject(property!.JSObject, _this.JSObject, args);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json)!;
        }
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, string identifier) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, string identifier, object? arg1) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions) - pass the arguments as an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T InvokePropertyJsonApply<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSHandle(identifier);
            using var jsObject = Reflect.ApplyJSObject(property!.JSObject, _this.JSObject, args);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
        }
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        #endregion
    }
}
