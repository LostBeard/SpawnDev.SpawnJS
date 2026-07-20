// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The WakeLock interface of the Screen Wake Lock API can be used to request a lock that prevents device screens from dimming or locking when an application needs to keep running.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/WakeLock
    /// </summary>
    public class WakeLock : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public WakeLock(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns a Promise that fulfills with a WakeLockSentinel object if the screen wake lock is granted.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<WakeLockSentinel> Request(string type = "screen") => JSRef!.CallAsync<WakeLockSentinel>("request", type);
    }
}
