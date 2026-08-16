using Microsoft.Extensions.DependencyInjection;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// SpawnJS app builder
    /// </summary>
    public class SpawnJSAppBuilder
    {
        SpawnJSApp? _app = null;
        /// <summary>
        /// App startup args
        /// </summary>
        public string[]? Args { get; } = null;
        /// <summary>
        /// Service collection
        /// </summary>
        public IServiceCollection Services { get; }
        /// <summary>
        /// New instance
        /// </summary>
        /// <param name="args"></param>
        public SpawnJSAppBuilder(string[]? args = null)
        {
            Args = args;
            Services = new ServiceCollection();
            Services.AddSingleton(sp => _app!);
            Services.AddSpawnJSRuntime();
        }
        /// <summary>
        /// Creates a default SpawnJSAppBuilder with the default services:<br/>
        /// BackgroundServiceManager - handles auto-starting IBackground services when SpawnJSApp.RunAsync() is called<br/>
        /// SpawnJSRuntime - Required Javascript runtime<br/>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static SpawnJSAppBuilder CreateDefault(string[]? args = null)
        {
            var builder = new SpawnJSAppBuilder(args);
            return builder;
        }
        /// <summary>
        /// Creates a default SpawnJSAppBuilder with the default services:<br/>
        /// BackgroundServiceManager - handles auto-starting IBackground services when SpawnJSApp.RunAsync() is called<br/>
        /// SpawnJSRuntime - Required Javascript runtime<br/>
        /// </summary>
        /// <param name="args"></param>
        /// <param name="js">The SpawnJSRuntime singleton</param>
        /// <returns></returns>
        public static SpawnJSAppBuilder CreateDefault(out SpawnJSRuntime js)
        {
            var builder = new SpawnJSAppBuilder();
            js = SpawnJSRuntime.Instance;
            return builder;
        }
        /// <summary>
        /// Creates a default SpawnJSAppBuilder with the default services:<br/>
        /// BackgroundServiceManager - handles auto-starting IBackground services when SpawnJSApp.RunAsync() is called<br/>
        /// SpawnJSRuntime - Required Javascript runtime<br/>
        /// </summary>
        /// <param name="args"></param>
        /// <param name="js">The SpawnJSRuntime singleton</param>
        /// <returns></returns>
        public static SpawnJSAppBuilder CreateDefault(string[]? args, out SpawnJSRuntime js)
        {
            var builder = new SpawnJSAppBuilder(args);
            js = SpawnJSRuntime.Instance;
            return builder;
        }
        /// <summary>
        /// Build the SpawnJSApp
        /// </summary>
        /// <returns></returns>
        public SpawnJSApp Build()
        {
            var serviceProvider = Services.BuildServiceProvider();
            _app = new SpawnJSApp(Args, serviceProvider);
            return _app;
        }
    }
}
