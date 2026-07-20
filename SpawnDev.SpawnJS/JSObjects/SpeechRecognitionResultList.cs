using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.SpawnJSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The SpeechRecognitionResultList interface of the Web Speech API represents a list of SpeechRecognitionResult objects, or a single one, if results are being returned in standard mode.
    /// </summary>
    public class SpeechRecognitionResultList : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public SpeechRecognitionResultList(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Returns the number of SpeechRecognitionResult objects contained in the list.
        /// </summary>
        public int Length => JSRef!.Get<int>("length");

        /// <summary>
        /// Indexer for accessing SpeechRecognitionResult objects.
        /// </summary>
        public SpeechRecognitionResult this[int index] => JSRef!.Call<SpeechRecognitionResult>("item", index);
    }
}
