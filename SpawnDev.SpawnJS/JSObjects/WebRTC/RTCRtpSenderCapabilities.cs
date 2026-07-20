// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The RTCRtpSenderCapabilities dictionary provides information about the capabilities of an RTCRtpSender.
    /// </summary>
    public class RTCRtpSenderCapabilities
    {
        /// <summary>
        /// An array of objects, each describing one of the codecs supported by the RTCRtpSender.
        /// </summary>
        public List<RTCRtpTransceiverCodec> Codecs { get; set; }
        /// <summary>
        /// An array of objects, each providing the URI of a supported RTP header extension.
        /// </summary>
        public List<RTCRtpSenderCapabilityHeaderExtension> HeaderExtensions { get; set; }
    }
}
