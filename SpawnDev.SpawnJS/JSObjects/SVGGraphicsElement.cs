
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The SVGGraphicsElement interface represents SVG elements whose primary purpose is to directly render graphics into a group.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/SVGGraphicsElement
    /// </summary>
    public class SVGGraphicsElement : SVGElement
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public SVGGraphicsElement(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
