using System.Diagnostics.CodeAnalysis;
namespace SpawnDev.SpawnJS
{
    public partial class Callback
    {
        #region FuncCallbackCreate
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<TResult> Create<TResult>(Func<TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, TResult> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, TResult>(Func<T1, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, TResult> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, TResult>(Func<T1, T2, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, TResult> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, TResult>(Func<T1, T2, T3, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, TResult> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, TResult>(Func<T1, T2, T3, T4, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, TResult> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, T5, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, TResult> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, T5, T6, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(callback, false), callbackGroup);
#endregion
        #region FuncCallbackCreateOne
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<TResult> CreateOne<TResult>(Func<TResult> callback)
            => new FuncCallback<TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, TResult> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, TResult>(Func<T1, TResult> callback)
            => new FuncCallback<T1, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, TResult> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, TResult>(Func<T1, T2, TResult> callback)
            => new FuncCallback<T1, T2, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, TResult> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, TResult>(Func<T1, T2, T3, TResult> callback)
            => new FuncCallback<T1, T2, T3, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, TResult> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, TResult>(Func<T1, T2, T3, T4, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, TResult> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, T5, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, TResult> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, T5, T6, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, TResult> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(callback, true);
        #endregion
        #region ActionCallbackCreate
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback Create(Action callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1>(Action<T1> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2>(Action<T1, T2> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3>(Action<T1, T2, T3> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4>(Action<T1, T2, T3, T4> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5>(Action<T1, T2, T3, T4, T5> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4, T5>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6>(Action<T1, T2, T3, T4, T5, T6> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4, T5, T6>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7>(Action<T1, T2, T3, T4, T5, T6, T7> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4, T5, T6, T7>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(callback, false), callbackGroup);
        #endregion
        #region ActionCallbackCreateOne
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback CreateOne(Action callback)
            => new ActionCallback(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1>(Action<T1> callback)
            => new ActionCallback<T1>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2>(Action<T1, T2> callback)
            => new ActionCallback<T1, T2>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3>(Action<T1, T2, T3> callback)
            => new ActionCallback<T1, T2, T3>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4>(Action<T1, T2, T3, T4> callback)
            => new ActionCallback<T1, T2, T3, T4>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5>(Action<T1, T2, T3, T4, T5> callback)
            => new ActionCallback<T1, T2, T3, T4, T5>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6>(Action<T1, T2, T3, T4, T5, T6> callback)
            => new ActionCallback<T1, T2, T3, T4, T5, T6>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7>(Action<T1, T2, T3, T4, T5, T6, T7> callback)
            => new ActionCallback<T1, T2, T3, T4, T5, T6, T7>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
            => new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)
            => new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateOne<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)
            => new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(callback, true);
        #endregion
    }
}
