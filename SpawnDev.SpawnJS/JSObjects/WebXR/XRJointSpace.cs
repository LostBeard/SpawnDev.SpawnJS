// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRJointSpace
    /// </summary>
    public class XRJointSpace : XRSpace
    {
        /// <inheritdoc/>
        public XRJointSpace(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The name of the joint that is tracked. See XRHand for possible hand joint names.
        /// </summary>
        public string JointName => JSRef!.Get<string>("jointName");
    }
}
