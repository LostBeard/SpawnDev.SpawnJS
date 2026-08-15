
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A dictionary object containing metadata for the EncodedAudioChunk
    /// </summary>
    public class EncodedAudioChunkMetadata
    {
        /// <summary>
        /// Audio decoder configuration.
        /// </summary>
        [JsonPropertyName("decoderConfig")]
        public AudioDecoderConfig? DecoderConfig { get; set; }
    }
}
