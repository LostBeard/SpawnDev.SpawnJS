using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;

namespace TestsShared
{
    /// <summary>
    /// Task and Promise marshalled as each other.<br/>
    /// <br/>
    /// The failure being guarded against is silent: before TaskMarshaller a Task was a plain class to the
    /// graph, so ObjectMarshaller claimed it and produced <c>{result, id, status, ...}</c> - an object
    /// Javascript can hold and never await, with no exception raised. So the tests assert that Javascript
    /// receives something with a <c>then</c>, and that awaiting it actually yields the value.
    /// </summary>
    public class TaskMarshallerTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// A Task crosses as a real Promise - thenable, and NOT a property-walked Task object.
        /// </summary>
        [SpawnJSTest]
        public async Task TaskCrossesAsAPromiseTest()
        {
            JS.Set("__taskPlain", Task.CompletedTask);

            var ctor = JS.Get<string>("__taskPlain.constructor.name");
            if (ctor != "Promise") throw new Exception($"a Task crossed as a {ctor}, expected a Promise");

            JS.CallVoid("eval", "globalThis.__taskThenType = typeof globalThis.__taskPlain.then;");
            var thenType = JS.Get<string>("__taskThenType");
            if (thenType != "function") throw new Exception($"the value is not thenable (typeof then = {thenType})");

            // the exact wrong result this marshaller exists to prevent
            if (!JS.IsUndefined("__taskPlain.status"))
                throw new Exception("the Task was property-walked into a plain object instead of a Promise");
        }

        /// <summary>
        /// And the Promise actually RESOLVES - a Promise that never settles would still pass the shape
        /// check above, so this awaits it Javascript side and records the outcome.
        /// </summary>
        [SpawnJSTest]
        public async Task TaskWithResultResolvesWithTheValueTest()
        {
            JS.Set("__taskValue", Task.FromResult(42));
            // null means not settled yet - reading it as int? keeps the value a NUMBER on the Javascript
            // side, which is what it should be
            JS.CallVoid("eval", "globalThis.__taskValueSeen = null; " +
                                "globalThis.__taskValue.then(v => globalThis.__taskValueSeen = v);");

            // give the promise a turn of the event loop to settle
            await Task.Delay(50);

            var seen = JS.Get<int?>("__taskValueSeen");
            if (seen == null) throw new Exception("the promise never settled");
            if (seen != 42) throw new Exception($"the promise resolved with {seen}, expected 42");
        }

        /// <summary>
        /// A Task that completes later must resolve when it completes, not at marshal time.
        /// </summary>
        [SpawnJSTest]
        public async Task DelayedTaskResolvesWhenItCompletesTest()
        {
            var source = new TaskCompletionSource<string>();
            JS.Set("__taskDelayed", source.Task);
            JS.CallVoid("eval", "globalThis.__taskDelayedSeen = ''; " +
                                "globalThis.__taskDelayed.then(v => globalThis.__taskDelayedSeen = v);");

            await Task.Delay(20);
            var early = JS.Get<string>("__taskDelayedSeen");
            if (early != "") throw new Exception($"the promise settled early with '{early}'");

            source.SetResult("done");
            await Task.Delay(50);

            var seen = JS.Get<string>("__taskDelayedSeen");
            if (seen != "done") throw new Exception($"after completion the promise held '{seen}'");
        }

        /// <summary>
        /// A faulted Task must REJECT the promise rather than resolving it, or Javascript silently treats
        /// a failure as success.
        /// </summary>
        [SpawnJSTest]
        public async Task FaultedTaskRejectsThePromiseTest()
        {
            var source = new TaskCompletionSource<int>();
            source.SetException(new Exception("boom"));
            JS.Set("__taskFaulted", source.Task);
            JS.CallVoid("eval", "globalThis.__taskFaultedState = 'pending'; " +
                                "globalThis.__taskFaulted.then(() => globalThis.__taskFaultedState = 'resolved', " +
                                "() => globalThis.__taskFaultedState = 'rejected');");

            await Task.Delay(50);

            var state = JS.Get<string>("__taskFaultedState");
            if (state != "rejected") throw new Exception($"a faulted Task left the promise '{state}'");
        }

