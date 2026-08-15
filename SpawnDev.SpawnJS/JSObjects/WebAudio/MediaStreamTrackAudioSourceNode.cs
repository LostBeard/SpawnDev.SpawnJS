
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The MediaStreamTrackAudioSourceNode interface is a type of AudioNode which represents a source of audio data taken from a specific MediaStreamTrack obtained through the WebRTC or Media Capture and Streams APIs.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/MediaStreamTrackAudioSourceNode
    /// </summary>
    public class MediaStreamTrackAudioSourceNode : AudioNode
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public MediaStreamTrackAudioSourceNode(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
