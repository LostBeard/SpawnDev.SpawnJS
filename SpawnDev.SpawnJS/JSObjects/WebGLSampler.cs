using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.SpawnJSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The WebGLSampler interface is part of the WebGL 2 API and stores sampling parameters for WebGLTexture access inside of a shader.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/WebGLSampler
    /// </summary>
    public class WebGLSampler : WebGLObject
    {
        /// <inheritdoc/>
        public WebGLSampler(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
