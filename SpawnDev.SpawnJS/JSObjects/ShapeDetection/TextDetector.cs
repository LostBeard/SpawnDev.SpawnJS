// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
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
        public Task<List<DetectedText>> Detect(Union<Blob, Element, ImageData, ImageBitmap, OffscreenCanvas> imageBitmapSource) => JSRef!.CallAsync<List<DetectedText>>("detect", imageBitmapSource);
    }
}
