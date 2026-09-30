using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.Demo.UnitTests;
using SpawnDev.SpawnJS.JSObjects;

// twin.html boots two runtimes of this app into one page, each with ("twin", name)
if (args.Length >= 2 && args[0] == "twin")
{
    await TwinTests.Run(args[1]);
    return;
}

var JS = SpawnJSRuntime.Instance;
JS.Verbose = false;

// SpawnJS.TestRunner drives this app with ?tests=[filter] and parses the TEST:/RESULTS: lines the
// suite writes.
using var location = JS.Get<Location>("location")!;
using var pageUrl = new URL(location.Href);
using var query = pageUrl.SearchParams;
// ?bench=[filter] runs the interop benchmark instead of the suite
var benchFilter = query.Get("bench");
if (benchFilter != null)
{
    SpawnDev.SpawnJS.Demo.Bench.InteropBench.Run(benchFilter);
    return;
}
await MarshallerTests.Run(query.Get("tests") ?? "");

//await ToddsMiscTests.Run();
