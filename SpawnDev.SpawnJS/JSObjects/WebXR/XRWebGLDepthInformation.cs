// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The XRWebGLDepthInformation interface contains depth information from the GPU/WebGL (returned by XRWebGLBinding.getDepthInformation()).<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRWebGLDepthInformation
    /// </summary>
    public class XRWebGLDepthInformation : XRDepthInformation
    {
        /// <inheritdoc/>
        public XRWebGLDepthInformation(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// A WebGLTexture containing depth buffer information as an opaque texture.
        /// </summary>
        public WebGLTexture Texture => JSRef!.Get<WebGLTexture>("texture");
    }
}
