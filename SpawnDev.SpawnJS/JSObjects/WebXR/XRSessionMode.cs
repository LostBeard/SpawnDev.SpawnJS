
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webxr/#enumdef-xrsessionmode
    /// </summary>
    public enum XRSessionMode
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("inline")]
        Inline,
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("immersive-vr")]
        ImmersiveVR,
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("immersive-ar")]
        ImmersiveAR,
    }
}
