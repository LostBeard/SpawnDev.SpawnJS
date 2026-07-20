using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Marshallers;

// disambiguate the Javascript wrappers from their System counterparts
using JSArray = SpawnDev.SpawnJS.JSObjects.Array;
using JSTypeError = SpawnDev.SpawnJS.JSObjects.TypeError;

namespace TestsShared
{
    /// <summary>
    /// Union marshalling tests.<br/>
    /// Every test here drives a real Javascript value through the real marshaller graph. The .Net ➡️ JS
    /// direction must be indistinguishable from writing the unwrapped value, and the JS ➡️ .Net direction
    /// must land in the arm that matches the live Javascript value - which is decided by inspecting the
    /// value itself, not by any type tag we sent along with it.
    /// </summary>
    public class UnionTests(SpawnJSRuntime JS)
    {
        const string Key = "_spawnjs_union_test";

        /// <summary>
        /// Writes value to a global, reads it back as T, then clears the global.
        /// Round tripping through globalThis is what makes this a real marshal in both directions rather
        /// than a .Net-side conversion.
        /// </summary>
        T RoundTrip<T>(object? value)
        {
            JS.Set(Key, value);
            try
            {
                return JS.Get<T>(Key);
            }
            finally
            {
                JS.Delete(Key);
            }
        }

        /// <summary>
        /// The Javascript typeof of whatever currently sits at the test key
        /// </summary>
        string TypeOfTestKey()
        {
            using var handle = new SpawnJSHandle(JS.JSHandle, Key, true);
            return handle.JSType;
        }

        // ---------------------------------------------------------------- .Net -> JS

        /// <summary>
        /// A union must write exactly what the value it holds would have written on its own. If it wrapped
        /// or tagged the value in any way, the plain read below would not see a bare string.
        /// </summary>
        [SpawnJSTest]
        public async Task UnionWritesUnwrappedValueTest()
        {
            Union<string, int> union = "hello";
            var readBack = RoundTrip<string>(union);
            if (readBack != "hello") throw new Exception($"Union<string,int> holding a string wrote '{readBack}', expected 'hello'");

            Union<string, int> numberUnion = 42;
            var readBackNumber = RoundTrip<int>(numberUnion);
            if (readBackNumber != 42) throw new Exception($"Union<string,int> holding an int wrote {readBackNumber}, expected 42");
        }

        /// <summary>
        /// The typeof seen by Javascript must be the value's own type, not "object".
        /// </summary>
        [SpawnJSTest]
        public async Task UnionWritesNativeJSTypeTest()
        {
            Union<string, int> union = "hello";
            JS.Set(Key, union);
            try
            {
                var jsType = TypeOfTestKey();
                if (jsType != "string") throw new Exception($"Union holding a string produced a Javascript {jsType}, expected string");
            }
            finally
            {
                JS.Delete(Key);
            }

            Union<string, int> numberUnion = 7;
            JS.Set(Key, numberUnion);
            try
            {
                var jsType = TypeOfTestKey();
                if (jsType != "number") throw new Exception($"Union holding an int produced a Javascript {jsType}, expected number");
            }
            finally
            {
                JS.Delete(Key);
            }
        }

        // ---------------------------------------------------------------- JS -> .Net, primitives

        /// <summary>
        /// Arm selection for the Javascript primitive types. Each of these round trips as its own typeof,
        /// so the marshaller has to read the live value to know which arm to build.
        /// </summary>
        [SpawnJSTest]
        public async Task UnionSelectsStringArmTest()
        {
            var union = RoundTrip<Union<string, int>>("a string");
            if (union == null) throw new Exception("Union came back null");
            if (union.ValueType != typeof(string)) throw new Exception($"Selected arm {union.ValueType.Name}, expected String");
            if ((string)union != "a string") throw new Exception($"Value was '{(string)union}', expected 'a string'");
        }

        [SpawnJSTest]
        public async Task UnionSelectsNumberArmTest()
        {
            var union = RoundTrip<Union<string, int>>(42);
            if (union == null) throw new Exception("Union came back null");
            if (union.ValueType != typeof(int)) throw new Exception($"Selected arm {union.ValueType.Name}, expected Int32");
            if ((int)union != 42) throw new Exception($"Value was {(int)union}, expected 42");
        }

