// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The HTMLTrackElement interface represents an HTML track element within the DOM.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/HTMLTrackElement
    /// </summary>
    public class HTMLTrackElement : HTMLElement
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public HTMLTrackElement(SpawnJSObjectReference _ref) : base(_ref) { }

        #region Properties
        /// <summary>
        /// A string that reflects the kind HTML attribute, indicating how the text track is meant to be used.
        /// </summary>
        public string Kind { get => JSRef!.Get<string>("kind"); set => JSRef!.Set("kind", value); }
        /// <summary>
        /// A string that reflects the src HTML attribute, indicating the address of the text track data.
        /// </summary>
        public string Src { get => JSRef!.Get<string>("src"); set => JSRef!.Set("src", value); }
        /// <summary>
        /// A string that reflects the srclang HTML attribute, indicating the language of the text track data.
        /// </summary>
        public string Srclang { get => JSRef!.Get<string>("srclang"); set => JSRef!.Set("srclang", value); }
        /// <summary>
        /// A string that reflects the label HTML attribute, listing a user-readable title for the track.
        /// </summary>
        public string Label { get => JSRef!.Get<string>("label"); set => JSRef!.Set("label", value); }
        /// <summary>
        /// A boolean that reflects the default HTML attribute, indicating that the track is to be enabled if the user's preferences do not indicate that another track would be more appropriate.
        /// </summary>
        public bool Default { get => JSRef!.Get<bool>("default"); set => JSRef!.Set("default", value); }
        /// <summary>
        /// Returns an unsigned short that reflects the Readiness state of the track.
        /// </summary>
        public int ReadyState => JSRef!.Get<int>("readyState");
        /// <summary>
        /// Returns the TextTrack object corresponding to the track element.
        /// </summary>
        public TextTrack Track => JSRef!.Get<TextTrack>("track");
        #endregion

        #region Events
        /// <summary>
        /// Fires when a TextTrack has changed the currently displaying cues. 
        /// </summary>
        public ActionEvent<Event> OnCueChange { get => new ActionEvent<Event>("cuechange", AddEventListener, RemoveEventListener); set { } }
        #endregion
    }
}