// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The IIRFilterNode interface of the Web Audio API is a AudioNode processor which implements a general infinite impulse response (IIR) filter; this type of filter can be used to implement tone control devices and graphic equalizers as well. It lets the parameters of the feedforward and feedback coefficients be specified, so that other types of filters can be implemented.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/IIRFilterNode
    /// </summary>
    public class IIRFilterNode : AudioNode
    {
        /// <inheritdoc/>
        public IIRFilterNode(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns a Float32Array containing the frequency response for the specified number of frequencies.
        /// </summary>
        public void GetFrequencyResponse(Float32Array frequencyHz, Float32Array magResponse, Float32Array phaseResponse) => JSRef!.CallVoid("getFrequencyResponse", frequencyHz, magResponse, phaseResponse);
    }
}
