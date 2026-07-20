// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The SpeechRecognitionEvent interface of the Web Speech API represents the event object for the result and nomatch events, and contains all the data associated with an interim or final speech recognition result.
    /// </summary>
    public class SpeechRecognitionEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public SpeechRecognitionEvent(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Returns the lowest index value result in the SpeechRecognitionResultList "array" that has actually changed.
        /// </summary>
        public int ResultIndex => JSRef!.Get<int>("resultIndex");

        /// <summary>
        /// Returns a SpeechRecognitionResultList object representing all the speech recognition results for the current session.
        /// </summary>
        public SpeechRecognitionResultList Results => JSRef!.Get<SpeechRecognitionResultList>("results");
    }
}
