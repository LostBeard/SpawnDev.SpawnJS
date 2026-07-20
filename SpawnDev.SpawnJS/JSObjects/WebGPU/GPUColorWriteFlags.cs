// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#typedefdef-gpucolorwriteflags
    /// </summary>
    [Flags]
    public enum GPUColorWriteFlags : uint
    {
        /// <summary>
        /// Red
        /// </summary>
        RED = 0x1,
        /// <summary>
        /// Green
        /// </summary>
        GREEN = 0x2,
        /// <summary>
        /// Blue
        /// </summary>
        BLUE = 0x4,
        /// <summary>
        /// Alpha
        /// </summary>
        ALPHA = 0x8,
        /// <summary>
        /// All
        /// </summary>
        ALL = 0xF,
    }
}
