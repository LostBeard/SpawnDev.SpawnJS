using SpawnDev.SpawnJS;
using TestsShared;

var JS = new SpawnJSRuntime();
JS.Verbose = false;

// `?filter=Name` in the url runs only the matching tests, so a single page load can be scoped
// from the address bar or by the SpawnJS.TestRunner harness.
await TestSuiteRunner.RunAllAsync(TestSuiteRunner.FilterFromLocation());
