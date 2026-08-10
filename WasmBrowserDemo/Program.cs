using SpawnDev.SpawnJS;
using TestsShared;

var builder = SpawnJSAppBuilder.CreateDefault(args, out var JS);
JS.Verbose = true;

var testStr = "Hello world!";
JS.Set("_testStr", testStr);
var readSJSOR = JS.Get<SpawnJSObjectReference>("_testStr");
var readBackString = JS.Get<SpawnDev.SpawnJS.JSObjects.String>("_testStr");
var readbackStr = readBackString.ToString();
var nmt = true;


// `?filter=Name` in the url runs only the matching tests, so a single page load can be scoped
// from the address bar or by the SpawnJS.TestRunner harness.
await TestSuiteRunner.RunAllAsync(TestSuiteRunner.FilterFromLocation());

await builder.Build().RunAsync();
