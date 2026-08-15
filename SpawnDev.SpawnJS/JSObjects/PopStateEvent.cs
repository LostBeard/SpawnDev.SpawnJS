using System.Diagnostics.CodeAnalysis;

using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The PopStateEvent interface of the HTML 5 History API represents an event that is fired when the active history entry changes.
    /// </summary>
    public class PopStateEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public PopStateEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns the state object associated with the event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T StateAs<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>() => JSRef!.Get<T>("state");
    }
}