        /// <summary>
        /// The read direction: a Javascript Promise becomes a Task that can be awaited in .Net.
        /// </summary>
        [SpawnJSTest]
        public async Task JavascriptPromiseReadsAsATaskTest()
        {
            JS.CallVoid("eval", "globalThis.__promiseToTask = Promise.resolve(5);");
            var task = JS.Get<Task<int>>("__promiseToTask");
            if (task == null) throw new Exception("read back as null");
            var value = await task;
            if (value != 5) throw new Exception($"awaited value was {value}, expected 5");
        }

        /// <summary>
        /// A Promise resolving with a string, to confirm the result is typed by the Task's own generic
        /// argument rather than guessed from the Javascript value.
        /// </summary>
        [SpawnJSTest]
        public async Task JavascriptPromiseReadsAsTypedTaskTest()
        {
            JS.CallVoid("eval", "globalThis.__promiseText = new Promise(r => setTimeout(() => r('later'), 10));");
            var task = JS.Get<Task<string>>("__promiseText");
            var value = await task!;
            if (value != "later") throw new Exception($"awaited value was '{value}'");
        }

        /// <summary>
        /// A non generic Task target awaits completion without a result.
        /// </summary>
        [SpawnJSTest]
        public async Task JavascriptPromiseReadsAsNonGenericTaskTest()
        {
            JS.CallVoid("eval", "globalThis.__promiseVoid = new Promise(r => setTimeout(r, 10));");
            var task = JS.Get<Task>("__promiseVoid");
            await task!;
            // reaching here without throwing is the assertion
        }

        /// <summary>
        /// A rejected Javascript Promise must FAULT the Task. Without this a failure crossing into .Net
        /// would look like a completed task and the caller would carry on with no value.
        /// </summary>
        [SpawnJSTest]
        public async Task RejectedPromiseFaultsTheTaskTest()
        {
            JS.CallVoid("eval", "globalThis.__promiseRejects = new Promise((_, reject) => setTimeout(() => reject(new Error('nope')), 10));");
            var task = JS.Get<Task<int>>("__promiseRejects");

            var threw = false;
            try { await task!; }
            catch (Exception) { threw = true; }
            if (!threw) throw new Exception("a rejected promise completed the task instead of faulting it");
        }

        /// <summary>
        /// Promise.ThenAsync(Type) with a PRIMITIVE result.<br/>
        /// It read the resolved value through a Callback&lt;SpawnJSHandle&gt;, and a SpawnJSHandle proxies
        /// a Javascript OBJECT - so a promise resolving with a number threw "JSObject proxy of number is
        /// not supported" inside the callback, which never reached the TaskCompletionSource. The returned
        /// Task then never completed, so this hung rather than failing. Only object-resolving promises
        /// worked, which is why nothing caught it.
        /// </summary>
        [SpawnJSTest]
        public async Task ThenAsyncByTypeHandlesPrimitiveResultsTest()
        {
            JS.CallVoid("eval", "globalThis.__thenByTypeNumber = Promise.resolve(11);");
            using var numberPromise = JS.Get<Promise>("__thenByTypeNumber")!;
            var number = await numberPromise.ThenAsync(typeof(int));
            if (number is not int n || n != 11) throw new Exception($"resolved with '{number}'");

            JS.CallVoid("eval", "globalThis.__thenByTypeText = Promise.resolve('words');");
            using var textPromise = JS.Get<Promise>("__thenByTypeText")!;
            var text = await textPromise.ThenAsync(typeof(string));
            if (text as string != "words") throw new Exception($"resolved with '{text}'");
        }

        /// <summary>
        /// The case that motivates the whole marshaller: Javascript calling a .Net method that returns a
        /// Task gets back a Promise it can await, rather than an object it cannot.
        /// </summary>
        [SpawnJSTest]
        public async Task JavascriptAwaitsADotnetAsyncCallbackTest()
        {
            using var callback = Callback.Create(async () =>
            {
                await Task.Delay(10);
                return 99;
            });
            JS.Set("__asyncCallback", callback);

            JS.CallVoid("eval", "globalThis.__asyncCallbackSeen = null; " +
                                "Promise.resolve(globalThis.__asyncCallback()).then(v => globalThis.__asyncCallbackSeen = v);");

            await Task.Delay(80);

            var seen = JS.Get<int?>("__asyncCallbackSeen");
            if (seen == null) throw new Exception("javascript never saw the async callback settle");
            if (seen != 99) throw new Exception($"javascript received {seen}, expected 99");
        }
    }
}
