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
            _ToString = new Lazy<string>(() => Error.JSRef!.IsUndefined("toString") ? base.ToString() : Error.ToString() ?? base.ToString());
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
