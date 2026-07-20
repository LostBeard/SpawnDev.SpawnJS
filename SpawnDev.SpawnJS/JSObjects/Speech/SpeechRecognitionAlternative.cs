using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The SpeechRecognitionAlternative interface of the Web Speech API represents a single recognition alternative.
    /// </summary>
    public class SpeechRecognitionAlternative : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public SpeechRecognitionAlternative(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// A string containing the transcript of the recognized speech.
        /// </summary>
        public string Transcript => JSRef!.Get<string>("transcript");

        /// <summary>
        /// A numeric value representing the confidence of the recognition.
        /// </summary>
        public float Confidence => JSRef!.Get<float>("confidence");
    }
}
