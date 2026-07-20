// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Represents the result of a read operation on a ReadableStream.
    /// </summary>
    public class ReadableStreamReadResult : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public ReadableStreamReadResult(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// A boolean indicating whether the stream has been closed.
        /// </summary>
        public bool Done => JSRef!.Get<bool>("done");

        /// <summary>
        /// The value read from the stream.
        /// </summary>
        /// <remarks>
        /// For BYOB readers, this is typically the TypedArray view that was passed into the Read() method.
        /// </remarks>
        public SpawnJSObject? Value => JSRef!.Get<SpawnJSObject?>("value");
    }
}
