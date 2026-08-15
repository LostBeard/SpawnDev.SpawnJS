using System.Diagnostics.CodeAnalysis;
namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// CallbackGroup can be used as a container for a group of Callbacks allowing the entire group to be disposed with 1 call.
    /// </summary>
    public class CallbackGroup : IDisposable
    {
        public static T TryAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(T callback, CallbackGroup? callbackGroup) where T : Callback
        {
            callbackGroup?.Add(callback);
            return callback;
        }
        /// <summary>
        /// A List of the Callbacks in this group
        /// </summary>
        public List<Callback> Callbacks { get; } = new List<Callback>();
        /// <summary>
        /// Add a Callback the Callback List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wrapper"></param>
        /// <returns></returns>
        public T Add<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(T wrapper) where T : Callback
        {
            Callbacks.Add(wrapper);
            return wrapper;
        }
        /// <summary>
        /// Dispose all Callbacks and empty the Callback List
        /// </summary>
        public void Clear()
        {
            foreach (var cbw in Callbacks)
            {
                cbw.Dispose();
            }
            Callbacks.Clear();
        }
        /// <summary>
        /// Dispose all Callbacks and empty the Callback List
        /// </summary>
        public void Dispose()
        {
            Clear();
        }
    }
}
