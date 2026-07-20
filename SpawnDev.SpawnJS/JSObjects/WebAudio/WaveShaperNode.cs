// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The WaveShaperNode interface represents a non-linear distorter. It is an AudioNode that use a curve to apply a wave shaping distortion to the signal. Beside obvious distortion effects, it is often used to add a warm feeling to the signal.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/WaveShaperNode
    /// </summary>
    public class WaveShaperNode : AudioNode
    {
        /// <inheritdoc/>
        public WaveShaperNode(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// A Float32Array of numbers describing the distortion to apply.
        /// </summary>
        public Float32Array? Curve { get => JSRef!.Get<Float32Array?>("curve"); set => JSRef!.Set("curve", value); }
        /// <summary>
        /// A string indicating whether to oversample the signal or not.
        /// </summary>
        public string Oversample { get => JSRef!.Get<string>("oversample"); set => JSRef!.Set("oversample", value); }
    }
}
