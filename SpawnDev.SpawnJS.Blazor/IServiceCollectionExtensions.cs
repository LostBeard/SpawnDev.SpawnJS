using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// SpawnDev.SpawnJS IServiceCollection extension methods
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Starts background IBackgroundService and IAsyncBackgroundService services and then calls RunAsync WebAssemblyHost.RunAsync<br/>
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static async Task SpawnJSRunAsync(this WebAssemblyHost _this)
        {
            var backgroundServiceManager = _this.Services.GetRequiredService<IBackgroundServiceManager>();
            await _this.Services.StartBackgroundServices();
            if (backgroundServiceManager.GlobalScope == GlobalScope.Window)
            {
                // in a window scope start normally
                await _this.RunAsync();
            }
            else
            {
                // in non-window scope do not call RunAsync as it is not scope aware
                await new TaskCompletionSource().Task;
            }
        }
    }
}
