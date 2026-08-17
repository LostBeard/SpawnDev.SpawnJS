using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace TestsShared
{
    /// <summary>
    /// The enum member names Javascript actually uses are not the C# names, which is the whole reason
    /// EnumString exists. [JsonPropertyName] carries that mapping as metadata - EnumString reads it
    /// reflectively, no serializer is involved.
    /// </summary>
    public enum TestChannelMode
    {
        [JsonPropertyName("max")]
        Max,
        [JsonPropertyName("clamped-max")]
        ClampedMax,
        [JsonPropertyName("explicit")]
        Explicit,
    }

    /// <summary>
    /// EnumString, EpochDateTime and DateTime marshalling, plus the generic Promise.
    /// </summary>
    public class MarshallerTests(SpawnJSRuntime JS)
    {
        const string Key = "_spawnjs_marshaller_test";

        T RoundTrip<T>(object? value)
        {
            JS.Set(Key, value);
            try { return JS.Get<T>(Key); }
            finally { JS.Delete(Key); }
        }

        string TypeOfTestKey()
        {
            using var handle = new SpawnJSHandle(JS.JSHandle, Key, true);
            return handle.JSType;
        }

        // ---------------------------------------------------------------- EnumString

        /// <summary>
        /// An EnumString must reach Javascript as the plain string the enum member maps to - not as an
        /// object, and not as the C# member name.
        /// </summary>
        [SpawnJSTest]
        public async Task EnumStringWritesTheJavascriptNameTest()
        {
            JS.Set(Key, new EnumString<TestChannelMode>(TestChannelMode.ClampedMax));
            try
            {
                if (TypeOfTestKey() != "string") throw new Exception($"EnumString reached Javascript as a {TypeOfTestKey()}, expected string");
                var raw = JS.Get<string>(Key);
                if (raw != "clamped-max") throw new Exception($"EnumString wrote '{raw}', expected 'clamped-max' (the JsonPropertyName, not the C# name)");
            }
            finally { JS.Delete(Key); }
        }

        /// <summary>
        /// Reading back must map the Javascript string to the enum member.
        /// </summary>
        [SpawnJSTest]
        public async Task EnumStringReadsBackToTheEnumTest()
        {
            var value = RoundTrip<EnumString<TestChannelMode>>("clamped-max");
            if (value == null) throw new Exception("EnumString came back null");
            if (!value.IsDefined) throw new Exception("EnumString did not resolve 'clamped-max' to a defined member");
            if (value.Enum != TestChannelMode.ClampedMax) throw new Exception($"EnumString resolved to {value.Enum}, expected ClampedMax");
            if (value.String != "clamped-max") throw new Exception($"EnumString kept string '{value.String}'");
        }

        /// <summary>
        /// A value the enum does not know is carried, not thrown away. That is what lets a wrapper survive
        /// a browser returning a newer enum value than the C# enum was written against.
        /// </summary>
        [SpawnJSTest]
        public async Task EnumStringCarriesUnknownValuesTest()
        {
            var value = RoundTrip<EnumString<TestChannelMode>>("a-value-from-the-future");
            if (value == null) throw new Exception("EnumString came back null for an unknown value");
            if (value.IsDefined) throw new Exception("An unknown Javascript value was reported as a defined enum member");
            if (value.Enum != null) throw new Exception($"An unknown value produced enum {value.Enum}, expected null");
            if (value.String != "a-value-from-the-future") throw new Exception($"The unknown value was not preserved, got '{value.String}'");
        }

        /// <summary>
        /// Round trip through Javascript for every member, so the mapping is proven in both directions.
        /// </summary>
        [SpawnJSTest]
        public async Task EnumStringRoundTripsEveryMemberTest()
        {
            foreach (var member in Enum.GetValues<TestChannelMode>())
            {
                var back = RoundTrip<EnumString<TestChannelMode>>(new EnumString<TestChannelMode>(member));
                if (back?.Enum != member) throw new Exception($"{member} round tripped to {back?.Enum?.ToString() ?? "null"}");
            }
        }

        [SpawnJSTest]
        public async Task EnumStringNullRoundTripsAsNullTest()
        {
            var value = RoundTrip<EnumString<TestChannelMode>>(null);
            if (value != null) throw new Exception($"null produced EnumString '{value.String}'");
        }

        // ---------------------------------------------------------------- Nullable numerics (long?/ushort?/...)

        /// <summary>
        /// Reading a genuine Javascript number back as <c>long?</c>. This is the exact shape that regressed in
        /// the BlazorJS-&gt;SpawnJS move: WebGPU adapter/device limits (maxStorageBufferBindingSize, maxBufferSize)
        /// are exposed as <c>long?</c>, and before <c>INumberNullableMarshaller</c> a <c>long?</c> read found no
        /// numeric marshaller and came back <c>null</c> - so the WebGPU device fell back to the 128 MiB default
        /// storage-buffer limit. <c>int?</c>/<c>double?</c> had their own marshallers and worked; <c>long?</c>
        /// did not.
        /// </summary>
        [SpawnJSTest]
        public async Task NullableLongReadsJavascriptNumberTest()
        {
            JS.Set(Key, 2147483644d);   // a real JS number, as a WebGPU limit is exposed
            try
            {
                var v = JS.Get<long?>(Key);
                if (v is null) throw new Exception("long? read a JS number as null - INumberNullableMarshaller missing?");
                if (v.Value != 2147483644L) throw new Exception($"long? read {v.Value}, expected 2147483644");
            }
            finally { JS.Delete(Key); }
        }

        /// <summary>A <c>long?</c> must round trip through Javascript in both directions.</summary>
        [SpawnJSTest]
        public async Task NullableLongRoundTripsTest()
        {
            var v = RoundTrip<long?>(2147483644L);
            if (v != 2147483644L) throw new Exception($"long? round tripped to {v?.ToString() ?? "null"}, expected 2147483644");
        }

        /// <summary>A null <c>long?</c> stays null across the round trip (not 0).</summary>
        [SpawnJSTest]
        public async Task NullableLongNullRoundTripsAsNullTest()
        {
            var v = RoundTrip<long?>(null);
            if (v != null) throw new Exception($"null produced long? {v}");
        }

        /// <summary>
        /// The marshaller is generic over <see cref="System.Numerics.INumber{TSelf}"/>, so every other nullable
        /// numeric (ushort?/uint?/ulong?/short?/...) reads a JS number too - not just <c>long?</c>. Proves the
        /// fix is the general one, and that it does not collide with the specific <c>int?</c>/<c>double?</c>
        /// marshallers (which still win for those types).
        /// </summary>
        [SpawnJSTest]
        public async Task OtherNullableNumericsReadJavascriptNumberTest()
        {
            JS.Set(Key, 40000d);
            try
            {
                if (JS.Get<ushort?>(Key) != (ushort)40000) throw new Exception($"ushort? read {JS.Get<ushort?>(Key)?.ToString() ?? "null"}");
                if (JS.Get<uint?>(Key) != 40000u) throw new Exception($"uint? read {JS.Get<uint?>(Key)?.ToString() ?? "null"}");
                if (JS.Get<ulong?>(Key) != 40000UL) throw new Exception($"ulong? read {JS.Get<ulong?>(Key)?.ToString() ?? "null"}");
                if (JS.Get<int?>(Key) != 40000) throw new Exception($"int? (specific marshaller) read {JS.Get<int?>(Key)?.ToString() ?? "null"}");
                if (JS.Get<double?>(Key) != 40000d) throw new Exception($"double? (specific marshaller) read {JS.Get<double?>(Key)?.ToString() ?? "null"}");
            }
            finally { JS.Delete(Key); }
        }

        // ---------------------------------------------------------------- EpochDateTime

        /// <summary>
        /// EpochDateTime exists because many Javascript APIs express a moment as milliseconds since the
        /// epoch, so it has to reach Javascript as a number.
        /// </summary>
        [SpawnJSTest]
        public async Task EpochDateTimeWritesANumberTest()
        {
            var when = new DateTime(2026, 7, 20, 12, 30, 15, DateTimeKind.Utc).ToLocalTime();
            var epoch = new EpochDateTime(when);
            JS.Set(Key, epoch);
            try
            {
                if (TypeOfTestKey() != "number") throw new Exception($"EpochDateTime reached Javascript as a {TypeOfTestKey()}, expected number");
                var ms = JS.Get<double>(Key);
                if ((long)ms != epoch.ValueEpoch) throw new Exception($"EpochDateTime wrote {ms}, expected {epoch.ValueEpoch}");
            }
            finally { JS.Delete(Key); }
        }

        /// <summary>
        /// Round trip must preserve the instant to the millisecond, which is all the epoch representation
        /// can carry.
        /// </summary>
        [SpawnJSTest]
        public async Task EpochDateTimeRoundTripsTest()
        {
            var when = new DateTime(2026, 7, 20, 12, 30, 15, DateTimeKind.Utc).ToLocalTime();
            var back = RoundTrip<EpochDateTime>(new EpochDateTime(when));
            if (back == null) throw new Exception("EpochDateTime came back null");
            var drift = Math.Abs((back.Value - when).TotalMilliseconds);
            if (drift > 1) throw new Exception($"EpochDateTime round trip drifted {drift}ms (got {back.Value:O}, expected {when:O})");
        }

        [SpawnJSTest]
        public async Task EpochDateTimeNullRoundTripsAsNullTest()
        {
            var value = RoundTrip<EpochDateTime>(null);
            if (value != null) throw new Exception($"null produced EpochDateTime {value.Value:O}");
        }

        // ---------------------------------------------------------------- DateTime

        /// <summary>
        /// A plain DateTime writes the round trip ISO string, matching what SpawnDev.BlazorJS produced.
        /// Parity by default: code moving over must behave the same.
        /// </summary>
        [SpawnJSTest]
        public async Task DateTimeRoundTripsAsIsoStringTest()
        {
            var when = new DateTime(2026, 7, 20, 12, 30, 15, DateTimeKind.Utc);
            JS.Set(Key, when);
            try
            {
                if (TypeOfTestKey() != "string") throw new Exception($"DateTime reached Javascript as a {TypeOfTestKey()}, expected string");
                var back = JS.Get<DateTime>(Key);
                if (Math.Abs((back.ToUniversalTime() - when).TotalMilliseconds) > 1)
                    throw new Exception($"DateTime round tripped to {back:O}, expected {when:O}");
            }
            finally { JS.Delete(Key); }
        }

        /// <summary>
        /// Javascript is inconsistent about how APIs report a time, so reading must also accept a real
        /// Date object and a raw epoch number - the caller asked for a DateTime either way.
        /// </summary>
        [SpawnJSTest]
        public async Task DateTimeReadsJavascriptDateAndNumberTest()
        {
            var when = new DateTime(2026, 7, 20, 12, 30, 15, DateTimeKind.Utc);
            var ms = (double)new EpochDateTime(when.ToLocalTime()).ValueEpoch;

            using var jsDate = JS.New("Date", ms);
            JS.Set(Key, jsDate);
            try
            {
                var fromDate = JS.Get<DateTime>(Key);
                if (Math.Abs((fromDate.ToUniversalTime() - when).TotalMilliseconds) > 1)
                    throw new Exception($"A Javascript Date read as {fromDate:O}, expected {when:O}");
            }
            finally { JS.Delete(Key); }

            var fromNumber = RoundTrip<DateTime>(ms);
            if (Math.Abs((fromNumber.ToUniversalTime() - when).TotalMilliseconds) > 1)
                throw new Exception($"An epoch number read as {fromNumber:O}, expected {when:O}");
        }

        // ---------------------------------------------------------------- Promise<T>

        /// <summary>
        /// Baseline: the NON generic Promise over a completed Task. Runs first so a failure in the generic
        /// tests can be attributed - if this one fails too, the generic form is not the cause.
        /// </summary>
        [SpawnJSTest(Timeout = 5000)]
        public async Task PromiseFromCompletedTaskResolvesTest()
        {
            var promise = new Promise(Task.CompletedTask);
            await promise.ThenAsync();
        }

        /// <summary>
        /// Same baseline for a Task that completes later, since an already completed Task can continue
        /// inline and take a different path through the constructor.
        /// </summary>
        [SpawnJSTest(Timeout = 5000)]
        public async Task PromiseFromDelayedTaskResolvesTest()
        {
            var promise = new Promise(Task.Delay(10));
            await promise.ThenAsync();
        }

        /// <summary>
        /// Splits the value-resolving path in half. This promise is created and resolved entirely by
        /// Javascript, so only the reading side (then + typed callback) is under test. If this passes but
        /// the .Net Task version does not, the fault is in how the constructor resolves, not in how the
        /// result is read back.
        /// </summary>
        [SpawnJSTest(Timeout = 5000)]
        public async Task PromiseResolvedByJavascriptReadsValueTest()
        {
            using var promise = JS.Call<Promise>("Promise.resolve", 42)!;
            var result = await promise.ThenAsync<int>();
            if (result != 42) throw new Exception($"A Javascript resolved promise gave {result}, expected 42");
        }

        /// <summary>
        /// Baseline for a promise that resolves WITH A VALUE, taken on the non generic Promise using the
        /// base's own typed ThenAsync. This separates "resolving with a value is broken" from "the generic
        /// subclass is broken" - the two failing tests above could be either.
        /// </summary>
        [SpawnJSTest(Timeout = 5000)]
        public async Task PromiseResolvesWithAValueTest()
        {
            var promise = new Promise(Task.FromResult(42));
            var result = await promise.ThenAsync<int>();
            if (result != 42) throw new Exception($"Promise resolved to {result}, expected 42");
        }

        /// <summary>
        /// The generic Promise must resolve to its typed result. It derives from the non generic Promise
        /// and relies on the base constructor noticing the Task is generic and passing the result through,
        /// so this covers that inheritance rather than a copied implementation.
        /// </summary>
        [SpawnJSTest(Timeout = 5000)]
        public async Task GenericPromiseResolvesTypedResultTest()
        {
            var promise = new Promise<int>(Task.FromResult(41 + 1));
            var result = await promise.ThenAsync();
            if (result != 42) throw new Exception($"Promise<int> resolved to {result}, expected 42");
        }

        /// <summary>
        /// A generic Promise really is a Promise, which is what lets it be passed anywhere the non generic
        /// one is accepted.
        /// </summary>
        [SpawnJSTest]
        public async Task GenericPromiseIsAPromiseTest()
        {
            using var promise = new Promise<string>(Task.FromResult("ok"));
            if (promise is not Promise) throw new Exception("Promise<T> does not derive from Promise");
            var constructorNames = promise.JSRef!.ConstructorNames();
            if (System.Array.IndexOf(constructorNames, "Promise") < 0)
                throw new Exception($"Promise<T> is backed by [{string.Join(",", constructorNames)}], expected a Javascript Promise");
        }

        /// <summary>
        /// A rejected promise has to surface as a faulted Task rather than hanging.
        /// </summary>
        [SpawnJSTest(Timeout = 5000)]
        public async Task GenericPromiseFromFuncResolvesTest()
        {
            var promise = new Promise<string>(() => Task.FromResult("from func"));
            var result = await promise.ThenAsync();
            if (result != "from func") throw new Exception($"Promise<string> resolved to '{result}'");
        }
    }
}
