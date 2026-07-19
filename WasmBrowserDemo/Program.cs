using SpawnDev.SpawnJS;
using TestsShared;

SpawnJSRuntime.Verbose = true;
var JS = new SpawnJSRuntime();
var tests = new JSInteropTestsCore(JS);
await tests.RunAsync();
