// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The MIDIInput interface of the Web MIDI API receives messages from a MIDI input port.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/MIDIInput
    /// </summary>
    public class MIDIInput : MIDIPort
    {
        #region Constructors
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public MIDIInput(SpawnJSObjectReference _ref) : base(_ref) { }
        #endregion

        /// <summary>
        /// Fired when the current port receives a MIDI message.
        /// </summary>
        public ActionEvent<MIDIMessageEvent> OnMIDIMessage { get => new ActionEvent<MIDIMessageEvent>("midimessage", AddEventListener, RemoveEventListener); set { } }
    }
}
