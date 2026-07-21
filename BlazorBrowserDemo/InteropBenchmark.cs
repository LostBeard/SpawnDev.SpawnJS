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

            // Reading an ARRAY and reading a RECORD back. These are the shapes where a proxy used to be
            // created per CONTAINER and then, through the borrowed handle each element was addressed
            // against, per ELEMENT as well - so the cost scaled with length rather than being a fixed
            // charge. Neither was covered by any case above, which is why they are measured separately
            // rather than assumed to have moved with the others.
            blazor.CallVoid("eval", $"globalThis.{Target}.numbers = [1,2,3,4,5,6,7,8,9,10];");
            blazor.CallVoid("eval", $"globalThis.{Target}.record = {{a:1,b:2,c:3,d:4,e:5}};");
            var containerIterations = iterations / 4;

            Measure("read a 10 element array", containerIterations,
                () => { for (var i = 0; i < containerIterations; i++) _ = blazorRef.JSRef!.Get<int[]>("numbers"); },
                () => { for (var i = 0; i < containerIterations; i++) _ = spawnRef.JSRef!.Get<int[]>("numbers"); });

            Measure("read a 5 member record", containerIterations,
                () => { for (var i = 0; i < containerIterations; i++) _ = blazorRef.JSRef!.Get<Dictionary<string, int>>("record"); },
                () => { for (var i = 0; i < containerIterations; i++) _ = spawnRef.JSRef!.Get<Dictionary<string, int>>("record"); });

            // THE ARGUMENT TRANSPORT ITSELF. Both arms end with Javascript summing the same N doubles,
            // so the Javascript work is identical by construction and the difference is exactly what .Net
            // paid to deliver them:
            //   slot arm - the buffer lives Javascript side, so each argument costs a boundary crossing
            //   heap arm - the buffer lives in .Net memory, which Javascript views directly, so the
            //              writes are plain array stores and only the call itself crosses
            using var heapArgs = new SJS.HeapArgBuffer(64);
            heapArgs.Bind();
            const int argCount = 5;
            var transportIterations = iterations / 2;

            // The JS side arm writes 5 arguments the way the transport does today - one crossing each.
            MeasureOne($"{argCount} args written JS side ({argCount} crossings, no call)", transportIterations,
                () =>
                {
                    for (var i = 0; i < transportIterations; i++)
                        for (var a = 0; a < argCount; a++) spawnRef.JSRef!.Set("arg", a + 0.5);
                });

            // The .Net side arm writes the same 5 arguments AND makes the call. It does strictly MORE
            // work than the arm above, so if it still wins the result is not a matter of interpretation.
            MeasureOne($"{argCount} args written .Net side (0 crossings, PLUS the call)", transportIterations,
                () =>
                {
                    for (var i = 0; i < transportIterations; i++)
                    {
                        for (var a = 0; a < argCount; a++) heapArgs.Write(a, a + 0.5);
                        _ = SJS.SlotInterop.HeapSum(0, argCount);
                    }
                });

            // LAYOUT A/B, both in .Net memory, same argument count, same Javascript work shape.
            // structure-of-arrays: values packed 8 bytes apart, tags in a separate region
            // interleaved:         one padded 16 byte slot per argument, tag inside it - the shape the
            //                      .Net runtime's own marshaller uses
            using var heapFrame = new SJS.HeapArgFrame(64);
            heapFrame.BindProbe();

            MeasureOne($"{argCount} args, structure-of-arrays, UNtagged", transportIterations,
                () =>
                {
                    for (var i = 0; i < transportIterations; i++)
                    {
                        for (var a = 0; a < argCount; a++) heapArgs.Write(a, a + 0.5);
                        _ = SJS.SlotInterop.HeapSum(0, argCount);
                    }
                });

            MeasureOne($"{argCount} args, interleaved padded, UNtagged", transportIterations,
                () =>
                {
                    for (var i = 0; i < transportIterations; i++)
                    {
                        for (var a = 0; a < argCount; a++) heapFrame.Write(a, a + 0.5);
                        _ = SJS.SlotInterop.FrameSum(argCount);
                    }
                });

            // Tagged is the case a real transport actually runs, and it is where the layouts differ most:
            // SoA reads two regions far apart, interleaved reads one slot twice.
            MeasureOne($"{argCount} args, structure-of-arrays, TAGGED", transportIterations,
                () =>
                {
                    for (var i = 0; i < transportIterations; i++)
                    {
                        for (var a = 0; a < argCount; a++) heapArgs.WriteTagged(a, SJS.HeapArgBuffer.TagNumber, a + 0.5);
                        _ = SJS.SlotInterop.HeapTaggedSum(heapArgs.TagOffsetFromValues, 0, argCount);
                    }
                });

            MeasureOne($"{argCount} args, interleaved padded, TAGGED byte", transportIterations,
                () =>
                {
                    for (var i = 0; i < transportIterations; i++)
                    {
                        for (var a = 0; a < argCount; a++) heapFrame.WriteTaggedByte(a, SJS.HeapArgFrame.TagNumber, a + 0.5);
                        _ = SJS.SlotInterop.FrameTaggedSum(argCount);
                    }
                });

            // The byte arm above pays a MemoryMarshal.AsBytes span per write on the .Net side and a second
            // heap view plus a mixed width read on the Javascript side. Storing the tag as a float64 in the
            // padding - which exists either way - removes both, and isolates the LAYOUT from that overhead.
            MeasureOne($"{argCount} args, interleaved padded, TAGGED f64", transportIterations,
                () =>
                {
                    for (var i = 0; i < transportIterations; i++)
                    {
                        for (var a = 0; a < argCount; a++) heapFrame.WriteTagged(a, SJS.HeapArgFrame.TagNumber, a + 0.5);
                        _ = SJS.SlotInterop.FrameTaggedSumF64(argCount);
                    }
                });

            // STRING ARGUMENTS - the last undecided piece of the frame layout. A string needs two fields
            // (where the characters are, how many), which is why the runtime's own slot is wider. Either
            // the frame grows an aux field, or strings keep crossing one at a time.
            // Both arms end with Javascript holding the same N strings and summing their lengths.
            var strings = new[] { "setBindGroup", "compute", "a", "vertexBufferLayoutDescriptor", "rgba8unorm" };
            using var stringFrame = new SJS.HeapArgFrame3(64);
            var stringArgsSlot = SJS.SlotInterop.NewArray();
            var stringIterations = iterations / 4;

            MeasureOne($"{strings.Length} string args, crossing one at a time", stringIterations,
                () =>
                {
                    for (var i = 0; i < stringIterations; i++)
                    {
                        for (var a = 0; a < strings.Length; a++) SJS.SlotInterop.SetStringAt(stringArgsSlot, a, strings[a]);
                        _ = SJS.SlotInterop.SlotStringLength(stringArgsSlot, strings.Length);
                    }
                });

            // bind ONCE - binding is itself a crossing, and doing it per iteration would measure that
            // rather than the transport.
            stringFrame.Bind();
            MeasureOne($"{strings.Length} string args, pinned address in frame", stringIterations,
                () =>
                {
                    for (var i = 0; i < stringIterations; i++)
                    {
                        for (var a = 0; a < strings.Length; a++) stringFrame.WriteString(a, strings[a]);
                        _ = SJS.SlotInterop.FrameStringLength(strings.Length);
                        stringFrame.ReleasePins();
                    }
                });
            // the two-field frame is what the other arms bind; restore it so their numbers stay comparable
            heapFrame.BindProbe();

            // END TO END: the generic dispatcher, with the argument frame OFF and ON. One variable.
            // These are the operations that actually reach NetRun - a dotted path defeats the slot fast
            // paths, so the arguments go through the transport. The obj-ref cases above never get here,
            // which is why they do not move.
            var genericIterations = iterations / 2;
            foreach (var useFrame in new[] { false, true })
            {
                SJS.SpawnJSRuntime.UseArgFrame = useFrame;
                var label = useFrame ? "frame" : "JS-side buffer";

                MeasureOne($"generic get int (dotted) - {label}", genericIterations,
                    () => { for (var i = 0; i < genericIterations; i++) _ = spawn.Get<int>($"{Target}.number"); });

                MeasureOne($"generic call method (dotted) - {label}", genericIterations,
                    () => { for (var i = 0; i < genericIterations; i++) _ = spawn.Call<int>($"{Target}.method", i); });

                MeasureOne($"generic get object handle (dotted) - {label}", genericIterations,
                    () =>
                    {
                        for (var i = 0; i < genericIterations; i++)
                        {
                            using var o = spawn.Get<SJS.SpawnJSObject>($"{Target}.child");
                        }
                    });
            }
            SJS.SpawnJSRuntime.UseArgFrame = true;

            // The INBOUND direction - Javascript calling .Net. Outbound carries only (cmd, offset,
            // length) over a flat buffer; this measures whether inbound does the same. Every DOM event,
            // every callback and every promise settlement takes this path, so it is the highest frequency
            // JS->.Net cost in a real app.
            // FIRST: what is the inbound cost actually MADE of? A one argument callback measures ~13us,
            // but at most two crossings are involved - so most of it is not the boundary. Scaling the
            // ARGUMENT COUNT separates per-argument cost from fixed dispatch cost, and decides whether an
            // inbound frame is worth building at all or whether the dispatch itself is the problem.
            var arityHits = 0;
            using var inbound0 = SJS.Callback.Create(() => arityHits++);
            using var inbound1 = SJS.Callback.Create<int>(_ => arityHits++);
            using var inbound4 = SJS.Callback.Create<int, int, int, int>((_, _, _, _) => arityHits++);
            spawn.Set("__benchArity0", inbound0);
            spawn.Set("__benchArity1", inbound1);
            spawn.Set("__benchArity4", inbound4);
            blazor.CallVoid("eval",
                "globalThis.__benchArity0Loop = function(n) { for (var i = 0; i < n; i++) globalThis.__benchArity0(); };" +
                "globalThis.__benchArity1Loop = function(n) { for (var i = 0; i < n; i++) globalThis.__benchArity1(i); };" +
                "globalThis.__benchArity4Loop = function(n) { for (var i = 0; i < n; i++) globalThis.__benchArity4(i, i, i, i); };");
            var arityIterations = iterations / 2;
            MeasureOne("inbound arity 0 (no arguments at all)", arityIterations,
                () => spawn.CallVoid("__benchArity0Loop", arityIterations));
            MeasureOne("inbound arity 1", arityIterations,
                () => spawn.CallVoid("__benchArity1Loop", arityIterations));
            MeasureOne("inbound arity 4", arityIterations,
                () => spawn.CallVoid("__benchArity4Loop", arityIterations));

            var inboundHits = 0;
            using var inboundVoid = SJS.Callback.Create<int>(_ => inboundHits++);
            spawn.Set("__benchInboundVoid", inboundVoid);
            using var inboundValue = SJS.Callback.Create<int, int>(v => v + 1);
            spawn.Set("__benchInboundValue", inboundValue);
            blazor.CallVoid("eval",
                "globalThis.__benchInboundVoidLoop = function(n) { for (var i = 0; i < n; i++) globalThis.__benchInboundVoid(i); };" +
                "globalThis.__benchInboundValueLoop = function(n) { var t = 0; for (var i = 0; i < n; i++) t += globalThis.__benchInboundValue(i); return t; };");

            MeasureOne("inbound: JS calls .Net void callback", iterations,
                () => spawn.CallVoid("__benchInboundVoidLoop", iterations));

            MeasureOne("inbound: JS calls .Net callback with a return", iterations,
                () => _ = spawn.Call<int>("__benchInboundValueLoop", iterations));

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
