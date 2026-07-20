// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ImageBitmapRenderingContext interface is a canvas rendering context that provides variables and methods for replacing the canvas's contents with a given ImageBitmap. Its context id (the first argument to HTMLCanvasElement.getContext() or OffscreenCanvas.getContext()) is "bitmaprenderer".<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ImageBitmapRenderingContext
    /// </summary>
    public class ImageBitmapRenderingContext : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public ImageBitmapRenderingContext(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns the CanvasElement that the context is bound to.
        /// </summary>
        public HTMLCanvasElement Canvas => JSRef!.Get<HTMLCanvasElement>("canvas");
        /// <summary>
        /// Transfers the underlying bitmap data from an ImageBitmap to the canvas, so that the canvas displays the image.<br/>
        /// The ImageBitmap is consumed and can no longer be used.
        /// </summary>
        /// <param name="bitmap">An ImageBitmap object to transfer.</param>
        public void TransferFromImageBitmap(ImageBitmap bitmap) => JSRef!.CallVoid("transferFromImageBitmap", bitmap);
    }
}
