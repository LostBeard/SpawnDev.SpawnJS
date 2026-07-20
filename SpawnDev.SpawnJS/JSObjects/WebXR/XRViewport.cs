// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRViewport
    /// </summary>
    public class XRViewport : SpawnJSObject
    {
        /// <inheritdoc/>
        public XRViewport(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The offset from the origin of the destination graphics surface (typically a XRWebGLLayer) to the left edge of the viewport, in pixels.
        /// </summary>
        public int X => JSRef!.Get<int>("x");
        /// <summary>
        /// The offset from the origin of the viewport to the bottom edge of the viewport; WebGL's coordinate system places (0, 0) at the bottom left corner of the surface.
        /// </summary>
        public int Y => JSRef!.Get<int>("y");
        /// <summary>
        /// The width, in pixels, of the viewport.
        /// </summary>
        public int Width => JSRef!.Get<int>("width");
        /// <summary>
        /// The height, in pixels, of the viewport.
        /// </summary>
        public int Height => JSRef!.Get<int>("height");
    }
}
