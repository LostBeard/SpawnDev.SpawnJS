namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Long keyed property access, plus two shapes the ported wrappers use that had no SpawnJS equivalent.
    /// </summary>
    public partial class SpawnJSObjectReference
    {
        /// <summary>
        /// Reads an indexed property using a long index.<br/>
        /// TypedArray indexes with a long because a typed array can hold more elements than an int can
        /// address, and a Javascript array index is a double anyway - narrowing to int here would cap the
        /// addressable range for no reason.
        /// </summary>
        public T Get<T>(long identifier) => JS.NetRun<T>("getProperty", new object?[] { JSObject, identifier });

        /// <summary>
        /// Writes an indexed property using a long index.<br/>
        /// Deliberately NOT generic, mirroring the int keyed Set(int, object?). A generic
        /// Set&lt;T&gt;(long, T) makes every Set(0, someObject) call ambiguous, because an int literal
        /// converts to both int and long and the generic form matches the value equally well.
        /// </summary>
        public void Set(long identifier, object? value) => JS.NetRunVoid("setProperty", new object?[] { JSObject, identifier, value });

        /// <summary>
        /// The constructor name of the value at the given key, for example "Uint8Array".<br/>
        /// Note this is the value's own constructor name, which for a subclass of a built in is the
        /// derived name - unlike Object.prototype.toString, which reports the base.
        /// </summary>
        public string? ConstructorName(string identifier)
        {
            var names = JS.GetPropertyConstructorNames(JSObject, identifier);
            return names.Length > 0 ? names[0] : null;
        }

        /// <summary>
        /// Calls a method with no arguments, awaiting the promise it returns.<br/>
        /// The Apply named members take a pre-built argument array; this no argument form exists so a
        /// wrapper can spell the call the same way whether or not it passes anything.
        /// </summary>
        public Task<T> CallApplyAsync<T>(string identifier) => CallApplyAsync<T>(identifier, new object?[] { });
    }
}
