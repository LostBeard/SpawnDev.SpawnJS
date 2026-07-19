using SpawnDev.SpawnJS.Native;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;

namespace SpawnDev.SpawnJS.Extensions
{
    /// <summary>
    /// Low level JSObject extension methods for invoking the JSObject or its properties.
    /// Prefer SpawnJSObjectReference / SpawnJSObject for normal use - these are for when you must work at the JSObject level.
    /// </summary>
    public static partial class JSObjectExtensions
    {
        #region Invoke
        /// <summary>
        /// Invoke the JSObject as a constructor (new) - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static JSObject? InvokeConstructorApply(this JSObject _this, object?[] args) => Reflect.ConstructJSObject(_this, args);
        /// <summary>
        /// Invoke the JSObject as a constructor (new)
        /// </summary>
        public static JSObject? InvokeConstructor(this JSObject _this) => _this.InvokeConstructorApply(new object?[] { });
        /// <summary>
        /// Invoke the JSObject as a constructor (new)
        /// </summary>
        public static JSObject? InvokeConstructor(this JSObject _this, object? arg1) => _this.InvokeConstructorApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject as a constructor (new)
        /// </summary>
        public static JSObject? InvokeConstructor(this JSObject _this, object? arg1, object? arg2) => _this.InvokeConstructorApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject as a constructor (new)
        /// </summary>
        public static JSObject? InvokeConstructor(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject as a constructor (new)
        /// </summary>
        public static JSObject? InvokeConstructor(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject as a constructor (new)
        /// </summary>
        public static JSObject? InvokeConstructor(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject as a constructor (new)
        /// </summary>
        public static JSObject? InvokeConstructor(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject as a constructor (new)
        /// </summary>
        public static JSObject? InvokeConstructor(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject as a constructor (new)
        /// </summary>
        public static JSObject? InvokeConstructor(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject as a constructor (new)
        /// </summary>
        public static JSObject? InvokeConstructor(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject as a constructor (new)
        /// </summary>
        public static JSObject? InvokeConstructor(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeConstructorApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        public static void InvokeVoidApply(this JSObject _this, object?[] args) => Reflect.ApplyVoid(_this, null, args);
        /// <summary>
        /// Invoke the JSObject
        /// </summary>
        public static void InvokeVoid(this JSObject _this) => _this.InvokeVoidApply(new object?[] { });
        /// <summary>
        /// Invoke the JSObject
        /// </summary>
        public static void InvokeVoid(this JSObject _this, object? arg1) => _this.InvokeVoidApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject
        /// </summary>
        public static void InvokeVoid(this JSObject _this, object? arg1, object? arg2) => _this.InvokeVoidApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject
        /// </summary>
        public static void InvokeVoid(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject
        /// </summary>
        public static void InvokeVoid(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject
        /// </summary>
        public static void InvokeVoid(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject
        /// </summary>
        public static void InvokeVoid(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject
        /// </summary>
        public static void InvokeVoid(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject
        /// </summary>
        public static void InvokeVoid(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject
        /// </summary>
        public static void InvokeVoid(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject
        /// </summary>
        public static void InvokeVoid(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeVoidApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject and return as JSObject - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static JSObject? InvokeJSObjectApply(this JSObject _this, object?[] args) => Reflect.ApplyJSObject(_this, null, args);
        /// <summary>
        /// Invoke the JSObject and return as JSObject
        /// </summary>
        public static JSObject? InvokeJSObject(this JSObject _this) => _this.InvokeJSObjectApply(new object?[] { });
        /// <summary>
        /// Invoke the JSObject and return as JSObject
        /// </summary>
        public static JSObject? InvokeJSObject(this JSObject _this, object? arg1) => _this.InvokeJSObjectApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject and return as JSObject
        /// </summary>
        public static JSObject? InvokeJSObject(this JSObject _this, object? arg1, object? arg2) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject and return as JSObject
        /// </summary>
        public static JSObject? InvokeJSObject(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject and return as JSObject
        /// </summary>
        public static JSObject? InvokeJSObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject and return as JSObject
        /// </summary>
        public static JSObject? InvokeJSObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject and return as JSObject
        /// </summary>
        public static JSObject? InvokeJSObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject and return as JSObject
        /// </summary>
        public static JSObject? InvokeJSObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject and return as JSObject
        /// </summary>
        public static JSObject? InvokeJSObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject and return as JSObject
        /// </summary>
        public static JSObject? InvokeJSObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject and return as JSObject
        /// </summary>
        public static JSObject? InvokeJSObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeJSObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject and return as object - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object? InvokeObjectApply(this JSObject _this, object?[] args) => Reflect.ApplyObject(_this, null, args);
        /// <summary>
        /// Invoke the JSObject and return as object
        /// </summary>
        public static object? InvokeObject(this JSObject _this) => _this.InvokeObjectApply(new object?[] { });
        /// <summary>
        /// Invoke the JSObject and return as object
        /// </summary>
        public static object? InvokeObject(this JSObject _this, object? arg1) => _this.InvokeObjectApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject and return as object
        /// </summary>
        public static object? InvokeObject(this JSObject _this, object? arg1, object? arg2) => _this.InvokeObjectApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject and return as object
        /// </summary>
        public static object? InvokeObject(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject and return as object
        /// </summary>
        public static object? InvokeObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject and return as object
        /// </summary>
        public static object? InvokeObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject and return as object
        /// </summary>
        public static object? InvokeObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject and return as object
        /// </summary>
        public static object? InvokeObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject and return as object
        /// </summary>
        public static object? InvokeObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject and return as object
        /// </summary>
        public static object? InvokeObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject and return as object
        /// </summary>
        public static object? InvokeObject(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeObjectApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject and return as byte[] - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static byte[]? InvokeByteArrayApply(this JSObject _this, object?[] args) => Reflect.ApplyByteArray(_this, null, args);
        /// <summary>
        /// Invoke the JSObject and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this JSObject _this) => _this.InvokeByteArrayApply(new object?[] { });
        /// <summary>
        /// Invoke the JSObject and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this JSObject _this, object? arg1) => _this.InvokeByteArrayApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this JSObject _this, object? arg1, object? arg2) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject and return as byte[]
        /// </summary>
        public static byte[]? InvokeByteArray(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeByteArrayApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject and return as string - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string? InvokeStringApply(this JSObject _this, object?[] args) => Reflect.ApplyString(_this, null, args);
        /// <summary>
        /// Invoke the JSObject and return as string
        /// </summary>
        public static string? InvokeString(this JSObject _this) => _this.InvokeStringApply(new object?[] { });
        /// <summary>
        /// Invoke the JSObject and return as string
        /// </summary>
        public static string? InvokeString(this JSObject _this, object? arg1) => _this.InvokeStringApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject and return as string
        /// </summary>
        public static string? InvokeString(this JSObject _this, object? arg1, object? arg2) => _this.InvokeStringApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject and return as string
        /// </summary>
        public static string? InvokeString(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject and return as string
        /// </summary>
        public static string? InvokeString(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject and return as string
        /// </summary>
        public static string? InvokeString(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject and return as string
        /// </summary>
        public static string? InvokeString(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject and return as string
        /// </summary>
        public static string? InvokeString(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject and return as string
        /// </summary>
        public static string? InvokeString(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject and return as string
        /// </summary>
        public static string? InvokeString(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject and return as string
        /// </summary>
        public static string? InvokeString(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeStringApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject and return as int - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int InvokeInt32Apply(this JSObject _this, object?[] args) => Reflect.ApplyInt32(_this, null, args);
        /// <summary>
        /// Invoke the JSObject and return as int
        /// </summary>
        public static int InvokeInt32(this JSObject _this) => _this.InvokeInt32Apply(new object?[] { });
        /// <summary>
        /// Invoke the JSObject and return as int
        /// </summary>
        public static int InvokeInt32(this JSObject _this, object? arg1) => _this.InvokeInt32Apply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject and return as int
        /// </summary>
        public static int InvokeInt32(this JSObject _this, object? arg1, object? arg2) => _this.InvokeInt32Apply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject and return as int
        /// </summary>
        public static int InvokeInt32(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject and return as int
        /// </summary>
        public static int InvokeInt32(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject and return as int
        /// </summary>
        public static int InvokeInt32(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject and return as int
        /// </summary>
        public static int InvokeInt32(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject and return as int
        /// </summary>
        public static int InvokeInt32(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject and return as int
        /// </summary>
        public static int InvokeInt32(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject and return as int
        /// </summary>
        public static int InvokeInt32(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject and return as int
        /// </summary>
        public static int InvokeInt32(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeInt32Apply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject and return as double - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static double InvokeDoubleApply(this JSObject _this, object?[] args) => Reflect.ApplyDouble(_this, null, args);
        /// <summary>
        /// Invoke the JSObject and return as double
        /// </summary>
        public static double InvokeDouble(this JSObject _this) => _this.InvokeDoubleApply(new object?[] { });
        /// <summary>
        /// Invoke the JSObject and return as double
        /// </summary>
        public static double InvokeDouble(this JSObject _this, object? arg1) => _this.InvokeDoubleApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject and return as double
        /// </summary>
        public static double InvokeDouble(this JSObject _this, object? arg1, object? arg2) => _this.InvokeDoubleApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject and return as double
        /// </summary>
        public static double InvokeDouble(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject and return as double
        /// </summary>
        public static double InvokeDouble(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject and return as double
        /// </summary>
        public static double InvokeDouble(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject and return as double
        /// </summary>
        public static double InvokeDouble(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject and return as double
        /// </summary>
        public static double InvokeDouble(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject and return as double
        /// </summary>
        public static double InvokeDouble(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject and return as double
        /// </summary>
        public static double InvokeDouble(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject and return as double
        /// </summary>
        public static double InvokeDouble(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeDoubleApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject and return as bool - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool InvokeBooleanApply(this JSObject _this, object?[] args) => Reflect.ApplyBoolean(_this, null, args);
        /// <summary>
        /// Invoke the JSObject and return as bool
        /// </summary>
        public static bool InvokeBoolean(this JSObject _this) => _this.InvokeBooleanApply(new object?[] { });
        /// <summary>
        /// Invoke the JSObject and return as bool
        /// </summary>
        public static bool InvokeBoolean(this JSObject _this, object? arg1) => _this.InvokeBooleanApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject and return as bool
        /// </summary>
        public static bool InvokeBoolean(this JSObject _this, object? arg1, object? arg2) => _this.InvokeBooleanApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject and return as bool
        /// </summary>
        public static bool InvokeBoolean(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject and return as bool
        /// </summary>
        public static bool InvokeBoolean(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject and return as bool
        /// </summary>
        public static bool InvokeBoolean(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject and return as bool
        /// </summary>
        public static bool InvokeBoolean(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject and return as bool
        /// </summary>
        public static bool InvokeBoolean(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject and return as bool
        /// </summary>
        public static bool InvokeBoolean(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject and return as bool
        /// </summary>
        public static bool InvokeBoolean(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject and return as bool
        /// </summary>
        public static bool InvokeBoolean(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeBooleanApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject and return as nullable int - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int? InvokeInt32NullableApply(this JSObject _this, object?[] args) => Reflect.ApplyInt32Nullable(_this, null, args);
        /// <summary>
        /// Invoke the JSObject and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this JSObject _this) => _this.InvokeInt32NullableApply(new object?[] { });
        /// <summary>
        /// Invoke the JSObject and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this JSObject _this, object? arg1) => _this.InvokeInt32NullableApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this JSObject _this, object? arg1, object? arg2) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject and return as nullable int
        /// </summary>
        public static int? InvokeInt32Nullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeInt32NullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject and return as nullable double - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static double? InvokeDoubleNullableApply(this JSObject _this, object?[] args) => Reflect.ApplyDoubleNullable(_this, null, args);
        /// <summary>
        /// Invoke the JSObject and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this JSObject _this) => _this.InvokeDoubleNullableApply(new object?[] { });
        /// <summary>
        /// Invoke the JSObject and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this JSObject _this, object? arg1) => _this.InvokeDoubleNullableApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this JSObject _this, object? arg1, object? arg2) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject and return as nullable double
        /// </summary>
        public static double? InvokeDoubleNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeDoubleNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject and return as nullable bool - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool? InvokeBooleanNullableApply(this JSObject _this, object?[] args) => Reflect.ApplyBooleanNullable(_this, null, args);
        /// <summary>
        /// Invoke the JSObject and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this JSObject _this) => _this.InvokeBooleanNullableApply(new object?[] { });
        /// <summary>
        /// Invoke the JSObject and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this JSObject _this, object? arg1) => _this.InvokeBooleanNullableApply(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this JSObject _this, object? arg1, object? arg2) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject and return as nullable bool
        /// </summary>
        public static bool? InvokeBooleanNullable(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeBooleanNullableApply(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization - pass the arguments as an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T InvokeJsonApply<T>(this JSObject _this, object?[] args)
        {
            using var jsObject = Reflect.ApplyJSObject(_this, null, args);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json)!;
        }
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this) => _this.InvokeJsonApply<T>(new object?[] { });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, object? arg1) => _this.InvokeJsonApply<T>(new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, object? arg1, object? arg2) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, object? arg1, object? arg2, object? arg3) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeJsonApply<T>(new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization (with JsonSerializerOptions) - pass the arguments as an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T InvokeJsonApply<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, object?[] args)
        {
            using var jsObject = Reflect.ApplyJSObject(_this, null, args);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
        }
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, object? arg1) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the JSObject and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokeJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokeJsonApply<T>(jsonSerializerOptions, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        #endregion
        #region InvokeProperty
        /// <summary>
        /// Invoke the property as a constructor (new) - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static JSObject InvokePropertyConstructorApply(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            var jsObj = Reflect.ConstructJSObject(property!, args);
            return jsObj!;
        }
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static JSObject InvokePropertyConstructor(this JSObject _this, string identifier) => _this.InvokePropertyConstructorApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static JSObject InvokePropertyConstructor(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static JSObject InvokePropertyConstructor(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static JSObject InvokePropertyConstructor(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static JSObject InvokePropertyConstructor(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static JSObject InvokePropertyConstructor(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static JSObject InvokePropertyConstructor(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static JSObject InvokePropertyConstructor(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static JSObject InvokePropertyConstructor(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static JSObject InvokePropertyConstructor(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property as a constructor (new)
        /// </summary>
        public static JSObject InvokePropertyConstructor(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyConstructorApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        public static void InvokePropertyVoidApply(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            Reflect.ApplyVoid(property!, _this, args);
        }
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this JSObject _this, string identifier) => _this.InvokePropertyVoidApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property
        /// </summary>
        public static void InvokePropertyVoid(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property asynchronously - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task InvokePropertyVoidApplyAsync(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            await Reflect.ApplyVoidAsync(property!, _this, args);
        }
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this JSObject _this, string identifier) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property asynchronously
        /// </summary>
        public static Task InvokePropertyVoidAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as object - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object? InvokePropertyObjectApply(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            return Reflect.ApplyObject(property!, _this, args);
        }
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this JSObject _this, string identifier) => _this.InvokePropertyObjectApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as object
        /// </summary>
        public static object? InvokePropertyObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as JSObject - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static JSObject? InvokePropertyJSObjectApply(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            return Reflect.ApplyJSObject(property!, _this, args);
        }
        /// <summary>
        /// Invoke the property and return as JSObject
        /// </summary>
        public static JSObject? InvokePropertyJSObject(this JSObject _this, string identifier) => _this.InvokePropertyJSObjectApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as JSObject
        /// </summary>
        public static JSObject? InvokePropertyJSObject(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyJSObjectApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as JSObject
        /// </summary>
        public static JSObject? InvokePropertyJSObject(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyJSObjectApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as JSObject
        /// </summary>
        public static JSObject? InvokePropertyJSObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyJSObjectApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as JSObject
        /// </summary>
        public static JSObject? InvokePropertyJSObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyJSObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as JSObject
        /// </summary>
        public static JSObject? InvokePropertyJSObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyJSObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as JSObject
        /// </summary>
        public static JSObject? InvokePropertyJSObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyJSObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as JSObject
        /// </summary>
        public static JSObject? InvokePropertyJSObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyJSObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as JSObject
        /// </summary>
        public static JSObject? InvokePropertyJSObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyJSObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as JSObject
        /// </summary>
        public static JSObject? InvokePropertyJSObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyJSObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as JSObject
        /// </summary>
        public static JSObject? InvokePropertyJSObject(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyJSObjectApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property asynchronously and return as JSObject - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<JSObject> InvokePropertyJSObjectApplyAsync(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            return (await Reflect.ApplyJSObjectAsync(property!, _this, args))!;
        }
        /// <summary>
        /// Invoke the property asynchronously and return as JSObject
        /// </summary>
        public static Task<JSObject> InvokePropertyJSObjectAsync(this JSObject _this, string identifier) => _this.InvokePropertyJSObjectApplyAsync(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property asynchronously and return as JSObject
        /// </summary>
        public static Task<JSObject> InvokePropertyJSObjectAsync(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyJSObjectApplyAsync(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property asynchronously and return as JSObject
        /// </summary>
        public static Task<JSObject> InvokePropertyJSObjectAsync(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyJSObjectApplyAsync(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property asynchronously and return as JSObject
        /// </summary>
        public static Task<JSObject> InvokePropertyJSObjectAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyJSObjectApplyAsync(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property asynchronously and return as JSObject
        /// </summary>
        public static Task<JSObject> InvokePropertyJSObjectAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyJSObjectApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property asynchronously and return as JSObject
        /// </summary>
        public static Task<JSObject> InvokePropertyJSObjectAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyJSObjectApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property asynchronously and return as JSObject
        /// </summary>
        public static Task<JSObject> InvokePropertyJSObjectAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyJSObjectApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property asynchronously and return as JSObject
        /// </summary>
        public static Task<JSObject> InvokePropertyJSObjectAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyJSObjectApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property asynchronously and return as JSObject
        /// </summary>
        public static Task<JSObject> InvokePropertyJSObjectAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyJSObjectApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property asynchronously and return as JSObject
        /// </summary>
        public static Task<JSObject> InvokePropertyJSObjectAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyJSObjectApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property asynchronously and return as JSObject
        /// </summary>
        public static Task<JSObject> InvokePropertyJSObjectAsync(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyJSObjectApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as byte[] - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static byte[]? InvokePropertyByteArrayApply(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            return Reflect.ApplyByteArray(property!, _this, args);
        }
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this JSObject _this, string identifier) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as byte[]
        /// </summary>
        public static byte[]? InvokePropertyByteArray(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyByteArrayApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as string - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string? InvokePropertyStringApply(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            return Reflect.ApplyString(property!, _this, args);
        }
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this JSObject _this, string identifier) => _this.InvokePropertyStringApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as string
        /// </summary>
        public static string? InvokePropertyString(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyStringApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as int - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int InvokePropertyInt32Apply(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            return Reflect.ApplyInt32(property!, _this, args);
        }
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this JSObject _this, string identifier) => _this.InvokePropertyInt32Apply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as int
        /// </summary>
        public static int InvokePropertyInt32(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyInt32Apply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as double - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static double InvokePropertyDoubleApply(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            return Reflect.ApplyDouble(property!, _this, args);
        }
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this JSObject _this, string identifier) => _this.InvokePropertyDoubleApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as double
        /// </summary>
        public static double InvokePropertyDouble(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyDoubleApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as bool - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool InvokePropertyBooleanApply(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            return Reflect.ApplyBoolean(property!, _this, args);
        }
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this JSObject _this, string identifier) => _this.InvokePropertyBooleanApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as bool
        /// </summary>
        public static bool InvokePropertyBoolean(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyBooleanApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as nullable int - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int? InvokePropertyInt32NullableApply(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            return Reflect.ApplyInt32Nullable(property!, _this, args);
        }
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this JSObject _this, string identifier) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as nullable int
        /// </summary>
        public static int? InvokePropertyInt32Nullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyInt32NullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as nullable double - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static double? InvokePropertyDoubleNullableApply(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            return Reflect.ApplyDoubleNullable(property!, _this, args);
        }
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this JSObject _this, string identifier) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as nullable double
        /// </summary>
        public static double? InvokePropertyDoubleNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyDoubleNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as nullable bool - pass the arguments as an array.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool? InvokePropertyBooleanNullableApply(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            return Reflect.ApplyBooleanNullable(property!, _this, args);
        }
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this JSObject _this, string identifier) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as nullable bool
        /// </summary>
        public static bool? InvokePropertyBooleanNullable(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyBooleanNullableApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization - pass the arguments as an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T InvokePropertyJsonApply<T>(this JSObject _this, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            using var jsObject = Reflect.ApplyJSObject(property!, _this, args);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json)!;
        }
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, string identifier) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, string identifier, object? arg1) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, string identifier, object? arg1, object? arg2) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyJsonApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions) - pass the arguments as an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T InvokePropertyJsonApply<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object?[] args)
        {
            using var property = _this.GetPropertyAsJSObject(identifier);
            using var jsObject = Reflect.ApplyJSObject(property!, _this, args);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
        }
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Invoke the property and return as T using JSON serialization (with JsonSerializerOptions)
        /// </summary>
        public static T InvokePropertyJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10) => _this.InvokePropertyJsonApply<T>(jsonSerializerOptions, identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        #endregion
    }
}
