using SpawnDev.SpawnJS;

namespace TestsShared
{
    /// <summary>
    /// The flat call buffer. Arguments go over and results come back in one shared Javascript array, with
    /// each call owning a region of it and the top unwinding when the call completes.<br/>
    /// The failure mode here is silent: a call whose region is too small has its result overwritten by a
    /// nested call rather than throwing, so these tests target region ownership rather than the happy path.
    /// </summary>
    public class CallBufferTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// A call with NO arguments must still reserve a slot, because the result comes back in the first
        /// slot of the call's own region. Without a reservation the buffer top still points AT the result
        /// slot, so anything that calls while the result is being marshalled writes over it.<br/>
        /// newEasyPromise is exactly that shape - zero arguments, object result - so it is the case that
        /// would break first.
        /// </summary>
        [SpawnJSTest]
        public async Task ZeroArgumentCallReturnsItsResultTest()
        {
            for (var i = 0; i < 20; i++)
            {
                using var promise = JS.NewEasyPromise();
                if (promise?.JSObject == null)
                    throw new Exception($"A zero argument call returned nothing on iteration {i}");
                var chain = promise.ConstructorNames;
                if (System.Array.IndexOf(chain, "Promise") < 0)
                    throw new Exception($"A zero argument call returned [{string.Join(",", chain)}], expected a Promise - its result slot was overwritten");
            }
        }

        /// <summary>
        /// Results must survive being marshalled into a type whose marshaller itself calls into Javascript.
        /// That nested call runs while the outer result is still being read, which is when a region that is
        /// too small gets trampled.
        /// </summary>
        [SpawnJSTest]
        public async Task NestedCallsDoNotTrampleAnOuterResultTest()
        {
            const string probe = "_spawnjs_callbuffer_probe";
            JS.Set(probe, JS.New("Object"));
            JS.Set($"{probe}.inner", JS.New("Object"));
            JS.Set($"{probe}.tag", "outer-intact");
            JS.Set($"{probe}.number", 1);

            for (var i = 0; i < 20; i++)
            {
                // reading an object forces a marshaller that itself goes back into Javascript
                using var inner = JS.Get<SpawnJSObject>($"{probe}.inner");
                var tag = JS.Get<string>($"{probe}.tag");
                if (tag != "outer-intact")
                    throw new Exception($"Outer value read back as '{tag}' on iteration {i}");
            }
        }

        /// <summary>
        /// The two directions must not share a region.<br/>
        /// This is the case that breaks if inbound (JS -> .Net) writes into the OUTBOUND buffer: .Net is
        /// mid-call with a live region when Javascript calls back into .Net, and the inbound arguments
        /// land inside the outer call's region. Both directions have their own buffer and their own top
        /// for exactly this reason, so the interleaving below has to come out clean.
        /// <br/>
        /// The shape is deliberately three deep: .Net calls out to Javascript, Javascript calls back in to
        /// .Net, and that inbound handler calls out to Javascript again while the first outbound call is
        /// still on the stack.
        /// </summary>
        [SpawnJSTest]
        public async Task InboundCallDuringAnOutboundCallDoesNotTrampleItTest()
        {
            const string probe = "_spawnjs_interleave_probe";
            JS.Set(probe, JS.New("Object"));
            JS.Set($"{probe}.tag", "outer-intact");
            JS.Set($"{probe}.number", 11);

            // the inbound handler reads Javascript itself, so an outbound call runs INSIDE the inbound one
            using var reentrant = Callback.Create<int, string>(n =>
            {
                var inner = JS.Get<string>($"{probe}.tag");
                var innerNumber = JS.Get<int>($"{probe}.number");
                return $"{inner}:{innerNumber}:{n}";
            });
            JS.Set("__interleaveCallback", reentrant);

            // a Javascript function that calls back into .Net while .Net is waiting on it
            JS.CallVoid("eval",
                "globalThis.__interleaveOuter = function(a, b) { " +
                "  var fromNet = globalThis.__interleaveCallback(a); " +
                "  return fromNet + '|' + b; " +
                "};");

            for (var i = 0; i < 20; i++)
            {
                var result = JS.Call<string>("__interleaveOuter", i, "tail");
                var expected = $"outer-intact:11:{i}|tail";
                if (result != expected)
                    throw new Exception($"Interleaved call returned '{result}', expected '{expected}' on iteration {i}");

                // and the outer values must be untouched by all that traffic
                var tag = JS.Get<string>($"{probe}.tag");
                if (tag != "outer-intact")
                    throw new Exception($"Outer value read back as '{tag}' after interleaving on iteration {i}");
            }
        }

        /// <summary>
        /// The buffer is a stack, so a completed call must leave the top exactly where it found it. Drift
        /// in either direction is a leak: upward grows the buffer forever, downward hands a later call a
        /// region an earlier one is still using.
        /// </summary>
        [SpawnJSTest]
        public async Task BufferTopReturnsToWhereItStartedTest()
        {
            // a mix of arities, including zero, since zero is the case that has no arguments to reserve
            const string probe = "_spawnjs_callbuffer_probe";
            JS.Set(probe, JS.New("Object"));
            JS.Set($"{probe}.tag", "tag");
            JS.Set($"{probe}.number", 1);

            _ = JS.NewEasyPromise();
            _ = JS.Get<int>($"{probe}.number");
            JS.Set($"{probe}.number", 7);
            _ = JS.Get<string>($"{probe}.tag");

            // If the top drifted, a later call reads or writes the wrong slot. Reading a known value back
            // is what proves the regions still line up.
            var number = JS.Get<int>($"{probe}.number");
            if (number != 7) throw new Exception($"Read back {number} after writing 7; buffer regions have drifted");
        }
    }
}
