using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// White balance options for media stream track capabilities
    /// </summary>
    public enum WhiteBalanceModeEnum
    {
        /// <summary>
        /// The source is using an automatic white balance mode.
        /// </summary>
        [JsonPropertyName("none")]
        None,
        /// <summary>
        /// The source is using a manual white balance mode.
        /// </summary>
        [JsonPropertyName("manual")]
        Manual,
        /// <summary>
        /// Single shot
        /// </summary>
        [JsonPropertyName("single-shot")]
        SingleShot,
        /// <summary>
        /// Continuous
        /// </summary>
        [JsonPropertyName("continuous")]
        Continuous,
    }
}
