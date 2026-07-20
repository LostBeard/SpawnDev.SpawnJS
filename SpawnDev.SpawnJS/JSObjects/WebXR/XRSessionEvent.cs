// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRSessionEvent
    /// </summary>
    public class XRSessionEvent : Event
    {
        /// <inheritdoc/>
        public XRSessionEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The XRSession to which the event refers.
        /// </summary>
        public XRSession Session => JSRef!.Get<XRSession>("session");
    }
}
