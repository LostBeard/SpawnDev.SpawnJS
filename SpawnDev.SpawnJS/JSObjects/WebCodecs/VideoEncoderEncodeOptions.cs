using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A dictionary object containing options for the encode method
    /// </summary>
    public class VideoEncoderEncodeOptions
    {
        /// <summary>
        /// A boolean indicating if the frame depends on the successful encoding of previous frames.
        /// </summary>
        [JsonPropertyName("keyFrame")]
        public bool? KeyFrame { get; set; }
    }
}
