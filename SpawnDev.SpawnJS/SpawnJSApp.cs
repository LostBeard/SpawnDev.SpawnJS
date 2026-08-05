namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// SpawnJS app
    /// </summary>
    public class SpawnJSApp : IDisposable, IAsyncDisposable
    {
        private TaskCompletionSource? _appRun = null;
        /// <summary>
        /// App startup args
        /// </summary>
        public string[]? Args { get; } = null;
        /// <summary>
        /// Service provider
        /// </summary>
        public IServiceProvider Services { get; }
        /// <summary>
        /// True if disposed
        /// </summary>
        public bool IsDisposed { get; set; }
        /// <summary>
        /// True if disposing
        /// </summary>
        public bool IsDisposing { get; set; }
        /// <summary>
        /// True if Exit was called
        /// </summary>
        public bool Exited { get; private set; }
        /// <summary>
        /// New instance
        /// </summary>
        /// <param name="args"></param>
        /// <param name="services"></param>
        public SpawnJSApp(string[]? args, IServiceProvider services)
        {
            Args = args;
            Services = services;
        }
        /// <summary>
        /// Dispose the app and resources
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed || IsDisposing) return;
            IsDisposing = true;
            // dispose
            if (Services is IAsyncDisposable asyncDisposable) _ = asyncDisposable.DisposeAsync();
            else if (Services is IDisposable disposable) disposable.Dispose();
            IsDisposed = true;
            IsDisposing = false;
        }
        /// <summary>
        /// Run the app
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync()
        {
            if (Exited || IsDisposed || IsDisposing) return;
            if (_appRun != null) return;
            _appRun = new TaskCompletionSource();
            await _appRun.Task;
            await DisposeAsync();
        }
        /// <summary>
        /// Exit the app
        /// </summary>
        public void Exit()
        {
            Exited = true;
            _appRun?.SetResult();
        }
        /// <summary>
        /// Dispose the app and resources
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            if (IsDisposed || IsDisposing) return;
            IsDisposing = true;
            if (Services is IAsyncDisposable asyncDisposable) await asyncDisposable.DisposeAsync();
            else if (Services is IDisposable disposable) disposable.Dispose();
            IsDisposed = true;
            IsDisposing = false;
        }
    }
}
