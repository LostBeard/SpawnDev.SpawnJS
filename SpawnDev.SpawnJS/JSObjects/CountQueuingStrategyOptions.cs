// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/CountQueuingStrategy/CountQueuingStrategy#options
    /// </summary>
    public class CountQueuingStrategyOptions
    {
        /// <summary>
        /// The total number of chunks that can be contained in the internal queue before backpressure is applied.
        /// </summary>
        public int HighWaterMark { get; set; }
    }
}
