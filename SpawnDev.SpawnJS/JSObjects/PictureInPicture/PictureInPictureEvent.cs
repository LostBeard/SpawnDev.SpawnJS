
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The PictureInPictureEvent interface represents picture-in-picture-related events, including enterpictureinpicture and leavepictureinpicture.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/PictureInPictureEvent
    /// </summary>
    public class PictureInPictureEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public PictureInPictureEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns the PictureInPictureWindow interface that the event relates to.
        /// </summary>
        public PictureInPictureWindow PictureInPictureWindow => JSRef!.Get<PictureInPictureWindow>("pictureInPictureWindow");
    }
}
