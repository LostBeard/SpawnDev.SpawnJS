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
        static readonly Type[] _argumentTypes = [];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
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
        static readonly Type[] _argumentTypes = [typeof(T1)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            _callback(ReadArg<T1>(ref reader, 0, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount),
                ReadArg<T5>(ref reader, 4, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount),
                ReadArg<T5>(ref reader, 4, argsCount),
                ReadArg<T6>(ref reader, 5, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount),
                ReadArg<T5>(ref reader, 4, argsCount),
                ReadArg<T6>(ref reader, 5, argsCount),
                ReadArg<T7>(ref reader, 6, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount),
                ReadArg<T5>(ref reader, 4, argsCount),
                ReadArg<T6>(ref reader, 5, argsCount),
                ReadArg<T7>(ref reader, 6, argsCount),
                ReadArg<T8>(ref reader, 7, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount),
                ReadArg<T5>(ref reader, 4, argsCount),
                ReadArg<T6>(ref reader, 5, argsCount),
                ReadArg<T7>(ref reader, 6, argsCount),
                ReadArg<T8>(ref reader, 7, argsCount),
                ReadArg<T9>(ref reader, 8, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount),
                ReadArg<T5>(ref reader, 4, argsCount),
                ReadArg<T6>(ref reader, 5, argsCount),
                ReadArg<T7>(ref reader, 6, argsCount),
                ReadArg<T8>(ref reader, 7, argsCount),
                ReadArg<T9>(ref reader, 8, argsCount),
                ReadArg<T10>(ref reader, 9, argsCount));
        }
    }
}
