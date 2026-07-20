using SpawnDev.SpawnJS;

namespace TestsShared
{
    /// <summary>
    /// Global scope detection, which answers "where am I running".<br/>
    /// A library that runs in a page, a worker and a console host needs this before it touches anything
    /// scope-specific - there is no document in a worker and no importScripts on a window. It is read from
    /// the constructor name of globalThis, which is how SpawnDev.BlazorJS has always determined it.
    /// </summary>
    public class GlobalScopeTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// The scope name must actually resolve. An empty string would mean the lookup silently failed and
        /// every Is* flag below would read false, which looks like "not a window and not a worker" rather
        /// than like an error.
        /// </summary>
        [SpawnJSTest]
        public async Task GlobalScopeNameResolvesTest()
        {
            var scope = JS.GlobalScopeName;
            Console.WriteLine($"GLOBAL-SCOPE: '{scope}' IsBrowser={JS.IsBrowser} IsWindow={JS.IsWindow} IsWorker={JS.IsWorker}");
            if (string.IsNullOrEmpty(scope))
                throw new Exception("GlobalScopeName is empty - the constructor name of globalThis did not resolve");
        }

        /// <summary>
        /// By the time anything runs, the runtime exists - and IsCreated must be able to say so without
        /// creating it. The test suite is holding a runtime already, so a false here would mean the flag
        /// is not tracking the instance at all.
        /// </summary>
        [SpawnJSTest]
        public async Task IsCreatedReportsExistingRuntimeTest()
        {
            if (!SpawnJSRuntime.IsCreated)
                throw new Exception("IsCreated is false while a runtime is in use");
            if (!ReferenceEquals(SpawnJSRuntime.Instance, JS))
                throw new Exception("Instance is not the runtime the tests were given - more than one exists");
        }

        /// <summary>
        /// The flags have to be mutually consistent. A scope that reported both window and worker, or
        /// neither on a host that is plainly one of them, would send callers down the wrong path.
        /// </summary>
        [SpawnJSTest]
        public async Task ScopeFlagsAreConsistentTest()
        {
            if (JS.IsWindow && JS.IsWorker)
                throw new Exception($"Scope '{JS.GlobalScopeName}' reports BOTH window and worker");

            var workerFlags = (JS.IsDedicatedWorkerGlobalScope ? 1 : 0)
                            + (JS.IsSharedWorkerGlobalScope ? 1 : 0)
                            + (JS.IsServiceWorkerGlobalScope ? 1 : 0);
            if (workerFlags > 1)
                throw new Exception($"Scope '{JS.GlobalScopeName}' reports more than one worker kind");
            if (JS.IsWorker != (workerFlags == 1))
                throw new Exception($"IsWorker={JS.IsWorker} disagrees with the individual worker flags for '{JS.GlobalScopeName}'");
        }

        /// <summary>
        /// In a browser page the scope must be Window. This is the case the browser lane can assert
        /// outright, and it is what proves the mechanism reads a real value rather than a plausible blank.
        /// </summary>
        [SpawnJSTest]
        public async Task BrowserPageReportsWindowTest()
        {
            HostCapabilities.RequireBrowser();
            if (JS.GlobalScopeName != "Window")
                throw new Exception($"A browser page reported scope '{JS.GlobalScopeName}', expected Window");
            if (!JS.IsWindow) throw new Exception("IsWindow is false in a browser page");
            if (JS.IsWorker) throw new Exception("IsWorker is true in a browser page");
            if (!JS.IsBrowser) throw new Exception("IsBrowser is false in a browser page");
        }
    }
}
