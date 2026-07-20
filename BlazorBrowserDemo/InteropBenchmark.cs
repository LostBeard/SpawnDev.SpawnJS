using SpawnDev.BlazorJS;
using System.Diagnostics;
using BJS = SpawnDev.BlazorJS;
using SJS = SpawnDev.SpawnJS;

namespace BlazorBrowserDemo
{
    /// <summary>
    /// Head to head: the same interop operations through SpawnDev.BlazorJS (which serialises every
    /// non-primitive across the boundary as Json) and through SpawnDev.SpawnJS (which does not).
    /// <br/>
    /// The operations measured are deliberately the SMALL ones - read a property, write a property, call a
    /// method, take a handle to an object. That is the traffic an orchestration heavy consumer like
    /// SpawnDev.ILGPU actually generates: thousands of tiny calls setting up a dispatch, not a few big
    /// buffer transfers. Bulk data already avoids Json on both sides, so it is not where a difference
    /// would show.
    /// <br/>
    /// Both runtimes hit the SAME Javascript object and perform the SAME operation, so the only variable
    /// is the interop path.
    /// </summary>
    public static class InteropBenchmark
    {
        const string Target = "_spawnjs_bench_target";

        public static void Run(SJS.SpawnJSRuntime spawn, BJS.BlazorJSRuntime blazor, int iterations = 20000)
        {
            // one shared target object, created once, so neither runtime pays for setup during timing
            blazor.Set(Target, blazor.New("Object"));
            blazor.Set($"{Target}.number", 41);
            blazor.Set($"{Target}.text", "hello");
            blazor.Set($"{Target}.child", blazor.New("Object"));
            blazor.CallVoid("eval", $"globalThis.{Target}.method = function(a) {{ return a + 1; }};");

            Console.WriteLine($"BENCH-START iterations={iterations}");

            Measure("get int", iterations,
                () => { for (var i = 0; i < iterations; i++) _ = blazor.Get<int>($"{Target}.number"); },
                () => { for (var i = 0; i < iterations; i++) _ = spawn.Get<int>($"{Target}.number"); });

            Measure("get string", iterations,
                () => { for (var i = 0; i < iterations; i++) _ = blazor.Get<string>($"{Target}.text"); },
                () => { for (var i = 0; i < iterations; i++) _ = spawn.Get<string>($"{Target}.text"); });

            Measure("set int", iterations,
                () => { for (var i = 0; i < iterations; i++) blazor.Set($"{Target}.number", i); },
                () => { for (var i = 0; i < iterations; i++) spawn.Set($"{Target}.number", i); });

            Measure("call method", iterations,
                () => { for (var i = 0; i < iterations; i++) _ = blazor.Call<int>($"{Target}.method", i); },
                () => { for (var i = 0; i < iterations; i++) _ = spawn.Call<int>($"{Target}.method", i); });

            // taking a handle to an object is the case Json hurts most: BlazorJS has to serialise a
            // reference, SpawnJS hands back a live handle
            Measure("get object handle", iterations,
                () =>
                {
                    for (var i = 0; i < iterations; i++)
                    {
                        using var o = blazor.Get<BJS.JSObject>($"{Target}.child");
                    }
                },
                () =>
                {
                    for (var i = 0; i < iterations; i++)
                    {
                        using var o = spawn.Get<SJS.SpawnJSObject>($"{Target}.child");
                    }
                });

            // The cases above address a DOTTED path from globalThis, which forces both runtimes to resolve
            // "a.b" at every call. Real consumers hold a reference to an object and use a simple key -
            // that is what ILGPU does when it plumbs a dispatch - so measure that shape too.
            using var spawnRef = spawn.Get<SJS.SpawnJSObject>(Target)!;
            using var blazorRef = blazor.Get<BJS.JSObject>(Target)!;

            Measure("get int (obj ref)", iterations,
                () => { for (var i = 0; i < iterations; i++) _ = blazorRef.JSRef!.Get<int>("number"); },
                () => { for (var i = 0; i < iterations; i++) _ = spawnRef.JSRef!.Get<int>("number"); });

            Measure("get string (obj ref)", iterations,
                () => { for (var i = 0; i < iterations; i++) _ = blazorRef.JSRef!.Get<string>("text"); },
                () => { for (var i = 0; i < iterations; i++) _ = spawnRef.JSRef!.Get<string>("text"); });

            Measure("set int (obj ref)", iterations,
                () => { for (var i = 0; i < iterations; i++) blazorRef.JSRef!.Set("number", i); },
                () => { for (var i = 0; i < iterations; i++) spawnRef.JSRef!.Set("number", i); });

            Measure("call method (obj ref)", iterations,
                () => { for (var i = 0; i < iterations; i++) _ = blazorRef.JSRef!.Call<int>("method", i); },
                () => { for (var i = 0; i < iterations; i++) _ = spawnRef.JSRef!.Call<int>("method", i); });

            Console.WriteLine("BENCH-DONE");
        }

        static void Measure(string name, int iterations, Action blazorWork, Action spawnWork)
        {
            // warm up both paths so neither pays first-call costs inside the timed run
            try { blazorWork(); } catch (Exception ex) { Console.WriteLine($"BENCH: {name}|blazor-failed|{ex.GetType().Name}: {ex.Message}"); return; }
            try { spawnWork(); } catch (Exception ex) { Console.WriteLine($"BENCH: {name}|spawn-failed|{ex.GetType().Name}: {ex.Message}"); return; }

            var sw = Stopwatch.StartNew();
            blazorWork();
            sw.Stop();
            var blazorMs = sw.Elapsed.TotalMilliseconds;

            sw.Restart();
            spawnWork();
            sw.Stop();
            var spawnMs = sw.Elapsed.TotalMilliseconds;

            var ratio = spawnMs > 0 ? blazorMs / spawnMs : 0;
            Console.WriteLine($"BENCH: {name}|blazor {blazorMs:F1}ms|spawnjs {spawnMs:F1}ms|{ratio:F2}x");
        }
    }
}
