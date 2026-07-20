using SpawnDev.SpawnJS;

namespace TestsShared
{
    /// <summary>
    /// Browser side of the globals probe. The console host runs the same code through
    /// <c>--probe-globals</c>; this is how the browser half is captured.
    /// </summary>
    public class GlobalsProbeTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// Emits one line per wrapper type saying whether a Javascript global of that name exists here.<br/>
        /// This is a diagnostic, not an assertion, and it writes a thousand lines - so it stays skipped
        /// unless the run was filtered for it by name. Capture it with:
        /// <c>dotnet run --project SpawnJS.TestRunner GlobalsProbe --verbose</c>
        /// </summary>
        [SpawnJSTest(Timeout = 120000)]
        public async Task GlobalsProbeTest()
        {
            if (TestSuiteRunner.CurrentFilter?.Contains("GlobalsProbe", StringComparison.OrdinalIgnoreCase) != true)
            {
                throw new SkipTestException("diagnostic - run with the GlobalsProbe filter to emit it");
            }
            GlobalsProbe.Emit(JS);
        }
    }
}