        [SpawnJSTest]
        public async Task UnionSelectsBooleanArmTest()
        {
            var union = RoundTrip<Union<string, bool>>(true);
            if (union == null) throw new Exception("Union came back null");
            if (union.ValueType != typeof(bool)) throw new Exception($"Selected arm {union.ValueType.Name}, expected Boolean");
            if (!(bool)union) throw new Exception("Value was false, expected true");
        }

        /// <summary>
        /// The same union type must be able to come back as either arm across successive reads. This is
        /// what proves selection is driven by the value and not cached per union type.
        /// </summary>
        [SpawnJSTest]
        public async Task UnionSelectionIsPerValueNotPerTypeTest()
        {
            var asString = RoundTrip<Union<string, int>>("text");
            var asNumber = RoundTrip<Union<string, int>>(99);
            var asStringAgain = RoundTrip<Union<string, int>>("text again");

            if (asString.ValueType != typeof(string)) throw new Exception($"First read selected {asString.ValueType.Name}, expected String");
            if (asNumber.ValueType != typeof(int)) throw new Exception($"Second read selected {asNumber.ValueType.Name}, expected Int32");
            if (asStringAgain.ValueType != typeof(string)) throw new Exception($"Third read selected {asStringAgain.ValueType.Name}, expected String");
            if ((int)asNumber != 99) throw new Exception($"Second read value was {(int)asNumber}, expected 99");
        }

        /// <summary>
        /// null and undefined must both read as a null union rather than throwing or picking an arm.
        /// typeof null is "object" in Javascript, so null is only distinguishable by its class name -
        /// that path is easy to get wrong and lands in the object branch if it is.
        /// </summary>
        [SpawnJSTest]
        public async Task UnionReadsNullAndUndefinedAsNullTest()
        {
            var fromNull = RoundTrip<Union<string, int>>(null);
            if (fromNull != null) throw new Exception($"A Javascript null produced a union holding {fromNull.ValueType.Name}, expected null");

            JS.Delete(Key);
            var fromUndefined = JS.Get<Union<string, int>>(Key);
            if (fromUndefined != null) throw new Exception($"An undefined property produced a union holding {fromUndefined.ValueType.Name}, expected null");
        }

        // ---------------------------------------------------------------- JS -> .Net, objects

        /// <summary>
        /// Arm selection by the Javascript value's own class name, for a value Javascript created itself.
        /// An Error is neither a string nor an Array, so only the class name can resolve this.
        /// </summary>
        [SpawnJSTest]
        public async Task UnionSelectsObjectArmByClassNameTest()
        {
            using var error = JS.New("Error", "boom");
            JS.Set(Key, error);
            try
            {
                var union = JS.Get<Union<string, Error>>(Key);
                if (union == null) throw new Exception("Union came back null for a Javascript Error");
                if (union.ValueType != typeof(Error)) throw new Exception($"Selected arm {union.ValueType.Name}, expected Error");
                using var value = (Error)union;
                if (value.Message != "boom") throw new Exception($"Error message was '{value.Message}', expected 'boom'");
            }
            finally
            {
                JS.Delete(Key);
            }
        }

        /// <summary>
        /// The class name must beat the single-class-arm fallback: with two class arms present, only a real
        /// class name match can pick the right one.
        /// </summary>
        [SpawnJSTest]
        public async Task UnionSelectsAmongMultipleObjectArmsTest()
        {
            using var array = JS.New("Array");
            JS.Set(Key, array);
            try
            {
                var union = JS.Get<Union<Error, JSArray>>(Key);
                if (union == null) throw new Exception("Union came back null for a Javascript Array");
                if (union.ValueType != typeof(JSArray)) throw new Exception($"Selected arm {union.ValueType.Name}, expected Array");
            }
            finally
            {
                JS.Delete(Key);
            }

            using var error = JS.New("Error", "x");
            JS.Set(Key, error);
            try
            {
                var union = JS.Get<Union<Error, JSArray>>(Key);
                if (union == null) throw new Exception("Union came back null for a Javascript Error");
                if (union.ValueType != typeof(Error)) throw new Exception($"Selected arm {union.ValueType.Name}, expected Error");
            }
            finally
            {
                JS.Delete(Key);
            }
        }

