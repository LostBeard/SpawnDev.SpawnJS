// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The PresentationAvailability interface of the Presentation API provides a way to detect when there is a display available to start a presentation.
    /// </summary>
    public class PresentationAvailability : EventTarget
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public PresentationAvailability(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Returns a boolean value that is true if there is an available display to start a presentation, and false otherwise.
        /// </summary>
        public bool Value => JSRef!.Get<bool>("value");

        /// <summary>
        /// Fired when the availability of a presentation display changes.
        /// </summary>
        public ActionEvent<Event> OnChange { get => new ActionEvent<Event>("change", AddEventListener, RemoveEventListener); set { } }
    }
}
