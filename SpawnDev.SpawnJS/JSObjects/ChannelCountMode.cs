// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ChannelCountMode, in conjuction with the node’s channelCount and channelInterpretation values, is used to determine the computedNumberOfChannels that controls how inputs to a node are to be mixed. The computedNumberOfChannels is determined as shown below. See § 4 Channel Up-Mixing and Down-Mixing for more information on how mixing is to be done.<br/>
    /// https://www.w3.org/TR/webaudio/#enumdef-channelcountmode
    /// </summary>
    public enum ChannelCountMode
    {
        /// <summary>
        /// computedNumberOfChannels is the maximum of the number of channels of all connections to an input. In this mode channelCount is ignored.
        /// </summary>
        [JsonPropertyName("max")]
        Max,
        /// <summary>
        /// computedNumberOfChannels is determined as for "max" and then clamped to a maximum value of the given channelCount.
        /// </summary>
        [JsonPropertyName("clamped-max")]
        ClampedMax,
        /// <summary>
        /// computedNumberOfChannels is the exact value as specified by the channelCount.
        /// </summary>
        [JsonPropertyName("explicit")]
        Explicit,
    }
}
