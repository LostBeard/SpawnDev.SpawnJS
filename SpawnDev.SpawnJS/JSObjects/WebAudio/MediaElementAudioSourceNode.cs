// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{

    /// <summary>
    /// The MediaElementAudioSourceNode interface represents an audio source consisting of an HTML audio or video element. It is an AudioNode that acts as an audio source.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/MediaElementAudioSourceNode
    /// </summary>
    public class MediaElementAudioSourceNode : AudioNode
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public MediaElementAudioSourceNode(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The MediaElementAudioSourceNode() constructor creates a new MediaElementAudioSourceNode object instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options"></param>
        public MediaElementAudioSourceNode(AudioContext context, MediaElementAudioSourceNodeOptions options) : base(JS.New(nameof(MediaElementAudioSourceNode), context, options)) { }
        /// <summary>
        /// The HTMLMediaElement used when constructing this MediaStreamAudioSourceNode.
        /// </summary>
        public HTMLMediaElement MediaElement => JSRef!.Get<HTMLMediaElement>("mediaElement");
    }
}
