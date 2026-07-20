// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The FocusEvent interface represents focus-related events, including focus, blur, focusin, and focusout.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/FocusEvent
    /// </summary>
    public class FocusEvent : UIEvent
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public FocusEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// An EventTarget representing a secondary target for this event. In some cases (such as when tabbing in or out a page), this property may be set to null for security reasons.
        /// </summary>
        public EventTarget? RelatedTarget => JSRef!.Get<EventTarget?>("relatedTarget");
    }
}
