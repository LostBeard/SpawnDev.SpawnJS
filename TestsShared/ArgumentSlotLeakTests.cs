using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.Native;
using System.Text.Json.Serialization;

namespace TestsShared
{
    /// <summary>
    /// The slot table must not grow when a call is made and returns.<br/>
    /// <br/>
    /// A slot table entry is a STRONG reference held for the life of the process, so anything that takes a
    /// slot and is owned by nobody is a permanent leak of both the entry and the Javascript value it names.
    /// An object passed as a call argument used to be built into a slot whose id was handed over as the
    /// argument - which nothing then freed. It cost two entries per WebGPU dispatch: 4000 live slots after
    /// 2000 launches, and every later call paid for the growth.
    /// <br/><br/>
    /// It is deliberately measured against the TABLE, not against
    /// <see cref="SpawnJSHandle.LiveSlotCount"/>. That counter only sees slots a handle owns, and the
    /// leaked slots were allocated Javascript side and owned by nothing - so it read zero throughout and
    /// could never have caught this. A guard that cannot observe the failure is not a guard.
    /// </summary>
    public class ArgumentSlotLeakTests(SpawnJSRuntime JS)
    {
        public class Descriptor
        {
            [JsonPropertyName("x")]
            public int X { get; set; }
            [JsonPropertyName("label")]
            public string? Label { get; set; }
        }

        public class Nested
        {
            [JsonPropertyName("inner")]
            public Descriptor? Inner { get; set; }
            [JsonPropertyName("n")]
            public int N { get; set; }
        }

        /// <summary>Defines a probe that takes an object and returns a number, so no result takes a slot.</summary>
        void DefineProbe() => JS.Call<int>("eval",
            "globalThis.__sjsArgLeakProbe=function(o){return (o&&o.x)|0;},1");

        const int Iterations = 200;

        [SpawnJSTest]
        public async Task ObjectArgumentLeavesNoSlotBehindTest()
        {
            DefineProbe();
            var arg = new Descriptor { X = 7, Label = "d" };

            // Twice first. Property names intern on first use and a repeated string VALUE interns on its
            // second sighting, both legitimate one offs per type. Two warm calls settle every one of them,
            // so what the loop then measures is purely per CALL growth and the assertion can be exact.
            var warm = JS.Call<int>("__sjsArgLeakProbe", arg);
            if (warm != 7) throw new Exception($"probe read x as {warm}, so the object never arrived");
            JS.Call<int>("__sjsArgLeakProbe", arg);

            var before = SlotInterop.SlotTableCount();
            for (var i = 0; i < Iterations; i++)
            {
                if (JS.Call<int>("__sjsArgLeakProbe", arg) != 7)
                    throw new Exception($"call {i} read the wrong value");
            }
            var after = SlotInterop.SlotTableCount();

            if (after != before)
                throw new Exception($"{Iterations} object arguments left {after - before} slots behind "
                    + $"(table {before} -> {after})");
        }

        /// <summary>
        /// The nested case, because an inline object is built by reading a frame region and a member that
        /// is itself an object reserves a region of its own. If nesting ever falls back to taking a slot,
        /// this grows and the flat test above still passes.
        /// </summary>
        [SpawnJSTest]
        public async Task NestedObjectArgumentLeavesNoSlotBehindTest()
        {
            DefineProbe();
            var arg = new Nested { Inner = new Descriptor { X = 3, Label = "inner" }, N = 5 };

            JS.Call<int>("eval", "globalThis.__sjsNestProbe=function(o){return (o&&o.inner&&o.inner.x)|0;},1");
            var warm = JS.Call<int>("__sjsNestProbe", arg);
            if (warm != 3) throw new Exception($"nested probe read inner.x as {warm}");
            JS.Call<int>("__sjsNestProbe", arg);   // settle the repeated string value's interning too

            var before = SlotInterop.SlotTableCount();
            for (var i = 0; i < Iterations; i++)
            {
                if (JS.Call<int>("__sjsNestProbe", arg) != 3)
                    throw new Exception($"call {i} read the wrong nested value");
            }
            var after = SlotInterop.SlotTableCount();

            if (after != before)
                throw new Exception($"{Iterations} nested object arguments left {after - before} slots behind "
                    + $"(table {before} -> {after})");
        }

        /// <summary>
        /// A string that never repeats must not be interned. Interning holds a Javascript slot for the life
        /// of the process, so doing it on first sight fills the table with strings that will never be read
        /// again - which is what awaiting a promise did, twice per await, via Callback's <c>cb_{n}</c> name.
        /// </summary>
        [SpawnJSTest]
        public async Task UniqueStringArgumentsAreNotInternedTest()
        {
            JS.Call<int>("eval", "globalThis.__sjsStrProbe=function(s){return s.length;},1");

            var before = SlotInterop.SlotTableCount();
            for (var i = 0; i < Iterations; i++)
            {
                var unique = $"unique-string-{Guid.NewGuid():N}-{i}";
                if (JS.Call<int>("__sjsStrProbe", unique) != unique.Length)
                    throw new Exception($"call {i} did not receive the string intact");
            }
            var after = SlotInterop.SlotTableCount();

            if (after != before)
                throw new Exception($"{Iterations} unique strings left {after - before} slots behind "
                    + $"(table {before} -> {after})");
        }

        /// <summary>
        /// The other half of the rule: a string that DOES repeat still ends up interned, so the saving that
        /// interning exists for is not quietly lost. One slot for the literal, however many times it is used.
        /// </summary>
        [SpawnJSTest]
        public async Task RepeatedStringArgumentIsInternedOnceTest()
        {
            JS.Call<int>("eval", "globalThis.__sjsStrProbe=function(s){return s.length;},1");
            var repeated = $"repeated-literal-{Guid.NewGuid():N}";

            var before = SlotInterop.SlotTableCount();
            for (var i = 0; i < Iterations; i++)
            {
                if (JS.Call<int>("__sjsStrProbe", repeated) != repeated.Length)
                    throw new Exception($"call {i} did not receive the string intact");
            }
            var grew = SlotInterop.SlotTableCount() - before;

            // exactly one: the second sighting interns it, every later one hits the cache
            if (grew != 1)
                throw new Exception($"a repeated string took {grew} slots over {Iterations} uses, expected 1");
        }

        /// <summary>
        /// The value has to survive the trip, not just leave no slot - an inline object is built out of a
        /// frame region that the outer call must not have released yet, so a lifetime mistake shows up as
        /// missing or wrong members rather than as a leak.
        /// </summary>
        [SpawnJSTest]
        public async Task InlineObjectArgumentArrivesIntactTest()
        {
            JS.Call<int>("eval",
                "globalThis.__sjsShapeProbe=function(o){return JSON.stringify([o.x,o.label,o.inner?o.inner.x:null]);},1");

            var flat = JS.Call<string>("__sjsShapeProbe", new Descriptor { X = 11, Label = "hello" });
            if (flat != "[11,\"hello\",null]") throw new Exception($"flat object arrived as {flat}");

            // Nested carries inner and n, so x and label are genuinely absent - undefined, which
            // JSON.stringify writes as null. What matters is that inner.x came through the nested region.
            var nested = JS.Call<string>("__sjsShapeProbe", new Nested { Inner = new Descriptor { X = 4 }, N = 1 });
            if (nested != "[null,null,4]") throw new Exception($"nested object arrived as {nested}");
        }
    }
}
