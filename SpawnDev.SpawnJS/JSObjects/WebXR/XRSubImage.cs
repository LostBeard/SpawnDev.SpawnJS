using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The XRSubImage interface of the WebXR Device API represents what viewport of the GPU texture to use for rendering.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRSubImage
    /// </summary>
    public class XRSubImage : SpawnJSObject
    {
        /// <inheritdoc/>
        public XRSubImage(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The XRViewport used when rendering the sub image.
        /// </summary>
        public XRViewport Viewport => JSRef!.Get<XRViewport>("viewport");
    }
}
