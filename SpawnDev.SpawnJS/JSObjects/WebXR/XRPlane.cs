// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The XRPlane interface of the WebXR Device API describes a detected flat surface in the real world. An XRPlane is detected by the underlying XR system and represents a polygon on a surface.<br/>
    /// https://immersive-web.github.io/real-world-geometry/plane-detection.html#xrplane-interface
    /// </summary>
    public class XRPlane : SpawnJSObject
    {
        /// <inheritdoc/>
        public XRPlane(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns an XRSpace tracking the plane's position and orientation.
        /// </summary>
        public XRSpace PlaneSpace => JSRef!.Get<XRSpace>("planeSpace");
        /// <summary>
        /// Returns an array of DOMPointReadOnly objects representing the vertices of the plane's polygon, in the coordinate system of planeSpace.
        /// </summary>
        public DOMPointReadOnly[] Polygon => JSRef!.Get<DOMPointReadOnly[]>("polygon");
        /// <summary>
        /// Returns a DOMHighResTimeStamp indicating the last time the plane's data was updated.
        /// </summary>
        public double LastChangedTime => JSRef!.Get<double>("lastChangedTime");
        /// <summary>
        /// Returns a string indicating the semantic label of the plane (e.g., "floor", "wall", "ceiling"), or null if not available.
        /// </summary>
        public string? SemanticLabel => JSRef!.Get<string?>("semanticLabel");
        /// <summary>
        /// Returns a string indicating the orientation of the plane. Values include "horizontal" and "vertical".
        /// </summary>
        public string? Orientation => JSRef!.Get<string?>("orientation");
    }
}
