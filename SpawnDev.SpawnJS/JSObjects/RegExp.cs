// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RegExp
    /// </summary>
    public class RegExp : SpawnJSObject
    {
        /// <inheritdoc/>
        public RegExp(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The RegExp() constructor creates RegExp objects.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="flags"></param>
        public RegExp(string pattern, string? flags = null) : base(flags == null ? JS.New(nameof(RegExp), pattern) : JS.New(nameof(RegExp), pattern, flags)) { }

        // TODO
    }
}
