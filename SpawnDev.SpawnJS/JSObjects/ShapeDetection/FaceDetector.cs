
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The FaceDetector interface of the Face Detection API detects faces in images.
    /// </summary>
    public class FaceDetector : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public FaceDetector(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Creates a new FaceDetector object.
        /// </summary>
        public FaceDetector(FaceDetectorOptions? options = null) : base(JS.New(nameof(FaceDetector), options)) { }

        /// <summary>
        /// Detects faces in the specified image.
        /// </summary>
        /// <param name="imageBitmapSource">The image to detect faces in.</param>
        /// <returns>A Promise that returns an array of DetectedFace objects.</returns>
        public Task<List<DetectedFace>> Detect(Union<Blob, Element, ImageData, ImageBitmap, OffscreenCanvas> imageBitmapSource) => JSRef!.CallAsync<global::SpawnDev.SpawnJS.Union<global::SpawnDev.SpawnJS.JSObjects.Blob, global::SpawnDev.SpawnJS.JSObjects.Element, global::SpawnDev.SpawnJS.JSObjects.ImageData, global::SpawnDev.SpawnJS.JSObjects.ImageBitmap, global::SpawnDev.SpawnJS.JSObjects.OffscreenCanvas>, List<DetectedFace>>("detect", imageBitmapSource);
    }
}
