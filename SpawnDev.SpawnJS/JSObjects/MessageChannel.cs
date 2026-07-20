// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The MessageChannel interface of the Channel Messaging API allows us to create a new message channel and send data through it via its two MessagePort properties.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/MessageChannel
    /// </summary>
    public class MessageChannel : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public MessageChannel(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The MessageChannel() constructor of the MessageChannel interface returns a new MessageChannel object with two new MessagePort objects.
        /// </summary>
        public MessageChannel() : base(JS.New(nameof(MessageChannel))) { }
        /// <summary>
        /// Returns port1 of the channel.
        /// </summary>
        public MessagePort Port1 => JSRef!.Get<MessagePort>("port1");
        /// <summary>
        /// Returns port2 of the channel.
        /// </summary>
        public MessagePort Port2 => JSRef!.Get<MessagePort>("port2");
    }
}
