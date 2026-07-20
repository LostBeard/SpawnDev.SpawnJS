// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// This specifies the options that can be used in constructing all AudioNodes. All members are optional. However, the specific values used for each node depends on the actual node.<br/>
    /// https://www.w3.org/TR/webaudio/#AudioNodeOptions
    /// </summary>
    public class AudioNodeOptions
    {
        /// <summary>
        /// Desired number of channels for the channelCount attribute.
        /// </summary>
        public uint ChannelCount { get; set; }
        /// <summary>
        /// Desired mode for the channelCountMode attribute.
        /// </summary>
        public EnumString<ChannelCountMode> ChannelCountMode { get; set; }
        /// <summary>
        /// Desired mode for the channelInterpretation attribute.
        /// </summary>
        public EnumString<ChannelInterpretation> ChannelInterpretation { get; set; }
    }
}
