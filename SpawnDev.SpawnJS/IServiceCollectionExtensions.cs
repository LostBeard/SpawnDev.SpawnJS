using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

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
        public static IServiceCollection AddSpawnJSRuntime(this IServiceCollection _this)
        {
            _this.GetSpawnJSRuntime();
            return _this;
        }
        /// <summary>
        /// Adds the SpawnJSRuntime singleton service and initializes it.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="JS">SpawnJSRuntime singleton instance</param>
        /// <returns></returns>
        public static IServiceCollection AddSpawnJSRuntime(this IServiceCollection _this, out SpawnJSRuntime JS)
        {
            JS = _this.GetSpawnJSRuntime();
            return _this;
        }
        /// <summary>
        /// Gets SpawnJSRuntime from the current IServiceCollection, adding it if it is not found.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static SpawnJSRuntime GetSpawnJSRuntime(this IServiceCollection _this) => _this.GetSpawnJSRuntime(true)!;
        /// <summary>
        /// Gets SpawnJSRuntime from the current IServiceCollection, adding it if it is not found and allowAdd == true.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="allowAdd"></param>
        /// <returns></returns>
        public static SpawnJSRuntime? GetSpawnJSRuntime(this IServiceCollection _this, bool allowAdd)
        {
            var existing = _this.FirstOrDefault(o => o.ServiceType == typeof(SpawnJSRuntime));
            var JS = existing?.ImplementationInstance as SpawnJSRuntime;
            if (JS == null && allowAdd)
            {
                //var bgManager = _this.GetBackgroundServiceManager();
                //bgManager.OnStarted += BgManager_OnStarted;
                JS = new SpawnJSRuntime();
                // set global scope for background service manager
                _this.SetGlobalScope(JS.GlobalScope);
                _this.AddSingleton<SpawnJSRuntime>(JS);
            }
            return JS;
        }
        //private static Task BgManager_OnStarted(BackgroundServiceManager bgManager, GlobalScope globalScope)
        //{
        //    var JS = bgManager.Descriptors.GetSpawnJSRuntime(false);
        //    JS?.SetReady();
        //    return Task.CompletedTask;
        //}
//        /// <summary>
//        /// Services implementing IBackgroundService or IAsyncBackgroundService will be started
//        /// Services implementing IAsyncBackgroundService will have their IAsyncBackgroundService.Ready Task property awaited in parallel<br/>
//        /// Singletons registered with an auto start GlobalScope that matches the current scope will be started<br/>
//        /// Background services must be careful to not take too long in their InitAsync methods as other services are waiting to init and the app is waiting to start
//        /// </summary>
//        /// <param name="_this"></param>
//        /// <returns></returns>
//        public static async Task<WebAssemblyHost> StartBackgroundServices(this WebAssemblyHost _this)
//        {
//            var JS = _this.Services.GetRequiredService<SpawnJSRuntime>();
//            await _this.Services.StartBackgroundServices();
//            return _this;
//        }
//        /// <summary>
//        /// SpawnJSRunAsync() is a scope aware replacement for RunAsync(). SpawnJSRunAsync will:<br />
//        /// - Start IBackgroundService services, IAsyncBackgroundService services and services registered with a GlobalScope enum value that is compatible the current GlobalScope.<br />
//        /// - Call RunAsync(), but only if running in a Window global scope to prevent components from loading in Worker scopes.<br />
//        /// </summary>
//        /// <param name="_this"></param>
//        /// <param name="serviceOnlyMode">Disable component loading in a Window global scope.</param>
//        /// <returns></returns>
//        public static async Task SpawnJSRunAsync(this WebAssemblyHost _this, bool serviceOnlyMode = false)
//        {
//            await _this.StartBackgroundServices();
//            var JS = _this.Services.GetRequiredService<SpawnJSRuntime>();
//            if (JS.IsWindow && !serviceOnlyMode)
//            {
//#if DEBUG && true
//                Console.WriteLine($"SpawnJSRunAsync mode: Default");
//#endif
//                // run as normal where Spawn has the window global context it expects
//                await _this.RunAsync();
//            }
//            else
//            {
//#if DEBUG && true
//                Console.WriteLine($"SpawnJSRunAsync mode: ServiceOnlyMode");
//#endif
//                // This is a worker so we are going to use this to allow services in workers without the html renderer trying to load pages
//                var tcs = new TaskCompletionSource<object>();
//                await tcs.Task;
//            }
//        }
    }
}
