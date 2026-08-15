using System.Diagnostics.CodeAnalysis;
namespace SpawnDev.SpawnJS.Events
{
    /// <summary>
    /// A Callback manager with reference count support
    /// </summary>
    public class CallbackRef
    {
        /// <summary>
        /// A copy of the tracked Callbacks Dictionary keyed by the target method.<br/>
        /// The below methods are used for managing tracked callbacks:<br/>
        /// - AddRef - Returns a Callback for the given method and increments the Callback's reference count by 1.<br/>
        /// - RefGet - Returns a Callback for the given method. The Callback reference count is only incremented if the Callback is created. Optionally return null if the Callback does not already exist.<br/>
        /// - RefDel - Reduces the given methods reference count by 1 and returns the updated reference count. If the RefCount reaches 0 the Callback will be Disposed.<br/>
        /// - RefDispose - Forces the Callback to Dispose regardless of RefCount<br/>
        /// - GetRefCount - Returns the reference count for the given method<br/>
        /// - IsTracked - Returns true if the given method is being tracked<br/>
        /// Note: Only Callbacks created using Callback.RefAdd() and Callback.RefGet() are tracked<br/>
        /// </summary>
        internal Dictionary<Delegate, Callback> TrackedCallbacks => _TrackedCallbacks.ToDictionary(o => o.Key, o => o.Value);
        private Dictionary<Delegate, Callback> _TrackedCallbacks { get; } = new Dictionary<Delegate, Callback>();
        TCallback TrackCallback<TCallback>(Delegate callback, TCallback callbackJS) where TCallback : Callback
        {
            _TrackedCallbacks.Add(callback, callbackJS);
            callbackJS.OnDisposed += () => _TrackedCallbacks.Remove(callback);
            return callbackJS;
        }
        // ActionCallback
        // - RefAdd
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public ActionCallback RefAdd(Action callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (ActionCallback)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public ActionCallback<T1> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1>(Action<T1> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (ActionCallback<T1>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public ActionCallback<T1, T2> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2>(Action<T1, T2> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (ActionCallback<T1, T2>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public ActionCallback<T1, T2, T3> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3>(Action<T1, T2, T3> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (ActionCallback<T1, T2, T3>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public ActionCallback<T1, T2, T3, T4> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4>(Action<T1, T2, T3, T4> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (ActionCallback<T1, T2, T3, T4>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public ActionCallback<T1, T2, T3, T4, T5> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5>(Action<T1, T2, T3, T4, T5> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (ActionCallback<T1, T2, T3, T4, T5>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public ActionCallback<T1, T2, T3, T4, T5, T6> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6>(Action<T1, T2, T3, T4, T5, T6> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (ActionCallback<T1, T2, T3, T4, T5, T6>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public ActionCallback<T1, T2, T3, T4, T5, T6, T7> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7>(Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (ActionCallback<T1, T2, T3, T4, T5, T6, T7>)info;
        }
        // - RefGet
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public ActionCallback? RefGet(Action callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (ActionCallback)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public ActionCallback<T1>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1>(Action<T1> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (ActionCallback<T1>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public ActionCallback<T1, T2>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2>(Action<T1, T2> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (ActionCallback<T1, T2>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public ActionCallback<T1, T2, T3>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3>(Action<T1, T2, T3> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (ActionCallback<T1, T2, T3>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public ActionCallback<T1, T2, T3, T4>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4>(Action<T1, T2, T3, T4> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (ActionCallback<T1, T2, T3, T4>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public ActionCallback<T1, T2, T3, T4, T5>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5>(Action<T1, T2, T3, T4, T5> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (ActionCallback<T1, T2, T3, T4, T5>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public ActionCallback<T1, T2, T3, T4, T5, T6>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6>(Action<T1, T2, T3, T4, T5, T6> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (ActionCallback<T1, T2, T3, T4, T5, T6>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public ActionCallback<T1, T2, T3, T4, T5, T6, T7>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7>(Action<T1, T2, T3, T4, T5, T6, T7> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (ActionCallback<T1, T2, T3, T4, T5, T6, T7>)info! : (!allowCreate ? null : RefAdd(callback));
        // FuncCallback
        // - RefAdd
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public FuncCallback<TResult> RefAdd<TResult>(Func<TResult> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (FuncCallback<TResult>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public FuncCallback<T1, TResult> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, TResult>(Func<T1, TResult> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (FuncCallback<T1, TResult>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public FuncCallback<T1, T2, TResult> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, TResult>(Func<T1, T2, TResult> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (FuncCallback<T1, T2, TResult>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public FuncCallback<T1, T2, T3, TResult> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, TResult>(Func<T1, T2, T3, TResult> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (FuncCallback<T1, T2, T3, TResult>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public FuncCallback<T1, T2, T3, T4, TResult> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, TResult>(Func<T1, T2, T3, T4, TResult> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (FuncCallback<T1, T2, T3, T4, TResult>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public FuncCallback<T1, T2, T3, T4, T5, TResult> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (FuncCallback<T1, T2, T3, T4, T5, TResult>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public FuncCallback<T1, T2, T3, T4, T5, T6, TResult> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (FuncCallback<T1, T2, T3, T4, T5, T6, TResult>)info;
        }
        /// <summary>
        /// Returns a Callback for the given method and increments the Callback's reference count by 1
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <returns>A Callback object</returns>
        public FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult> RefAdd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) info = TrackCallback(callback, Callback.Create(callback));
            else info.RefCount++;
            return (FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>)info;
        }
        // - RefGet
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public FuncCallback<TResult>? RefGet<TResult>(Func<TResult> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (FuncCallback<TResult>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public FuncCallback<T1, TResult>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, TResult>(Func<T1, TResult> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (FuncCallback<T1, TResult>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public FuncCallback<T1, T2, TResult>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, TResult>(Func<T1, T2, TResult> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (FuncCallback<T1, T2, TResult>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public FuncCallback<T1, T2, T3, TResult>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, TResult>(Func<T1, T2, T3, TResult> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (FuncCallback<T1, T2, T3, TResult>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public FuncCallback<T1, T2, T3, T4, TResult>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, TResult>(Func<T1, T2, T3, T4, TResult> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (FuncCallback<T1, T2, T3, T4, TResult>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public FuncCallback<T1, T2, T3, T4, T5, TResult>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (FuncCallback<T1, T2, T3, T4, T5, TResult>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public FuncCallback<T1, T2, T3, T4, T5, T6, TResult>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (FuncCallback<T1, T2, T3, T4, T5, T6, TResult>)info! : (!allowCreate ? null : RefAdd(callback));
        /// <summary>
        /// Returns a Callback for the given method<br/>The Callback reference count is only incremented if the Callback is created
        /// </summary>
        /// <param name="callback">The method to return a Callback for</param>
        /// <param name="allowCreate">If the callback does not already exist and true a new Callback will be created and returned, if false null will be returned</param>
        /// <returns>A Callback object or null</returns>
        public FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>? RefGet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> callback, bool allowCreate = true) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? (FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>)info! : (!allowCreate ? null : RefAdd(callback));
        // All
        // - RefDel
        /// <summary>
        /// Reduces the given methods reference count by 1 and returns the updated reference count<br/>
        /// If the RefCount reaches 0 the Callback will be Disposed
        /// </summary>
        /// <param name="callback"></param>
        /// <returns>The new RefCount</returns>
        public int RefDel(Delegate callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) return 0;
            info.RefCount--;
            return info.RefCount;
        }
        /// <summary>
        /// Forces the Callback to Dispose regardless of RefCount
        /// </summary>
        /// <param name="callback"></param>
        /// <returns>true if the Callback was disposed</returns>
        public bool RefDispose(Delegate callback)
        {
            if (!_TrackedCallbacks.TryGetValue(callback, out Callback? info)) return false;
            info.Dispose();
            return true;
        }
        /// <summary>
        /// Returns true of the Callback is being tracked<br/>
        /// Note: Only Callbacks created using Callback.RefAdd() and Callback.RefGet() are tracked<br/>
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool IsTracked(Delegate callback) => _TrackedCallbacks.ContainsKey(callback);
        // - GetRefCount
        /// <summary>
        /// Returns the reference count for the given method
        /// </summary>
        /// <param name="callback"></param>
        /// <returns>A Callback object or null</returns>
        public int GetRefCount(Delegate callback) => _TrackedCallbacks.TryGetValue(callback, out Callback? info) ? info!.RefCount : 0;
    }
}
