// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRAnchor
    /// </summary>
    public class XRAnchor : SpawnJSObject
    {
        /// <inheritdoc/>
        public XRAnchor(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns an XRSpace object to locate the anchor relative to other XRSpace objects.
        /// </summary>
        public XRSpace AnchorSpace => JSRef!.Get<XRSpace>("anchorSpace");
        /// <summary>
        /// Removes the anchor.
        /// </summary>
        /// <returns></returns>
        public Task<XRAnchor> Delete() => JSRef!.CallAsync<XRAnchor>("delete");
    }
}
