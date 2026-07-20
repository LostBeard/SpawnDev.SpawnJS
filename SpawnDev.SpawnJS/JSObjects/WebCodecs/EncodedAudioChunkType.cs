// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The type of the chunk.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EncodedAudioChunkType
    {
        /// <summary>
        /// The chunk is a key chunk, which does not rely on other frames for decoding.
        /// </summary>
        [JsonPropertyName("key")]
        Key,
        /// <summary>
        /// The chunk is a delta chunk, which relies on other frames for decoding.
        /// </summary>
        [JsonPropertyName("delta")]
        Delta,
    }
}
