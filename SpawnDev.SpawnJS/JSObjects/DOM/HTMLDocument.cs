// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// For historical reasons, Window objects have a window.HTMLDocument property whose value is the Document interface. So you can think of HTMLDocument as an alias for Document, and you can find documentation for HTMLDocument members under the documentation for the Document interface.
    /// </summary>
    public class HTMLDocument : Document
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public HTMLDocument(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
