using Microsoft.Extensions.DependencyInjection;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// SpawnJS app builder
    /// </summary>
    public class SpawnJSAppBuilder
    {
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
        }
        /// <summary>
        /// Creates a default app
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static SpawnJSAppBuilder CreateDefault(string[]? args = null)
        {
            var builder = new SpawnJSAppBuilder(args);
            // empty
            return builder;
        }
        /// <summary>
        /// Build the SpawnJSApp
        /// </summary>
        /// <returns></returns>
        public SpawnJSApp Build()
        {
            SpawnJSApp? app = null;
            Services.AddSingleton(sp => app!);
            var serviceProvider = Services.BuildServiceProvider();
            app = new SpawnJSApp(Args, serviceProvider);
            return app;
        }
    }
}
