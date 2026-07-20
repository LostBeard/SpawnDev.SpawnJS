// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The MIDIInputMap read-only interface of the Web MIDI API provides the set of MIDI input ports that are currently available.
    /// </summary>
    public class MIDIInputMap : Map<string, MIDIInput>
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MIDIInputMap"/>.
        /// </summary>
        /// <param name="_ref"></param>
        public MIDIInputMap(SpawnJSObjectReference _ref) : base(_ref) { }
        #endregion
    }
}
