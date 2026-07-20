// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// ChannelInterpretation
    /// https://www.w3.org/TR/webaudio/#enumdef-channelinterpretation
    /// </summary>
    public enum ChannelInterpretation
    {
        /// <summary>
        /// use up-mix equations or down-mix equations. In cases where the number of channels do not match any of these basic speaker layouts, revert to "discrete".
        /// </summary>
        [JsonPropertyName("speakers")]
        Speakers,
        /// <summary>
        /// Up-mix by filling channels until they run out then zero out remaining channels. Down-mix by filling as many channels as possible, then dropping remaining channels.
        /// </summary>
        [JsonPropertyName("discrete")]
        Discrete,
    }
}
