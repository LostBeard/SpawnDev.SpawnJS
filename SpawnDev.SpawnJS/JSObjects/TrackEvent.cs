// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The TrackEvent interface of the HTML DOM API is used for events which represent changes to a set of available tracks on an HTML media element; these events are addtrack and removetrack.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/TrackEvent
    /// </summary>
    public class TrackEvent : Event
    {
        /// <inheritdoc/>
        public TrackEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The DOM track object the event is in reference to. If not null, this is always an object of one of the media track types: AudioTrack, VideoTrack, or TextTrack).
        /// </summary>
        public MediaTrack Track => JSRef!.Get<Union<AudioTrack, VideoTrack, TextTrack>>("track").As<MediaTrack>();
    }
}