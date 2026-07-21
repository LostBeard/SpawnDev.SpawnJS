using SpawnDev.SpawnJS;
using System.Text.Json.Serialization;

namespace TestsShared
{
    /// <summary>
    /// Members of an object being cloned into Javascript, with the emphasis on the cases that a
    /// per member marshaller cache could get wrong.<br/>
    /// ObjectMarshaller resolves the marshaller for a member once and reuses it, but ONLY when the
    /// member's declared type pins the runtime type - a value type or a sealed class. A member declared
    /// as a base class may hold any subclass, and the two do not marshal the same way, so that case has
    /// to keep asking the value for its own type. These tests hold that line: if the optimisation is ever
    /// widened to cache by declared type unconditionally, the first test here fails.
    /// </summary>
    public class ObjectMemberMarshalTests(SpawnJSRuntime JS)
    {
        public class BaseShape
        {
            [JsonPropertyName("kind")]
            public string? Kind { get; set; }
        }

        public class DerivedShape : BaseShape
        {
            [JsonPropertyName("extra")]
            public int Extra { get; set; }
        }

        public struct Point
        {
            [JsonPropertyName("x")]
            public int X { get; set; }
            [JsonPropertyName("y")]
            public int Y { get; set; }
        }

        /// <summary>
        /// A descriptor shaped like the WebIDL ones that carry `required` members.
        /// </summary>
        public class RequiredHolder
        {
            [JsonPropertyName("format")]
            public required string Format { get; set; }
            [JsonPropertyName("offset")]
            public int Offset { get; set; }
        }

        public class Holder
        {
            /// <summary>A nullable custom struct - selected for as Point, walked as Point.</summary>
            [JsonPropertyName("point")]
            public Point? Point { get; set; }
            /// <summary>Declared as the base - the runtime type is NOT knowable from the declaration.</summary>
            [JsonPropertyName("shape")]
            public BaseShape? Shape { get; set; }
            /// <summary>Sealed, so the fast path applies.</summary>
            [JsonPropertyName("label")]
            public string? Label { get; set; }
            /// <summary>A nullable value type - the marshaller is selected for the UNWRAPPED type.</summary>
            [JsonPropertyName("count")]
            public int? Count { get; set; }
        }

        /// <summary>
        /// A member declared as a base type but holding a derived value must marshal as the DERIVED type,
        /// so the derived member reaches Javascript.<br/>
        /// Caching a marshaller against the declared type would walk the base type's members only, and
        /// "extra" would silently never be written - the value would still be a well formed object, which
        /// is exactly why this needs asserting rather than eyeballing.
        /// </summary>
        [SpawnJSTest]
        public async Task DerivedValueInBaseTypedMemberMarshalsAsDerivedTest()
        {
            // the assumption this test rests on: if BaseShape were sealed the fast path would apply and
            // the test would be checking nothing, so assert the shape of the fixture itself
            if (typeof(BaseShape).IsSealed || typeof(BaseShape).IsValueType)
                throw new Exception("BaseShape must be an open class or this test has no teeth");

            JS.Set("__memberTestHolder", new Holder
            {
                Shape = new DerivedShape { Kind = "derived", Extra = 7 },
                Label = "held",
                Count = 3,
            });

            var kind = JS.Get<string>("__memberTestHolder.shape.kind");
            if (kind != "derived") throw new Exception($"shape.kind read back as '{kind}'");
            var extra = JS.Get<double>("__memberTestHolder.shape.extra");
            if (extra != 7) throw new Exception($"shape.extra read back as {extra} - the derived member did not cross");
        }

        /// <summary>
        /// The same member holding a plain base value must still marshal as the base, and must not carry
        /// over anything from a previous derived value.
        /// </summary>
        [SpawnJSTest]
        public async Task BaseValueInBaseTypedMemberMarshalsAsBaseTest()
        {
            JS.Set("__memberTestBase", new Holder { Shape = new DerivedShape { Kind = "first", Extra = 9 } });
            JS.Set("__memberTestBase", new Holder { Shape = new BaseShape { Kind = "second" } });

            var kind = JS.Get<string>("__memberTestBase.shape.kind");
            if (kind != "second") throw new Exception($"shape.kind read back as '{kind}'");
            var extra = JS.Get<string?>("__memberTestBase.shape.extra");
            if (extra != null) throw new Exception($"shape.extra should be absent on a base value, got '{extra}'");
        }

