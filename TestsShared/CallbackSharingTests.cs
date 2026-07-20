using SpawnDev.SpawnJS;

namespace TestsShared
{
    /// <summary>
    /// Callback lifetime and Javascript function sharing.<br/>
    /// A Callback used to register a brand new Javascript function, and so allocate a brand new JSObject,
    /// every single time one was created - even for a delegate that already had one alive. Two
    /// subscriptions of the same handler cost two boundary crossings and two JS functions for nothing.
    /// The function is now created once per delegate and reference counted by the Callbacks holding it.
    /// <br/>
    /// Disposal stays manual, exactly as in SpawnDev.BlazorJS. There is no finalizer.
    /// </summary>
    public class CallbackSharingTests(SpawnJSRuntime JS)
    {
        // instance methods, so each delegate created from them has the same method and the same target and
        // therefore compares equal - which is what makes them share
        void HandlerA() { _aCalls++; }
        void HandlerB() { _bCalls++; }
        int _aCalls;
        int _bCalls;

        /// <summary>
        /// Invokes a Callback's Javascript function from the Javascript side, which is the only way to
        /// prove the function is really wired to the .Net method.
        /// </summary>
        void InvokeFromJS(Callback callback)
        {
            const string key = "_spawnjs_cb_under_test";
            JS.Set(key, callback);
            try
            {
                JS.CallVoid(key);
            }
            finally
            {
                JS.Delete(key);
            }
        }

        /// <summary>
        /// Two Callbacks over the same delegate must share one Javascript function - the same JS object,
        /// not merely an equal id. Object identity is checked with Javascript strict equality.
        /// </summary>
        [SpawnJSTest]
        public async Task SameDelegateSharesOneJSFunctionTest()
        {
            Action handler = HandlerA;
            using var first = Callback.Create(handler);
            using var second = Callback.Create(handler);

            if (ReferenceEquals(first, second))
                throw new Exception("Create returned the same Callback twice; each consumer needs its own disposable Callback");
            if (first.Id != second.Id)
                throw new Exception($"Shared Callbacks got different ids ({first.Id} vs {second.Id})");
            if (!JS.ObjectEquals(first.JSHandle.JSObject, second.JSHandle.JSObject, true))
                throw new Exception("The two Callbacks are backed by different Javascript function objects");
            if (first.SharedHolderCount != 2)
                throw new Exception($"Shared holder count was {first.SharedHolderCount}, expected 2");
        }

        /// <summary>
        /// Different delegates must not share. Without this the sharing test above would also pass for an
        /// implementation that handed the same function to everyone.
        /// </summary>
        [SpawnJSTest]
        public async Task DifferentDelegatesDoNotShareTest()
        {
            Action a = HandlerA;
            Action b = HandlerB;
            using var first = Callback.Create(a);
            using var second = Callback.Create(b);

            if (first.Id == second.Id)
                throw new Exception("Two different delegates were given the same callback id");
            if (JS.ObjectEquals(first.JSHandle.JSObject, second.JSHandle.JSObject, true))
                throw new Exception("Two different delegates were given the same Javascript function");
            if (first.SharedHolderCount != 1)
                throw new Exception($"Holder count was {first.SharedHolderCount}, expected 1");
        }

        /// <summary>
        /// A capturing lambda is a distinct closure each time it is written, so it is genuinely a
        /// different delegate and must get its own function. Sharing those would be wrong.
        /// </summary>
        [SpawnJSTest]
        public async Task CapturingLambdasDoNotShareTest()
        {
            var counter = 0;
            using var first = Callback.Create(new Action(() => counter++));
            using var second = Callback.Create(new Action(() => counter++));

            if (JS.ObjectEquals(first.JSHandle.JSObject, second.JSHandle.JSObject, true))
                throw new Exception("Two separate closures shared one Javascript function");
        }

