using System;
using System.Linq;
using SpawnDev.SpawnJS.JSObjects;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS.Demo.Bench
{
    /// <summary>
    /// Interop benchmark, run with <c>?bench=[filter]</c>. Every case reports TWO numbers:
    /// <list type="bullet">
    /// <item>us/op - median of 5 timed batches, each at least ~150 ms long (performance.now() is coarse).</item>
    /// <item>crossings/op - .Net to JS boundary crossings, counted JS side by <c>__sjsCrossings</c>
    /// (see spawnjs-bench.js). The count is exact on a loaded machine; the time is not.</item>
    /// </list>
    /// Output uses the TEST:/RESULTS: contract SpawnJS.TestRunner parses (<c>--bench</c>).
    /// Measure a PUBLISHED Release build - a dev-server build is not what ships.
    /// </summary>
    public static class InteropBench
    {
        static SpawnJSRuntime JS => SpawnJSRuntime.Instance;
        static string _filter = "";
        static double _readOverhead;

        // The bench POCOs are read only by the reflection marshaller, so a trimmed publish strips their getters
        // and the write throws Arg_GetMethNotFnd. Root them - the same thing any consumer POCO needs today.
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(BenchDto16))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(BenchInner))]
        public static void Run(string filter)
        {
            _filter = filter ?? "";
            if (!JS.Has("__sjsCrossings"))
            {
                Console.WriteLine("TEST: bench setup|Error|0|crossing counter not installed (main.js installs it for ?bench)");
                Console.WriteLine("RESULTS: bench setup failed");
                return;
            }
            // what one read of the counter costs, so each case can subtract it
            JS.CallVoid("__sjsCrossings.reset");
            _readOverhead = JS.Get<double>("__sjsCrossings.count");
            Console.WriteLine($"TEST: bench calibration|Success|0|a counter read costs {_readOverhead} crossings");

            using var bench = JS.Get<SpawnJSObjectReference>("__bench")!;
            using var layout = JS.Get<GPUBindGroupLayout>("__bench.layout")!;
            using var buf0 = JS.Get<GPUBuffer>("__bench.buf0")!;
            using var buf1 = JS.Get<GPUBuffer>("__bench.buf1")!;
            using var buf2 = JS.Get<GPUBuffer>("__bench.buf2")!;
            var dto = BenchDto16.Create();
            // the shape of one ILGPU dispatch's bind group: a layout ref and three buffer bindings
            var desc = new GPUBindGroupDescriptor
            {
                Label = "bench",
                Layout = layout,
                Entries = new[]
                {
                    new GPUBindGroupEntry { Binding = 0, Resource = new GPUBufferBinding { Buffer = buf0, Offset = 0, Size = 4096 } },
                    new GPUBindGroupEntry { Binding = 1, Resource = new GPUBufferBinding { Buffer = buf1, Offset = 256, Size = 1024 } },
                    new GPUBindGroupEntry { Binding = 2, Resource = new GPUBufferBinding { Buffer = buf2 } },
                },
            };
            var ints1000 = Enumerable.Range(0, 1000).ToArray();
            var doubles1000 = Enumerable.Range(0, 1000).Select(i => i * 0.5).ToArray();
            var strings8 = new[] { "setPipeline", "setBindGroup", "dispatchWorkgroups", "end", "finish", "submit", "rgba8unorm", "compute" };
            var bytes4096 = new byte[4096];

            Case("get double (held obj)", () => bench.Get<double>("num"));
            Case("call void 0 args", () => bench.CallVoid("sink0"));
            Case("call void 1 double", () => bench.CallVoid("sink1", 1.5));
            Case("call void 5 mixed (num,str,bool,ref,num)", () => bench.CallVoid("sink5", 1.5, "setBindGroup", true, buf0, 7));
            Case("call void Dto16 POCO", () => bench.CallVoid("sink1", dto));
            Case("call void GPUBindGroupDescriptor (3 entries)", () => bench.CallVoid("sink1", desc));
            Case("call void int[1000]", () => bench.CallVoid("sink1", ints1000));
            Case("call void double[1000]", () => bench.CallVoid("sink1", doubles1000));
            Case("call void string[8]", () => bench.CallVoid("sink1", strings8));
            Case("call void byte[4096]", () => bench.CallVoid("sink1", bytes4096));
            Case("call return Dto16 POCO", () => bench.Call<BenchDto16>("makeDto16"));
            Case("call return int[1000]", () => bench.Call<int[]>("makeInts1000"));

            Console.WriteLine("RESULTS: bench done");
        }

        static void Case(string name, Action op)
        {
            if (_filter.Length > 0 && !name.Contains(_filter, StringComparison.OrdinalIgnoreCase)) return;
            var total = Stopwatch.StartNew();
            try
            {
                for (var i = 0; i < 50; i++) op();   // warm marshaller caches and the JS JIT

                // crossings, from one op's breakdown and an average over 20
                JS.CallVoid("__sjsCrossings.reset");
                op();
                JS.CallVoid("__sjsCrossings.snapshot");
                var breakdown = JS.Get<string>("__sjsCrossings.snap");
                const int countOps = 20;
                JS.CallVoid("__sjsCrossings.reset");
                for (var i = 0; i < countOps; i++) op();
                var crossings = (JS.Get<double>("__sjsCrossings.count") - _readOverhead) / countOps;

                // time: grow the batch until it is long enough to time, then take the median of 5
                var n = 16;
                while (true)
                {
                    var ms = TimeBatch(op, n);
                    if (ms >= 150 || n >= 1 << 22) break;
                    n *= 2;
                }
                var samples = new double[5];
                for (var s = 0; s < samples.Length; s++) samples[s] = TimeBatch(op, n) * 1000.0 / n;
                System.Array.Sort(samples);
                var median = samples[samples.Length / 2];
                total.Stop();
                Console.WriteLine($"TEST: bench {name}|Success|{total.ElapsedMilliseconds}|{median:F2} us/op, {crossings:F1} crossings/op (n={n}, min {samples[0]:F2}, max {samples[^1]:F2}) first op: {breakdown}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TEST: bench {name}|Error|{total.ElapsedMilliseconds}|{ex.GetType().Name}: {ex.Message.Replace('\n', ' ')}");
            }
        }

        static double TimeBatch(Action op, int n)
        {
            var t = Stopwatch.GetTimestamp();
            for (var i = 0; i < n; i++) op();
            return Stopwatch.GetElapsedTime(t).TotalMilliseconds;
        }
    }

    public class BenchInner
    {
        public int A { get; set; }
        public double B { get; set; }
        public string? C { get; set; }
    }

    /// <summary>16 members, roughly the mix of ILGPU's WasmFlatDispatchMessage: numbers, strings, flags, a small array, a nested object and a null.</summary>
    public class BenchDto16
    {
        public int Int1 { get; set; }
        public int Int2 { get; set; }
        public int Int3 { get; set; }
        public int Int4 { get; set; }
        public double Dbl1 { get; set; }
        public double Dbl2 { get; set; }
        public double Dbl3 { get; set; }
        public double Dbl4 { get; set; }
        public string? Str1 { get; set; }
        public string? Str2 { get; set; }
        public string? Str3 { get; set; }
        public bool Flag1 { get; set; }
        public bool Flag2 { get; set; }
        public int[]? Ints { get; set; }
        public BenchInner? Inner { get; set; }
        public string? Maybe { get; set; }

        public static BenchDto16 Create() => new()
        {
            Int1 = 1, Int2 = 2, Int3 = 3, Int4 = 4,
            Dbl1 = 1.5, Dbl2 = 2.5, Dbl3 = 3.5, Dbl4 = 4.5,
            Str1 = "setBindGroup", Str2 = "rgba8unorm", Str3 = "compute",
            Flag1 = true, Flag2 = false,
            Ints = new[] { 1, 2, 3, 4 },
            Inner = new BenchInner { A = 7, B = 8.5, C = "inner" },
            Maybe = null,
        };
    }
}
