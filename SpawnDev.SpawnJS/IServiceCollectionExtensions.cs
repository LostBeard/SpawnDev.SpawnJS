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
        /// Adds the SpawnJSRuntime singleton service and initializes it.<br/>
        /// Also adds BackgroundServiceManager if not already added
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
    }
}
