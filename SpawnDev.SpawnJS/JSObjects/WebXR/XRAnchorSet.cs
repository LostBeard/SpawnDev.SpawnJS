// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRAnchorSet
    /// </summary>
    public class XRAnchorSet : Set<XRAnchor>
    {
        /// <inheritdoc/>
        public XRAnchorSet(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public XRAnchorSet(IEnumerable<XRAnchor> iterable) : base(JS.New("Set", iterable)) { }
    }
}
