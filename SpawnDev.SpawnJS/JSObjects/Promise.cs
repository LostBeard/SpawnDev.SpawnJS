using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The Promise object represents the eventual completion (or failure) of an asynchronous operation and its resulting value.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise
    /// </summary>
    public class Promise : SpawnJSObject
    {
        /// <summary>
        /// Explicit conversion from a Task to a Promise
        /// </summary>
        public static explicit operator Promise(Task t) => new Promise(t);
        /// <summary>
        /// Explicit conversion from a Func&lt;Task> to a Promise
        /// </summary>
        public static explicit operator Promise(Func<Task> fn) => new Promise(fn);
        /// <summary>
        /// The Promise() constructor creates Promise objects.
        /// </summary>
        public Promise(Func<Task> task) : base(JS.New("Promise", Callback.CreateOne((Function resolveFunc, Function rejectFunc) =>
        {
            task().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    rejectFunc.CallVoid();
                }
                else if (t.IsCanceled)
                {
                    rejectFunc.CallVoid();
                }
                else
                {
                    // the task has finished, so read the result synchronously. GetResult awaits and hands
                    // back a Task, which would resolve the Javascript promise with a marshalled .Net Task
                    // object instead of the value.
                    if (t.GetType().IsGenericType)
                    {
                        var result = t.GetCompletedResult();
                        resolveFunc.ApplyVoid(null, new object?[] { result });
                    }
                    else
                    {
                        resolveFunc.CallVoid();
                    }
                }
                resolveFunc.Dispose();
                rejectFunc.Dispose();
            });
        })))
        { }
        /// <summary>
        /// The Promise() constructor creates Promise objects.
        /// </summary>
        public Promise(Task task) : base(JS.New("Promise", Callback.CreateOne((Function resolveFunc, Function rejectFunc) =>
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    rejectFunc.CallVoid();
                }
                else if (t.IsCanceled)
                {
                    rejectFunc.CallVoid();
                }
                else
                {
                    // the task has finished, so read the result synchronously. GetResult awaits and hands
                    // back a Task, which would resolve the Javascript promise with a marshalled .Net Task
                    // object instead of the value.
                    if (t.GetType().IsGenericType)
                    {
                        var result = t.GetCompletedResult();
                        resolveFunc.ApplyVoid(null, new object?[] { result });
                    }
                    else
                    {
                        resolveFunc.CallVoid();
                    }
                }
                resolveFunc.Dispose();
                rejectFunc.Dispose();
            });
        })))
        {
            
        }
        /// <summary>
        /// The Promise() constructor creates Promise objects.
        /// </summary>
        public Promise(Action<Function, Function> executor) : base(JS.New("Promise", Callback.CreateOne(executor))) { }
        /// <summary>
        /// The Promise() constructor creates Promise objects.
        /// </summary>
        public Promise(Action<Function> executor) : base(JS.New("Promise", Callback.CreateOne(executor))) { }
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public Promise(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Set the methods for the Promise onFulfilled and onRejected events
        /// </summary>
        public void ThenCatch<TCatch>(ActionCallback thenCallback, ActionCallback<TCatch> catchCallback) => JSRef!.CallVoid("then", thenCallback, catchCallback);
        /// <summary>
        /// Set the methods for the Promise onFulfilled and onRejected events
        /// </summary>
        public void Then(ActionCallback thenCallback, ActionCallback catchCallback) => JSRef!.CallVoid("then", thenCallback, catchCallback);
        /// <summary>
        /// Set the methods for the Promise onFulfilled and onRejected events
        /// </summary>
        public void Then<TResult>(ActionCallback<TResult> thenCallback, ActionCallback catchCallback) => JSRef!.CallVoid("then", thenCallback, catchCallback);
        /// <summary>
        /// Set the methods for the Promise onFulfilled and onRejected events
        /// </summary>
        public void ThenCatch<TResult, TCatch>(ActionCallback<TResult> thenCallback, ActionCallback<TCatch> catchCallback) => JSRef!.CallVoid("then", thenCallback, catchCallback);
        /// <summary>
        /// Asynchronously wait for a Promise to complete
        /// </summary>
        /// <remarks>
        /// Javascript attaches to the promise with its own closures and reports the outcome by call id -
        /// see <see cref="PromiseAwaiter"/>. This used to build two <see cref="Callback"/>s and a
        /// <see cref="CallbackGroup"/> per await, each a Javascript function created, registered, invoked
        /// once and torn down.
        /// </remarks>
        public Task ThenAsync(int timeoutMS = 0)
            => WithTimeout(PromiseAwaiter.Await(JSRef!.JSHandle), timeoutMS);

        /// <summary>
        /// Faults the task if it has not settled within <paramref name="timeoutMS"/>. Zero means no
        /// timeout, and then the awaiter's task is handed back untouched.
        /// </summary>
        static Task WithTimeout(Task task, int timeoutMS)
        {
            if (timeoutMS <= 0) return task;
            return Race(task, timeoutMS);

            static async Task Race(Task task, int timeoutMS)
            {
                using var cts = new CancellationTokenSource();
                var delay = Task.Delay(timeoutMS, cts.Token);
                if (await Task.WhenAny(task, delay) != task) throw new Exception("Timed out");
                cts.Cancel();
                await task;
            }
        }

        /// <inheritdoc cref="WithTimeout(Task, int)"/>
        static Task<TResult> WithTimeout<TResult>(Task<TResult> task, int timeoutMS)
        {
            if (timeoutMS <= 0) return task;
            return Race(task, timeoutMS);

            static async Task<TResult> Race(Task<TResult> task, int timeoutMS)
            {
                using var cts = new CancellationTokenSource();
                var delay = Task.Delay(timeoutMS, cts.Token);
                if (await Task.WhenAny(task, delay) != task) throw new Exception("Timed out");
                cts.Cancel();
                return await task;
            }
        }
        /// <summary>
        /// Asynchronously wait for a Promise to complete
        /// </summary>
        public Task ThenCatchAsync<TCatch>(int timeoutMS = 0)
        {
            var t = new TaskCompletionSource();
            var callbacks = new CallbackGroup();
            var cancellationTokenSource = timeoutMS > 0 ? new CancellationTokenSource() : null;
            ThenCatch(callbacks.Add(Callback.Create(() =>
            {
                if (t.TrySetResult())
                {
                    cancellationTokenSource?.Dispose();
                    callbacks.Dispose();
                }
            })), callbacks.Add(new ActionCallback<TCatch>((catchValue) =>
            {
                if (t.TrySetException(new PromiseCatchException<TCatch>(catchValue)))
                {
                    cancellationTokenSource?.Dispose();
                    callbacks.Dispose();
                }
            })));
            cancellationTokenSource?.Token.Register(() =>
            {
                if (t.TrySetException(new Exception("Timed out")))
                {
                    cancellationTokenSource?.Dispose();
                    callbacks.Dispose();
                }
            });
            cancellationTokenSource?.CancelAfter(timeoutMS);
            return t.Task;
        }
        /// <summary>
        /// Asynchronously wait for a Promise to complete
        /// </summary>
        /// <inheritdoc cref="ThenAsync(int)"/>
        public Task<TResult> ThenAsync<TResult>(int timeoutMS = 0)
            => WithTimeout(PromiseAwaiter.Await<TResult>(JSRef!.JSHandle), timeoutMS);
        /// <summary>
        /// Asynchronously wait for a Promise to complete
        /// </summary>
        public Task ThenAsync(CancellationToken cancellationToken)
        {
            var t = new TaskCompletionSource();
            var callbacks = new CallbackGroup();
            ThenCatch(callbacks.Add(Callback.Create(() =>
            {
                if (t.TrySetResult())
                {
                    callbacks.Dispose();
                }
            })), callbacks.Add(Callback.Create((Error? error) =>
            {
                var ex = Promise.UnknownErrorToException(error);
                if (t.TrySetException(ex))
                {
                    callbacks.Dispose();
                }
            })));
            if (cancellationToken != CancellationToken.None)
            {
                cancellationToken.Register(() =>
                {
                    if (t.TrySetException(new Exception("Timed out")))
                    {
                        callbacks.Dispose();
                    }
                });
            }
            return t.Task;
        }
        /// <summary>
        /// Asynchronously wait for a Promise to complete
        /// </summary>
        public Task<TResult> ThenAsync<TResult>(CancellationToken cancellationToken)
        {
            var t = new TaskCompletionSource<TResult>();
            var callbacks = new CallbackGroup();
            ThenCatch(callbacks.Add(Callback.Create<TResult>((result) =>
            {
                if (t.TrySetResult(result))
                {
                    callbacks.Dispose();
                }
            })), callbacks.Add(Callback.Create((Error? error) =>
            {
                var ex = Promise.UnknownErrorToException(error);
                if (t.TrySetException(ex))
                {
                    callbacks.Dispose();
                }
            })));
            if (cancellationToken != CancellationToken.None)
            {
                cancellationToken.Register(() =>
                {
                    if (t.TrySetException(new Exception("Timed out")))
                    {
                        callbacks.Dispose();
                    }
                });
            }
            return t.Task;
        }
        /// <summary>
        /// Asynchronously wait for a Promise to resolve, marshalling the result as the given type.<br/>
        /// The generic <see cref="ThenAsync{TResult}(int)"/> is the one to use when the type is known at
        /// compile time; this exists for the dispatcher, which only has a runtime Type. The resolved value
        /// arrives as a handle and is marshalled through the registry, so it costs no more than the typed
        /// form and needs no generic instantiation per return type.
        /// </summary>
        public Task<object?> ThenAsync(Type resultType, int timeoutMS = 0)
        {
            // Delegates to the generic overload rather than reading the resolved value through a
            // Callback<SpawnJSHandle>. A SpawnJSHandle is a proxy over a Javascript OBJECT, so a promise
            // resolving with a primitive threw "JSObject proxy of number is not supported" inside the
            // callback - which never reached the TaskCompletionSource, so the returned Task simply never
            // completed. Promise.resolve(5) hung this method; only object-resolving promises worked.
            var typedTask = ThenAsyncFor(resultType).Invoke(this, new object[] { timeoutMS })!;
            return AsObjectTaskFor(resultType).Invoke(null, new object[] { typedTask }) as Task<object?>
                ?? throw new Exception($"{nameof(ThenAsync)}: could not adapt Task<{resultType.Name}>");
        }

        static readonly System.Collections.Concurrent.ConcurrentDictionary<Type, System.Reflection.MethodInfo> _thenAsyncByResultType = new();
        static readonly System.Collections.Concurrent.ConcurrentDictionary<Type, System.Reflection.MethodInfo> _asObjectTaskByResultType = new();

        static System.Reflection.MethodInfo ThenAsyncFor(Type resultType) => _thenAsyncByResultType.GetOrAdd(resultType, t =>
            typeof(Promise).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Single(m => m.Name == nameof(ThenAsync)
                          && m.IsGenericMethodDefinition
                          && m.GetParameters() is { Length: 1 } p
                          && p[0].ParameterType == typeof(int))
                .MakeGenericMethod(t));

        static System.Reflection.MethodInfo AsObjectTaskFor(Type resultType) => _asObjectTaskByResultType.GetOrAdd(resultType, t =>
            typeof(Promise).GetMethod(nameof(AsObjectTask), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .MakeGenericMethod(t));

        /// <summary>
        /// Task&lt;T&gt; as Task&lt;object?&gt;. Awaiting rather than casting keeps the failure behaviour
        /// intact - a faulted source faults the result with the same exception.
        /// </summary>
        static async Task<object?> AsObjectTask<T>(Task<T> source) => await source;
        /// <summary>
        /// Handles converting a value from a Promise catch event into an exception.<br/>
        /// These are usually of the type `Error`, but can be anything<br/>
        /// The error object is tested here to create JSException that can represent it in a useful manner
        /// </summary>
        internal static JSException UnknownErrorToException(Error? error)
        {
            JSException? ex = null;
            string? errorName = null;
            string? errorMessage = null;
            if (error != null)
            {
                var typeofError = error.JSRef!.TypeOf();
                switch (typeofError)
                {
                    case "string":
                        {
                            errorMessage = error.JSRefAs<string>();
                            break;
                        }
                    case "object":
                        {
                            var cNames = error.JSRef!.ConstructorNames();
                            errorName = cNames.FirstOrDefault();
                            if (cNames.Contains("Error"))
                            {
                                ex = error.ToException();
                            }
                            else
                            {
                                errorMessage = error.Message;
                            }
                            break;
                        }
                }
            }
            ex ??= new JSException(!string.IsNullOrEmpty(errorMessage) ? errorMessage : "Unknown error", errorName);
            return ex;
        }
    }
}
