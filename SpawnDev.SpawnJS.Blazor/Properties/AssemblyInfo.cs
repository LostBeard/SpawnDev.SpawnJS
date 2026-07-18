// SpawnDev.SpawnJS.Blazor adapts SpawnJS to the Blazor IJSInProcessRuntime / IJSInProcessObjectReference
// interfaces. Like the core library, every API depends on the browser JS runtime and is only supported on
// the "browser" platform. Declaring that at the assembly level is the honest, accurate platform annotation
// (NOT a warning suppression) and resolves intra-assembly CA1416 "reachable on all platforms" warnings.
[assembly: System.Runtime.Versioning.SupportedOSPlatform("browser")]
