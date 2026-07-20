// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRCPUDepthInformation
    /// </summary>
    public class XRCPUDepthInformation : XRDepthInformation
    {
        /// <inheritdoc/>
        public XRCPUDepthInformation(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// An ArrayBuffer containing depth-buffer information in raw format.
        /// </summary>
        public ArrayBuffer Data => JSRef!.Get<ArrayBuffer>("data");
        /// <summary>
        /// Returns the depth in meters at (x, y) in normalized view coordinates.
        /// </summary>
        /// <param name="x">X coordinate (origin at the left, grows to the right).</param>
        /// <param name="y">Y coordinate (origin at the top, grows downward).</param>
        /// <returns>Depth in meters</returns>
        public float GetDepthInMeters(float x, float y) => JSRef!.Call<float>("getDepthInMeters", x, y);
    }
}
