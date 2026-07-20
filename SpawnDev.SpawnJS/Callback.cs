using SpawnDev.SpawnJS.JSObjects;
using System.Collections.Concurrent;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Callback wraps action and function to pass to JS
    /// </summary>
    public class Callback : IMarshalOutByJSHandle, IDisposable
    {
        /// <summary>
        /// SpawnJSRuntime
        /// </summary>
        static private SpawnJSRuntime JS => SpawnJSRuntime.Instance ?? throw new InvalidOperationException("SpawnJSRuntime has not been created.");
        static long _id = 0;
        /// <summary>
        /// Callback id
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// The delegate
        /// </summary>
        public Delegate Func { get; private set; }
        /// <summary>
        /// True if disposed
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// Handle to the JS function that calls this method
        /// </summary>
        public SpawnJSHandle JSHandle { get; private set; }
        /// <summary>
        /// If true, this callback will be disposed after it fires 1 once
        /// </summary>
        public bool Once { get; private set; }
        /// <summary>
        /// New instance
        /// </summary>
        public Callback(Delegate func, string? id = null) : this(func, false, id) { }
        /// <summary>
        /// New instance
        /// </summary>
        public Callback(Delegate func, bool once, string? id = null)
        {
            Once = once;
            _function = CallbackFunction.Acquire(func, once, id, this);
            Id = _function.Id;
            Func = _function.Func;
            JSHandle = _function.JSHandle;
        }
        /// <summary>
        /// The Javascript function this Callback dispatches through. Shared with every other live
        /// Callback over the same delegate, so the JS/.Net boundary is crossed once per delegate rather
        /// than once per Callback.
        /// </summary>
        readonly CallbackFunction _function;
        /// <summary>
        /// Create a callback
        /// </summary>
        public static Callback Create(Delegate func, bool once, string? id = null) => new Callback(func, once, id);
        /// <summary>
        /// Create a callback
        /// </summary>
        public static Callback Create(Delegate func, string? id = null) => new Callback(func, id);
        /// <summary>
        /// Create a callback that will be diposed after 1 call
        /// </summary>
        public static Callback CreateOne(Delegate func, string? id = null) => new Callback(func, true, id);
        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsDisposed) return;
            Dispose(true);
        }
        /// <summary>
        /// Fired when this Callback is disposed. CallbackRef uses this to stop tracking it.
        /// </summary>
        public event Action? OnDisposed;
        /// <summary>
        /// How many event subscriptions are holding this Callback.<br/>
        /// Managed by <see cref="CallbackRef"/>: every += takes a reference and every -= releases one,
        /// so the same .Net method subscribed to several events shares one JS function and is only
        /// disposed when the last subscription goes away. Setting it to 0 or less disposes.
        /// </summary>
        public int RefCount
        {
            get => _refCount;
            set
            {
                _refCount = value;
                if (_refCount <= 0) Dispose();
            }
        }
        int _refCount = 1;
        /// <inheritdoc/>
        public void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            IsDisposed = true;
            // Release this Callback's share of the Javascript function. The function, its registration and
            // its JSObject only go away once the last Callback over the delegate has released it, so
            // disposing one Callback never breaks a sibling that is still subscribed.
            _function.Release(this);
            OnDisposed?.Invoke();
        }
        // There is deliberately NO finalizer. A Callback must be disposed manually, which is the same
        // contract SpawnDev.BlazorJS has always had and it works. A finalizer would be dead code anyway:
        // every Callback is reachable from the static registration table for as long as Javascript can
        // dispatch to it, so it is never eligible for collection.

        #region JS -> .Net handlers
        // Named .Net handlers that Javascript can invoke by name. The inbound path is the exact mirror
        // of the outbound one: JS calls SpawnJSInterop.jsToNetCall(name, ...args) (or jsToNetCallApply),
        // which forwards (name, argsArray) to the _JSToNetCall JSExport binding; here we marshal each
        // argument IN through the marshaller graph (typed by the handler's parameters), invoke the handler,
        // and marshal the result back OUT as a one-element [result] array so JS reads index 0 - symmetric
        // with the outbound [ret] wrapper.
        static readonly ConcurrentDictionary<string, Callback> _jsToNetHandlers = new ConcurrentDictionary<string, Callback>();

        /// <summary>
        /// One Javascript function per delegate, shared by every live Callback over that delegate.<br/>
        /// Creating a Callback used to unconditionally register a new JS function and so allocate a new
        /// JSObject, even when the exact same .Net method already had one. Two subscriptions of one
        /// handler cost two boundary crossings and two JS functions for no reason. The function is now
        /// created on first use and reference counted, so the boundary is crossed once per delegate and
        /// released when the last holder disposes.
        /// </summary>
        internal class CallbackFunction
        {
            /// <summary>
            /// Shareable functions keyed on the delegate. Delegate equality is method plus target, so two
            /// separately constructed delegates over the same instance method are the same key, while a
            /// capturing lambda makes a distinct closure and correctly gets its own function.
            /// </summary>
            static readonly Dictionary<Delegate, CallbackFunction> _byDelegate = new Dictionary<Delegate, CallbackFunction>();

            public string Id { get; private init; } = "";
            public Delegate Func { get; private init; } = null!;
            public SpawnJSHandle JSHandle { get; private init; } = null!;
            /// <summary>
            /// True when this function is in the shared table and must be removed from it on release
            /// </summary>
            public bool Shared { get; private init; }
            /// <summary>
            /// Every live Callback holding this function. The first one is what the dispatch table points
            /// at; if it is disposed while others are still alive the registration moves to a survivor,
            /// so Javascript never dispatches through a disposed Callback.
            /// </summary>
            readonly List<Callback> _holders = new List<Callback>();
            /// <summary>
            /// How many live Callbacks share this Javascript function
            /// </summary>
            public int HolderCount => _holders.Count;

            /// <summary>
            /// Returns the Javascript function for this delegate, creating it only if it does not exist,
            /// and records holder as one of its users.
            /// </summary>
            public static CallbackFunction Acquire(Delegate func, bool once, string? id, Callback holder)
            {
                // A `once` callback disposes itself after firing, and an explicitly named one is a named
                // intent rather than an anonymous callback. Neither may share a function with anything
                // else - a shared function that self-disposes would pull the rug out from under the other
                // holders, and a named intent has to keep its own identity.
                var shareable = !once && id == null;
                if (shareable && _byDelegate.TryGetValue(func, out var existing))
                {
                    existing._holders.Add(holder);
                    return existing;
                }
                var callbackId = id ?? $"cb_{_id++}";
                // void and value returning delegates need different Javascript wrappers: the void one
                // must not marshal a return value back.
                var register = func.Method.ReturnType == typeof(void) ? "registerCallbackVoid" : "registerCallback";
                var fn = new CallbackFunction
                {
                    Id = callbackId,
                    Func = func,
                    JSHandle = JS.NetRun<SpawnJSHandle>(register, new object[] { callbackId }),
                    Shared = shareable,
                };
                fn._holders.Add(holder);
                if (shareable) _byDelegate[func] = fn;
                _jsToNetHandlers[callbackId] = holder;
                return fn;
            }

            /// <summary>
            /// Drops one holder. The Javascript function is only unregistered and released once the last
            /// holder is gone.
            /// </summary>
            public void Release(Callback holder)
            {
                _holders.Remove(holder);
                if (_holders.Count > 0)
                {
                    // still in use - hand the registration to a holder that is still alive
                    if (_jsToNetHandlers.TryGetValue(Id, out var registered) && ReferenceEquals(registered, holder))
                    {
                        _jsToNetHandlers[Id] = _holders[0];
                    }
                    return;
                }
                if (Shared) _byDelegate.Remove(Func);
                _jsToNetHandlers.TryRemove(Id, out _);
                JSHandle.Dispose();
            }

            /// <summary>
            /// The number of distinct Javascript callback functions currently alive. Diagnostics only.
            /// </summary>
            public static int SharedFunctionCount => _byDelegate.Count;
        }

        /// <summary>
        /// How many live Callbacks share this Callback's Javascript function, including this one.<br/>
        /// A delegate used by several simultaneous Callbacks has exactly one Javascript function behind
        /// all of them.
        /// </summary>
        public int SharedHolderCount => _function.HolderCount;

        /// <summary>
        /// The number of distinct shared Javascript callback functions currently alive. Diagnostics only.
        /// </summary>
        public static int SharedFunctionCount => CallbackFunction.SharedFunctionCount;

        /// <summary>
        /// Register a .Net handler that Javascript can invoke by name via
        /// <c>SpawnJSInterop.jsToNetCall(name, ...args)</c> or <c>jsToNetCallApply(name, args)</c>.
        /// Arguments are marshalled in (typed by the handler's parameters) and the result is marshalled
        /// back out using the runtime Marshallers.
        /// </summary>
        /// <param name="name">The intent name Javascript uses to select this handler.</param>
        /// <param name="handler">The .Net delegate to invoke.</param>
        public static Callback AddHandler(string name, Delegate handler) => _jsToNetHandlers[name] = new Callback(handler, name);

        /// <summary>
        /// Register a .Net handler that Javascript can invoke by name via
        /// <c>SpawnJSInterop.jsToNetCall(name, ...args)</c> or <c>jsToNetCallApply(name, args)</c>.
        /// Arguments are marshalled in (typed by the handler's parameters) and the result is marshalled
        /// back out using the runtime Marshallers.
        /// </summary>
        /// <param name="handler">The .Net delegate to invoke.</param>
        public static Callback AddHandler(Delegate handler) => new Callback(handler);

        /// <summary>
        /// Register a .Net handler that Javascript can invoke by name via
        /// <c>SpawnJSInterop.jsToNetCall(name, ...args)</c> or <c>jsToNetCallApply(name, args)</c>.
        /// Arguments are marshalled in (typed by the handler's parameters) and the result is marshalled
        /// back out using the runtime Marshallers.
        /// </summary>
        /// <param name="callback">Register the callback</param>
        public static Callback AddHandler(Callback callback) => _jsToNetHandlers[callback.Id] = callback;

        /// <summary>
        /// Remove a previously registered JS-callable .Net handler.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if a handler was removed.</returns>
        public static bool RemoveHandler(string name)
        {
            // Disposing is what unregisters. Removing the dictionary entry on its own would strand the
            // Javascript function and its JSObject with nothing left able to release them.
            if (!_jsToNetHandlers.TryGetValue(name, out var callback)) return false;
            callback.Dispose();
            return true;
        }

        /// <summary>
        /// Remove a previously registered JS-callable .Net handler.
        /// </summary>
        /// <param name="callback"></param>
        /// <returns>True if a handler was removed.</returns>
        public static bool RemoveHandler(Callback callback) => callback != null && RemoveHandler(callback.Id);

        /// <summary>
        /// Remove a previously registered JS-callable .Net handler.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns>True if a handler was removed.</returns>
        public static int RemoveHandlers(Delegate handler) => _jsToNetHandlers.ToArray().Where(o => o.Value.Func == handler).Select(o => _jsToNetHandlers.TryRemove(o.Value.Id, out _)).Count();

        /// <summary>
        /// Get all callbacks registered for the specified delegate
        /// </summary>
        /// <param name="handler"></param>
        /// <returns>True if a handler was removed.</returns>
        public static List<Callback> GetHandlers(Delegate handler) => _jsToNetHandlers.ToArray().Where(o => o.Value.Func == handler).Select(o => o.Value).ToList();

        /// <summary>
        /// Returns true if a handler with the specified name is registered.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasHandler(string name) => _jsToNetHandlers.ContainsKey(name);

        /// <summary>
        /// Check if a Delegate has handlers
        /// </summary>
        /// <param name="handler"></param>
        /// <returns>True if a handler was removed.</returns>
        public static bool HasHandler(Delegate handler) => _jsToNetHandlers.ToArray().Any(o => o.Value.Func == handler);

        /// <summary>
        /// Returns true if a handler with the specified name is registered.
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool HasHandler(Callback callback) => callback != null && _jsToNetHandlers.ContainsKey(callback.Id);

        /// <summary>
        /// Dispatch a JS -> .Net call. Invoked from Javascript through the _JSToNetCall JSExport binding.
        /// Marshals the JS argument array into the handler's parameters, invokes it, and returns the
        /// result wrapped in a one-element JS array so JS reads index 0.
        /// </summary>
        internal static SpawnJSHandle JSToNetDispatch(string cmd, SpawnJSHandle argsArray)
        {
            if (JS.Verbose) Console.WriteLine($">> JSToNetDispatch: {cmd}");
            if (!_jsToNetHandlers.TryGetValue(cmd, out var handler))
                throw new Exception($"SpawnJSRuntime: no JS->.Net handler registered for '{cmd}'");
            var method = handler.Func.Method;
            var parameters = method.GetParameters();
            var netArgs = new object?[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
                netArgs[i] = JS.MarshallJSToNet(parameters[i].ParameterType, argsArray, i);
            // invoke first, then dispose. Disposing a `once` handler before the call would release its
            // JS function handle while that very call is still in flight.
            var result = handler.Func.DynamicInvoke(netArgs);
            if (handler.Once)
            {
                handler.Dispose();
            }
            // Package the result as [result] so JS reads index 0 (mirror of the outbound [ret] wrapper).
            var retArray = JS.NewJSArray();
            if (method.ReturnType != typeof(void))
                JS.MarshallNetToJS(retArray, 0, result);
            if (JS.Verbose) Console.WriteLine($"<< JSToNetDispatch: {cmd}");
            return retArray;
        }
        #endregion


        // FuncCallback Create
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<TResult> Create<TResult>(Func<TResult> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new FuncCallback<TResult>(callback, false) : callbackGroup.Add(new FuncCallback<TResult>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, TResult> Create<T1, TResult>(Func<T1, TResult> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new FuncCallback<T1, TResult>(callback, false) : callbackGroup.Add(new FuncCallback<T1, TResult>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, TResult> Create<T1, T2, TResult>(Func<T1, T2, TResult> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new FuncCallback<T1, T2, TResult>(callback, false) : callbackGroup.Add(new FuncCallback<T1, T2, TResult>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, TResult> Create<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new FuncCallback<T1, T2, T3, TResult>(callback, false) : callbackGroup.Add(new FuncCallback<T1, T2, T3, TResult>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, TResult> Create<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new FuncCallback<T1, T2, T3, T4, TResult>(callback, false) : callbackGroup.Add(new FuncCallback<T1, T2, T3, T4, TResult>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, TResult> Create<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new FuncCallback<T1, T2, T3, T4, T5, TResult>(callback, false) : callbackGroup.Add(new FuncCallback<T1, T2, T3, T4, T5, TResult>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, TResult> Create<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new FuncCallback<T1, T2, T3, T4, T5, T6, TResult>(callback, false) : callbackGroup.Add(new FuncCallback<T1, T2, T3, T4, T5, T6, TResult>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult> Create<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>(callback, false) : callbackGroup.Add(new FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>(callback, false));
        // FuncCallback CreateOne
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<TResult> CreateOne<TResult>(Func<TResult> callback) => new FuncCallback<TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, TResult> CreateOne<T1, TResult>(Func<T1, TResult> callback) => new FuncCallback<T1, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, TResult> CreateOne<T1, T2, TResult>(Func<T1, T2, TResult> callback) => new FuncCallback<T1, T2, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, TResult> CreateOne<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> callback) => new FuncCallback<T1, T2, T3, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, TResult> CreateOne<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> callback) => new FuncCallback<T1, T2, T3, T4, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, TResult> CreateOne<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> callback) => new FuncCallback<T1, T2, T3, T4, T5, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, TResult> CreateOne<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> callback) => new FuncCallback<T1, T2, T3, T4, T5, T6, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult> CreateOne<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> callback) => new FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>(callback, true);
        // ActionCallback Create
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback Create(Action callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new ActionCallback(callback, false) : callbackGroup.Add(new ActionCallback(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1> Create<T1>(Action<T1> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new ActionCallback<T1>(callback, false) : callbackGroup.Add(new ActionCallback<T1>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2> Create<T1, T2>(Action<T1, T2> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new ActionCallback<T1, T2>(callback, false) : callbackGroup.Add(new ActionCallback<T1, T2>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3> Create<T1, T2, T3>(Action<T1, T2, T3> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new ActionCallback<T1, T2, T3>(callback, false) : callbackGroup.Add(new ActionCallback<T1, T2, T3>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4> Create<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new ActionCallback<T1, T2, T3, T4>(callback, false) : callbackGroup.Add(new ActionCallback<T1, T2, T3, T4>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new ActionCallback<T1, T2, T3, T4, T5>(callback, false) : callbackGroup.Add(new ActionCallback<T1, T2, T3, T4, T5>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new ActionCallback<T1, T2, T3, T4, T5, T6>(callback, false) : callbackGroup.Add(new ActionCallback<T1, T2, T3, T4, T5, T6>(callback, false));
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> callback, CallbackGroup? callbackGroup = null) => callbackGroup == null ? new ActionCallback<T1, T2, T3, T4, T5, T6, T7>(callback, false) : callbackGroup.Add(new ActionCallback<T1, T2, T3, T4, T5, T6, T7>(callback, false));
        // ActionCallback CreateOne
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback CreateOne(Action callback) => new ActionCallback(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1> CreateOne<T1>(Action<T1> callback) => new ActionCallback<T1>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2> CreateOne<T1, T2>(Action<T1, T2> callback) => new ActionCallback<T1, T2>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3> CreateOne<T1, T2, T3>(Action<T1, T2, T3> callback) => new ActionCallback<T1, T2, T3>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4> CreateOne<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback) => new ActionCallback<T1, T2, T3, T4>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5> CreateOne<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> callback) => new ActionCallback<T1, T2, T3, T4, T5>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6> CreateOne<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> callback) => new ActionCallback<T1, T2, T3, T4, T5, T6>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7> CreateOne<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> callback) => new ActionCallback<T1, T2, T3, T4, T5, T6, T7>(callback, true);

    }
    /// <summary>
    /// CallbackGroup can be used as a container for a group of Callbacks allowing the entire group to be disposed with 1 call.
    /// </summary>
    public class CallbackGroup : IDisposable
    {
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
        public T Add<T>(T wrapper) where T : Callback
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
