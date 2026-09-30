using SpawnDev.SpawnJS.Events;
using System.Collections.Concurrent;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// WORK IN PROGRESS - NOT WIRED UP YET. Intended to wrap a .Net method so it can be passed to JS and
    /// invoked directly (the JS -> .Net callback path). The JS-side dispatch (FireCallback / FireCallbackAsync
    /// in SpawnJSRuntime) is currently a logging placeholder. See the commented ActionCallback sketch below
    /// for the intended shape.
    /// </summary>
    public abstract partial class Callback : IDisposable
    {
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
        /// <summary>
        /// Callback id incrementer
        /// </summary>
        private static double _callbackIdNext = 0;
        /// <summary>
        /// Callback id
        /// </summary>
        public double Id { get; private set; }
        /// <summary>
        /// The number of times this Callback has been called
        /// </summary>
        public long CalledCount { get; private set; }
        /// <summary>
        /// Returns true if the Callback has been called at least once
        /// </summary>
        public bool HasBeenCalled => CalledCount > 0;
        /// <summary>
        /// Holds all active Callbacks
        /// </summary>
        private static ConcurrentDictionary<double, Callback> _callbacks = new ConcurrentDictionary<double, Callback>();
        /// <summary>
        /// The number of active Callbacks
        /// </summary>
        public static int CallbackCount => _callbacks.Count;
        /// <summary>
        /// Returns true if the Callback should only fire at most once
        /// </summary>
        public bool Once { get; private set; }
        /// <summary>
        /// Returns true if the Callback was sent to Javascript at least once.<br/>
        /// A Callback that is not sent to Javascript does not have to and will not notify Javascript when it disposes.<br/>
        /// </summary>
        public bool Sent { get; internal set; }
        /// <summary>
        /// New Callback instance
        /// </summary>
        /// <param name="once"></param>
        public Callback(bool once)
        {
            Id = ++_callbackIdNext;
            Once = once;
            _callbacks.TryAdd(Id, this);
        }
        /// <summary>
        /// The method inheriting classes must provide. Invokes the handler. Every argument Javascript passed is already in .Net memory, written by
        /// <see cref="ArgumentSchemas"/>: read them in order with <see cref="ReadArg{T}"/>.
        /// </summary>
        /// <param name="reader">The arguments, in order</param>
        /// <param name="args">The Javascript arguments array, where a return value is written (at index argsCount)</param>
        /// <param name="argsCount">How many arguments Javascript passed</param>
        protected abstract void HandleCallback(ref JSTapeReader reader, SpawnJSObjectReference args, double argsCount);
        /// <summary>The .Net types of the arguments, in order. Javascript writes each by its type's schema.</summary>
        protected virtual Type[] ArgumentTypes => Type.EmptyTypes;
        JSSchema[]? _argumentSchemas;
        /// <summary>The schemas Javascript writes the arguments by, sent with the Callback.</summary>
        internal JSSchema[] ArgumentSchemas => _argumentSchemas ??= ArgumentTypes.Select(t => SpawnJSRuntime.Instance.GetMarshaller(t).Schema).ToArray();
        /// <summary>
        /// Reads argument <paramref name="index"/>. Every declared argument is written (a missing one as undefined), so
        /// each is read in order; one Javascript did not pass reads as default, as it always did.
        /// </summary>
        protected static T ReadArg<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] T>(ref JSTapeReader reader, int index, double argsCount)
        {
            var value = SpawnJSRuntime.Instance.GetMarshaller<T>().Read(ref reader);
            return index < argsCount ? value : default!;
        }
        /// <summary>
        /// Recieved the notifications call from Javascript when a Callabck has been called
        /// </summary>
        /// <param name="callbackId">The callback being called</param>
        /// <param name="argsId">The Javascript arguments array; a return value is written to it</param>
        /// <param name="argsCount">How many arguments Javascript passed</param>
        internal static void HandleCallback(double callbackId, double argsId, double argsCount)
        {
            // Where Javascript wrote the arguments in the runtime's inbound buffer, in cells - or, below zero, minus the
            // bytes of arguments too large for it, held for _spawnJSInteropCallResult. Cell 0 carries it because a
            // generated JSImport callback takes at most 3 arguments. Read first: a nested callback (which would write it
            // again) can only start once this one calls into Javascript.
            var inboundOffset = SpawnJSRuntime.Instance.InboundCells[0];
            if (_callbacks.TryGetValue(callbackId, out var callback))
            {
                // we use preventDispose = true on this SpawnJSObjectReference to save an unnecessary JS call in the dispose...
                // this array will be automatically released after the call returns, so calling release on it again would be a waste
                // args will never be null as JS is sending args from `function(...args)` which is always an array
                var args = SpawnJSObjectReference.FromID(argsId, preventDispose: true)!;
                // increment the CalledCount
                callback.CalledCount++;
                // Dispose now if Once (Javascript has already removed it on its end)
                if (callback.Once) callback.Dispose();
                // fire the strongly typed ActionCallback/FuncCallback handler, reading the arguments Javascript wrote
                var js = SpawnJSRuntime.Instance;
                if (inboundOffset >= 0)
                {
                    var reader = new JSTapeReader(js.InboundCells.AsSpan((int)inboundOffset));
                    callback.HandleCallback(ref reader, args, argsCount);
                }
                else
                {
                    var reader = new JSTapeReader(js.FetchHeldResult((int)-inboundOffset));
                    callback.HandleCallback(ref reader, args, argsCount);
                }
            }
        }
        /// <summary>
        /// Returns true if teh Callback has been disposed and can no long fire
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// Dispose the Callabck and release all resources
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            _callbacks.TryRemove(Id, out _);
            OnDisposed?.Invoke();
            // notify JS to release the Calback to prevent additional calls.
            // only need to notify JS if the Callabck was actually sent
            // and Javascript has not already released it (it auto-releases Callbacks with Once == true)
            var jsSideReleasedIt = Once && HasBeenCalled;
            // DotnetInstance.Id: the Javascript function was registered under this runtime's instance id. Instance.Id is
            // the globalThis sentinel (-1), which matched nothing, so no Callback was ever released Javascript side.
            if (Sent && !jsSideReleasedIt) SpawnJSRuntime._releaseCallback(SpawnJSRuntime.Instance.DotnetInstance.Id, Id);
            Id = 0;
        }
    }
}
