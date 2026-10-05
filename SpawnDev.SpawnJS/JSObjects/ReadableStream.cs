
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ReadableStream interface of the Streams API represents a readable stream of byte data. The Fetch API offers a concrete instance of a ReadableStream through the body property of a Response object.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream
    /// </summary>
    [Transferable(TransferRequired = true)]
    public class ReadableStream : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public ReadableStream(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Creates and returns a readable stream object from the given handlers.
        /// </summary>
        public ReadableStream() : base(JS.New(nameof(ReadableStream))) { }
        /// <summary>
        /// Creates and returns a readable stream object from the given handlers.
        /// </summary>
        /// <param name="underlyingSource"></param>
        public ReadableStream(ReadableStreamUnderlyingSource underlyingSource) : base(JS.New(nameof(ReadableStream), underlyingSource)) { }
        /// <summary>
        /// Returns a boolean indicating whether or not the readable stream is locked to a reader.
        /// </summary>
        public bool Locked => JSRef!.Get<bool>("locked");
        /// <summary>
        /// Returns a Promise that resolves when the stream is canceled. Calling this method signals a loss of interest in the stream by a consumer. The supplied reason argument will be given to the underlying source, which may or may not use it.
        /// </summary>
        /// <returns></returns>
        public Task Cancel() => JSRef!.CallVoidAsync("cancel");
        /// <summary>
        /// Returns a Promise that resolves when the stream is canceled. Calling this method signals a loss of interest in the stream by a consumer. The supplied reason argument will be given to the underlying source, which may or may not use it.
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public Task Cancel(string reason) => JSRef!.CallVoidAsync("cancel", reason);
        /// <summary>
        /// Creates a reader and locks the stream to it. While the stream is locked, no other reader can be acquired until this one is released.
        /// </summary>
        /// <returns></returns>
        public ReadableStreamDefaultReader GetReader() => JSRef!.Call<ReadableStreamDefaultReader>("getReader");
        /// <summary>
        /// Creates a reader and locks the stream to it. While the stream is locked, no other reader can be acquired until this one is released.
        /// </summary>
        /// <returns></returns>
        public ReadableStreamBYOBReader GetReaderBYOB() => JSRef!.Call<ReadableStreamGetReaderOptions, ReadableStreamBYOBReader>("getReader", new ReadableStreamGetReaderOptions { Mode = "byob" });
        /// <summary>
        /// The pipeTo() method of the ReadableStream interface pipes the current ReadableStream to a given WritableStream and returns a Promise that fulfills when the piping process completes successfully, or rejects if any errors were encountered.<br/>
        /// Piping a stream will generally lock it for the duration of the pipe, preventing other readers from locking it.<br/>
        /// </summary>
        /// <param name="destination">A WritableStream that acts as the final destination for the ReadableStream.</param>
        /// <returns>A Promise that resolves when the piping process has completed.</returns>
        public Task PipeTo(WritableStream destination) => JSRef!.CallVoidAsync("pipeTo", destination);
        /// <summary>
        /// The pipeTo() method of the ReadableStream interface pipes the current ReadableStream to a given WritableStream and returns a Promise that fulfills when the piping process completes successfully, or rejects if any errors were encountered.<br/>
        /// Piping a stream will generally lock it for the duration of the pipe, preventing other readers from locking it.<br/>
        /// </summary>
        /// <param name="destination">A WritableStream that acts as the final destination for the ReadableStream.</param>
        /// <param name="options">PipeToOptions</param>
        /// <returns>A Promise that resolves when the piping process has completed.</returns>
        public Task PipeTo(WritableStream destination, PipeToOptions options) => JSRef!.CallVoidAsync("pipeTo", destination, options);
        /// <summary>
        /// The pipeThrough() method of the ReadableStream interface provides a chainable way of piping the current stream through a transform stream or any other writable/readable pair.<br/>
        /// Piping a stream will generally lock it for the duration of the pipe, preventing other readers from locking it.
        /// </summary>
        /// <param name="transformStream">A TransformStream (or an object with the structure {writable, readable}) consisting of a readable stream and a writable stream working together to transform some data from one form to another. Data written to the writable stream can be read in some transformed state by the readable stream. For example, a TextDecoder, has bytes written to it and strings read from it, while a video decoder has encoded bytes written to it and uncompressed video frames read from it.</param>
        /// <returns>The readable side of the transformStream.</returns>
        public ReadableStream PipeThrough(TransformStream transformStream) => JSRef!.Call<TransformStream, ReadableStream>("pipeThrough", transformStream);
        /// <summary>
        /// The pipeThrough() method of the ReadableStream interface provides a chainable way of piping the current stream through a transform stream or any other writable/readable pair.<br/>
        /// Piping a stream will generally lock it for the duration of the pipe, preventing other readers from locking it.
        /// </summary>
        /// <param name="transformStream">A TransformStream (or an object with the structure {writable, readable}) consisting of a readable stream and a writable stream working together to transform some data from one form to another. Data written to the writable stream can be read in some transformed state by the readable stream. For example, a TextDecoder, has bytes written to it and strings read from it, while a video decoder has encoded bytes written to it and uncompressed video frames read from it.</param>
        /// <param name="options">The options that should be used when piping to the writable stream.</param>
        /// <returns>The readable side of the transformStream.</returns>
        public ReadableStream PipeThrough(TransformStream transformStream, PipeThroughOptions options) => JSRef!.Call<TransformStream, PipeThroughOptions, ReadableStream>("pipeThrough", transformStream, options);
        /// <summary>
        /// Pipes this stream through a CompressionStream: pipeThrough() takes any {writable, readable} pair, and a CompressionStream is one
        /// (it is not a TransformStream, in Javascript or here). Returns the readable side, the compressed bytes.
        /// </summary>
        /// <param name="transformStream">The CompressionStream to pipe through.</param>
        /// <returns>The readable side of the CompressionStream.</returns>
        public ReadableStream PipeThrough(CompressionStream transformStream) => JSRef!.Call<CompressionStream, ReadableStream>("pipeThrough", transformStream);
        /// <summary>
        /// Pipes this stream through a CompressionStream with pipe options. Returns the readable side, the compressed bytes.
        /// </summary>
        /// <param name="transformStream">The CompressionStream to pipe through.</param>
        /// <param name="options">The options that should be used when piping to the writable stream.</param>
        /// <returns>The readable side of the CompressionStream.</returns>
        public ReadableStream PipeThrough(CompressionStream transformStream, PipeThroughOptions options) => JSRef!.Call<CompressionStream, PipeThroughOptions, ReadableStream>("pipeThrough", transformStream, options);
        /// <summary>
        /// Pipes this stream through a DecompressionStream: pipeThrough() takes any {writable, readable} pair, and a DecompressionStream is one
        /// (it is not a TransformStream, in Javascript or here). Returns the readable side, the decompressed bytes.
        /// </summary>
        /// <param name="transformStream">The DecompressionStream to pipe through.</param>
        /// <returns>The readable side of the DecompressionStream.</returns>
        public ReadableStream PipeThrough(DecompressionStream transformStream) => JSRef!.Call<DecompressionStream, ReadableStream>("pipeThrough", transformStream);
        /// <summary>
        /// Pipes this stream through a DecompressionStream with pipe options. Returns the readable side, the decompressed bytes.
        /// </summary>
        /// <param name="transformStream">The DecompressionStream to pipe through.</param>
        /// <param name="options">The options that should be used when piping to the writable stream.</param>
        /// <returns>The readable side of the DecompressionStream.</returns>
        public ReadableStream PipeThrough(DecompressionStream transformStream, PipeThroughOptions options) => JSRef!.Call<DecompressionStream, PipeThroughOptions, ReadableStream>("pipeThrough", transformStream, options);
        /// <summary>
        /// Pipes this stream through a TextDecoderStream: pipeThrough() takes any {writable, readable} pair, and a TextDecoderStream is one
        /// (it is not a TransformStream, in Javascript or here). Returns the readable side, the decoded strings.
        /// </summary>
        /// <param name="transformStream">The TextDecoderStream to pipe through.</param>
        /// <returns>The readable side of the TextDecoderStream.</returns>
        public ReadableStream PipeThrough(TextDecoderStream transformStream) => JSRef!.Call<TextDecoderStream, ReadableStream>("pipeThrough", transformStream);
        /// <summary>
        /// Pipes this stream through a TextDecoderStream with pipe options. Returns the readable side, the decoded strings.
        /// </summary>
        /// <param name="transformStream">The TextDecoderStream to pipe through.</param>
        /// <param name="options">The options that should be used when piping to the writable stream.</param>
        /// <returns>The readable side of the TextDecoderStream.</returns>
        public ReadableStream PipeThrough(TextDecoderStream transformStream, PipeThroughOptions options) => JSRef!.Call<TextDecoderStream, PipeThroughOptions, ReadableStream>("pipeThrough", transformStream, options);
        /// <summary>
        /// Pipes this stream through a TextEncoderStream: pipeThrough() takes any {writable, readable} pair, and a TextEncoderStream is one
        /// (it is not a TransformStream, in Javascript or here). Returns the readable side, the UTF-8 bytes.
        /// </summary>
        /// <param name="transformStream">The TextEncoderStream to pipe through.</param>
        /// <returns>The readable side of the TextEncoderStream.</returns>
        public ReadableStream PipeThrough(TextEncoderStream transformStream) => JSRef!.Call<TextEncoderStream, ReadableStream>("pipeThrough", transformStream);
        /// <summary>
        /// Pipes this stream through a TextEncoderStream with pipe options. Returns the readable side, the UTF-8 bytes.
        /// </summary>
        /// <param name="transformStream">The TextEncoderStream to pipe through.</param>
        /// <param name="options">The options that should be used when piping to the writable stream.</param>
        /// <returns>The readable side of the TextEncoderStream.</returns>
        public ReadableStream PipeThrough(TextEncoderStream transformStream, PipeThroughOptions options) => JSRef!.Call<TextEncoderStream, PipeThroughOptions, ReadableStream>("pipeThrough", transformStream, options);
        /// <summary>
        /// The tee() method of the ReadableStream interface tees the current readable stream, returning a two-element array containing the two resulting branches as new ReadableStream instances.
        /// This is useful for allowing two readers to read a stream sequentially or simultaneously, perhaps at different speeds.For example, you might do this in a ServiceWorker if you want to fetch a response from the server and stream it to the browser, but also stream it to the ServiceWorker cache.Since a response body cannot be consumed more than once, you'd need two copies to do this.
        /// A teed stream will partially signal backpressure at the rate of the faster consumer of the two ReadableStream branches, and unread data is enqueued internally on the slower consumed ReadableStream without any limit or backpressure. That is, when both branches have an unread element in their internal queue, then the original ReadableStream's controller's internal queue will start to fill up, and once its desiredSize ≤ 0 or byte stream controller desiredSize ≤ 0, then the controller will stop calling pull(controller) on the underlying source passed to ReadableStream(). If only one branch is consumed, then the entire body will be enqueued in memory.Therefore, you should not use the built-in tee() to read very large streams in parallel at different speeds.Instead, search for an implementation that fully backpressures to the speed of the slower consumed branch.
        /// To cancel the stream you then need to cancel both resulting branches. Teeing a stream will generally lock it for the duration, preventing other readers from locking it.
        /// </summary>
        /// <returns>An Array containing two ReadableStream instances.</returns>
        public ReadableStream[] Tee() => JSRef!.Call<ReadableStream[]>("tee");

    }
}
