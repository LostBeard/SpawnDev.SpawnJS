// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#dictdef-gpublendstate
    /// </summary>
    public class GPUBlendState
    {
        /// <summary>
        /// Defines the blending behavior of the corresponding render target for color channels.
        /// </summary>
        public GPUBlendComponent Color { get; set; }
        /// <summary>
        /// Defines the blending behavior of the corresponding render target for the alpha channel.
        /// </summary>
        public GPUBlendComponent Alpha { get; set; }
    }
}
