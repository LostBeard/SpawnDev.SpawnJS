namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// SpawnJS app
    /// </summary>
    public class SpawnJSApp
    {
        private TaskCompletionSource _appRun = new TaskCompletionSource();
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
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// True if disposing
        /// </summary>
        public bool IsDisposing { get; private set; }
        /// <summary>
        /// True if Exit was called
        /// </summary>
        public bool Exited { get; private set; }
        /// <summary>
        /// True if Starting
        /// </summary>
        public bool Starting { get; private set; }
        /// <summary>
        /// True if Running
        /// </summary>
        public bool Running { get; private set; }
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
        /// Starts background services based on scope and keeps the app alive until Exit is called.
        /// </summary>
        public async Task RunAsync()
        {
            if (Exited || Starting || Running) return;
            Starting = true;
            await Services.StartBackgroundServices();
            Starting = false;
            if (!Exited)
            {
                Running = true;
                await _appRun.Task;
                Running = false;
                Exited = true;
            }
            await DisposeAsync();
        }
        /// <summary>
        /// Starts background services based on scope and keeps the app alive until Exit is called.
        /// </summary>
        public async Task RunAsync(Func<SpawnJSApp, Task> whenReady)
        {
            if (Exited || Starting || Running) return;
            Starting = true;
            await Services.StartBackgroundServices();
            Starting = false;
            if (!Exited)
            {
                Running = true;
                if (whenReady != null) await whenReady(this);
                await _appRun.Task;
                Running = false;
                Exited = true;
            }
            await DisposeAsync();
        }
        /// <summary>
        /// Starts background services based on scope and keeps the app alive until Exit is called.
        /// </summary>
        public async Task RunAsync(Action<SpawnJSApp> whenReady)
        {
            if (Exited || Starting || Running) return;
            Starting = true;
            await Services.StartBackgroundServices();
            Starting = false;
            if (!Exited)
            {
                Running = true;
                if (whenReady != null) whenReady(this);
                await _appRun.Task;
                Running = false;
                Exited = true;
            }
            await DisposeAsync();
        }
        /// <summary>
        /// Exit the app
        /// </summary>
        public void Exit()
        {
            if (Exited) return;
            Exited = true;
            _appRun.TrySetResult();
        }
        /// <summary>
        /// Dispose the app and resources
        /// </summary>
        async ValueTask DisposeAsync()
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
