using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Base class for parameter types used when calling SubtleCrypto.generateKey
    /// </summary>
    public class KeyGenParams
    {
        /// <summary>
        /// A string.
        /// </summary>
        [JsonPropertyName("name")]
        public virtual string Name { get; set; }
    }
}
