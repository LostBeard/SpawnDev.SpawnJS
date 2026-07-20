// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/AudioDestinationNode
    /// </summary>
    public class AudioDestinationNode : AudioNode
    {
        /// <inheritdoc/>
        public AudioDestinationNode(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// An unsigned long defining the maximum number of channels that the physical device can handle.
        /// </summary>
        public long MaxChannelCount => JSRef!.Get<long>("maxChannelCount");
    }
}
