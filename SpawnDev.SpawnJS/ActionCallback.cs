using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class ActionCallback : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator ActionCallback?(Action? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public ActionCallback(Action callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class ActionCallback<T1> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator ActionCallback<T1>?(Action<T1>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback">.Net target method</param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public ActionCallback(Action<T1> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class ActionCallback<T1, T2> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator ActionCallback<T1, T2>?(Action<T1, T2>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public ActionCallback(Action<T1, T2> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class ActionCallback<T1, T2, T3> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator ActionCallback<T1, T2, T3>?(Action<T1, T2, T3>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public ActionCallback(Action<T1, T2, T3> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class ActionCallback<T1, T2, T3, T4> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator ActionCallback<T1, T2, T3, T4>?(Action<T1, T2, T3, T4>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public ActionCallback(Action<T1, T2, T3, T4> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class ActionCallback<T1, T2, T3, T4, T5> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator ActionCallback<T1, T2, T3, T4, T5>?(Action<T1, T2, T3, T4, T5>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public ActionCallback(Action<T1, T2, T3, T4, T5> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class ActionCallback<T1, T2, T3, T4, T5, T6> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator ActionCallback<T1, T2, T3, T4, T5, T6>?(Action<T1, T2, T3, T4, T5, T6>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public ActionCallback(Action<T1, T2, T3, T4, T5, T6> callback, bool once = false) : base(callback, once) { }
    }
    /// <summary>
    /// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    /// </summary>
    public class ActionCallback<T1, T2, T3, T4, T5, T6, T7> : Callback
    {
        /// <summary>
        /// Implicitly converts a .Net method into a Callback
        /// </summary>
        /// <param name="callback">.Net target method</param>
        //public static implicit operator ActionCallback<T1, T2, T3, T4, T5, T6, T7>?(Action<T1, T2, T3, T4, T5, T6, T7>? callback) => callback == null ? null : callback.CallbackGet(true);
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="once">If true, the Callback will be disposed after the first call</param>
        public ActionCallback(Action<T1, T2, T3, T4, T5, T6, T7> callback, bool once = false) : base(callback, once) { }
    }
}
