
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The TextDetector interface of the Text Detection API detects text in images.
    /// </summary>
    public class TextDetector : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public TextDetector(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Creates a new TextDetector object.
        /// </summary>
        public TextDetector() : base(JS.New(nameof(TextDetector))) { }

        /// <summary>
        /// Detects text in the specified image.
        /// </summary>
        /// <param name="imageBitmapSource">The image to detect text in.</param>
        /// <returns>A Promise that returns an array of DetectedText objects.</returns>
        public Task<List<DetectedText>> Detect(Union<Blob, Element, ImageData, ImageBitmap, OffscreenCanvas> imageBitmapSource) => JSRef!.CallAsync<global::SpawnDev.SpawnJS.Union<global::SpawnDev.SpawnJS.JSObjects.Blob, global::SpawnDev.SpawnJS.JSObjects.Element, global::SpawnDev.SpawnJS.JSObjects.ImageData, global::SpawnDev.SpawnJS.JSObjects.ImageBitmap, global::SpawnDev.SpawnJS.JSObjects.OffscreenCanvas>, List<DetectedText>>("detect", imageBitmapSource);
    }
}
