
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The HTMLSlotElement interface of the Shadow DOM API enables access to the name and assigned nodes of an HTML slot element.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/HTMLSlotElement<br/>
    /// </summary>
    public class HTMLSlotElement : HTMLElement
    {
        #region Constructors
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public HTMLSlotElement(SpawnJSObjectReference _ref) : base(_ref) { }
        #endregion
        /// <summary>
        /// A string used to get and set the slot's name.
        /// </summary>
        public string Name { get => JSRef!.Get<string>("name"); set => JSRef!.Set("name", value); }
    }
}
