// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The XRHand interface is pair iterator (an ordered map) with the key being the hand joints and the value being an XRJointSpace.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRHand
    /// </summary>
    public class XRHand : Map<string, XRJointSpace>
    {
        /// <inheritdoc/>
        public XRHand(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
