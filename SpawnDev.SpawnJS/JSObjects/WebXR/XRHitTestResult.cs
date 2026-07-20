// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRHitTestResult
    /// </summary>
    public class XRHitTestResult : SpawnJSObject
    {
        /// <inheritdoc/>
        public XRHitTestResult(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The createAnchor() method of the XRHitTestResult interface creates an XRAnchor from a hit test result that is attached to a real-world object.
        /// </summary>
        /// <returns></returns>
        public Task<XRAnchor> CreateAnchor() => JSRef!.CallAsync<XRAnchor>("createAnchor");
        /// <summary>
        /// The getPose() method of the XRHitTestResult interface returns the XRPose of the hit test result relative to the given base space.
        /// </summary>
        /// <param name="baseSpace">An XRSpace to use as the base or origin for computing the relative position and orientation of hit test results.</param>
        /// <returns></returns>
        public XRPose GetPose(XRSpace baseSpace) => JSRef!.Call<XRPose>("getPose", baseSpace);
    }
}
