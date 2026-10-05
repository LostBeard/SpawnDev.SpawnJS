using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The TextEncoderStream interface of the Encoding API converts a stream of strings into bytes in the UTF-8 encoding. It is the streaming equivalent of TextEncoder.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/TextEncoderStream
    /// </summary>
    public class TextEncoderStream : SpawnJSObject
    {
        /// <summary>
        /// The TextEncoderStream() constructor creates a new TextEncoderStream object which is used to convert a stream of strings into bytes using UTF-8 encoding.
        /// </summary>
        public TextEncoderStream() : base(JS.New(nameof(TextEncoderStream))) { }
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public TextEncoderStream(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Always "utf-8".
        /// </summary>
        public string Encoding => JSRef!.Get<string>("encoding");
        /// <summary>
        /// Returns the ReadableStream instance controlled by this object.
        /// </summary>
        public ReadableStream Readable => JSRef!.Get<ReadableStream>("readable");
        /// <summary>
        /// Returns the WritableStream instance controlled by this object.
        /// </summary>
        public WritableStream Writable => JSRef!.Get<WritableStream>("writable");
    }
}
