// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The DecompressionStream interface of the Compression Streams API is an API for decompressing a stream of data.
    /// </summary>
    public class DecompressionStream : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public DecompressionStream(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Creates a new DecompressionStream.
        /// </summary>
        /// <param name="format">The compression format. One of: "gzip", "deflate", "deflate-raw".</param>
        public DecompressionStream(string format) : base(JS.New(nameof(DecompressionStream), format)) { }

        /// <summary>
        /// Returns a ReadableStream instance that can be used to read decompressed data from the stream.
        /// </summary>
        public ReadableStream Readable => JSRef!.Get<ReadableStream>("readable");

        /// <summary>
        /// Returns a WritableStream instance that can be used to write compressed data to the stream.
        /// </summary>
        public WritableStream Writable => JSRef!.Get<WritableStream>("writable");
    }
}
