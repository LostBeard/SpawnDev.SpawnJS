using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    // Contains String keyed property methods
    public partial class SpawnJSObjectReference
    {
        /// <summary>
        /// Get object property as T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public T Get<T>(string identifier)
            => TryGetFast<T>(identifier, out T fast) ? fast : JS.NetRun<T>("getProperty", new object[] { JSObject, identifier });
        /// <summary>
        /// Get object property as T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string identifier) => JS.NetRunAsync<T>("getProperty", new object[] { JSObject, identifier });
        /// <summary>
        /// Check if an object has a property with the specified identifier.<br/>
        /// Resolves dotted paths and null-conditionals the same way Get, Set and Call do, so
        /// <c>Has("navigator.gpu")</c> answers the question you actually asked.
        /// </summary>
        /// <param name="identifier">Property name, or a dotted path such as "navigator.gpu"</param>
        /// <param name="useIn">If true uses the `in` operator (walks the prototype chain), otherwise hasOwnProperty</param>
        public bool Has(string identifier, bool useIn = true)
            => JS.NetRun<bool>("hasProperty", new object[] { JSObject, identifier, useIn });

        /// <summary>
        /// Check if an object has a property with the exact specified key, treating the identifier as
        /// a literal property name rather than a path.<br/>
        /// This is the `in` operator (or hasOwnProperty), so <c>HasDirect("a.b")</c> asks whether a
        /// property literally named "a.b" exists - it does not walk into <c>a</c>.
        /// </summary>
        /// <param name="identifier">A literal property name, never treated as a path</param>
        /// <param name="useIn">If true uses the `in` operator (walks the prototype chain), otherwise hasOwnProperty</param>
        public bool HasDirect(string identifier, bool useIn = true)
            => useIn ? JS.NetRun<bool>("_in", new object[] { identifier, JSObject }) : JS.NetRun<bool>("hasOwnPropertySafe", new object[] { JSObject, identifier });

        /// <summary>
        /// Alias for !Has. Resolves dotted paths.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public bool IsUndefined(string identifier) => !Has(identifier);

        /// <summary>
        /// Set an object property to the specified value
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public void Set(string identifier, object? value)
        {
            if (TrySetFast(identifier, value)) return;
            JS.NetRunVoid("setProperty", new object[] { JSObject, identifier, value! });
        }

        /// <summary>
        /// Delete an object property
        /// </summary>
        /// <param name="identifier"></param>
        public void Delete(string identifier) => JS.NetRunVoid("deleteProperty", new object[] { JSObject, identifier });

        /// <summary>
        /// Copy a property from this object to another object
        /// </summary>
        public void CopyPropertyTo(string srcIdentifier, JSObject destObj, string destIdentifier) => JS.NetRunVoid("copyProperty", new object[] { JSObject, srcIdentifier, destObj, destIdentifier });
        /// <summary>
        /// Copy a property from this object to another object
        /// </summary>
        public void CopyPropertyTo(string srcIdentifier, JSObject destObj) => JS.NetRunVoid("copyProperty", new object[] { JSObject, srcIdentifier, destObj, srcIdentifier });
        /// <summary>
        /// Move a property from this object to another object
        /// </summary>
        public void MovePropertyTo(string srcIdentifier, JSObject destObj, string destIdentifier) => JS.NetRunVoid("moveProperty", new object[] { JSObject, srcIdentifier, destObj, destIdentifier });
        /// <summary>
        /// Move a property from this object to another object
        /// </summary>
        public void MovePropertyTo(string srcIdentifier, JSObject destObj) => JS.NetRunVoid("moveProperty", new object[] { JSObject, srcIdentifier, destObj, srcIdentifier });
        /// <summary>
        /// Copy an object property from another object to this object
        /// </summary>
        public void CopyPropertyFrom(string destIdentifier, JSObject srcObj, string srcIdentifier) => JS.NetRunVoid("copyProperty", new object[] { srcObj, srcIdentifier, JSObject, destIdentifier });
        /// <summary>
        /// Copy an object property from another object to this object
        /// </summary>
        public void CopyPropertyFrom(JSObject srcObj, string srcIdentifier) => JS.NetRunVoid("copyProperty", new object[] { srcObj, srcIdentifier, JSObject, srcIdentifier });
        /// <summary>
        /// Move an object property from another object to this object
        /// </summary>
        public void MovePropertyFrom(string destIdentifier, JSObject srcObj, string srcIdentifier) => JS.NetRunVoid("moveProperty", new object[] { srcObj, destIdentifier, JSObject, srcIdentifier });
        /// <summary>
        /// Move an object property from another object to this object
        /// </summary>
        public void MovePropertyFrom(JSObject srcObj, string srcIdentifier) => JS.NetRunVoid("moveProperty", new object[] { srcObj, srcIdentifier, JSObject, srcIdentifier });

        #region New-SpawnJSObjectReference
        /// <summary>
        /// Invoke a property constructor and return as type T
        /// </summary>
        public SpawnJSObjectReference New(string identifier)
            => NewApply(identifier, new object?[] { });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1)
            => NewApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2)
            => NewApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference NewApply(string identifier, object?[] args) => JS.NetRun<SpawnJSObjectReference>("invokePropertyConstructor", new object[] { JSObject, identifier, args });
        #endregion

        #region New-T
        /// <summary>
        /// Invoke a property constructor and return as type T
        /// </summary>
        public T New<T>(string identifier)
            => NewApply<T>(identifier, new object?[] { });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1)
            => NewApply<T>(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2)
            => NewApply<T>(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T NewApply<T>(string identifier, object?[] args) => JS.NetRun<T>("invokePropertyConstructor", new object[] { JSObject, identifier, args });
        #endregion

        #region Call
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier)
            => CallApply<T>(identifier, new object?[] { });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1)
            => CallApply<T>(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2)
            => CallApply<T>(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Invoke a property and return as type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T CallApply<T>(string identifier, object?[] args) => JS.NetRun<T>("invokeProperty", new object[] { JSObject, identifier, args });
        #endregion

        #region CallVoid
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier)
            => CallVoidApply(identifier, new object?[] { });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1)
            => CallVoidApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2)
            => CallVoidApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Invoke a property
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        public void CallVoidApply(string identifier, object?[] args) => JS.NetRunVoid("invokeProperty", new object[] { JSObject, identifier, args });
        #endregion

        #region CallVoidAsync
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier)
            => CallVoidApplyAsync(identifier, new object?[] { });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1)
            => CallVoidApplyAsync(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Invoke a property
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        public Task CallVoidApplyAsync(string identifier, object?[] args) => JS.NetRunVoidAsync("invokeProperty", new object[] { JSObject, identifier, args });
        #endregion

        #region CallAsync
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier)
            => CallApplyAsync<T>(identifier, new object?[] { });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1)
            => CallApplyAsync<T>(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Invoke a property
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        public Task<T> CallApplyAsync<T>(string identifier, object?[] args) => JS.NetRunAsync<T>("invokeProperty", new object[] { JSObject, identifier, args });
        #endregion
    }
}
