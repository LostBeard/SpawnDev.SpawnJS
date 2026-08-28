using System;
using System.Collections.Generic;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    /// <summary>
    /// <see cref="SpawnJSRuntime.AppBaseUri"/> is the URL the app was LOADED from - its own
    /// <c>main.*</c> / framework-folder origin, not the host page's <c>document.baseURI</c>. Every worker
    /// entrypoint URL is built on it, so when it resolves one level wrong every worker 404s.
    /// <para>
    /// It is produced by <c>SpawnJSInterop.appRootFromLoadUrl</c>, which normalizes whatever URL a runtime
    /// artifact was loaded from back to the app root. These tests drive THAT function - the real one, on
    /// the real global - over the shapes the resolver is actually handed, because the interesting cases
    /// (a renamed framework folder, an app served from a sub-path, an older bundle reporting its own URL)
    /// cannot all be produced by the Demo's own layout.
    /// </para>
    /// <para>
    /// The regression this guards: the normalizer used to strip a trailing <c>_framework/</c> BY NAME.
    /// SpawnDev.SpawnJS.WebWorkers can rename that folder on publish
    /// (<c>SpawnJSWebWorkersFrameworkFolderName</c>, because a browser extension may not have a root
    /// folder starting with '_'), and the name match then failed silently: the app root came back as the
    /// framework folder itself and every worker was requested one level too deep.
    /// </para>
    /// </summary>
    public static class AppRootTests
    {
        static SpawnJSRuntime JS => SpawnJSRuntime.Instance;

        static string AppRoot(string? loadUrl) =>
            JS.Call<string?, string>("SpawnJSInterop.appRootFromLoadUrl", loadUrl);

        /// <summary>
        /// Runs the app-root cases through the caller's test harness, so counting and reporting stay in
        /// one place.
        /// </summary>
        public static void Run(Action<string, Action> test)
        {
            // Each case: what a runtime artifact was loaded from -> the app root it implies.
            var cases = new List<(string Name, string? LoadUrl, string Expected)>
            {
                // The measured shapes. Module.mainScriptUrlOrBlob is dotnet.js's own url and
                // resources.assembly[0].resolvedUrl is a boot resource; both sit in the framework folder.
                ("RuntimeEntryFingerprinted", "http://h/_framework/dotnet.4zl0ndnpfh.js", "http://h/"),
                ("RuntimeEntryUnfingerprinted", "http://h/_framework/dotnet.js", "http://h/"),
                ("RuntimeEntryUnderSubPath", "http://h/app/_framework/dotnet.js", "http://h/app/"),
                ("BootResourceAssembly", "http://h/app/_framework/System.Private.CoreLib.abc.wasm", "http://h/app/"),
                ("BootResourceIcuData", "http://h/app/_framework/icudt_EFIGS.abc.dat", "http://h/app/"),
                ("PthreadWorkerModule", "http://h/_framework/dotnet.native.worker.abc.mjs", "http://h/"),

                // The renamed framework folder - what the by-name strip got wrong.
                ("RenamedFrameworkFolder", "http://h/app/framework/dotnet.js", "http://h/app/"),
                ("RenamedFrameworkFolderResource", "http://h/app/framework/System.Private.CoreLib.abc.wasm", "http://h/app/"),

                // A bundled entrypoint sits AT the app root, so it must NOT be walked up. This is what
                // SpawnDev.SpawnJS.WebWorkers bundles before 2.1.9 reported as the runtime's own url.
                ("BundledEntrypointModule", "http://h/app/main.module.js", "http://h/app/"),
                ("BundledEntrypointClassic", "http://h/main.classic.js", "http://h/"),

                // Query and fragment are not part of the base.
                ("QueryAndFragmentStripped", "http://h/app/_framework/dotnet.js?v=1#x", "http://h/app/"),

                // Unusable inputs resolve to '' so the caller falls through to the next candidate or its
                // own fallback, rather than silently building worker URLs on a wrong base.
                ("BlobUrlRejected", "blob:http://h/1234", ""),
                ("EmptyRejected", "", ""),
                ("NullRejected", null, ""),
            };

            foreach (var (name, loadUrl, expected) in cases)
            {
                test($"AppRoot.{name}", () =>
                {
                    var got = AppRoot(loadUrl);
                    if (got != expected)
                        throw new Exception($"appRootFromLoadUrl({(loadUrl == null ? "null" : $"'{loadUrl}'")}) -> '{got}', expected '{expected}'");
                });
            }

            // The live value, in this app, in this scope. The Demo is served co-located with its page, so
            // the page must live under the app root. (Under a CDN load the two diverge on purpose - that
            // is exactly why AppBaseUri exists - so this asserts only the co-located case.)
            test("AppRoot.LiveAppBaseUriIsAncestorOfPage", () =>
            {
                var baseUri = JS.AppBaseUri;
                if (string.IsNullOrEmpty(baseUri))
                    throw new Exception("AppBaseUri is empty - the runtime origin did not resolve");
                if (!baseUri.EndsWith("/"))
                    throw new Exception($"AppBaseUri '{baseUri}' does not end with '/' - it is not a base other URLs resolve against");
                var here = JS.Get<string>("location.href") ?? "";
                if (!here.StartsWith(baseUri, StringComparison.Ordinal))
                    throw new Exception($"location.href '{here}' is not under AppBaseUri '{baseUri}' - the base did not resolve to the app root");
            });
        }
    }
}
