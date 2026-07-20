// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The HashChangeEvent interface represents events that fire when the fragment identifier of the URL has changed.
    /// </summary>
    public class HashChangeEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public HashChangeEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The new URL to which the window is navigating.
        /// </summary>
        public string NewURL => JSRef!.Get<string>("newURL");
        /// <summary>
        /// The previous URL from which the window is navigating.
        /// </summary>
        public string OldURL => JSRef!.Get<string>("oldURL");
    }
}
