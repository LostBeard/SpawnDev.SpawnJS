// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/BaseAudioContext/createDelay
    /// </summary>
    public class DelayNode : AudioNode
    {
        /// <inheritdoc/>
        public DelayNode(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Crates a new instance
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options"></param>
        public DelayNode(BaseAudioContext context, DelayNodeOptions? options = null) : base(options == null ? JS.New(nameof(DelayNode), context) : JS.New(nameof(DelayNode), context, options)) { }
        /// <summary>
        /// An a-rate AudioParam representing the amount of delay to apply, specified in seconds.
        /// </summary>
        public AudioParam DelayTime => JSRef!.Get<AudioParam>("delayTime");
    }
}
