using System.Diagnostics.CodeAnalysis;
namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<TResult> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a FuncCallback.</summary>
        public static implicit operator FuncCallback<TResult>?(Func<TResult>? callback) => callback == null ? null : new FuncCallback<TResult>(callback);
        Func<TResult> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="func">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public FuncCallback(Func<TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback();
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, TResult> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a FuncCallback.</summary>
        public static implicit operator FuncCallback<T1, TResult>?(Func<T1, TResult>? callback) => callback == null ? null : new FuncCallback<T1, TResult>(callback);
        Func<T1, TResult> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="func">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public FuncCallback(Func<T1, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(argsCount <= 0 ? default! : args.Get<T1>(0));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, TResult> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a FuncCallback.</summary>
        public static implicit operator FuncCallback<T1, T2, TResult>?(Func<T1, T2, TResult>? callback) => callback == null ? null : new FuncCallback<T1, T2, TResult>(callback);
        Func<T1, T2, TResult> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="func">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public FuncCallback(Func<T1, T2, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, TResult> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a FuncCallback.</summary>
        public static implicit operator FuncCallback<T1, T2, T3, TResult>?(Func<T1, T2, T3, TResult>? callback) => callback == null ? null : new FuncCallback<T1, T2, T3, TResult>(callback);
        Func<T1, T2, T3, TResult> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="func">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public FuncCallback(Func<T1, T2, T3, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, TResult> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a FuncCallback.</summary>
        public static implicit operator FuncCallback<T1, T2, T3, T4, TResult>?(Func<T1, T2, T3, T4, TResult>? callback) => callback == null ? null : new FuncCallback<T1, T2, T3, T4, TResult>(callback);
        Func<T1, T2, T3, T4, TResult> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="func">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public FuncCallback(Func<T1, T2, T3, T4, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, TResult> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a FuncCallback.</summary>
        public static implicit operator FuncCallback<T1, T2, T3, T4, T5, TResult>?(Func<T1, T2, T3, T4, T5, TResult>? callback) => callback == null ? null : new FuncCallback<T1, T2, T3, T4, T5, TResult>(callback);
        Func<T1, T2, T3, T4, T5, TResult> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="func">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public FuncCallback(Func<T1, T2, T3, T4, T5, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, TResult> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a FuncCallback.</summary>
        public static implicit operator FuncCallback<T1, T2, T3, T4, T5, T6, TResult>?(Func<T1, T2, T3, T4, T5, T6, TResult>? callback) => callback == null ? null : new FuncCallback<T1, T2, T3, T4, T5, T6, TResult>(callback);
        Func<T1, T2, T3, T4, T5, T6, TResult> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="func">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public FuncCallback(Func<T1, T2, T3, T4, T5, T6, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, TResult> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a FuncCallback.</summary>
        public static implicit operator FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>?(Func<T1, T2, T3, T4, T5, T6, T7, TResult>? callback) => callback == null ? null : new FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>(callback);
        Func<T1, T2, T3, T4, T5, T6, T7, TResult> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="func">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public FuncCallback(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, TResult> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a FuncCallback.</summary>
        public static implicit operator FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, TResult>?(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>? callback) => callback == null ? null : new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(callback);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="func">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public FuncCallback(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6),
                argsCount <= 7 ? default! : args.Get<T8>(7));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T9, TResult> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a FuncCallback.</summary>
        public static implicit operator FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>?(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>? callback) => callback == null ? null : new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(callback);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="func">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public FuncCallback(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6),
                argsCount <= 7 ? default! : args.Get<T8>(7),
                argsCount <= 8 ? default! : args.Get<T9>(8));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T10, TResult> : Callback
    {
        /// <summary>Implicitly converts a .Net delegate into a FuncCallback.</summary>
        public static implicit operator FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>?(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>? callback) => callback == null ? null : new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(callback);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> _callback;
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="func">The method to call</param>
        /// <param name="once">If true, the Callback will automatically be disposed after the first call.</param>
        public FuncCallback(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        /// argsis the incoming data AND where the outgoing result will be written (at index 0)
        /// argsdoes not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6),
                argsCount <= 7 ? default! : args.Get<T8>(7),
                argsCount <= 8 ? default! : args.Get<T9>(8),
                argsCount <= 9 ? default! : args.Get<T10>(9));
            args?.Set(argsCount, ret);
        }
    }
}
