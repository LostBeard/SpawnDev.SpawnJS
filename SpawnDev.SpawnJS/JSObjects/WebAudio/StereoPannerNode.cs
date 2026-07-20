// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The StereoPannerNode interface of the Web Audio API represents a simple stereo panner node that can be used to pan an audio stream left or right. It is an AudioNode that processes a linear audio stream (stereo or mono) as input and outputs a stereo stream (2 channels).<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/StereoPannerNode
    /// </summary>
    public class StereoPannerNode : AudioNode
    {
        /// <inheritdoc/>
        public StereoPannerNode(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// An a-rate AudioParam holding the pan value. A value of -1 represents full left pan, 0 represents the center, and 1 represents full right pan.
        /// </summary>
        public AudioParam Pan => JSRef!.Get<AudioParam>("pan");
    }
}
