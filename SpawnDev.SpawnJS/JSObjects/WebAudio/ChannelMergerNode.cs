// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ChannelMergerNode interface, often used in conjunction with its opposite, ChannelSplitterNode, reunites different mono inputs into a single output. Each input is used to fill a channel of the output. This is useful for accessing each channels separately, e.g. for performing channel mixing where a separate gain must be controlled on each channel.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ChannelMergerNode
    /// </summary>
    public class ChannelMergerNode : AudioNode
    {
        /// <inheritdoc/>
        public ChannelMergerNode(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
