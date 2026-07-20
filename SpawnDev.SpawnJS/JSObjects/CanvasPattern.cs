// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The CanvasPattern interface represents an opaque object describing a pattern, based on a image, a canvas or a video, created by the CanvasRenderingContext2D.createPattern() method.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/CanvasPattern
    /// </summary>
    public class CanvasPattern : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public CanvasPattern(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The CanvasPattern.setTransform() method of the Canvas 2D API sets the transformation matrix that will be used when rendering the pattern.
        /// </summary>
        /// <param name="matrix"></param>
        public void SetTransform(DOMMatrix matrix) => JSRef!.CallVoid("setTransform", matrix);
    }
}
