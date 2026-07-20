namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The Promise object represents the eventual completion (or failure) of an asynchronous operation and its resulting value.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise
    /// </summary>
    /// <typeparam name="TResult">The type the promise resolves to</typeparam>
    /// <remarks>
    /// This derives from the non generic <see cref="Promise"/> rather than repeating it. The base
    /// constructors already resolve a generic Task's result (they check the Task's runtime type), and the
    /// base already carries the whole Then/ThenCatch/ThenAsync surface including typed overloads, so the
    /// generic form only has to fix TResult in the signatures.
    /// </remarks>
    public class Promise<TResult> : Promise
    {
        /// <summary>
        /// Explicit conversion from a Task&lt;TResult> to a Promise&lt;TResult>
        /// </summary>
        public static explicit operator Promise<TResult>(Task<TResult> task) => new Promise<TResult>(task);
        /// <summary>
        /// Explicit conversion from a Func&lt;Task&lt;TResult>> to a Promise&lt;TResult>
        /// </summary>
        public static explicit operator Promise<TResult>(Func<Task<TResult>> task) => new Promise<TResult>(task);
        /// <summary>
        /// The Promise() constructor creates Promise objects.
        /// </summary>
        public Promise(Task<TResult> task) : base(task) { }
        /// <summary>
        /// The Promise() constructor creates Promise objects.
        /// </summary>
        public Promise(Func<Task<TResult>> task) : base(() => (Task)task()) { }
        /// <summary>
        /// The Promise() constructor creates Promise objects.
        /// </summary>
        public Promise(Action<Function, Function> executor) : base(executor) { }
        /// <summary>
        /// The Promise() constructor creates Promise objects.
        /// </summary>
        public Promise(Action<Function> executor) : base(executor) { }
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public Promise(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Set the methods for the Promise onFulfilled and onRejected events
        /// </summary>
        public void Then(ActionCallback<TResult> thenCallback, ActionCallback catchCallback) => Then<TResult>(thenCallback, catchCallback);
        /// <summary>
        /// Asynchronously wait for the Promise to resolve to its result
        /// </summary>
        public new Task<TResult> ThenAsync(int timeoutMS = 0) => ThenAsync<TResult>(timeoutMS);
        /// <summary>
        /// Asynchronously wait for the Promise to resolve to its result
        /// </summary>
        public new Task<TResult> ThenAsync(CancellationToken cancellationToken) => ThenAsync<TResult>(cancellationToken);
    }
}
