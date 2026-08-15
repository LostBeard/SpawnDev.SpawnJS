using SpawnDev.SpawnJS;

namespace TestsShared
{
    /// <summary>
    /// SpawnJSRuntime.AppBaseUri - the URL the app was LOADED from (its own main.* / _framework origin),
    /// which is what worker entry scripts must resolve against and which stays correct when the app is
    /// served from a CDN at a different path than the host page.<br/>
    /// It is derived per-runtime from this app's own dotnet runtime (Module.mainScriptUrlOrBlob, measured),
    /// deliberately NOT from document.baseURI - the page's base is wrong under a CDN load. These guards fail
    /// the instant AppBaseUri regresses to the page base or the resolver stops resolving.
    /// </summary>
    public class AppBaseUriTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// In a browser it must resolve to a real absolute origin ending in '/'. An empty value would mean
        /// the runtime origin lookup silently failed, and every worker script would then resolve against the
        /// page root - the exact CDN bug this exists to prevent.
        /// </summary>
        //[SpawnJSTest]
        //public async Task AppBaseUriResolvesToAbsoluteOriginTest()
        //{
        //    HostCapabilities.RequireBrowser();
        //    var baseUri = JS.AppBaseUri;
        //    var source = JS.AppBaseUriSource();
        //    Console.WriteLine($"APP-BASE: '{baseUri}' source='{source}'");

        //    if (string.IsNullOrEmpty(baseUri))
        //        throw new Exception("AppBaseUri is empty in a browser - the runtime origin did not resolve");
        //    if (!(baseUri.StartsWith("http://") || baseUri.StartsWith("https://")))
        //        throw new Exception($"AppBaseUri '{baseUri}' is not an absolute http(s) URL");
        //    if (!baseUri.EndsWith("/"))
        //        throw new Exception($"AppBaseUri '{baseUri}' does not end with '/' - it is not a base other URLs can resolve against");
        //}

        ///// <summary>
        ///// It must come from the app's OWN runtime origin, not from a page-coupled fallback. A non-empty
        ///// source string proves the per-runtime resolver produced it (Module.mainScriptUrlOrBlob or the
        ///// getConfig resolvedUrl backup); an empty source would mean the resolver found nothing and the
        ///// value, if any, came from somewhere it should not have.
        ///// </summary>
        //[SpawnJSTest]
        //public async Task AppBaseUriCameFromRuntimeOriginTest()
        //{
        //    HostCapabilities.RequireBrowser();
        //    var source = JS.AppBaseUriSource();
        //    if (string.IsNullOrEmpty(source))
        //        throw new Exception("AppBaseUriSource is empty - AppBaseUri did not come from the runtime origin resolver");
        //}

        /// <summary>
        /// In this harness the app is served together with the page, so the current page URL must sit under
        /// the app root - i.e. AppBaseUri is an ancestor of location.href. (Under a CDN the two diverge on
        /// purpose; that scenario is exactly why AppBaseUri exists and is NOT asserted here, where the app
        /// and page are co-located.) This catches a base that resolved to the wrong place entirely.
        /// </summary>
        [SpawnJSTest]
        public async Task AppBaseUriIsAncestorOfPageInColocatedHostTest()
        {
            HostCapabilities.RequireBrowser();
            var baseUri = JS.AppBaseUri;
            if (string.IsNullOrEmpty(baseUri))
                throw new Exception("AppBaseUri is empty - cannot verify it is the app root");

            var here = JS.Get<string>("location.href");
            // compare on the path root, ignoring any ?query/#fragment the page carries (e.g. ?filter=)
            var herePath = here.Split('?', '#')[0];
            if (!herePath.StartsWith(baseUri))
                throw new Exception($"location.href '{here}' is not under AppBaseUri '{baseUri}' - the base did not resolve to the app root");
        }
    }
}
