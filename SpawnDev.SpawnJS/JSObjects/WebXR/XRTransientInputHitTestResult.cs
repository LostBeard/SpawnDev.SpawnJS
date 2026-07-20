// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The XRTransientInputHitTestResult interface of the WebXR Device API contains an array of results of a hit test for transient input, grouped by input source.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRTransientInputHitTestResult
    /// </summary>
    public class XRTransientInputHitTestResult : SpawnJSObject
    {
        /// <inheritdoc/>
        public XRTransientInputHitTestResult(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns the XRInputSource that was used to compute the results array.
        /// </summary>
        public XRInputSource InputSource => JSRef!.Get<XRInputSource>("inputSource");
        /// <summary>
        /// Returns an array of XRHitTestResult objects containing the hit test results for the input source, ordered by distance along the ray used to perform the hit test, with the closest result at position 0.
        /// </summary>
        public XRHitTestResult[] Results => JSRef!.Get<XRHitTestResult[]>("results");
    }
}
