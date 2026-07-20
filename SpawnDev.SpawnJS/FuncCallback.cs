using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class FuncCallback<TResult> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator FuncCallback<TResult>?(Func<TResult>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback">.Net target method</param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public FuncCallback(Func<TResult> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class FuncCallback<T1, TResult> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator FuncCallback<T1, TResult>?(Func<T1, TResult>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback">.Net target method</param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public FuncCallback(Func<T1, TResult> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class FuncCallback<T1, T2, TResult> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator FuncCallback<T1, T2, TResult>?(Func<T1, T2, TResult>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback">.Net target method</param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public FuncCallback(Func<T1, T2, TResult> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class FuncCallback<T1, T2, T3, TResult> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator FuncCallback<T1, T2, T3, TResult>?(Func<T1, T2, T3, TResult>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback">.Net target method</param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public FuncCallback(Func<T1, T2, T3, TResult> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class FuncCallback<T1, T2, T3, T4, TResult> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator FuncCallback<T1, T2, T3, T4, TResult>?(Func<T1, T2, T3, T4, TResult>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback">.Net target method</param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public FuncCallback(Func<T1, T2, T3, T4, TResult> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class FuncCallback<T1, T2, T3, T4, T5, TResult> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator FuncCallback<T1, T2, T3, T4, T5, TResult>?(Func<T1, T2, T3, T4, T5, TResult>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback">.Net target method</param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public FuncCallback(Func<T1, T2, T3, T4, T5, TResult> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class FuncCallback<T1, T2, T3, T4, T5, T6, TResult> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator FuncCallback<T1, T2, T3, T4, T5, T6, TResult>?(Func<T1, T2, T3, T4, T5, T6, TResult>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback">.Net target method</param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public FuncCallback(Func<T1, T2, T3, T4, T5, T6, TResult> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>?(Func<T1, T2, T3, T4, T5, T6, T7, TResult>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback">.Net target method</param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public FuncCallback(Func<T1, T2, T3, T4, T5, T6, T7, TResult> callback, bool once = false) : base(callback, once) { }
    }
}
