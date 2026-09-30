using System.Text.Json.Serialization;

using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
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
        // Optional (spec default "filtering"): unset must be OMITTED - an explicit null is an invalid enum value.
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; set; }
    }
}