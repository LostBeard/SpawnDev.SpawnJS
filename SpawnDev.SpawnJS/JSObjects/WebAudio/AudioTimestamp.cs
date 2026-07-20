using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Returned from AudioContext.GetOutputTimestamp()<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/AudioContext/getOutputTimestamp#return_value
    /// </summary>
    public class AudioTimestamp : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public AudioTimestamp(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// contextTime: A point in the time coordinate system of the currentTime for the BaseAudioContext; the time after the audio context was first created.
        /// </summary>
        public double ContextTime => JSRef!.Get<double>("contextTime");
        /// <summary>
        /// performanceTime: A point in the time coordinate system of a Performance interface; the time after the document containing the audio context was first rendered
        /// </summary>
        public double PerformanceTime => JSRef!.Get<double>("performanceTime");
    }
}