        /// <summary>
        /// A nullable value type member carries its value as a plain number. The marshaller is selected
        /// for int, not for int?, because a boxed Nullable&lt;int&gt; reports int as its runtime type -
        /// selecting for the declared int? would find a different marshaller than the value itself needs.
        /// </summary>
        [SpawnJSTest]
        public async Task NullableValueMemberCrossesAsItsValueTest()
        {
            JS.Set("__memberTestNullable", new Holder { Count = 42, Label = "x" });
            var count = JS.Get<double>("__memberTestNullable.count");
            if (count != 42) throw new Exception($"count read back as {count}");
        }

        /// <summary>
        /// And a null in that same member must cross as null rather than taking the fast path, which is
        /// only valid for a non null value.
        /// </summary>
        [SpawnJSTest]
        public async Task NullNullableValueMemberCrossesAsNullTest()
        {
            JS.Set("__memberTestNull", new Holder { Count = null, Label = "x" });
            var count = JS.Get<string?>("__memberTestNull.count");
            if (count != null) throw new Exception($"count should be null, read back '{count}'");
            // the neighbouring member must be unaffected - a null must not disturb the walk
            var label = JS.Get<string>("__memberTestNull.label");
            if (label != "x") throw new Exception($"label read back as '{label}'");
        }

        /// <summary>
        /// A nullable value type must READ BACK, not just write.<br/>
        /// This is the case that was broken: a write types itself from the boxed value, which reports int,
        /// but a read types itself from the DECLARATION, which is Nullable&lt;int&gt; - a value type that
        /// is neither an enum nor primitive, so StructMarshaller claimed it and walked HasValue/Value.
        /// Javascript held the right number the whole time, so only reading it back catches this.
        /// </summary>
        [SpawnJSTest]
        public async Task NullableValueMemberReadsBackTest()
        {
            JS.Set("__memberTestNullableRead", new Holder { Count = 5, Label = "x" });
            // assert Javascript really holds the number first, so a failure below is unambiguously a READ
            var raw = JS.Get<double>("__memberTestNullableRead.count");
            if (raw != 5) throw new Exception($"javascript holds {raw}, so this is a write failure not a read failure");
            var back = JS.Get<Holder>("__memberTestNullableRead");
            if (back?.Count != 5) throw new Exception($"count read back as '{back?.Count}'");
        }

        /// <summary>
        /// The same for a nullable CUSTOM struct, which resolves to a different marshaller than a nullable
        /// number does and so exercises the other half of the unwrap.
        /// </summary>
        [SpawnJSTest]
        public async Task NullableStructMemberRoundTripsTest()
        {
            JS.Set("__memberTestPoint", new Holder { Point = new Point { X = 3, Y = 4 }, Label = "x" });
            var x = JS.Get<double>("__memberTestPoint.point.x");
            if (x != 3) throw new Exception($"point.x in javascript is {x}");
            var back = JS.Get<Holder>("__memberTestPoint");
            if (back?.Point == null) throw new Exception("point read back as null");
            if (back.Point.Value.X != 3 || back.Point.Value.Y != 4)
                throw new Exception($"point read back as ({back.Point.Value.X},{back.Point.Value.Y})");
        }

        /// <summary>
        /// And an absent nullable struct must read back as null rather than as a zeroed struct - a plain
        /// struct has no way to say "absent", but a nullable one does and must use it.
        /// </summary>
        [SpawnJSTest]
        public async Task AbsentNullableStructReadsBackAsNullTest()
        {
            JS.Set("__memberTestNoPoint", new Holder { Label = "x" });
            var back = JS.Get<Holder>("__memberTestNoPoint");
            if (back?.Point != null)
                throw new Exception($"absent point read back as ({back!.Point!.Value.X},{back.Point.Value.Y}) instead of null");
        }

