// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRJointPose
    /// </summary>
    public class XRJointPose : XRPose
    {
        /// <inheritdoc/>
        public XRJointPose(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The read-only radius property of the XRJointPose interface indicates the radius (distance from skin) for a joint.
        /// </summary>
        public float Radius => JSRef!.Get<float>("radius");
    }
}
