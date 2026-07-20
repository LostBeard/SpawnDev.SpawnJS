// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Returned by ReadableStreamDefaultReader.Read
    /// </summary>
    public class ReadableStreamReaderReadResponse : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public ReadableStreamReaderReadResponse(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// True if there is no more data to read
        /// </summary>
        public bool Done => JSRef!.Get<bool>("done");
        /// <summary>
        /// The current chunk if not done
        /// </summary>
        public Uint8Array? Value => JSRef!.Get<Uint8Array?>("value");
    }
}
