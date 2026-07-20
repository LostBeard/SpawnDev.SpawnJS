using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The RTCCertificate interface provides an object representing a certificate used by an RTCPeerConnection for authentication.
    /// </summary>
    public class RTCCertificate : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public RTCCertificate(SpawnJSObjectReference _ref) : base(_ref) { }
    }
    /// <summary>
    /// The RTCIdentityAssertion interface represents an identity assertion which can be used to authenticate an RTCPeerConnection's remote peer.
    /// </summary>
    public class RTCIdentityAssertion : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public RTCIdentityAssertion(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
