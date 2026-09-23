namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A .Net exception that represents a Javascript Error and makes the Error information available if needed
    /// </summary>
    public class JSException : Exception
    {
        /// <summary>
        /// Returns the error message
        /// </summary>
        public override string Message => _Message.Value;
        /// <summary>
        /// Returns the Error type name
        /// </summary>
        public string Name => _Name.Value;
        /// <summary>
        /// Returns the Error toString() value
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _ToString.Value;
        private Lazy<string> _Message;
        private Lazy<string> _Name;
        private Lazy<string> _ToString;
        /// <summary>
        /// The Javascript Error this exception represents
        /// </summary>
        public Error? Error { get; private set; }
        /// <summary>
        /// Creates a new Exception to represent a Javascript Error
        /// </summary>
        public JSException(Error error) : base()
        {
            Error = error;
            _Message = new Lazy<string>(() => Error.Message ?? "");
            _Name = new Lazy<string>(() => Error.Name ?? "");
            _ToString = new Lazy<string>(() => !Error.JSRef!.Has("toString") ? base.ToString() : Error.ToString() ?? base.ToString());
        }
        /// <summary>
        /// Builds the exception for an async interop call's error string. The JS side sends a NAMED error
        /// (Error subclasses, DOMException, OverconstrainedError) as "\u0001" + name + "\u0002" + message so
        /// the name survives; anything else is the plain message.
        /// </summary>
        internal static JSException FromInteropError(string error)
        {
            if (error.Length > 1 && error[0] == '\u0001')
            {
                var sep = error.IndexOf('\u0002');
                if (sep > 1)
                {
                    var name = error.Substring(1, sep - 1);
                    var message = error.Substring(sep + 1);
                    return new JSException(string.IsNullOrEmpty(message) ? name : message, name);
                }
            }
            return new JSException(error);
        }
        /// <summary>
        /// Creates a new Exception to represent a Javascript Error
        /// </summary>
        public JSException(string message, string? name = null) : base()
        {
            _Message = new Lazy<string>(message);
            _Name = new Lazy<string>(() => name ?? "");
            _ToString = new Lazy<string>(string.IsNullOrEmpty(name) ? $"{message}" : $"{name}: {message}");
        }
    }
}