        /// <summary>
        /// The whole point of the reference count: disposing one holder must leave the other working.
        /// This is the regression that would bite hardest - a shared function torn down early looks like
        /// an event that silently stops firing.
        /// </summary>
        [SpawnJSTest]
        public async Task DisposingOneHolderLeavesTheOtherWorkingTest()
        {
            Action handler = HandlerA;
            var first = Callback.Create(handler);
            using var second = Callback.Create(handler);

            _aCalls = 0;
            InvokeFromJS(second);
            if (_aCalls != 1) throw new Exception($"Handler fired {_aCalls} times before any dispose, expected 1");

            first.Dispose();
            if (!first.IsDisposed) throw new Exception("First Callback did not report itself disposed");
            if (second.IsDisposed) throw new Exception("Disposing one Callback disposed its sibling");
            if (second.SharedHolderCount != 1)
                throw new Exception($"Holder count after one dispose was {second.SharedHolderCount}, expected 1");

            // the surviving Callback must still dispatch - the JS function must not have been released
            InvokeFromJS(second);
            if (_aCalls != 2) throw new Exception($"Handler fired {_aCalls} times after the sibling was disposed, expected 2");
            if (!Callback.HasHandler(second.Id)) throw new Exception("The surviving Callback lost its handler registration");
        }

        /// <summary>
        /// Releasing the last holder must actually tear the function down, otherwise this is a leak with
        /// extra steps.
        /// </summary>
        [SpawnJSTest]
        public async Task LastDisposeReleasesTheJSFunctionTest()
        {
            Action handler = HandlerA;
            var first = Callback.Create(handler);
            var second = Callback.Create(handler);
            var id = first.Id;

            first.Dispose();
            if (!Callback.HasHandler(id)) throw new Exception("The function was unregistered while a holder was still alive");

            second.Dispose();
            if (Callback.HasHandler(id)) throw new Exception("The last dispose left the handler registered");
            if (!second.JSHandle.IsDisposed) throw new Exception("The last dispose left the Javascript function handle alive");
        }

        /// <summary>
        /// After the last holder is gone, the same delegate must be able to get a fresh function rather
        /// than a stale released one.
        /// </summary>
        [SpawnJSTest]
        public async Task DelegateCanBeReacquiredAfterFullReleaseTest()
        {
            Action handler = HandlerA;
            var first = Callback.Create(handler);
            var firstId = first.Id;
            first.Dispose();

            using var second = Callback.Create(handler);
            if (second.Id == firstId) throw new Exception("A released function was handed out again instead of a fresh one");
            if (second.JSHandle.IsDisposed) throw new Exception("The re-acquired Callback got a disposed Javascript handle");

            _aCalls = 0;
            InvokeFromJS(second);
            if (_aCalls != 1) throw new Exception($"Re-acquired Callback fired {_aCalls} times, expected 1");
        }

        /// <summary>
        /// A `once` Callback disposes itself after firing, so sharing one would tear the function out from
        /// under every other holder. It must always get its own.
        /// </summary>
        [SpawnJSTest]
        public async Task OnceCallbacksAreNeverSharedTest()
        {
            Action handler = HandlerB;
            using var normal = Callback.Create(handler);
            var once = Callback.CreateOne(handler);

            if (once.Id == normal.Id) throw new Exception("A once Callback shared a function with a normal Callback");
            if (JS.ObjectEquals(once.JSHandle.JSObject, normal.JSHandle.JSObject, true))
                throw new Exception("A once Callback shared a Javascript function with a normal Callback");

            _bCalls = 0;
            InvokeFromJS(once);
            if (_bCalls != 1) throw new Exception($"Once Callback fired {_bCalls} times, expected 1");
            if (!once.IsDisposed) throw new Exception("Once Callback did not dispose itself after firing");

            // the normal Callback over the same delegate must be untouched by the once one disposing
            if (normal.IsDisposed) throw new Exception("A once Callback disposing took its delegate's normal Callback with it");
            _bCalls = 0;
            InvokeFromJS(normal);
            if (_bCalls != 1) throw new Exception($"Normal Callback fired {_bCalls} times after the once Callback fired, expected 1");
        }

