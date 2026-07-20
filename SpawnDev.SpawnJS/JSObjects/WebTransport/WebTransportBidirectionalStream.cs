// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The WebTransportBidirectionalStream interface of the WebTransport API represents a bidirectional stream created by a WebTransport.
    /// </summary>
    public class WebTransportBidirectionalStream : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public WebTransportBidirectionalStream(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// The readable property of the WebTransportBidirectionalStream interface returns a WebTransportReceiveStream instance that can be used to read data from the bidirectional stream.
        /// </summary>
        public WebTransportReceiveStream Readable => JSRef!.Get<WebTransportReceiveStream>("readable");
        /// <summary>
        /// The writable property of the WebTransportBidirectionalStream interface returns a WebTransportSendStream instance that can be used to write data to the bidirectional stream.
        /// </summary>
        public WebTransportSendStream Writable => JSRef!.Get<WebTransportSendStream>("writable");
    }
}
