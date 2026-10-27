using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    /// <summary>
    /// The IDisposable watcher (<see cref="SpawnJSRuntime.EnableIDisposableWatcher"/>): what it costs per wrapper, and that a
    /// leaked wrapper still names where it was created.
    /// </summary>
    public static class DisposableWatcherTests
    {
        /// <summary>Registers this group's cases with the harness's async test wrapper.</summary>
        /// <param name="testAsync">The suite's TestAsync(name, body) helper.</param>
        public static async Task Run(Func<string, Func<Task>, Task> testAsync)
        {
            var js = SpawnJSRuntime.Instance;

            await testAsync("DisposableWatcher cost per wrapper", () =>
            {
                const int n = 50;
                bool was = SpawnJSRuntime.EnableIDisposableWatcher;
                try
                {
                    double Wrappers(bool watch)
                    {
                        SpawnJSRuntime.EnableIDisposableWatcher = watch;
                        var sw = Stopwatch.StartNew();
                        for (int i = 0; i < n; i++)
                        {
                            using var o = js.Get<SpawnJSObject>("document");
                        }
                        return sw.Elapsed.TotalMilliseconds / n;
                    }
                    double Raw(Func<object> capture)
                    {
                        var sw = Stopwatch.StartNew();
                        for (int i = 0; i < n; i++) _ = capture();
                        return sw.Elapsed.TotalMilliseconds / n;
                    }
                    Wrappers(false);
                    double off = Wrappers(false), on = Wrappers(true);
                    double withFiles = Raw(() => new StackTrace(true).ToString());
                    double framesOnly = Raw(() => new StackTrace(false));
                    double framesFormatted = Raw(() => new StackTrace(false).ToString());
                    double filesOnly = Raw(() => new StackTrace(true));
                    var sample = new StackTrace(true);
                    int withLine = 0;
                    for (int i = 0; i < sample.FrameCount; i++) if (sample.GetFrame(i)?.GetFileLineNumber() > 0) withLine++;
                    Console.WriteLine($"[DisposableWatcher] per wrapper: off {off:F3} ms, on {on:F3} ms; StackTrace(true).ToString() {withFiles:F3} ms, " +
                        $"StackTrace(false) {framesOnly:F3} ms, StackTrace(false).ToString() {framesFormatted:F3} ms, StackTrace(true) {filesOnly:F3} ms; " +
                        $"frames with a line number {withLine}/{sample.FrameCount}");
                    // Formatting the creation stack on every wrapper made the watcher cost ~3-4 ms per wrapper (measured 3.2 / 3.9
                    // here, and 250 ms-3 s per GPU call in SpawnDev.Codecs' suite); capturing the frames costs ~0.06 ms
                    if (on > 1.0) throw new Exception($"a wrapper costs {on:F3} ms with the watcher on ({off:F3} ms off)");
                }
                finally { SpawnJSRuntime.EnableIDisposableWatcher = was; }
                return Task.CompletedTask;
            });

            await testAsync("DisposableWatcher names the creation site of a leaked wrapper", () =>
            {
                bool was = SpawnJSRuntime.EnableIDisposableWatcher;
                SpawnJSRuntime.EnableIDisposableWatcher = true;
                try
                {
                    using var o = js.Get<SpawnJSObject>("document");
                    var field = typeof(SpawnJSObject).GetField("_creationStackTrace", BindingFlags.NonPublic | BindingFlags.Instance)
                        ?? throw new Exception("SpawnJSObject has no _creationStackTrace field");
                    var st = field.GetValue(o) as StackTrace ?? throw new Exception("no creation stack was recorded with the watcher on");
                    var writer = new StringWriter();
                    var stdout = Console.Out;
                    Console.SetOut(writer);
                    try { SpawnJSRuntime.IDisposableFinalizerAlert(o, st); }
                    finally { Console.SetOut(stdout); }
                    var alert = writer.ToString();
                    if (!alert.Contains("was garbage collected but was never Disposed")) throw new Exception($"alert: {alert}");
                    if (!alert.Contains(nameof(DisposableWatcherTests))) throw new Exception($"the alert does not name the creation site: {alert}");
                }
                finally { SpawnJSRuntime.EnableIDisposableWatcher = was; }
                return Task.CompletedTask;
            });
        }
    }
}
