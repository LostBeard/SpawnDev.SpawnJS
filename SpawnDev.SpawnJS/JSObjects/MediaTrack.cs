
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Base type for AudioTrack, VideoTrack, and TextTrack (non-spec, used to group MediaTrack types)
    /// </summary>
    public class MediaTrack : EventTarget
    {
        /// <inheritdoc/>
        public MediaTrack(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}