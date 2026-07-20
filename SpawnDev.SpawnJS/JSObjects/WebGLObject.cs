using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.SpawnJSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The WebGLObject is part of the WebGL API and is the parent interface for all WebGL objects.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/WebGLObject
    /// </summary>
    public class WebGLObject : SpawnJSObject
    {
        /// <inheritdoc/>
        public WebGLObject(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
