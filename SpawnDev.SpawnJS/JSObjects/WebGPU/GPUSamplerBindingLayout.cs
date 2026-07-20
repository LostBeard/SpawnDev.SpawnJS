// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#dictdef-gpusamplerbindinglayout
    /// </summary>
    public class GPUSamplerBindingLayout
    {
        /// <summary>
        /// Indicates the required type of a sampler bound to this bindings.
        /// Options are "filtering", "non-filtering", "comparison"
        /// </summary>
        public string? Type { get; set; }
    }
}