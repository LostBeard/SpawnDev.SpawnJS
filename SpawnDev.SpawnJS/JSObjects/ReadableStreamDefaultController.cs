// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ReadableStreamDefaultController interface of the Streams API represents a controller allowing control of a ReadableStream's state and internal queue. Default controllers are for streams that are not byte streams.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ReadableStreamDefaultController
    /// </summary>
    public class ReadableStreamDefaultController : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public ReadableStreamDefaultController(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Closes the associated stream.
        /// </summary>
        public void Close() => JSRef!.CallVoid("close");
        /// <summary>
        /// Enqueues a given chunk in the associated stream.
        /// </summary>
        public void Enqueue(object data) => JSRef!.CallVoid("enqueue", data);
        /// <summary>
        /// Causes any future interactions with the associated stream to error.
        /// </summary>
        public void Error() => JSRef!.CallVoid("error");
        /// <summary>
        /// Causes any future interactions with the associated stream to error.
        /// </summary>
        public void Error(object error) => JSRef!.CallVoid("error", error);
    }
}
