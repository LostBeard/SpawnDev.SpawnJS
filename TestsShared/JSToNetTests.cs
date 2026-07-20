using SpawnDev.SpawnJS;


namespace TestsShared
{
    /// <summary>
    /// JS ➡️ .Net inbound dispatch tests.<br/>
    /// Every test here drives the full production path: a Callback is marshalled out to Javascript as a
    /// real function, Javascript invokes it, the arguments are marshalled back in typed by the delegate's
    /// own parameters, the delegate runs, and the result is marshalled out through the [result] wrapper.<br/>
    /// The argument count matters. Each inbound argument is read through its own volatile SpawnJSHandle
    /// borrowed from the shared argument array, so anything that mishandles borrowed-parent ownership
    /// breaks at argument two and later, not at argument one.
    /// </summary>
    public class JSToNetTests(SpawnJSRuntime JS) : IDisposable
    {
        readonly List<string> _globals = new List<string>();

        /// <summary>
        /// Publishes a Callback on globalThis under a unique name so Javascript can invoke it by name.
        /// </summary>
        string Publish(Callback cb, string name)
        {
            var globalName = $"_spawnjs_test_{name}";
            JS.Set(globalName, cb);
            _globals.Add(globalName);
            return globalName;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            foreach (var name in _globals) JS.Delete(name);
            _globals.Clear();
        }

        /// <summary>
        /// Zero arguments, no return value. The minimum inbound round trip.
        /// </summary>
        [SpawnJSTest]
        public async Task JSToNetVoidNoArgsTest()
        {
            var fired = 0;
            using var cb = new Callback(new Action(() => fired++));
            var name = Publish(cb, nameof(JSToNetVoidNoArgsTest));
            JS.CallVoid(name);
            if (fired != 1) throw new Exception($"Handler fired {fired} times, expected 1");
            JS.CallVoid(name);
            if (fired != 2) throw new Exception($"Handler fired {fired} times, expected 2");
        }

        /// <summary>
        /// One argument in, one value out.
        /// </summary>
        [SpawnJSTest]
        public async Task JSToNetOneArgTest()
        {
            using var cb = new Callback(new Func<string, string>(msg => $"got:{msg}"));
            var name = Publish(cb, nameof(JSToNetOneArgTest));
            var result = JS.Call<string>(name, "hello");
            if (result != "got:hello") throw new Exception($"Expected 'got:hello', got '{result}'");
        }

        /// <summary>
        /// Two arguments. This is the first arity that reads the inbound argument array more than once,
        /// so it is the regression guard for borrowed-parent ownership in SpawnJSHandle.
        /// </summary>
        [SpawnJSTest]
        public async Task JSToNetTwoArgsTest()
        {
            using var cb = new Callback(new Func<string, string, string>((a, b) => $"{a}|{b}"));
            var name = Publish(cb, nameof(JSToNetTwoArgsTest));
            var result = JS.Call<string>(name, "one", "two");
            if (result != "one|two") throw new Exception($"Expected 'one|two', got '{result}'");
        }

        /// <summary>
        /// Three arguments of mixed types, so the inbound path has to select a different marshaller per
        /// argument rather than repeating one.
        /// </summary>
        [SpawnJSTest]
        public async Task JSToNetThreeMixedArgsTest()
        {
            using var cb = new Callback(new Func<string, int, bool, string>((s, i, b) => $"{s}:{i}:{b}"));
            var name = Publish(cb, nameof(JSToNetThreeMixedArgsTest));
            var result = JS.Call<string>(name, "n", 42, true);
            if (result != "n:42:True") throw new Exception($"Expected 'n:42:True', got '{result}'");
        }

        /// <summary>
        /// Verifies the .Net return value survives the [result] wrapper for a non-string type.
        /// </summary>
        [SpawnJSTest]
        public async Task JSToNetReturnsNumberTest()
        {
            using var cb = new Callback(new Func<int, int, int>((a, b) => a * b));
            var name = Publish(cb, nameof(JSToNetReturnsNumberTest));
            var result = JS.Call<int>(name, 6, 7);
            if (result != 42) throw new Exception($"Expected 42, got {result}");
        }

        /// <summary>
        /// A `once` Callback must complete the call it is disposed on. Disposing before the delegate runs
        /// would release the JS function handle while that very call is still in flight.
        /// </summary>
        [SpawnJSTest]
        public async Task JSToNetOnceDisposesAfterInvokeTest()
        {
            var calls = 0;
            var cb = new Callback(new Func<string, string>(msg => { calls++; return $"once:{msg}"; }), true);
            var name = Publish(cb, nameof(JSToNetOnceDisposesAfterInvokeTest));
            var result = JS.Call<string>(name, "x");
            if (result != "once:x") throw new Exception($"Expected 'once:x', got '{result}'");
            if (calls != 1) throw new Exception($"Handler ran {calls} times, expected 1");
            if (!cb.IsDisposed) throw new Exception("A `once` Callback should be disposed after its first invocation");
        }

        /// <summary>
        /// Disposing a Callback removes its handler, so a later invocation from Javascript must not
        /// silently reach a dead delegate.
        /// </summary>
        [SpawnJSTest]
        public async Task JSToNetDisposedCallbackIsUnregisteredTest()
        {
            var cb = new Callback(new Func<string, string>(msg => msg));
            var id = cb.Id;
            if (!Callback.HasHandler(id)) throw new Exception("Callback did not register a handler");
            cb.Dispose();
            if (Callback.HasHandler(id)) throw new Exception("Disposed Callback left its handler registered");
        }
    }
}
