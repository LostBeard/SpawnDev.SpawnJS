// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The BarProp interface of the Document Object Model represents the web browser user interface elements that are exposed to scripts in web pages. Each of the following interface elements are represented by a BarProp object.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/BarProp
    /// </summary>
    public class BarProp : SpawnJSObject
    {
        #region Constructors
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public BarProp(SpawnJSObjectReference _ref) : base(_ref) { }
        #endregion

        #region Properties
        /// <summary>
        /// A Boolean, which is true if the bar represented by the used interface element is visible.
        /// </summary>
        public bool Visible => JSRef!.Get<bool>("visible");
        #endregion
    }
}
