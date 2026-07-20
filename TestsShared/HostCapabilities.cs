using SpawnDev.SpawnJS;

namespace TestsShared
{
    /// <summary>
    /// Host capability checks.<br/>
    /// SpawnJS is meant to run in any .Net WASM host, not just a browser, so a test that needs a
    /// browser global must say so and skip elsewhere rather than fail. A red suite on the console
    /// host would hide real regressions behind expected absences.
    /// </summary>
    public static class HostCapabilities
    {
        static SpawnJSRuntime JS => SpawnJSRuntime.Instance ?? throw new InvalidOperationException("SpawnJSRuntime has not been created.");

        /// <summary>
        /// True when running in a browser (a `window` global exists)
        /// </summary>
        public static bool IsBrowser => JS.Has("window");

        /// <summary>
        /// Skips the calling test unless it is running in a browser
        /// </summary>
        public static void RequireBrowser()
        {
            if (!IsBrowser) throw new SkipTestException("requires a browser host (no window global)");
        }

        /// <summary>
        /// Skips the calling test unless the named global exists
        /// </summary>
        public static void RequireGlobal(string name)
        {
            if (!JS.Has(name)) throw new SkipTestException($"requires the '{name}' global, which this host does not provide");
        }
    }
}