        /// <summary>
        /// A named handler is an intent with its own identity, not an anonymous callback, so it must not
        /// be pooled with the delegate's shared function.
        /// </summary>
        [SpawnJSTest]
        public async Task NamedHandlersAreNeverSharedTest()
        {
            Action handler = HandlerA;
            using var anonymous = Callback.Create(handler);
            using var named = new Callback(handler, "_spawnjs_named_intent");

            if (named.Id != "_spawnjs_named_intent") throw new Exception($"Named handler got id '{named.Id}'");
            if (JS.ObjectEquals(named.JSHandle.JSObject, anonymous.JSHandle.JSObject, true))
                throw new Exception("A named handler shared the delegate's anonymous function");
            if (anonymous.SharedHolderCount != 1)
                throw new Exception($"Anonymous holder count was {anonymous.SharedHolderCount}, expected 1");
        }

        /// <summary>
        /// Sharing has to actually reduce the number of Javascript functions alive, not just report that
        /// it does. Counted across a batch so the assertion is about the real total.
        /// </summary>
        [SpawnJSTest]
        public async Task SharingReducesLiveJSFunctionCountTest()
        {
            Action handler = HandlerA;
            var before = Callback.SharedFunctionCount;
            var callbacks = new List<Callback>();
            for (var i = 0; i < 8; i++) callbacks.Add(Callback.Create(handler));
            try
            {
                var created = Callback.SharedFunctionCount - before;
                if (created != 1) throw new Exception($"8 Callbacks over one delegate created {created} Javascript functions, expected 1");
                if (callbacks[0].SharedHolderCount != 8)
                    throw new Exception($"Holder count was {callbacks[0].SharedHolderCount}, expected 8");
            }
            finally
            {
                foreach (var cb in callbacks) cb.Dispose();
            }
            if (Callback.SharedFunctionCount != before)
                throw new Exception($"Live function count was {Callback.SharedFunctionCount} after disposing all holders, expected {before}");
        }

        /// <summary>
        /// Disposal is manual and there is no finalizer, which is the SpawnDev.BlazorJS contract. Dropping
        /// a Callback without disposing it leaves its function registered on purpose - Javascript can
        /// still be holding that function, so collecting it out from under a live subscription would be
        /// worse than leaking it. This test pins the contract so nobody "helpfully" adds a finalizer.
        /// </summary>
        [SpawnJSTest]
        public async Task DroppedCallbackIsNotCollectedTest()
        {
            string id;
            {
                var dropped = Callback.Create(new Action(() => { }));
                id = dropped.Id;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            if (!Callback.HasHandler(id))
                throw new Exception("A dropped Callback was released without an explicit Dispose; disposal is manual by design");

            Callback.RemoveHandler(id);
            if (Callback.HasHandler(id)) throw new Exception("RemoveHandler did not unregister the handler");
        }

        /// <summary>
        /// Dispose must be idempotent. With a shared reference count, a second Dispose that released again
        /// would drop the count below the number of live holders and free a function still in use.
        /// </summary>
        [SpawnJSTest]
        public async Task DoubleDisposeReleasesOnlyOnceTest()
        {
            Action handler = HandlerA;
            var first = Callback.Create(handler);
            using var second = Callback.Create(handler);

            first.Dispose();
            first.Dispose();
            first.Dispose();

            if (second.SharedHolderCount != 1)
                throw new Exception($"Holder count after repeated disposes was {second.SharedHolderCount}, expected 1");
            if (!Callback.HasHandler(second.Id)) throw new Exception("Repeated disposes released a function still in use");

            _aCalls = 0;
            InvokeFromJS(second);
            if (_aCalls != 1) throw new Exception($"Surviving Callback fired {_aCalls} times, expected 1");
        }
    }
}
