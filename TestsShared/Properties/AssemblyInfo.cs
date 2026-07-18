// TestsShared exercises SpawnDev.SpawnJS, whose APIs depend on the browser JS runtime and are only
// supported on the "browser" platform. Declaring that at the assembly level is the accurate platform
// annotation (NOT a warning suppression) and resolves CA1416 "reachable on all platforms" warnings.
[assembly: System.Runtime.Versioning.SupportedOSPlatform("browser")]
