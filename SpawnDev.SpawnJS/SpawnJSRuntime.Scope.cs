namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Which global scope this runtime is executing in.<br/>
    /// A library that runs in both a page and a worker needs this to decide what is reachable: there is no
    /// document in a worker, and no importScripts on a window. SpawnDev.BlazorJS exposes the same checks and
    /// consumers use them by name.
    /// </summary>
    public partial class SpawnJSRuntime
    {
        /// <summary>
        /// True when running on the browser-wasm runtime. It asks the .Net runtime rather than Javascript,
        /// so it is valid before any interop has happened.<br/>
        /// ⚠️ This does NOT mean "running in a browser". A WebAssembly console app on Node also reports
        /// true, because it targets the same runtime - measured: the console host reports IsBrowser=True
        /// with a global scope of "Object". To ask whether there is a page, use <see cref="IsWindow"/>.
        /// SpawnDev.BlazorJS has the same semantics.
        /// </summary>
        public bool IsBrowser => OperatingSystem.IsBrowser();

        /// <summary>
        /// The constructor name of globalThis, which is what identifies the scope: "Window",
        /// "DedicatedWorkerGlobalScope", "SharedWorkerGlobalScope", "ServiceWorkerGlobalScope" - or on a
        /// non browser host, something else entirely.
        /// </summary>
        public string GlobalScopeName => _globalScopeName ??= ConstructorName() ?? "";
        string? _globalScopeName;

        /// <summary>
        /// True when running in a page rather than a worker
        /// </summary>
        public bool IsWindow => GlobalScopeName == "Window";

        /// <summary>
        /// True in a dedicated worker
        /// </summary>
        public bool IsDedicatedWorkerGlobalScope => GlobalScopeName == "DedicatedWorkerGlobalScope";

        /// <summary>
        /// True in a shared worker
        /// </summary>
        public bool IsSharedWorkerGlobalScope => GlobalScopeName == "SharedWorkerGlobalScope";

        /// <summary>
        /// True in a service worker
        /// </summary>
        public bool IsServiceWorkerGlobalScope => GlobalScopeName == "ServiceWorkerGlobalScope";

        /// <summary>
        /// True in any kind of worker
        /// </summary>
        public bool IsWorker => IsDedicatedWorkerGlobalScope || IsSharedWorkerGlobalScope || IsServiceWorkerGlobalScope;
    }
}
