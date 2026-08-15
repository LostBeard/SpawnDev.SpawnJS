using System.Diagnostics.CodeAnalysis;
namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// An Action Callback
    /// </summary>
    public class ActionCallback : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a ActionCallback.</summary>
        public static implicit operator ActionCallback?(Action? callback) => callback == null ? null : new ActionCallback(callback);
        Action _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="action">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public ActionCallback(Action action, bool once = false) : base(once)
        {
            _callback = action;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            _callback();
        }
    }
    /// <summary>
    /// An Action Callback
    /// </summary>
    public class ActionCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a ActionCallback.</summary>
        public static implicit operator ActionCallback<T1>?(Action<T1>? callback) => callback == null ? null : new ActionCallback<T1>(callback);
        Action<T1> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="action">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public ActionCallback(Action<T1> action, bool once = false) : base(once)
        {
            _callback = action;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            _callback(argsCount <= 0 ? default! : args.Get<T1>(0));
        }
    }
    /// <summary>
    /// An Action Callback
    /// </summary>
    public class ActionCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a ActionCallback.</summary>
        public static implicit operator ActionCallback<T1, T2>?(Action<T1, T2>? callback) => callback == null ? null : new ActionCallback<T1, T2>(callback);
        Action<T1, T2> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="action">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public ActionCallback(Action<T1, T2> action, bool once = false) : base(once)
        {
            _callback = action;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1));
        }
    }
    /// <summary>
    /// An Action Callback
    /// </summary>
    public class ActionCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a ActionCallback.</summary>
        public static implicit operator ActionCallback<T1, T2, T3>?(Action<T1, T2, T3>? callback) => callback == null ? null : new ActionCallback<T1, T2, T3>(callback);
        Action<T1, T2, T3> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="action">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public ActionCallback(Action<T1, T2, T3> action, bool once = false) : base(once)
        {
            _callback = action;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2));
        }
    }
    /// <summary>
    /// An Action Callback
    /// </summary>
    public class ActionCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a ActionCallback.</summary>
        public static implicit operator ActionCallback<T1, T2, T3, T4>?(Action<T1, T2, T3, T4>? callback) => callback == null ? null : new ActionCallback<T1, T2, T3, T4>(callback);
        Action<T1, T2, T3, T4> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="action">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public ActionCallback(Action<T1, T2, T3, T4> action, bool once = false) : base(once)
        {
            _callback = action;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3));
        }
    }
    /// <summary>
    /// An Action Callback
    /// </summary>
    public class ActionCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a ActionCallback.</summary>
        public static implicit operator ActionCallback<T1, T2, T3, T4, T5>?(Action<T1, T2, T3, T4, T5>? callback) => callback == null ? null : new ActionCallback<T1, T2, T3, T4, T5>(callback);
        Action<T1, T2, T3, T4, T5> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="action">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public ActionCallback(Action<T1, T2, T3, T4, T5> action, bool once = false) : base(once)
        {
            _callback = action;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4));
        }
    }
    /// <summary>
    /// An Action Callback
    /// </summary>
    public class ActionCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a ActionCallback.</summary>
        public static implicit operator ActionCallback<T1, T2, T3, T4, T5, T6>?(Action<T1, T2, T3, T4, T5, T6>? callback) => callback == null ? null : new ActionCallback<T1, T2, T3, T4, T5, T6>(callback);
        Action<T1, T2, T3, T4, T5, T6> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="action">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public ActionCallback(Action<T1, T2, T3, T4, T5, T6> action, bool once = false) : base(once)
        {
            _callback = action;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5));
        }
    }
    /// <summary>
    /// An Action Callback
    /// </summary>
    public class ActionCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a ActionCallback.</summary>
        public static implicit operator ActionCallback<T1, T2, T3, T4, T5, T6, T7>?(Action<T1, T2, T3, T4, T5, T6, T7>? callback) => callback == null ? null : new ActionCallback<T1, T2, T3, T4, T5, T6, T7>(callback);
        Action<T1, T2, T3, T4, T5, T6, T7> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="action">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public ActionCallback(Action<T1, T2, T3, T4, T5, T6, T7> action, bool once = false) : base(once)
        {
            _callback = action;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6));
        }
    }
    /// <summary>
    /// An Action Callback
    /// </summary>
    public class ActionCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a ActionCallback.</summary>
        public static implicit operator ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8>?(Action<T1, T2, T3, T4, T5, T6, T7, T8>? callback) => callback == null ? null : new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8>(callback);
        Action<T1, T2, T3, T4, T5, T6, T7, T8> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="action">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public ActionCallback(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, bool once = false) : base(once)
        {
            _callback = action;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6),
                argsCount <= 7 ? default! : args.Get<T8>(7));
        }
    }
    /// <summary>
    /// An Action Callback
    /// </summary>
    public class ActionCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T9> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a ActionCallback.</summary>
        public static implicit operator ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9>?(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>? callback) => callback == null ? null : new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(callback);
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="action">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public ActionCallback(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, bool once = false) : base(once)
        {
            _callback = action;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6),
                argsCount <= 7 ? default! : args.Get<T8>(7),
                argsCount <= 8 ? default! : args.Get<T9>(8));
        }
    }
    /// <summary>
    /// An Action Callback
    /// </summary>
    public class ActionCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T10> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a ActionCallback.</summary>
        public static implicit operator ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>?(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>? callback) => callback == null ? null : new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(callback);
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="action">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public ActionCallback(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, bool once = false) : base(once)
        {
            _callback = action;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6),
                argsCount <= 7 ? default! : args.Get<T8>(7),
                argsCount <= 8 ? default! : args.Get<T9>(8),
                argsCount <= 9 ? default! : args.Get<T10>(9));
        }
    }
}
