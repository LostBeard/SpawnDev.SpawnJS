using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.SpawnJSObjectReferenceAny
{
    /// <summary>
    /// Adds extension methods to SpawnJSObjectReference that allow using object as the property key
    /// </summary>
    public static class SpawnJSObjectReferenceKeyString
    {
        /// <summary>
        /// Get object property as T
        /// </summary>
        public static T Get<T>(this SpawnJSObjectReference _this, object identifier) => _this.JS.NetRun<T>("getProperty", new object[] { _this.JSObject, identifier });
        /// <summary>
        /// Get object property as T
        /// </summary>
        public static Task<T> GetAsync<T>(this SpawnJSObjectReference _this, object identifier) => _this.JS.NetRunAsync<T>("getProperty", new object[] { _this.JSObject, identifier });
        /// <summary>
        /// Check if an object has a property with the specified identifier
        /// </summary>
        public static bool Has(this SpawnJSObjectReference _this, object identifier, bool useIn = true)
            => useIn ? _this.JS.NetRun<bool>("_in", new object[] { identifier, _this.JSObject }) : _this.JS.NetRun<bool>("hasOwnPropertySafe", new object[] { _this.JSObject, identifier });

        /// <summary>
        /// Set an object property to the specified value
        /// </summary>
        public static void Set(this SpawnJSObjectReference _this, object identifier, object? value) => _this.JS.NetRunVoid("setProperty", new object[] { _this.JSObject, identifier, value! });

        /// <summary>
        /// Delete an object property
        /// </summary>
        public static void Delete(this SpawnJSObjectReference _this, object identifier) => _this.JS.NetRunVoid("deleteProperty", new object[] { _this.JSObject, identifier });

        /// <summary>
        /// Copy a property from this object to another object
        /// </summary>
        public static void CopyPropertyTo(this SpawnJSObjectReference _this, int srcIdentifier, JSObject destObj, int destIdentifier) => _this.JS.NetRunVoid("copyProperty", new object[] { _this.JSObject, srcIdentifier, destObj, destIdentifier });
        /// <summary>
        /// Copy a property from this object to another object
        /// </summary>
        public static void CopyPropertyTo(this SpawnJSObjectReference _this, int srcIdentifier, JSObject destObj) => _this.JS.NetRunVoid("copyProperty", new object[] { _this.JSObject, srcIdentifier, destObj, srcIdentifier });
        /// <summary>
        /// Move a property from this object to another object
        /// </summary>
        public static void MovePropertyTo(this SpawnJSObjectReference _this, int srcIdentifier, JSObject destObj, int destIdentifier) => _this.JS.NetRunVoid("moveProperty", new object[] { _this.JSObject, srcIdentifier, destObj, destIdentifier });
        /// <summary>
        /// Move a property from this object to another object
        /// </summary>
        public static void MovePropertyTo(this SpawnJSObjectReference _this, int srcIdentifier, JSObject destObj) => _this.JS.NetRunVoid("moveProperty", new object[] { _this.JSObject, srcIdentifier, destObj, srcIdentifier });
        /// <summary>
        /// Copy an object property from another object to this object
        /// </summary>
        public static void CopyPropertyFrom(this SpawnJSObjectReference _this, int destIdentifier, JSObject srcObj, int srcIdentifier) => _this.JS.NetRunVoid("copyProperty", new object[] { srcObj, srcIdentifier, _this.JSObject, destIdentifier });
        /// <summary>
        /// Copy an object property from another object to this object
        /// </summary>
        public static void CopyPropertyFrom(this SpawnJSObjectReference _this, JSObject srcObj, int srcIdentifier) => _this.JS.NetRunVoid("copyProperty", new object[] { srcObj, srcIdentifier, _this.JSObject, srcIdentifier });
        /// <summary>
        /// Move an object property from another object to this object
        /// </summary>
        public static void MovePropertyFrom(this SpawnJSObjectReference _this, int destIdentifier, JSObject srcObj, int srcIdentifier) => _this.JS.NetRunVoid("moveProperty", new object[] { srcObj, destIdentifier, _this.JSObject, srcIdentifier });
        /// <summary>
        /// Move an object property from another object to this object
        /// </summary>
        public static void MovePropertyFrom(this SpawnJSObjectReference _this, JSObject srcObj, int srcIdentifier) => _this.JS.NetRunVoid("moveProperty", new object[] { srcObj, srcIdentifier, _this.JSObject, srcIdentifier });

        #region New-SpawnJSObjectReference
        /// <summary>
        /// Invoke a property constructor and return as type T
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier)
            => _this.NewApply(identifier, new object?[] { });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1)
            => _this.NewApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2)
            => _this.NewApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference New(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => _this.NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static SpawnJSObjectReference NewApply(this SpawnJSObjectReference _this, object identifier, object?[] args) => _this.JS.NetRun<SpawnJSObjectReference>("invokePropertyConstructor", new object[] { _this.JSObject, identifier, args });
        #endregion

        #region New-T
        /// <summary>
        /// Invoke a property constructor and return as type T
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier)
            => _this.NewApply<T>(identifier, new object?[] { });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1)
            => _this.NewApply<T>(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T New<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => _this.NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public static T NewApply<T>(this SpawnJSObjectReference _this, object identifier, object?[] args) => _this.JS.NetRun<T>("invokePropertyConstructor", new object[] { _this.JSObject, identifier, args });
        #endregion

        #region Call
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier)
            => _this.CallApply<T>(identifier, new object?[] { });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1)
            => _this.CallApply<T>(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static T Call<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => _this.CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Invoke a property and return as type T
        /// </summary>
        public static T CallApply<T>(this SpawnJSObjectReference _this, object identifier, object?[] args) => _this.JS.NetRun<T>("invokeProperty", new object[] { _this.JSObject, identifier, args });
        #endregion

        #region CallVoid
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier)
            => _this.CallVoidApply(identifier, new object?[] { });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1)
            => _this.CallVoidApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static void CallVoid(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => _this.CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Invoke a property
        /// </summary>
        public static void CallVoidApply(this SpawnJSObjectReference _this, object identifier, object?[] args) => _this.JS.NetRunVoid("invokeProperty", new object[] { _this.JSObject, identifier, args });
        #endregion

        #region CallVoidAsync
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier)
            => _this.CallVoidApplyAsync(identifier, new object?[] { });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task CallVoidAsync(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => _this.CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Invoke a property
        /// </summary>
        public static Task CallVoidApplyAsync(this SpawnJSObjectReference _this, object identifier, object?[] args) => _this.JS.NetRunVoidAsync("invokeProperty", new object[] { _this.JSObject, identifier, args });
        #endregion

        #region CallAsync
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier)
            => _this.CallApplyAsync<T>(identifier, new object?[] { });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property
        /// </summary>
        public static Task<T> CallAsync<T>(this SpawnJSObjectReference _this, object identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => _this.CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Invoke a property
        /// </summary>
        public static Task<T> CallApplyAsync<T>(this SpawnJSObjectReference _this, object identifier, object?[] args) => _this.JS.NetRunAsync<T>("invokeProperty", new object[] { _this.JSObject, identifier, args });
        #endregion
    }
}
