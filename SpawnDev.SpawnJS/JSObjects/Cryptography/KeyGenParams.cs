// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
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
