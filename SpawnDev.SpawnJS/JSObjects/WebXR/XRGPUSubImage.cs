// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The XRGPUSubImage interface provides the GPU textures for rendering a view in an XR session.<br/>
    /// This is the WebGPU counterpart to XRWebGLSubImage.<br/>
    /// https://immersive-web.github.io/WebXR-WebGPU-Binding/
    /// </summary>
    public class XRGPUSubImage : XRSubImage
    {
        /// <inheritdoc/>
        public XRGPUSubImage(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The GPU texture to render the color attachment into for this view.
        /// </summary>
        public GPUTexture ColorTexture => JSRef!.Get<GPUTexture>("colorTexture");
        /// <summary>
        /// The GPU texture for the depth/stencil attachment, or null if not requested.
        /// </summary>
        public GPUTexture? DepthStencilTexture => JSRef!.Get<GPUTexture?>("depthStencilTexture");
        // Viewport is inherited from XRSubImage. It was redeclared here identically - same type, same
        // Javascript key, same body - which only hid the base member (CS0108) without changing behaviour.
    }
}
