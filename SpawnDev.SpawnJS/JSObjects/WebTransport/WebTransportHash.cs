// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A WebTransport certificate
    /// </summary>
    public class WebTransportHash
    {
        /// <summary>
        /// A string with the value: sha-256 (case-insensitive). Note that this string represents the algorithm to use to verify the hash, and that any hash using an unknown algorithm will be ignored. At the time of writing, SHA-256 is the only hash algorithm listed in the specification.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Algorithm { get; set; }

        /// <summary>
        /// An ArrayBuffer, TypedArray or DataView containing the hash value.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Union<byte[], ArrayBuffer, TypedArray, DataView>? Value { get; set; }
    }
}
