// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The SpeechRecognitionErrorEvent interface of the Web Speech API represents the error event in the speech recognition service.
    /// </summary>
    public class SpeechRecognitionErrorEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public SpeechRecognitionErrorEvent(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Returns the type of error raised.
        /// </summary>
        public string Error => JSRef!.Get<string>("error");

        /// <summary>
        /// Returns a message describing the error in more detail.
        /// </summary>
        public string Message => JSRef!.Get<string>("message");
    }
}
