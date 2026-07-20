// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The DetectedFace interface of the Mask Detection API represents a face detected by the FaceDetector.detect() method.
    /// </summary>
    public class DetectedFace : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public DetectedFace(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// A DOMRectReadOnly object indicating the dimensions and position of the detected face.
        /// </summary>
        public DOMRectReadOnly BoundingBox => JSRef!.Get<DOMRectReadOnly>("boundingBox");

        /// <summary>
        /// An array of Landmark objects, each representing a detection of a particular landmark.
        /// </summary>
        public List<SpawnJSObject>? Landmarks => JSRef!.Get<List<SpawnJSObject>?>("landmarks");
    }
}