        /// <summary>
        /// A Uint8Array must also be readable into a byte[] arm. The .Net type name can never match the
        /// Javascript class name here, so this only works if the explicit Uint8Array -> byte[] mapping is
        /// in place.
        /// </summary>
        [SpawnJSTest]
        public async Task UnionSelectsByteArrayArmForUint8ArrayTest()
        {
            using var bytes = JS.New("Uint8Array", 3);
            bytes.Set(0, 1);
            bytes.Set(1, 2);
            bytes.Set(2, 3);
            JS.Set(Key, bytes);
            try
            {
                var union = JS.Get<Union<string, byte[]>>(Key);
                if (union == null) throw new Exception("Union came back null for a Uint8Array read as byte[]");
                if (union.ValueType != typeof(byte[])) throw new Exception($"Selected arm {union.ValueType.Name}, expected Byte[]");
                var value = (byte[])union;
                if (value.Length != 3 || value[0] != 1 || value[1] != 2 || value[2] != 3)
                    throw new Exception($"byte[] was [{string.Join(",", value)}], expected [1,2,3]");
            }
            finally
            {
                JS.Delete(Key);
            }
        }

        /// <summary>
        /// A Javascript value whose class matches no arm directly must still resolve through its prototype
        /// chain. A TypeError's chain contains Error, so a union declaring only the base type resolves
        /// without the marshaller needing to know every Error subclass by name.<br/>
        /// This is the case the Blazor converter needed explicit per-type special casing for (it carried a
        /// hardcoded list of every typed array class to get the same effect).
        /// </summary>
        [SpawnJSTest]
        public async Task UnionSelectsBaseTypeArmViaPrototypeChainTest()
        {
            using var typeError = JS.New("TypeError", "wrong type");
            JS.Set(Key, typeError);
            try
            {
                // prove the assumption this test rests on rather than trusting it
                using var handle = new SpawnJSHandle(JS.JSHandle, Key, true);
                var chain = handle.ConstructorNames;
                if (System.Array.IndexOf(chain, "TypeError") < 0)
                    throw new Exception($"Prototype chain of a TypeError was [{string.Join(",", chain)}], expected it to contain TypeError");
                if (System.Array.IndexOf(chain, "Error") < 0)
                    throw new Exception($"Prototype chain of a TypeError was [{string.Join(",", chain)}], expected it to contain Error");
                // Object.prototype.toString reports "Error" for every Error subclass. That is exactly why
                // arm selection cannot use the class name: it would resolve a TypeError to an Error arm and
                // never look further. Assert the trap is really there so this test keeps its teeth.
                if (handle.JSClass != "Error")
                    throw new Exception($"A TypeError reported class name '{handle.JSClass}'; this test assumes Object.prototype.toString flattens it to Error");
                if (chain[0] != "TypeError")
                    throw new Exception($"Most derived chain entry was '{chain[0]}', expected TypeError");

                var union = JS.Get<Union<string, Error>>(Key);
                if (union == null) throw new Exception("Union came back null for a TypeError read as Error");
                if (union.ValueType != typeof(Error)) throw new Exception($"Selected arm {union.ValueType.Name}, expected Error");
                using var value = (Error)union;
                if (value.Message != "wrong type") throw new Exception($"Error message was '{value.Message}', expected 'wrong type'");
            }
            finally
            {
                JS.Delete(Key);
            }
        }

        /// <summary>
        /// When both the exact class and one of its base classes are arms, the most derived one wins.
        /// Walking the prototype chain outward from the value is what gives this ordering - iterating the
        /// arm list instead would make the answer depend on the order the arms were declared.
        /// </summary>
        [SpawnJSTest]
        public async Task UnionPrefersMostDerivedArmTest()
        {
            using var typeError = JS.New("TypeError", "wrong type");
            JS.Set(Key, typeError);
            try
            {
                // base type declared first
                var union = JS.Get<Union<Error, JSTypeError>>(Key);
                if (union == null) throw new Exception("Union came back null");
                if (union.ValueType != typeof(JSTypeError))
                    throw new Exception($"Selected arm {union.ValueType.Name}, expected the more derived TypeError");

                // and derived type declared first, to prove the answer is not just arm order
                var reversed = JS.Get<Union<JSTypeError, Error>>(Key);
                if (reversed.ValueType != typeof(JSTypeError))
                    throw new Exception($"Selected arm {reversed.ValueType.Name}, expected the more derived TypeError");
            }
            finally
            {
                JS.Delete(Key);
            }

            // and a plain Error still lands on Error, not on the derived arm
            using var error = JS.New("Error", "plain");
            JS.Set(Key, error);
            try
            {
                var union = JS.Get<Union<Error, JSTypeError>>(Key);
                if (union.ValueType != typeof(Error))
                    throw new Exception($"A plain Error selected arm {union.ValueType.Name}, expected Error");
            }
            finally
            {
                JS.Delete(Key);
            }
        }

