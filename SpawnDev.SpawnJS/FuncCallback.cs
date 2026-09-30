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
        static readonly Type[] _argumentTypes = [];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        /// <param name="reader">The arguments Javascript wrote into .Net memory, read in order</param>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
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
        static readonly Type[] _argumentTypes = [typeof(T1)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        /// <param name="reader">The arguments Javascript wrote into .Net memory, read in order</param>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(ReadArg<T1>(ref reader, 0, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        /// <param name="reader">The arguments Javascript wrote into .Net memory, read in order</param>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        /// <param name="reader">The arguments Javascript wrote into .Net memory, read in order</param>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        /// <param name="reader">The arguments Javascript wrote into .Net memory, read in order</param>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        /// <param name="reader">The arguments Javascript wrote into .Net memory, read in order</param>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount),
                ReadArg<T5>(ref reader, 4, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        /// <param name="reader">The arguments Javascript wrote into .Net memory, read in order</param>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount),
                ReadArg<T5>(ref reader, 4, argsCount),
                ReadArg<T6>(ref reader, 5, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        /// <param name="reader">The arguments Javascript wrote into .Net memory, read in order</param>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount),
                ReadArg<T5>(ref reader, 4, argsCount),
                ReadArg<T6>(ref reader, 5, argsCount),
                ReadArg<T7>(ref reader, 6, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        /// <param name="reader">The arguments Javascript wrote into .Net memory, read in order</param>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount),
                ReadArg<T5>(ref reader, 4, argsCount),
                ReadArg<T6>(ref reader, 5, argsCount),
                ReadArg<T7>(ref reader, 6, argsCount),
                ReadArg<T8>(ref reader, 7, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        /// <param name="reader">The arguments Javascript wrote into .Net memory, read in order</param>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount),
                ReadArg<T5>(ref reader, 4, argsCount),
                ReadArg<T6>(ref reader, 5, argsCount),
                ReadArg<T7>(ref reader, 6, argsCount),
                ReadArg<T8>(ref reader, 7, argsCount),
                ReadArg<T9>(ref reader, 8, argsCount));
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
        static readonly Type[] _argumentTypes = [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10)];
        /// <inheritdoc/>
        protected override Type[] ArgumentTypes => _argumentTypes;
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
        /// <param name="reader">The arguments Javascript wrote into .Net memory, read in order</param>
        /// <param name="args">The incoming AND outgoing buffer. Auto-released after the call</param>
        /// <param name="argsCount">The number of arguments in the args array</param>
        protected override void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount)
        {
            var ret = _callback(ReadArg<T1>(ref reader, 0, argsCount),
                ReadArg<T2>(ref reader, 1, argsCount),
                ReadArg<T3>(ref reader, 2, argsCount),
                ReadArg<T4>(ref reader, 3, argsCount),
                ReadArg<T5>(ref reader, 4, argsCount),
                ReadArg<T6>(ref reader, 5, argsCount),
                ReadArg<T7>(ref reader, 6, argsCount),
                ReadArg<T8>(ref reader, 7, argsCount),
                ReadArg<T9>(ref reader, 8, argsCount),
                ReadArg<T10>(ref reader, 9, argsCount));
            args?.Set(argsCount, ret);
        }
    }
}
