using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using TestsShared;

var JS = new SpawnJSRuntime();
JS.Verbose = false;


var window = JS.Get<Window>("window");
JS.Set("_windows", new Window[] { window, window, window });

var readBack = JS.Get<Window[]>("_windows");


// `?filter=Name` in the url runs only the matching tests, so a single page load can be scoped
// from the address bar or by the SpawnJS.TestRunner harness.
await TestSuiteRunner.RunAllAsync(TestSuiteRunner.FilterFromLocation());
