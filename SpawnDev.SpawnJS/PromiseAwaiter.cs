using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Native;
using System.Collections.Concurrent;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Awaiting a Javascript promise without creating a single Javascript function.<br/>
    /// <br/>
    /// The old shape built the bridge from the .Net side: <c>ThenAsync</c> made two <see cref="Callback"/>s
    /// and a <c>CallbackGroup</c> for <em>every await</em>, and each Callback meant a Javascript function
    /// created, given a handle, registered in a dispatch table, invoked once, and torn down. Per awaited
    /// promise. That is machinery Microsoft's own WASM interop never builds, and neither does this now.
    /// <br/><br/>
    /// The bridge is built from the Javascript side instead, out of parts that already exist:
    /// <list type="number">
    /// <item>.Net makes a <see cref="TaskCompletionSource"/>, files it under a numeric CALL ID, and hands
    /// the Task straight back to the caller.</item>
    /// <item>Javascript attaches to the promise with <c>then</c> using its OWN closures - nothing .Net has
    /// to register, hold, or release.</item>
    /// <item>When the promise settles, Javascript reports the outcome over the inbound call path that
    /// every callback already uses, carrying the call id and the value.</item>
    /// <item>.Net completes the TaskCompletionSource for that id.</item>
    /// </list>
    /// Per await: no Javascript function registered, and one number plus the settled value crossing once.
    /// </summary>
    internal static class PromiseAwaiter
    {
        /// <summary>
        /// A pending await, kept only until its promise settles. Removed on settle by whichever arm fires
        /// - leaving entries here would be a leak of the same shape as the slot leaks, so the removal is
        /// unconditional and happens before the continuation runs.
        /// </summary>
        interface IPending
        {
            void Resolve(SpawnJSHandle buffer, int offset);
            void Reject(Exception exception);
        }

        sealed class Pending<T> : IPending
        {
            public readonly TaskCompletionSource<T> Source = new TaskCompletionSource<T>();
            /// <summary>Null for a plain Task, which carries completion but no value.</summary>
            public Type? ResultType { get; init; }

            public void Resolve(SpawnJSHandle buffer, int offset)
            {
                if (ResultType == null)
                {
                    Source.TrySetResult(default!);
                    return;
                }
                var value = SpawnJSRuntime.Instance!.MarshallJSToNet(ResultType, buffer, offset);
                Source.TrySetResult((T)value!);
            }

            public void Reject(Exception exception) => Source.TrySetException(exception);
        }

        static readonly ConcurrentDictionary<double, IPending> _pending = new ConcurrentDictionary<double, IPending>();
        static double _nextId;

        /// <summary>Number of awaits still outstanding. Diagnostics, and what the leak guard reads.</summary>
        public static int PendingCount => _pending.Count;

        /// <summary>
        /// Awaits <paramref name="promise"/> as a <see cref="Task"/> carrying no value.
        /// </summary>
        public static Task Await(SpawnJSHandle promise)
        {
            var pending = new Pending<bool> { ResultType = null };
            return Register(promise, pending, pending.Source.Task);
        }

        /// <summary>
        /// Awaits <paramref name="promise"/> as a <see cref="Task{TResult}"/>, marshalling the resolved
        /// value as <typeparamref name="TResult"/>.
        /// </summary>
        public static Task<TResult> Await<TResult>(SpawnJSHandle promise)
        {
            var pending = new Pending<TResult> { ResultType = typeof(TResult) };
            return Register(promise, pending, pending.Source.Task);
        }

        static TTask Register<TTask>(SpawnJSHandle promise, IPending pending, TTask task)
        {
            var id = ++_nextId;
            // filed BEFORE Javascript is told, because a promise that is already settled calls straight
            // back - then is not deferred to a later turn when the value is there already
            _pending[id] = pending;
            if (!promise.TryGetSlot(out var slot))
            {
                _pending.TryRemove(id, out _);
                throw new InvalidOperationException("the value being awaited is not addressable as a Javascript reference");
            }
            SlotInterop.AwaitPromise(SpawnJSRuntime.Instance!.CtxId, slot, id);
            return task;
        }

        /// <summary>
        /// Javascript reporting that an awaited promise settled. The value - or the rejection reason -
        /// is in the caller's own region of the inbound buffer, exactly where an inbound callback's
        /// arguments live.
        /// </summary>
        internal static void Settle(double id, bool isError, SpawnJSHandle buffer, int offset)
        {
            // removed first: a promise settles once, and an entry left behind is a leak
            if (!_pending.TryRemove(id, out var pending)) return;
            if (isError)
            {
                var error = (Error?)SpawnJSRuntime.Instance!.MarshallJSToNet(typeof(Error), buffer, offset);
                pending.Reject(Promise.UnknownErrorToException(error));
                return;
            }
            pending.Resolve(buffer, offset);
        }
    }
}