        /// <summary>
        /// A type with a `required` member must still round trip.<br/>
        /// This is load bearing for the WebIDL descriptor types: `required` is enforced by the COMPILER
        /// for object initializers, but the marshaller builds objects reflectively via
        /// Activator.CreateInstance, which never runs that check. If reflective construction refused a
        /// type with required members, marking the descriptors `required` would break every READ of one.
        /// Asserting it here means the decision rests on observed behaviour, not on my reading of the
        /// language rules.
        /// </summary>
        [SpawnJSTest]
        public async Task RequiredMemberTypeRoundTripsTest()
        {
            JS.Set("__memberTestRequired", new RequiredHolder { Format = "rgba8unorm", Offset = 12 });
            var raw = JS.Get<string>("__memberTestRequired.format");
            if (raw != "rgba8unorm") throw new Exception($"javascript holds '{raw}'");
            var back = JS.Get<RequiredHolder>("__memberTestRequired");
            if (back?.Format != "rgba8unorm") throw new Exception($"required member read back as '{back?.Format}'");
            if (back?.Offset != 12) throw new Exception($"neighbouring member read back as '{back?.Offset}'");
        }

        /// <summary>
        /// Reading a nullable VALUE type directly, which is a different path from reading one as a member
        /// of an object.<br/>
        /// NetRun compared the returned value's type against the DECLARED type, and there is no boxed
        /// Nullable&lt;T&gt; at runtime - boxing one with a value produces a boxed T - so every non-null
        /// nullable value type read threw "expected Nullable`1 got Int32". Nothing caught it because the
        /// existing nullable tests all used string?, a reference type, where the two agree.
        /// </summary>
        [SpawnJSTest]
        public async Task NullableValueTypeReadsDirectlyTest()
        {
            JS.Set("__nullableDirect", 7);
            var value = JS.Get<int?>("__nullableDirect");
            if (value != 7) throw new Exception($"int? read back as {value}");

            JS.Set("__nullableDirectNull", (object?)null);
            var missing = JS.Get<int?>("__nullableDirectNull");
            if (missing != null) throw new Exception($"null read back as {missing}");
        }

        /// <summary>
        /// A marshalled object must carry NO enumerable Symbol keys.<br/>
        /// The .Net runtime tags every Javascript object it PROXIES with one, and a record-typed web API
        /// enumerates every own key and converts each to a string - which a Symbol cannot be - so a tagged
        /// descriptor makes WebGPU throw "Cannot convert a Symbol value to a string". The dictionary path
        /// used to rebuild objects Javascript side to strip the tag; the marshallers now write through the
        /// slot table instead, so nothing is ever proxied and there is no tag to strip.<br/>
        /// This covers the PLAIN object path, which never had that rebuild and so was never protected.
        /// </summary>
        [SpawnJSTest]
        public async Task MarshalledObjectHasNoEnumerableSymbolKeysTest()
        {
            JS.Set("__symbolProbe", new Holder { Label = "x", Count = 1, Shape = new DerivedShape { Kind = "d", Extra = 2 } });

            var onHolder = JS.Call<int>("eval",
                "Object.getOwnPropertySymbols(globalThis.__symbolProbe)" +
                ".filter(s => Object.getOwnPropertyDescriptor(globalThis.__symbolProbe, s).enumerable).length");
            if (onHolder != 0)
                throw new Exception($"the marshalled object carries {onHolder} enumerable Symbol key(s)");

            // and the NESTED object too - a nested descriptor is exactly what a web API walks into
            var onNested = JS.Call<int>("eval",
                "Object.getOwnPropertySymbols(globalThis.__symbolProbe.shape)" +
                ".filter(s => Object.getOwnPropertyDescriptor(globalThis.__symbolProbe.shape, s).enumerable).length");
            if (onNested != 0)
                throw new Exception($"the nested object carries {onNested} enumerable Symbol key(s)");
        }

        /// <summary>
        /// The fast path itself: a sealed typed member round trips. Runs after the cases above so that the
        /// cached marshaller has already been used at least once by the time this asserts on it.
        /// </summary>
        [SpawnJSTest]
        public async Task SealedTypedMemberRoundTripsTest()
        {
            JS.Set("__memberTestSealed", new Holder { Label = "sealed value", Count = 1 });
            var label = JS.Get<string>("__memberTestSealed.label");
            if (label != "sealed value") throw new Exception($"label read back as '{label}'");
            var back = JS.Get<Holder>("__memberTestSealed");
            if (back?.Label != "sealed value") throw new Exception($"round trip gave '{back?.Label}'");
            if (back?.Count != 1) throw new Exception($"round trip count gave '{back?.Count}'");
        }
    }
}
