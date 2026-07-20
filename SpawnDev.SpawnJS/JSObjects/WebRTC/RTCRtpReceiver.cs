// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The RTCRtpReceiver interface manages the reception and decoding of data for a MediaStreamTrack on an RTCPeerConnection.
    /// </summary>
    public class RTCRtpReceiver : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public RTCRtpReceiver(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The MediaStreamTrack which is being received and decoded by the RTCRtpReceiver.
        /// </summary>
        public MediaStreamTrack Track => JSRef!.Get<MediaStreamTrack>("track");
        /// <summary>
        /// The RTCDtlsTransport instance that is used for receiving the media.
        /// </summary>
        public RTCDtlsTransport Transport => JSRef!.Get<RTCDtlsTransport>("transport");
        /// <summary>
        /// The static method RTCRtpReceiver.getCapabilities() returns an object describing the codec and header extension capabilities supported by RTCRtpReceiver objects on the current device.
        /// </summary>
        /// <param name="kind">A string indicating the type of media for which the browser's receiver capabilities are requested. The supported media kinds are: audio and video.</param>
        /// <returns>A new object that indicates what capabilities the browser has for receiving the specified media kind over an RTCPeerConnection. If the browser doesn't have any support for the given media kind, the returned value is null.</returns>
        public static RTCRtpSenderCapabilities GetCapabilities(string kind) => JS.Call<RTCRtpSenderCapabilities>("RTCRtpReceiver.getCapabilities", kind);
    }
}
