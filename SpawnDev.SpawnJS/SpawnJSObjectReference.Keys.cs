namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Property enumeration and promise-valued property reads. Both exist in SpawnDev.BlazorJS on
    /// IJSInProcessObjectReference and the ported wrappers call them by the same names.
    /// </summary>
    public partial class SpawnJSObjectReference
    {
        /// <summary>
        /// The object's property names.
        /// </summary>
        /// <param name="hasOwnProperty">
        /// True restricts to the object's own enumerable keys. False also walks the prototype chain, which
        /// is what enumerating a DOM object's API needs - most of a DOM object's members live on its
        /// prototype, not on the instance.
        /// </param>
        public List<string> Keys(bool hasOwnProperty = false)
            => JS.NetRun<List<string>>("objectKeys", new object?[] { JSObject, hasOwnProperty });

        /// <summary>
        /// The property names of the object at the given key
        /// </summary>
        public List<string> Keys(string identifier, bool hasOwnProperty = false)
        {
            using var target = JSHandle.GetPropertyAsJSHandle(identifier);
            return target?.JSObject == null
                ? new List<string>()
                : JS.NetRun<List<string>>("objectKeys", new object?[] { target.JSObject, hasOwnProperty });
        }

        /// <summary>
        /// The property names of the object at the given index
        /// </summary>
        public List<string> Keys(long identifier, bool hasOwnProperty = false)
        {
            using var target = JSHandle.GetPropertyAsJSHandle(identifier);
            return target?.JSObject == null
                ? new List<string>()
                : JS.NetRun<List<string>>("objectKeys", new object?[] { target.JSObject, hasOwnProperty });
        }

        /// <summary>
        /// Awaits a property whose value is a promise, discarding the result.<br/>
        /// Reading the property is what starts the await - the property IS the promise, so this is a read,
        /// not a call.
        /// </summary>
        public Task GetVoidAsync(string identifier)
            => JS.NetRunVoidAsync("getProperty", new object?[] { JSObject, identifier });

        /// <summary>
        /// Awaits a promise valued property at the given index, discarding the result
        /// </summary>
        public Task GetVoidAsync(long identifier)
            => JS.NetRunVoidAsync("getProperty", new object?[] { JSObject, identifier });
    }
}
