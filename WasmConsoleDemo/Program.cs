using System;
using System.Threading.Tasks;
using SpawnDev.SpawnJS;
using TestsShared;

// Non-browser host. Runs the same suite the browser demo runs, so any test that fails here is
// telling us the code under it is browser-specific rather than host-neutral.
Console.WriteLine("WasmConsoleDemo starting");
var JS = new SpawnJSRuntime();
JS.Verbose = false;
var filter = args.Length > 0 ? args[0] : null;
var failed = await TestSuiteRunner.RunAllAsync(filter);
return failed;
