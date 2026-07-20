using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The WebGLShader is part of the WebGL API and can either be a vertex or a fragment shader. A WebGLProgram requires both types of shaders.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/WebGLShader
    /// </summary>
    public class WebGLShader : WebGLObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public WebGLShader(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
