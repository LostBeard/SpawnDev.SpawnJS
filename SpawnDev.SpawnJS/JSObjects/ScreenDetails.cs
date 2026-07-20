// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ScreenDetails interface of the Window Management API represents the details of all the screens available to the user's device.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ScreenDetails
    /// </summary>
    public class ScreenDetails : EventTarget
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public ScreenDetails(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// A single ScreenDetailed object representing detailed information about the screen that the current browser window is displayed in.
        /// </summary>
        public ScreenDetailed CurrentScreen => JSRef!.Get<ScreenDetailed>("currentScreen");
        /// <summary>
        /// An array of ScreenDetailed objects, each one representing detailed information about one specific screen available to the user's device.<br/>
        /// Note: screens only includes "extended" displays, not those that mirror another display.
        /// </summary>
        public Array<ScreenDetailed> Screens => JSRef!.Get<Array<ScreenDetailed>>("screens");
        /// <summary>
        /// Fired when the window's current screen changes in some way — for example available width or height, or orientation.
        /// </summary>
        public ActionEvent<Event> OnCurrentScreenChange { get => new ActionEvent<Event>("currentscreenchange", AddEventListener, RemoveEventListener); set { } }
        /// <summary>
        /// Fired when screens are connected to or disconnected from the system.
        /// </summary>
        public ActionEvent<Event> OnScreenChange { get => new ActionEvent<Event>("screenchange", AddEventListener, RemoveEventListener); set { } }
    }
}
