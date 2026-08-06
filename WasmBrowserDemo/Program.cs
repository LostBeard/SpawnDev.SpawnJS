using SpawnDev.SpawnJS;
using TestsShared;

var builder = SpawnJSAppBuilder.CreateDefault(args);

// `?filter=Name` in the url runs only the matching tests, so a single page load can be scoped
// from the address bar or by the SpawnJS.TestRunner harness.
await TestSuiteRunner.RunAllAsync(TestSuiteRunner.FilterFromLocation());

await builder.Build().RunAsync();
