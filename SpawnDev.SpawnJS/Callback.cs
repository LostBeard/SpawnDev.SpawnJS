namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Callback wraps action and function to pass to JS
    /// </summary>
    public class Callback : IDisposable, IMarshalOutByJSHandle
    {
        /// <summary>
        /// SpawnJSRuntime
        /// </summary>
        static private SpawnJSRuntime JS => SpawnJSRuntime.Instance ?? throw new InvalidOperationException("SpawnJSRuntime has not been created.");
        static long _id = 0;
        /// <summary>
        /// Callback id
        /// </summary>
        public string Id { get; } = $"cb_{_id++}";
        internal Delegate Func { get; private set; }
        /// <summary>
        /// True if disposed
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// Handle to the JS function that calls this method
        /// </summary>
        public SpawnJSHandle JSHandle { get; private set; }
        /// <summary>
        /// New instance
        /// </summary>
        public Callback(Delegate func)
        {
            Func = func;
            if (Func.Method.ReturnType == typeof(void))
            {
                JSHandle = JS.NetRun<SpawnJSHandle>("registerCallbackVoid", new object[] { Id });
            }
            else
            {
                JSHandle = JS.NetRun<SpawnJSHandle>("registerCallback", new object[] { Id });
            }
            JS.SetHandler(Id, Func);
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            JS.RemoveHandler(Id);
            JSHandle.Dispose();
        }
    }
}
