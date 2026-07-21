using SpawnDev.BlazorJS;
// imported for the SpawnJSHandle SetProperty extension methods the hand written floor case uses;
// the `BJS.` / `SJS.` prefixes elsewhere keep the two runtimes' types unambiguous
using SpawnDev.SpawnJS;
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

            // Marshalling a descriptor POCO is what a dispatch actually costs: ILGPU builds several of
            // these per kernel launch and each one walks its members through the marshaller registry.
            var descriptor = new BenchDescriptor
            {
                Label = "bench",
                Size = 4096,
                Usage = BenchUsage.Storage,
                MappedAtCreation = false,
                Offset = 0,
            };
            var pocoIterations = iterations / 4;
            Measure("marshal POCO", pocoIterations,
                () => { for (var i = 0; i < pocoIterations; i++) blazorRef.JSRef!.Set("poco", descriptor); },
                () => { for (var i = 0; i < pocoIterations; i++) spawnRef.JSRef!.Set("poco", descriptor); });

            // The WRITE FLOOR for the case above: the same five values written as scalars onto an object
            // already held by reference, with no reflection, no registry, and no object creation. This is
            // the irreducible part of marshalling that POCO, so the gap between it and "marshal POCO" is
            // everything the machinery adds. Measuring against the POCO case alone cannot separate the two.
            using var childRef = spawn.Get<SJS.SpawnJSObject>($"{Target}.child")!;
            using var blazorChildRef = blazor.Get<BJS.JSObject>($"{Target}.child")!;
            Measure("POCO write floor (5 scalar writes, held object)", pocoIterations,
                () =>
                {
                    for (var i = 0; i < pocoIterations; i++)
                    {
                        blazorChildRef.JSRef!.Set("label", "bench");
                        blazorChildRef.JSRef!.Set("size", 4096);
                        blazorChildRef.JSRef!.Set("usage", 128);
                        blazorChildRef.JSRef!.Set("mappedAtCreation", false);
                        blazorChildRef.JSRef!.Set("offset", 0);
                    }
                },
                () =>
                {
                    for (var i = 0; i < pocoIterations; i++)
                    {
                        childRef.JSRef!.Set("label", "bench");
                        childRef.JSRef!.Set("size", 4096);
                        childRef.JSRef!.Set("usage", 128);
                        childRef.JSRef!.Set("mappedAtCreation", false);
                        childRef.JSRef!.Set("offset", 0);
                    }
                });

            // Building the same shape through the PUBLIC api instead - create an object, write it, attach
            // it. Kept because it is what consuming code would naturally write, and it is worth knowing
            // that it costs more than handing the same POCO to the marshaller.
            Measure("POCO built by hand via public api", pocoIterations,
                () =>
                {
                    for (var i = 0; i < pocoIterations; i++)
                    {
                        using var o = blazor.New("Object");
                        o.Set("label", "bench");
                        o.Set("size", 4096);
                        o.Set("usage", 128);
                        o.Set("mappedAtCreation", false);
                        o.Set("offset", 0);
                        blazorRef.JSRef!.Set("poco", o);
                    }
                },
                () =>
                {
                    for (var i = 0; i < pocoIterations; i++)
                    {
                        using var o = spawn.NewJSObject();
                        o.SetProperty("label", "bench");
                        o.SetProperty("size", 4096);
                        o.SetProperty("usage", 128);
                        o.SetProperty("mappedAtCreation", false);
                        o.SetProperty("offset", 0);
                        spawnRef.JSRef!.Set("poco", o);
                    }
                });

            // Split the machinery cost by KIND. The gap between "marshal POCO" and the floor says how much
            // the machinery costs but not WHICH part, and guessing which part has been wrong every time it
            // has been guessed. These two arms do the member walk with the Javascript writes removed, so
            // reflection and registry lookup are each priced on their own.
            // qualified because BlazorJS carries an extension of the same name
            var members = SpawnDev.SpawnJS.Marshallers.TypeExtensions.GetTypeJsonProperties(typeof(BenchDescriptor));
            MeasureOne("POCO members: reflection only", pocoIterations, () =>
            {
                for (var i = 0; i < pocoIterations; i++)
                {
                    foreach (var m in members)
                    {
                        var v = m.PropertyInfo!.GetValue(descriptor);
                        if (!m.GetShouldWrite(v)) continue;
                        _ = m.GetJsonName();
                    }
                }
            });

            MeasureOne("POCO members: reflection + registry lookup", pocoIterations, () =>
            {
                for (var i = 0; i < pocoIterations; i++)
                {
                    foreach (var m in members)
                    {
                        var v = m.PropertyInfo!.GetValue(descriptor);
                        if (!m.GetShouldWrite(v)) continue;
                        _ = m.GetJsonName();
                        _ = spawn.GetMarshaller(v?.GetType());
                    }
                }
            });

            Console.WriteLine("BENCH-DONE");
        }

        static void MeasureOne(string name, int iterations, Action work)
        {
            try { work(); } catch (Exception ex) { Console.WriteLine($"BENCH: {name}|failed|{ex.GetType().Name}: {ex.Message}"); return; }
            var sw = Stopwatch.StartNew();
            work();
            sw.Stop();
            Console.WriteLine($"BENCH: {name}|spawnjs {sw.Elapsed.TotalMilliseconds:F1}ms|{sw.Elapsed.TotalMilliseconds * 1000 / iterations:F2}us per object");
        }

        /// <summary>
        /// Shaped after a real WebGPU descriptor - a string, a number, a numeric enum, a bool and a second
        /// number. Nothing here carries a JsonPropertyName, which is the common case for wrapper types and
        /// the case that pays the most per member.
        /// </summary>
        class BenchDescriptor
        {
            public string? Label { get; set; }
            public int Size { get; set; }
            public BenchUsage Usage { get; set; }
            public bool MappedAtCreation { get; set; }
            public int Offset { get; set; }
        }

        enum BenchUsage
        {
            Storage = 128,
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
