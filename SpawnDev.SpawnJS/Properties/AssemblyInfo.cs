// SpawnDev.SpawnJS is a .NET WebAssembly JavaScript interop library. Every public API
// depends on the browser JS runtime (JSImport/JSExport/JSHost/JSObject) and is only
// supported when running on the "browser" platform. Declaring that contract at the
// assembly level is the honest, accurate platform annotation - it is NOT a warning
// suppression: it tells the analyzer (and consumers) the true supported platform, which
// also resolves intra-assembly CA1416 "reachable on all platforms" warnings.
[assembly: System.Runtime.Versioning.SupportedOSPlatform("browser")]
