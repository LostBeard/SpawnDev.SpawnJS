using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.SpawnJSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The GestureEvent is a proprietary interface specific to WebKit which gives information regarding multi-touch gestures. Events using this interface include gesturestart, gesturechange, and gestureend.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/GestureEvent
    /// </summary>
    public class GestureEvent : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public GestureEvent(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
