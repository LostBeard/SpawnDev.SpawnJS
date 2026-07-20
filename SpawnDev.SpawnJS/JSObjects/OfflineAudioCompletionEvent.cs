// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The Web Audio API OfflineAudioCompletionEvent interface represents events that occur when the processing of an OfflineAudioContext is terminated. The complete event uses this interface.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/OfflineAudioCompletionEvent
    /// </summary>
    public class OfflineAudioCompletionEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public OfflineAudioCompletionEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// An AudioBuffer containing the result of the audio processing.
        /// </summary>
        public AudioBuffer RenderedBuffer => JSRef!.Get<AudioBuffer>("renderedBuffer");
    }
}