        // ---------------------------------------------------------------- round trip through JS

        /// <summary>
        /// A union written to Javascript and read straight back into the same union type must land in the
        /// arm it started in. This is the shape a wrapper property has: set then get.
        /// </summary>
        [SpawnJSTest]
        public async Task UnionRoundTripsThroughJavascriptTest()
        {
            Union<string, int> outbound = "round trip";
            var inbound = RoundTrip<Union<string, int>>(outbound);
            if (inbound == null) throw new Exception("Union came back null");
            if (inbound.ValueType != typeof(string)) throw new Exception($"Round trip changed the arm to {inbound.ValueType.Name}, expected String");
            if ((string)inbound != "round trip") throw new Exception($"Round trip value was '{(string)inbound}'");

            Union<string, int> outboundNumber = 1234;
            var inboundNumber = RoundTrip<Union<string, int>>(outboundNumber);
            if (inboundNumber.ValueType != typeof(int)) throw new Exception($"Round trip changed the arm to {inboundNumber.ValueType.Name}, expected Int32");
            if ((int)inboundNumber != 1234) throw new Exception($"Round trip value was {(int)inboundNumber}");
        }

        /// <summary>
        /// Arity beyond two arms, since selection walks the arm list.
        /// </summary>
        [SpawnJSTest]
        public async Task UnionHigherAritySelectsCorrectArmTest()
        {
            var asBool = RoundTrip<Union<string, int, bool>>(true);
            if (asBool.ValueType != typeof(bool)) throw new Exception($"Selected {asBool.ValueType.Name}, expected Boolean");

            var asInt = RoundTrip<Union<string, int, bool>>(5);
            if (asInt.ValueType != typeof(int)) throw new Exception($"Selected {asInt.ValueType.Name}, expected Int32");

            var asString = RoundTrip<Union<string, int, bool>>("s");
            if (asString.ValueType != typeof(string)) throw new Exception($"Selected {asString.ValueType.Name}, expected String");

            var fourArm = RoundTrip<Union<bool, string, int, Error>>(11);
            if (fourArm.ValueType != typeof(int)) throw new Exception($"Selected {fourArm.ValueType.Name}, expected Int32");
        }

        /// <summary>
        /// A Javascript value matching no arm at all must throw, naming the value and the union, rather
        /// than silently producing a default or a wrong arm.
        /// </summary>
        [SpawnJSTest]
        public async Task UnionThrowsOnUnmatchedValueTest()
        {
            JS.Set(Key, "a string");
            try
            {
                // no string arm, and a string is not an object, so nothing can match
                var union = JS.Get<Union<int, bool>>(Key);
                throw new Exception($"Expected a mismatch exception, got a union holding {union?.ValueType.Name ?? "null"}");
            }
            catch (Exception ex) when (ex.Message.Contains("matches no arm"))
            {
                // expected - and the message has to identify what failed
                if (!ex.Message.Contains("string")) throw new Exception($"Mismatch exception did not name the Javascript type: {ex.Message}");
            }
            finally
            {
                JS.Delete(Key);
            }
        }

        /// <summary>
        /// The runtime hands out a marshaller specialised to the concrete Union type. If the specialisation
        /// hook were ignored the generic registry instance would come back instead, which still works but
        /// re-resolves the arms on every single marshal.
        /// </summary>
        [SpawnJSTest]
        public async Task UnionMarshallerIsSpecialisedPerUnionTypeTest()
        {
            var first = JS.GetMarshaller(typeof(Union<string, int>));
            var second = JS.GetMarshaller(typeof(Union<string, int>));
            var other = JS.GetMarshaller(typeof(Union<string, bool>));

            if (first is not UnionMarshaller) throw new Exception($"Union type resolved to {first.GetType().Name}, expected UnionMarshaller");
            if (!ReferenceEquals(first, second)) throw new Exception("The same Union type produced two different marshaller instances");
            if (ReferenceEquals(first, other)) throw new Exception("Two different Union types shared one marshaller instance, so the arms cannot be precomputed");
        }
    }
}
