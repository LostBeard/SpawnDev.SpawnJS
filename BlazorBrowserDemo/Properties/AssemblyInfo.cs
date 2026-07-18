// BlazorBrowserDemo is a Blazor WebAssembly app - it runs only in the browser and exercises the
// browser-only SpawnDev.SpawnJS APIs. Declaring the platform at the assembly level is the accurate
// annotation (NOT a warning suppression) and resolves CA1416 "reachable on all platforms" warnings.
[assembly: System.Runtime.Versioning.SupportedOSPlatform("browser")]
