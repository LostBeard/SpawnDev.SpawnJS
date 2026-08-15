
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The HTMLDivElement interface provides special properties (beyond the regular HTMLElement interface it also has available to it by inheritance) for manipulating div elements.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/HTMLDivElement<br/>
    /// </summary>
    public class HTMLDivElement : HTMLElement
    {
        #region Constructors
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public HTMLDivElement(SpawnJSObjectReference _ref) : base(_ref) { }
        #endregion
    }
}
