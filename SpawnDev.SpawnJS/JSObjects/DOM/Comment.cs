// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The Comment interface represents textual notations within markup; although it is generally not visually shown, such comments are available to be read in the source view.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/Comment
    /// </summary>
    public class Comment : CharacterData
    {
        #region Constructors
        /// <summary>
        /// The Comment() constructor returns a newly created Comment object with the optional string given in parameter as its textual content.
        /// </summary>
        public Comment() : base(JS.New(nameof(Comment))) { }
        /// <summary>
        /// The Comment() constructor returns a newly created Comment object with the optional string given in parameter as its textual content.
        /// </summary>
        /// <param name="aString"></param>
        public Comment(string aString) : base(JS.New(nameof(Comment), aString)) { }
        /// <summary>
        /// Default deserialize constructor
        /// </summary>
        /// <param name="_ref"></param>
        public Comment(SpawnJSObjectReference _ref) : base(_ref) { }
        #endregion
    }
}
