// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#dictdef-gpuorigin2ddict
    /// </summary>
    public class GPUOrigin2DDict
    {
        /// <summary>
        /// origin.x refers to either GPUOrigin2DDict.x or the first item of the sequence (0 if not present).
        /// </summary>
        public GPUIntegerCoordinate X { get; set; }

        /// <summary>
        /// origin.y refers to either GPUOrigin2DDict.y or the second item of the sequence (0 if not present).
        /// </summary>
        public GPUIntegerCoordinate Y { get; set; }
    }
}
