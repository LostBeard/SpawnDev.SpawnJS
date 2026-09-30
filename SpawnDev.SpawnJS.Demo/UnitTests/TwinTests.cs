using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    /// <summary>
    /// Runs in EACH of two .Net runtimes booted into one page by twin.html (SpawnJS.TestRunner --twin). Every
    /// runtime has its own call tape; these prove the two never read or write each other's, including while both
    /// have calls in flight at once.
    /// </summary>
    public static class TwinTests
    {
        static SpawnJSRuntime JS => SpawnJSRuntime.Instance;

        public static async Task Run(string name)
        {
            var other = name == "A" ? "B" : "A";
            // (s, depth): does real tape work - a payload big enough to grow the tape - then, while depth lasts,
            // bounces the call back to the other runtime, which may bounce it back here, re-entering this runtime
            // while its outer call is still in Javascript's hands
            using var callback = new FuncCallback<string, int, string>((s, depth) =>
            {
                var key = $"__twinScratch_{name}";
                var payload = new string(name[0], 100_000);
                JS.Set(key, payload);
                var back = JS.Get<string>(key);
                if (back != payload) throw new Exception($"{name}: its own payload came back wrong (length {back?.Length})");
                var inner = depth > 0 ? JS.Call<string, string, int, string>("__twinBounce", other, s, depth - 1) : s;
                return $"{name}({inner})";
            });
            JS.Set($"__twinCallback_{name}", callback);
            // a bounded wait, so a runtime that never comes up is a failed test and not a hung page
            await Test(name, "OtherRuntimeIsUp", () => WaitFor($"__twinCallback_{other}"));

            await Test(name, "TwoRuntimesTwoHeaps", () =>
            {
                var separate = JS.Call<string>("__twinSeparate");
                if (separate != "2 instances, 2 ids, 2 heaps") throw new Exception(separate);
                return Task.CompletedTask;
            });

            await Test(name, "InterleavedCallsStayOnTheirOwnTape", async () =>
            {
                var key = $"__twinKey_{name}";
                for (var i = 0; i < 300; i++)
                {
                    var payload = $"{name}{i}" + (i % 10 == 0 ? new string('p', 100_000) : "");
                    JS.Set(key, payload);
                    // let the other runtime run between the write and the read
                    await Task.Yield();
                    var back = JS.Get<string>(key);
                    if (back != payload) throw new Exception($"iteration {i}: got length {back?.Length} starting '{back?.Substring(0, Math.Min(8, back?.Length ?? 0))}'");
                }
                if (JS.TapeDepth != 0) throw new Exception($"TapeDepth {JS.TapeDepth} after the calls");
            });

            await Test(name, "CallBouncesBetweenTheRuntimes", () =>
            {
                // this -> other -> this -> other -> this, every hop re-entering a runtime whose outer call is in flight
                var result = JS.Call<string, string, int, string>("__twinBounce", other, "x", 3);
                var expected = other == "B" ? "B(A(B(A(x))))" : "A(B(A(B(x))))";
                if (result != expected) throw new Exception($"got {result}, expected {expected}");
                if (JS.TapeDepth != 0) throw new Exception($"TapeDepth {JS.TapeDepth} after the calls");
                return Task.CompletedTask;
            });

            // the other runtime may still be bouncing calls through this one's callback
            JS.Set($"__twinDone_{name}", true);
            await Test(name, "OtherRuntimeFinished", () => WaitFor($"__twinDone_{other}"));
        }

        static async Task WaitFor(string global)
        {
            var sw = Stopwatch.StartNew();
            while (!JS.Has(global))
            {
                if (sw.Elapsed > TimeSpan.FromSeconds(30)) throw new TimeoutException($"{global} never appeared");
                await Task.Delay(10);
            }
        }

        static async Task Test(string runtime, string name, Func<Task> body)
        {
            var sw = Stopwatch.StartNew();
            string result = "Success", detail = "";
            try { await body(); }
            catch (Exception ex) { result = "Error"; detail = $"{ex.GetType().Name}: {ex.Message}"; }
            // the TEST: contract SpawnJS.TestRunner parses
            Console.WriteLine($"TEST: Twin.{runtime}.{name}|{result}|{sw.ElapsedMilliseconds}|{detail.Replace('\n', ' ').Replace('|', '/')}");
        }
    }
}
