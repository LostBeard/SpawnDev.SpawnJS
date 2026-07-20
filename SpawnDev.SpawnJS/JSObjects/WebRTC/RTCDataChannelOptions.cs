// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The RTCDataChannelOptions dictionary is used to provide options when creating an RTCDataChannel.
    /// </summary>
    public class RTCDataChannelOptions
    {
        /// <summary>
        /// Ordered
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Ordered { get; set; }

        /// <summary>
        /// MaxPacketLifeTime
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ushort? MaxPacketLifeTime { get; set; }

        /// <summary>
        /// MaxRetransmits
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ushort? MaxRetransmits { get; set; }

        /// <summary>
        /// Protocol
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Protocol { get; set; }

        /// <summary>
        /// Negotiated
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Negotiated { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ushort? Id { get; set; }
    }
}
