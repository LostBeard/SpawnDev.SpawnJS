using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.Demo.UnitTests;
using SpawnDev.SpawnJS.JSObjects;

var JS = SpawnJSRuntime.Instance;
JS.Verbose = false;

// SpawnJS.TestRunner drives this app with ?tests=[filter] and parses the TEST:/RESULTS: lines the
// suite writes.
using var location = JS.Get<Location>("location")!;
using var pageUrl = new URL(location.Href);
using var query = pageUrl.SearchParams;
await MarshallerTests.Run(query.Get("tests") ?? "");

//await ToddsMiscTests.Run();
