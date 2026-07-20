// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webxr/#enumdef-xrreferencespacetype
    /// </summary>
    public enum XRReferenceSpaceType
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("viewer")]
        Viewer,
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("local")]
        Local,
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("local-floor")]
        LocalFloor,
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("bounded-floor")]
        BoundedFloor,
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("unbounded")]
        Unbounded,
    }
}
