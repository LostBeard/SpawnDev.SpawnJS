namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Creates an instance representing an error that occurs regarding the global function eval().<br/>
    /// This exception is no longer thrown by Javascript, but the object remains for compatibility.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/EvalError
    /// </summary>
    public class EvalError : Error
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public EvalError(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Creates a new EvalError
        /// </summary>
        public EvalError() : base(JS.New(nameof(EvalError))) { }
        /// <summary>
        /// Creates a new EvalError
        /// </summary>
        /// <param name="message">A human-readable description of the error.</param>
        public EvalError(string message) : base(JS.New(nameof(EvalError), message)) { }
    }
    /// <summary>
    /// Creates an instance representing an error that occurs when a numeric variable or parameter is outside its valid range.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RangeError
    /// </summary>
    public class RangeError : Error
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public RangeError(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Creates a new RangeError
        /// </summary>
        public RangeError() : base(JS.New(nameof(RangeError))) { }
        /// <summary>
        /// Creates a new RangeError
        /// </summary>
        /// <param name="message">A human-readable description of the error.</param>
        public RangeError(string message) : base(JS.New(nameof(RangeError), message)) { }
    }
    /// <summary>
    /// Creates an instance representing an error that occurs when a non-existent variable is referenced.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/ReferenceError
    /// </summary>
    public class ReferenceError : Error
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public ReferenceError(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Creates a new ReferenceError
        /// </summary>
        public ReferenceError() : base(JS.New(nameof(ReferenceError))) { }
        /// <summary>
        /// Creates a new ReferenceError
        /// </summary>
        /// <param name="message">A human-readable description of the error.</param>
        public ReferenceError(string message) : base(JS.New(nameof(ReferenceError), message)) { }
    }
    /// <summary>
    /// Creates an instance representing a syntax error thrown when Javascript tries to interpret syntactically invalid code.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError
    /// </summary>
    public class SyntaxError : Error
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public SyntaxError(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Creates a new SyntaxError
        /// </summary>
        public SyntaxError() : base(JS.New(nameof(SyntaxError))) { }
        /// <summary>
        /// Creates a new SyntaxError
        /// </summary>
        /// <param name="message">A human-readable description of the error.</param>
        public SyntaxError(string message) : base(JS.New(nameof(SyntaxError), message)) { }
    }
    /// <summary>
    /// Creates an instance representing an error that occurs when a value is not of the expected type.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError
    /// </summary>
    public class TypeError : Error
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public TypeError(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Creates a new TypeError
        /// </summary>
        public TypeError() : base(JS.New(nameof(TypeError))) { }
        /// <summary>
        /// Creates a new TypeError
        /// </summary>
        /// <param name="message">A human-readable description of the error.</param>
        public TypeError(string message) : base(JS.New(nameof(TypeError), message)) { }
    }
    /// <summary>
    /// Creates an instance representing an error that occurs when a global URI handling function is used in a wrong way.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/URIError
    /// </summary>
    public class URIError : Error
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public URIError(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Creates a new URIError
        /// </summary>
        public URIError() : base(JS.New(nameof(URIError))) { }
        /// <summary>
        /// Creates a new URIError
        /// </summary>
        /// <param name="message">A human-readable description of the error.</param>
        public URIError(string message) : base(JS.New(nameof(URIError), message)) { }
    }
}
