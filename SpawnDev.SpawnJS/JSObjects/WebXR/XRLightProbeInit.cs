// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRSession/requestLightProbe#options
    /// </summary>
    public class XRLightProbeInit
    {
        /// <summary>
        /// The internal reflection format indicating how the texture data is represented, either srgba8 (default value) or rgba16f. See also XRSession.preferredReflectionFormat.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ReflectionFormat { get; set; }
    }
}
