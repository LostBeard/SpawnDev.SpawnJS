using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// SpawnDev.SpawnJS IServiceCollection extension methods
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the SpawnJSRuntime singleton service and initializes it.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static IServiceCollection AddSpawnJSRuntime(this IServiceCollection _this) => _this.AddSpawnJSRuntime(out var _);
        /// <summary>
        /// Adds the SpawnJSRuntime singleton service and initializes it.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="JS">SpawnJSRuntime singleton instance</param>
        /// <returns></returns>
        public static IServiceCollection AddSpawnJSRuntime(this IServiceCollection _this, out SpawnJSRuntime JS)
        {
            JS = SpawnJSRuntime.Instance;
            // register IBackgroundServiceManager
            _this.AddBackgroundServiceManager();
            // register SpawnJSRuntime service as the source for GlobalScope for IBackgroundServiceManager
            _this.TryAddSingleton<IGlobalScopeSource>(JS);
            // register SpawnJSRuntime
            _this.TryAddSingleton<SpawnJSRuntime>(JS);
            return _this;
        }
        /// <summary>
        /// Starts background services based on scope and calls RunAsync to keep the app alive
        /// </summary>
        public static async Task SpawnJSRunAsync(this SpawnJSApp _this)
        {
            await _this.Services.StartBackgroundServices();
            await _this.RunAsync();
        }
        /// <summary>
        /// Starts background services based on scope and calls RunAsync to keep the app alive
        /// </summary>
        public static async Task SpawnJSRunAsync(this SpawnJSApp _this, Func<SpawnJSApp, Task> whenReady)
        {
            await _this.Services.StartBackgroundServices();
            if (whenReady != null) await whenReady(_this);
            await _this.RunAsync();
        }
        /// <summary>
        /// Starts background services based on scope and calls RunAsync to keep the app alive
        /// </summary>
        public static async Task SpawnJSRunAsync(this SpawnJSApp _this, Action<SpawnJSApp> whenReady)
        {
            await _this.Services.StartBackgroundServices();
            if (whenReady != null) whenReady(_this);
            await _this.RunAsync();
        }
    }
}
