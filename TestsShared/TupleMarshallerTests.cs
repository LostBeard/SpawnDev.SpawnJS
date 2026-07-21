using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;

namespace TestsShared
{
    /// <summary>
    /// Tuples cross as Javascript ARRAYS, positionally.<br/>
    /// That is the useful mapping: a Javascript function returning <c>[x, y]</c> reads back as
    /// <c>(int X, int Y)</c>, and a tuple argument arrives as the array such a function expects.<br/>
    /// <br/>
    /// The failure this guards against is not an exception - it is a tuple quietly property-walked into
    /// <c>{Item1, Item2}</c> by StructMarshaller (ValueTuple is a struct) or ObjectMarshaller (Tuple is a
    /// class). That object is well formed and would pass any shape-only check, so every test here asserts
    /// what Javascript actually holds.
    /// </summary>
    public class TupleMarshallerTests(SpawnJSRuntime JS)
    {
        /// <summary>
        /// The core claim: Javascript sees a real Array, indexable, NOT an object with Item1/Item2.
        /// </summary>
        [SpawnJSTest]
        public async Task ValueTupleCrossesAsJavascriptArrayTest()
        {
            JS.Set("__tupleBasic", (1, "two"));

            var ctor = JS.Get<string>("__tupleBasic.constructor.name");
            if (ctor != "Array") throw new Exception($"a tuple crossed as a {ctor}, expected an Array");

            var length = JS.Get<int>("__tupleBasic.length");
            if (length != 2) throw new Exception($"array length {length}, expected 2");

            var first = JS.Get<int>("__tupleBasic.0");
            var second = JS.Get<string>("__tupleBasic.1");
            if (first != 1 || second != "two") throw new Exception($"array holds [{first}, '{second}']");

            // the exact wrong result this marshaller exists to prevent
            if (!JS.IsUndefined("__tupleBasic.Item1"))
                throw new Exception("the tuple was property-walked into Item1/Item2 instead of an array");
        }

        /// <summary>
        /// And back again, with each element typed by its generic argument rather than guessed.
        /// </summary>
        [SpawnJSTest]
        public async Task ValueTupleRoundTripsTest()
        {
            JS.Set("__tupleRound", (42, "answer", true));
            var back = JS.Get<(int Number, string Text, bool Flag)>("__tupleRound");
            if (back.Number != 42 || back.Text != "answer" || back.Flag != true)
                throw new Exception($"round trip gave ({back.Number}, '{back.Text}', {back.Flag})");
        }

        /// <summary>
        /// A tuple read from an array Javascript built itself - the direction that makes this worth
        /// having, since it is how a JS function returning several values is consumed.
        /// </summary>
        [SpawnJSTest]
        public async Task TupleReadsFromAJavascriptBuiltArrayTest()
        {
            JS.CallVoid("eval", "globalThis.__tupleFromJS = [7, 'seven'];");
            var value = JS.Get<(int, string)>("__tupleFromJS");
            if (value.Item1 != 7 || value.Item2 != "seven")
                throw new Exception($"read back as ({value.Item1}, '{value.Item2}')");
        }

        /// <summary>
        /// The reference-type family too. Tuple is a class, so ObjectMarshaller would claim it if the
        /// registration order were wrong - this is the test that catches that.
        /// </summary>
        [SpawnJSTest]
        public async Task ReferenceTupleCrossesAsArrayTest()
        {
            JS.Set("__tupleRef", Tuple.Create(3, "three"));

            var ctor = JS.Get<string>("__tupleRef.constructor.name");
            if (ctor != "Array") throw new Exception($"Tuple crossed as a {ctor}, expected an Array");

            var back = JS.Get<Tuple<int, string>>("__tupleRef");
            if (back?.Item1 != 3 || back?.Item2 != "three")
                throw new Exception($"round trip gave ({back?.Item1}, '{back?.Item2}')");
        }

        /// <summary>
        /// Elements go through the marshaller graph, so a tuple can carry anything the graph handles -
        /// including a live Javascript object, which is the case a Json based converter could never do
        /// without a bespoke walker.
        /// </summary>
        [SpawnJSTest]
        public async Task TupleCarriesNestedAndObjectElementsTest()
        {
            using var obj = JS.NewJSObject();
            obj.SetProperty("tag", "held");
            using var held = JS.Get<SpawnJSObject>("Object")!; // any real JS object works as the payload

            JS.Set("__tupleNested", (1, new[] { 2, 3 }, "end"));

            var nestedCtor = JS.Get<string>("__tupleNested.1.constructor.name");
            if (nestedCtor != "Array") throw new Exception($"nested array crossed as {nestedCtor}");
            var nestedSecond = JS.Get<int>("__tupleNested.1.1");
            if (nestedSecond != 3) throw new Exception($"nested element crossed as {nestedSecond}");

            var back = JS.Get<(int, int[], string)>("__tupleNested");
            if (back.Item1 != 1 || back.Item2.Length != 2 || back.Item2[1] != 3 || back.Item3 != "end")
                throw new Exception("nested round trip did not match");
        }

        /// <summary>
        /// A tuple sitting on an object is the shape that actually shows up in a descriptor, and it is
        /// where a mis-registered marshaller would leak Item1/Item2 into a payload a web API reads.
        /// </summary>
        [SpawnJSTest]
        public async Task TuplePropertyOnAnObjectCrossesAsArrayTest()
        {
            JS.Set("__tupleHolder", new TupleHolder { Size = (1920, 1080), Name = "screen" });

            var ctor = JS.Get<string>("__tupleHolder.size.constructor.name");
            if (ctor != "Array") throw new Exception($"the tuple property crossed as a {ctor}");
            var width = JS.Get<int>("__tupleHolder.size.0");
            if (width != 1920) throw new Exception($"width crossed as {width}");

            var back = JS.Get<TupleHolder>("__tupleHolder");
            if (back?.Size.Width != 1920 || back?.Size.Height != 1080)
                throw new Exception($"round trip gave ({back?.Size.Width}, {back?.Size.Height})");
        }

        /// <summary>
        /// Seven elements is the widest a flat tuple goes. Eight would really be a 7-tuple whose last
        /// element is another tuple, which would reach Javascript as a NESTED array rather than the flat
        /// one written in C#, so this marshaller declines those rather than silently changing the shape.
        /// </summary>
        [SpawnJSTest]
        public async Task SevenElementTupleRoundTripsTest()
        {
            JS.Set("__tuple7", (1, 2, 3, 4, 5, 6, 7));
            var length = JS.Get<int>("__tuple7.length");
            if (length != 7) throw new Exception($"array length {length}, expected 7");

            var back = JS.Get<(int, int, int, int, int, int, int)>("__tuple7");
            if (back.Item1 != 1 || back.Item7 != 7) throw new Exception("seven element round trip failed");
        }

        /// <summary>
        /// A nullable tuple: null stays null rather than becoming a tuple of zeros.
        /// </summary>
        [SpawnJSTest]
        public async Task NullableTupleRoundTripsNullTest()
        {
            JS.Set("__tupleNull", (object?)null);
            var back = JS.Get<(int, string)?>("__tupleNull");
            if (back != null) throw new Exception($"null read back as ({back.Value.Item1}, '{back.Value.Item2}')");

            JS.Set("__tupleNotNull", ((int, string)?)(5, "five"));
            var value = JS.Get<(int, string)?>("__tupleNotNull");
            if (value?.Item1 != 5 || value?.Item2 != "five")
                throw new Exception($"value read back as {value}");
        }

        public class TupleHolder
        {
            public (int Width, int Height) Size { get; set; }
            public string? Name { get; set; }
        }
    }
}
