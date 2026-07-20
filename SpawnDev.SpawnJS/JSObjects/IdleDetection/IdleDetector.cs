// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The IdleDetector interface of the Idle Detection API provides methods and events for detecting user idle state.
    /// </summary>
    public class IdleDetector : EventTarget
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public IdleDetector(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Creates a new IdleDetector.
        /// </summary>
        public IdleDetector() : base(JS.New(nameof(IdleDetector))) { }

        /// <summary>
        /// Returns the user's idle state.
        /// </summary>
        public string? UserState => JSRef!.Get<string?>("userState");

        /// <summary>
        /// Returns the screen's idle state.
        /// </summary>
        public string? ScreenState => JSRef!.Get<string?>("screenState");

        /// <summary>
        /// Request permission to use the Idle Detection API.
        /// </summary>
        public static Task<string> RequestPermission() => JS.CallAsync<string>("IdleDetector.requestPermission");

        /// <summary>
        /// Starts detecting idle changes.
        /// </summary>
        public Task Start(IdleOptions? options = null) => JSRef!.CallVoidAsync("start", options);

        /// <summary>
        /// Fired when the idle state of the user or screen changes.
        /// </summary>
        public ActionEvent<Event> OnChange { get => new ActionEvent<Event>("change", AddEventListener, RemoveEventListener); set { } }
    }
}
