// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The MIDIOutputMap read-only interface of the Web MIDI API provides the set of MIDI output ports that are currently available.
    /// </summary>
    public class MIDIOutputMap : Map<string, MIDIOutput>
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MIDIOutputMap"/>.
        /// </summary>
        /// <param name="_ref"></param>
        public MIDIOutputMap(SpawnJSObjectReference _ref) : base(_ref) { }
        #endregion
    }
}
