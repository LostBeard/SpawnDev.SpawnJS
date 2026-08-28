using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Marshaller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    /// <summary>
    /// Marshaller tests: create data, marshal it, verify it.
    /// <para>
    /// The direction each marshaller supports is stated in the registration block of
    /// <see cref="SpawnJSRuntime"/>: <c>&lt;-&gt;</c> marshals both ways, <c>-&gt;</c> is .Net to
    /// Javascript only. Tests follow that list in registration order, and an out-only marshaller is
    /// tested out-only.
    /// </para>
    /// <para>
    /// OUT is verified by asking Javascript what it actually got (typeof / constructor name / a String()
    /// of the value) rather than only by reading it back through the same marshaller - a marshaller
    /// wrong in both directions round trips perfectly. IN is verified by having Javascript create the
    /// value first (see spawnjs-tests.js) so the read is fed a genuine Javascript value.
    /// </para>
    /// </summary>
    public static class MarshallerTests
    {
        static SpawnJSRuntime JS => SpawnJSRuntime.Instance;
        const string K = "__mt";

        #region harness
        static int _pass, _fail, _skip;
        static readonly List<string> _failures = new();

        static void Test(string name, Action body)
        {
            var sw = Stopwatch.StartNew();
            string result = "Success", detail = "";
            try
            {
                body();
                _pass++;
            }
            catch (SkipException ex) { result = "Skipped"; detail = ex.Message; _skip++; }
            catch (Exception ex)
            {
                result = "Error";
                detail = $"{ex.GetType().Name}: {ex.Message}";
                _fail++;
                _failures.Add($"{name}: {detail}");
                JS.LogError($"FAILED {name}\n{ex}");
            }
            finally { JS.Delete(K); }
            sw.Stop();
            // pipe delimited single line, same contract the SpawnJS.TestRunner harness parses
            Console.WriteLine($"TEST: {name}|{result}|{sw.ElapsedMilliseconds}|{Sanitize(detail)}");
        }

        static async Task TestAsync(string name, Func<Task> body)
        {
            var sw = Stopwatch.StartNew();
            string result = "Success", detail = "";
            try
            {
                var task = body();
                var finished = await Task.WhenAny(task, Task.Delay(10000));
                if (finished != task) throw new TimeoutException("exceeded 10000ms");
                await task;
                _pass++;
            }
            catch (SkipException ex) { result = "Skipped"; detail = ex.Message; _skip++; }
            catch (Exception ex)
            {
                result = "Error";
                detail = $"{ex.GetType().Name}: {ex.Message}";
                _fail++;
                _failures.Add($"{name}: {detail}");
                JS.LogError($"FAILED {name}\n{ex}");
            }
            finally { JS.Delete(K); }
            sw.Stop();
            Console.WriteLine($"TEST: {name}|{result}|{sw.ElapsedMilliseconds}|{Sanitize(detail)}");
        }

        static string Sanitize(string v) => v.Replace("\r", " ").Replace("\n", " ").Replace("|", "/");

        class SkipException : Exception { public SkipException(string m) : base(m) { } }
        static void Skip(string why) => throw new SkipException(why);

        static void Assert(bool condition, string message)
        {
            if (!condition) throw new Exception(message);
        }
        static void AssertEqual<T>(T actual, T expected, string what)
        {
            if (!EqualityComparer<T>.Default.Equals(actual, expected))
                throw new Exception($"{what}: got [{Show(actual)}], expected [{Show(expected)}]");
        }
        static string Show(object? v) => v switch
        {
            null => "null",
            string s => s,
            System.Collections.IEnumerable e when v is not string => string.Join(",", e.Cast<object?>().Select(Show)),
            _ => v.ToString() ?? "null",
        };
        #endregion

        #region javascript-side instruments
        // What Javascript really holds at a global key, read WITHOUT the marshaller under test.
        // propertyTypeInfo is a direct typeof + class read on the JS side.
        static string TypeOfKey(string key = K) => JS.TypeOf(key);
        static string CtorOfKey(string key = K) => JS.ConstructorName(key);
        // "typeof:ConstructorName" of the value at a key, via the prototype chain (a derived type
        // reports itself, which Object.prototype.toString cannot do).
        static string DescribeKey(string key = K)
        {
            using var raw = JS.Get(key);
            return JS.Call<SpawnJSObjectReference?, string>("SpawnJSTests.describe", raw);
        }
        // String(value) as Javascript sees it
        static string StrKey(string key = K)
        {
            using var raw = JS.Get(key);
            return JS.Call<SpawnJSObjectReference?, string>("SpawnJSTests.str", raw);
        }
        // every element of an array/TypedArray at a key, stringified and comma joined
        static string ElementsKey(string key = K)
        {
            using var raw = JS.Get(key);
            return JS.Call<SpawnJSObjectReference?, string>("SpawnJSTests.elements", raw);
        }
        static string OwnKeysOf(string key = K)
        {
            using var raw = JS.Get(key);
            return JS.Call<SpawnJSObjectReference?, string>("SpawnJSTests.ownKeys", raw);
        }
        static string Js<T1>(string fn, T1 arg1) => JS.Call<T1, string>($"SpawnJSTests.{fn}", arg1);
        #endregion

        public static async Task Run()
        {
            _pass = _fail = _skip = 0;
            _failures.Clear();
            Console.WriteLine("READY: SpawnJS marshaller tests");

            Assert_JSHelpersLoaded();

            PocoMarshallerTests();
            IEnumerableMarshallerTests();
            VoidTypeMarshallerTests();
            ObjectMarshallerTests();
            StringMarshallerTests();
            INumberMarshallerTests();
            INumberNullableMarshallerTests();
            DoubleMarshallerTests();
            Int32MarshallerTests();
            BooleanMarshallerTests();
            TupleMarshallerTests();
            SpawnJSObjectReferenceMarshallerTests();
            ArrayMarshallerTests();
            ListMarshallerTests();
            DictionaryMarshallerTests();
            HeapViewDescriptorMarshallerTests();
            CallbackMarshallerTests();
            ByteArrayMarshallerTests();
            await TaskMarshallerTests();
            BigIntegerMarshallerTests();
            UnionMarshallerTests();
            DelegateMarshallerTests();
            SpawnJSObjectMarshallerTests();
            EnumMarshallerTests();
            EnumStringMarshallerTests();
            EpochDateTimeMarshallerTests();
            DateTimeMarshallerTests();
            HeapViewMarshallerTests();
            JsonElementMarshallerTests();
            HeapViewTests();
            TypedArrayHeapViewTests();
            // Not a marshaller, but it shares this harness's counting/reporting and must run before
            // the RESULTS line the test runner stops on.
            AppRootTests.Run(Test);

            Console.WriteLine($"RESULTS: Failed: {_fail} Passed: {_pass} Skipped: {_skip} Ran: {_pass + _fail + _skip}");
            if (_fail > 0)
            {
                JS.LogError($"{_fail} MARSHALLER TEST FAILURE(S):\n" + string.Join("\n", _failures));
            }
        }

        // The whole suite's Javascript half must be loaded, or every "verified" read below is vacuous.
        static void Assert_JSHelpersLoaded() => Test("Harness.JSHelpersLoaded", () =>
        {
            Assert(JS.Has("SpawnJSTests"), "globalThis.SpawnJSTests is missing - spawnjs-tests.js did not load");
            AssertEqual(Js("describe", "abc"), "string:String", "SpawnJSTests.describe is not answering");
        });

        // ==========================================================================================
        // PocoMarshaller - .Net POCO <-> plain JS object
        // ==========================================================================================
        public class Person
        {
            [JsonPropertyName("given_name")]
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public int Age { get; set; }
            public double? Score { get; set; }
            [JsonIgnore]
            public string? Secret { get; set; }
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? Nickname { get; set; }
            public string? City { get; set; }
        }

        // PocoMarshaller property-walks a POCO reflectively, so under trimming the APP is responsible for
        // preserving that POCO's accessors - the same contract as any reflection-based object mapping
        // (the library's DynamicallyAccessedMembers reaches the ctor, not the accessors).
        //
        // This bites in a way that is easy to miss, because the trimmer removes accessors INDIVIDUALLY:
        // only a property the app never READS in C# loses its getter. A trimmed run failed here with
        // ArgumentException Arg_GetMethNotFnd because `Nickname` is only ever assigned, never read, so
        // `get_Nickname` alone was trimmed while every other accessor survived - and PocoMarshaller has to
        // call GetValue on it to evaluate [JsonIgnore(WhenWritingNull)]. An untrimmed run is green either
        // way, so only the trimmed suite can catch this.
        //
        // [DynamicDependency] is the documented fix. These tests exist to keep proving it works.
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(Person))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicFields, typeof(Extent3D))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicFields, typeof(Vec2))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(Texture))]
        static void PocoMarshallerTests()
        {
            Test("PocoMarshaller.Out", () =>
            {
                var person = new Person { FirstName = "Ada", LastName = "Lovelace", Age = 36, Score = 99.5, Secret = "hidden", Nickname = null, City = null };
                JS.Set(K, person);
                AssertEqual(TypeOfKey(), "object", "a POCO must cross as a plain JS object");
                AssertEqual(CtorOfKey(), "Object", "constructor");
                using var raw = JS.Get(K)!;
                Assert(raw.Has("given_name"), "[JsonPropertyName] rename was not applied");
                Assert(!raw.Has("firstName"), "the original member name is still present");
                Assert(!raw.Has("secret"), "[JsonIgnore] member was written");
                Assert(!raw.Has("nickname"), "[JsonIgnore(WhenWritingNull)] + null member was written");
                Assert(raw.Has("city"), "a plain null member must still be written");
                AssertEqual(raw.Get<int>("age"), 36, "age");
                AssertEqual(raw.Get<double?>("score"), 99.5, "score");
            });

            Test("PocoMarshaller.In", () =>
            {
                var person = new Person { FirstName = "Ada", LastName = "Lovelace", Age = 36, Score = 99.5, City = null };
                JS.Set(K, person);
                var back = JS.Get<Person>(K)!;
                AssertEqual(back.FirstName, "Ada", "FirstName");
                AssertEqual(back.LastName, "Lovelace", "LastName");
                AssertEqual(back.Age, 36, "Age");
                AssertEqual(back.Score, 99.5, "Score");
                AssertEqual(back.City, null, "City");
                AssertEqual(back.Secret, null, "an ignored member must not come back");
            });

            Test("PocoMarshaller.InFromJavascriptObject", () =>
            {
                // built by Javascript, not by a .Net write, so the read is not reading its own output
                var back = JS.Call<Person>("SpawnJSTests.objectWithNullMember");
                Assert(back != null, "a JS object must read back as a POCO instance");
            });

            Test("PocoMarshaller.NullOut", () =>
            {
                JS.Set(K, (Person?)null);
                AssertEqual(DescribeKey(), "object:null", "a null POCO must cross as JS null");
            });

            Test("PocoMarshaller.UndefinedIn", () =>
            {
                var back = JS.Get<Person?>("__mt_missing");
                AssertEqual(back, null, "reading an absent key as a POCO must give null");
            });

            // A struct POCO is the same shape of thing as a class POCO - a plain bag of members that a
            // web API takes as a dictionary-like object - and consumers write them (a size, an extent, a
            // pair of coordinates). The write path is identical; the read path is where a struct differs,
            // because PropertyInfo.SetValue boxes its target, so setting members on an unboxed local
            // silently throws every write away.
            Test("PocoMarshaller.StructOut", () =>
            {
                var extent = new Extent3D { Width = 640, Height = 480, DepthOrArrayLayers = 1 };
                JS.Set(K, extent);
                AssertEqual(TypeOfKey(), "object", "a struct POCO must cross as a plain JS object");
                AssertEqual(CtorOfKey(), "Object", "constructor");
                AssertEqual(OwnKeysOf(), "width,height,depthOrArrayLayers", "members");
                AssertEqual(JS.Get<int>($"{K}.width"), 640, "width");
                AssertEqual(JS.Get<int>($"{K}.height"), 480, "height");
            });

            Test("PocoMarshaller.StructIn", () =>
            {
                var extent = new Extent3D { Width = 640, Height = 480, DepthOrArrayLayers = 2 };
                JS.Set(K, extent);
                var back = JS.Get<Extent3D>(K);
                AssertEqual(back.Width, 640, "Width");
                AssertEqual(back.Height, 480, "Height");
                AssertEqual(back.DepthOrArrayLayers, 2, "DepthOrArrayLayers");
            });

            Test("PocoMarshaller.StructFieldsRoundTrip", () =>
            {
                // fields rather than properties: the member walk handles both, and FieldInfo.SetValue has
                // the same boxing behaviour that makes a struct read need a boxed target
                var v = new Vec2 { X = 1.5, Y = -2.5, UnmarkedField = 99 };
                JS.Set(K, v);
                AssertEqual(OwnKeysOf(), "x,y", "only [JsonInclude] fields are members, matching System.Text.Json");
                var back = JS.Get<Vec2>(K);
                AssertEqual(back.X, 1.5, "X");
                AssertEqual(back.Y, -2.5, "Y");
                AssertEqual(back.UnmarkedField, 0d, "a field without [JsonInclude] must not cross");
            });

            Test("PocoMarshaller.NullableStructRoundTrip", () =>
            {
                Extent3D? some = new Extent3D { Width = 7, Height = 8, DepthOrArrayLayers = 9 };
                JS.Set(K, some);
                AssertEqual(TypeOfKey(), "object", "a non-null struct? must cross as an object");
                var back = JS.Get<Extent3D?>(K);
                Assert(back.HasValue, "a non-null struct? must read back with a value");
                AssertEqual(back!.Value.Width, 7, "Width");

                Extent3D? none = null;
                JS.Set(K, none);
                AssertEqual(DescribeKey(), "object:null", "a null struct? must cross as JS null");
                AssertEqual(JS.Get<Extent3D?>(K), null, "null round trip");
                AssertEqual(JS.Get<Extent3D?>("__mt_missing"), null, "absent property as a struct?");
            });

            Test("PocoMarshaller.StructInFromJavascriptObject", () =>
            {
                // built by Javascript, so the read is not reading its own write
                var back = JS.Call<Extent3D>("SpawnJSTests.objectWithNullMember");
                AssertEqual(back.Width, 0, "an absent member must leave the struct's default");
            });

            // A struct member INSIDE a class POCO - the nested case, where the outer walk hands the inner
            // struct to its own marshaller.
            Test("PocoMarshaller.NestedStructMember", () =>
            {
                var t = new Texture { Label = "tex", Size = new Extent3D { Width = 16, Height = 32, DepthOrArrayLayers = 1 } };
                JS.Set(K, t);
                AssertEqual(JS.Get<int>($"{K}.size.width"), 16, "nested struct member width");
                var back = JS.Get<Texture>(K)!;
                AssertEqual(back.Label, "tex", "Label");
                AssertEqual(back.Size.Width, 16, "nested Width");
                AssertEqual(back.Size.Height, 32, "nested Height");
            });
        }

        /// <summary>A struct POCO with properties, the shape a WebGPU-style descriptor uses.</summary>
        public struct Extent3D
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public int DepthOrArrayLayers { get; set; }
        }

        /// <summary>
        /// A struct POCO whose members are FIELDS rather than properties. Fields need [JsonInclude] to be
        /// marshalled at all - the same rule System.Text.Json uses, so a type moving between the two
        /// behaves the same. A field without it is silently not a member, which is what UnmarkedField pins.
        /// </summary>
        public struct Vec2
        {
            [JsonInclude] public double X;
            [JsonInclude] public double Y;
            public double UnmarkedField;
        }

        /// <summary>A class POCO holding a struct POCO, so the nested path is exercised.</summary>
        public class Texture
        {
            public string? Label { get; set; }
            public Extent3D Size { get; set; }
        }

        // ==========================================================================================
        // IEnumerableMarshaller - .Net IEnumerable<> -> JS Array   (OUT ONLY)
        // ==========================================================================================
        static void IEnumerableMarshallerTests()
        {
            Test("IEnumerableMarshaller.Out", () =>
            {
                IEnumerable<int> src = Enumerable.Range(5, 4);
                JS.Set(K, src);
                AssertEqual(CtorOfKey(), "Array", "an IEnumerable must cross as a JS Array");
                AssertEqual(ElementsKey(), "5,6,7,8", "elements");
            });

            Test("IEnumerableMarshaller.OutEmpty", () =>
            {
                IEnumerable<string> src = System.Array.Empty<string>();
                JS.Set(K, src);
                AssertEqual(CtorOfKey(), "Array", "constructor");
                AssertEqual(ElementsKey(), "", "an empty enumerable must cross as an empty array");
            });

            Test("IEnumerableMarshaller.NullOut", () =>
            {
                IEnumerable<int>? src = null;
                JS.Set(K, src);
                AssertEqual(DescribeKey(), "object:null", "a null IEnumerable must cross as JS null");
            });
        }

        // ==========================================================================================
        // VoidTypeMarshaller - nothing is marshalled
        // ==========================================================================================
        static void VoidTypeMarshallerTests()
        {
            Test("VoidTypeMarshaller.CallReturningVoid", () =>
            {
                // a void call must complete and must not write anything
                JS.CallVoid<string>("SpawnJSTests.str", "anything");
            });

            Test("VoidTypeMarshaller.CarriesNoValue", () =>
            {
                // VoidType is the "there is no value" type: it writes nothing of its own, so whatever it
                // is marshalled into stays undefined
                JS.Set(K, new VoidType());
                AssertEqual(TypeOfKey(), "undefined", "VoidType must not carry a value across");
            });
        }

        // ==========================================================================================
        // ObjectMarshaller - .Net object <-> JS Object
        // ==========================================================================================
        static void ObjectMarshallerTests()
        {
            Test("ObjectMarshaller.OutDispatchesOnRuntimeType", () =>
            {
                // declared object, runtime int - the write must type itself from the VALUE
                object boxed = 42;
                JS.Set(K, boxed);
                AssertEqual(TypeOfKey(), "number", "a boxed int must cross as a JS number");
                AssertEqual(StrKey(), "42", "value");
            });

            Test("ObjectMarshaller.OutDispatchesOnRuntimeTypeString", () =>
            {
                object boxed = "hello";
                JS.Set(K, boxed);
                AssertEqual(TypeOfKey(), "string", "a boxed string must cross as a JS string");
                AssertEqual(StrKey(), "hello", "value");
            });

            Test("ObjectMarshaller.NullOut", () =>
            {
                JS.Set(K, (object?)null);
                AssertEqual(DescribeKey(), "object:null", "a null object must cross as JS null");
            });
        }

        // ==========================================================================================
        // StringMarshaller - .Net string <-> JS string
        // ==========================================================================================
        static void StringMarshallerTests()
        {
            Test("StringMarshaller.RoundTrip", () =>
            {
                JS.Set(K, "Hello world!");
                AssertEqual(TypeOfKey(), "string", "must be a JS string primitive, not a String object");
                AssertEqual(JS.Get<string>(K), "Hello world!", "round trip");
            });

            Test("StringMarshaller.RoundTripNonAscii", () =>
            {
                // UTF-16 with an astral-plane pair, so any byte-level shortcut in the crossing shows up
                var value = "héllo 世界 🚀";
                JS.Set(K, value);
                AssertEqual(JS.Get<string>(K), value, "non-ascii round trip");
                AssertEqual(StrKey(), value, "Javascript sees the same string");
            });

            Test("StringMarshaller.RoundTripEmpty", () =>
            {
                JS.Set(K, "");
                AssertEqual(TypeOfKey(), "string", "an empty string is still a string");
                AssertEqual(JS.Get<string>(K), "", "empty round trip");
            });

            Test("StringMarshaller.In", () =>
            {
                AssertEqual(JS.Call<string, string>("SpawnJSTests.str", "from js"), "from js", "read of a JS string");
            });

            Test("StringMarshaller.NullOut", () =>
            {
                JS.Set(K, (string?)null);
                AssertEqual(DescribeKey(), "object:null", "a null string must cross as JS null");
            });

            Test("StringMarshaller.NullIn", () =>
            {
                AssertEqual(JS.Call<string?>("SpawnJSTests.nullValue"), null, "JS null must read as a null string");
            });

            Test("StringMarshaller.UndefinedIn", () =>
            {
                AssertEqual(JS.Call<string?>("SpawnJSTests.undefinedValue"), null, "JS undefined must read as a null string");
                AssertEqual(JS.Get<string?>("__mt_missing"), null, "an absent property must read as a null string");
            });
        }

        // ==========================================================================================
        // INumberMarshaller - .Net INumber<> <-> JS Number
        // ==========================================================================================
        // T flows into Get<T>, whose PublicConstructors requirement exists so a wrapper's deserialization
        // ctor survives trimming. A generic helper has to declare the same requirement or the trim
        // analyzer reports IL2091 - the library's contract, working as designed.
        static void NumberRoundTrip<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(T value, string expectedJs) where T : struct, INumber<T>
        {
            JS.Set(K, value);
            AssertEqual(TypeOfKey(), "number", $"{typeof(T).Name} must cross as a JS number");
            AssertEqual(StrKey(), expectedJs, $"{typeof(T).Name} value as Javascript sees it");
            AssertEqual(JS.Get<T>(K), value, $"{typeof(T).Name} round trip");
        }

        static void INumberMarshallerTests()
        {
            Test("INumberMarshaller.RoundTripAllIntegerTypes", () =>
            {
                NumberRoundTrip<byte>(42, "42");
                NumberRoundTrip<sbyte>(-42, "-42");
                NumberRoundTrip<ushort>(42000, "42000");
                NumberRoundTrip<short>(-30000, "-30000");
                NumberRoundTrip<uint>(4000000000, "4000000000");
                NumberRoundTrip<int>(-2000000000, "-2000000000");
                NumberRoundTrip<ulong>(9007199254740991, "9007199254740991"); // 2^53-1, the last exact integer in a double
                NumberRoundTrip<long>(-9007199254740991, "-9007199254740991");
            });

            Test("INumberMarshaller.RoundTripAllFloatTypes", () =>
            {
                NumberRoundTrip<float>(0.5f, "0.5");
                NumberRoundTrip<double>(-1.25, "-1.25");
                NumberRoundTrip<Half>((Half)0.5f, "0.5");
            });

            Test("INumberMarshaller.EdgeValues", () =>
            {
                NumberRoundTrip<byte>(0, "0");
                NumberRoundTrip<byte>(255, "255");
                NumberRoundTrip<sbyte>(-128, "-128");
                NumberRoundTrip<int>(int.MinValue, "-2147483648");
                NumberRoundTrip<int>(int.MaxValue, "2147483647");
                NumberRoundTrip<uint>(uint.MaxValue, "4294967295");
            });

            Test("INumberMarshaller.DoubleSpecialValues", () =>
            {
                JS.Set(K, double.NaN);
                AssertEqual(StrKey(), "NaN", "NaN must cross as JS NaN");
                Assert(double.IsNaN(JS.Get<double>(K)), "NaN did not round trip");

                JS.Set(K, double.PositiveInfinity);
                AssertEqual(StrKey(), "Infinity", "+Infinity must cross as JS Infinity");
                AssertEqual(JS.Get<double>(K), double.PositiveInfinity, "+Infinity round trip");

                JS.Set(K, double.NegativeInfinity);
                AssertEqual(StrKey(), "-Infinity", "-Infinity must cross as JS -Infinity");
                AssertEqual(JS.Get<double>(K), double.NegativeInfinity, "-Infinity round trip");
            });

            Test("INumberMarshaller.In", () =>
            {
                AssertEqual(JS.Call<string, int>("SpawnJSTests.numberFrom", "1234"), 1234, "read of a JS number as int");
                AssertEqual(JS.Call<string, double>("SpawnJSTests.numberFrom", "1.5"), 1.5, "read of a JS number as double");
                AssertEqual(JS.Call<string, long>("SpawnJSTests.numberFrom", "9007199254740991"), 9007199254740991L, "read of a JS number as long");
            });

            Test("INumberMarshaller.PrecisionCeilingIsReported", () =>
            {
                // A JS number is an f64, so a long past 2^53 CANNOT round trip. The contract is that this
                // is a known ceiling (BigInteger is the exact path) - the test pins where it starts so a
                // silent widening of the claim gets caught.
                long justPast = 9007199254740993L; // 2^53+1, not representable as a double
                JS.Set(K, justPast);
                var back = JS.Get<long>(K);
                Assert(back != justPast, "2^53+1 round tripped exactly - the f64 ceiling has moved, update this test and the docs");
                AssertEqual(back, 9007199254740992L, "the value must land on the nearest representable double, not become garbage");
            });
        }

        // ==========================================================================================
        // INumberNullableMarshaller - .Net Nullable<INumber<>> <-> JS Number?
        // ==========================================================================================
        static void NullableNumberRoundTrip<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(T value) where T : struct, INumber<T>
        {
            T? some = value;
            JS.Set(K, some);
            AssertEqual(TypeOfKey(), "number", $"{typeof(T).Name}? with a value must cross as a JS number");
            AssertEqual(JS.Get<T?>(K), some, $"{typeof(T).Name}? round trip");

            T? none = null;
            JS.Set(K, none);
            AssertEqual(DescribeKey(), "object:null", $"a null {typeof(T).Name}? must cross as JS null, not as 0");
            AssertEqual(JS.Get<T?>(K), null, $"null {typeof(T).Name}? round trip");

            AssertEqual(JS.Get<T?>("__mt_missing"), null, $"an absent property read as {typeof(T).Name}? must be null");
        }

        static void INumberNullableMarshallerTests()
        {
            Test("INumberNullableMarshaller.RoundTripAllTypes", () =>
            {
                NullableNumberRoundTrip<byte>(42);
                NullableNumberRoundTrip<sbyte>(-42);
                NullableNumberRoundTrip<ushort>(42000);
                NullableNumberRoundTrip<short>(-30000);
                NullableNumberRoundTrip<uint>(4000000000);
                NullableNumberRoundTrip<ulong>(9007199254740991);
                NullableNumberRoundTrip<long>(-9007199254740991);
                NullableNumberRoundTrip<float>(0.5f);
                NullableNumberRoundTrip<Half>((Half)0.5f);
            });

            // The regression this marshaller exists for: WebGPU limits are long?, and before it existed
            // every one of them read null - which silently capped the device at the 128MiB default.
            Test("INumberNullableMarshaller.LongNullableReadsItsValue", () =>
            {
                long? limit = 2147483648; // 2GiB, a real maxStorageBufferBindingSize
                JS.Set(K, limit);
                AssertEqual(JS.Get<long?>(K), limit, "long? must read back its value, not null");
                AssertEqual(StrKey(), "2147483648", "Javascript must hold the number itself");
            });

            Test("INumberNullableMarshaller.In", () =>
            {
                AssertEqual(JS.Call<string, long?>("SpawnJSTests.numberFrom", "2147483648"), 2147483648L, "read of a JS number as long?");
                AssertEqual(JS.Call<long?>("SpawnJSTests.nullValue"), null, "JS null must read as a null long?");
                AssertEqual(JS.Call<long?>("SpawnJSTests.undefinedValue"), null, "JS undefined must read as a null long?");
            });
        }

        // ==========================================================================================
        // DoubleMarshaller / DoubleNullableMarshaller
        // ==========================================================================================
        static void DoubleMarshallerTests()
        {
            Test("DoubleMarshaller.RoundTrip", () =>
            {
                JS.Set(K, 3.14159265358979);
                AssertEqual(TypeOfKey(), "number", "typeof");
                AssertEqual(JS.Get<double>(K), 3.14159265358979, "round trip");
            });

            Test("DoubleNullableMarshaller.RoundTrip", () =>
            {
                JS.Set(K, (double?)2.5);
                AssertEqual(JS.Get<double?>(K), 2.5, "value round trip");
                JS.Set(K, (double?)null);
                AssertEqual(DescribeKey(), "object:null", "null double? must cross as JS null");
                AssertEqual(JS.Get<double?>(K), null, "null round trip");
            });

            Test("DoubleNullableMarshaller.UndefinedIn", () =>
            {
                AssertEqual(JS.Get<double?>("__mt_missing"), null, "absent property as double?");
                AssertEqual(JS.Call<double?>("SpawnJSTests.undefinedValue"), null, "JS undefined as double?");
            });
        }

        // ==========================================================================================
        // Int32Marshaller / Int32NullableMarshaller
        // ==========================================================================================
        static void Int32MarshallerTests()
        {
            Test("Int32Marshaller.RoundTrip", () =>
            {
                JS.Set(K, 123456);
                AssertEqual(TypeOfKey(), "number", "typeof");
                AssertEqual(JS.Get<int>(K), 123456, "round trip");
            });

            Test("Int32NullableMarshaller.RoundTrip", () =>
            {
                JS.Set(K, (int?)-7);
                AssertEqual(JS.Get<int?>(K), -7, "value round trip");
                JS.Set(K, (int?)null);
                AssertEqual(DescribeKey(), "object:null", "null int? must cross as JS null");
                AssertEqual(JS.Get<int?>(K), null, "null round trip");
                AssertEqual(JS.Get<int?>("__mt_missing"), null, "absent property as int?");
            });
        }

        // ==========================================================================================
        // BooleanMarshaller / BooleanNullableMarshaller
        // ==========================================================================================
        static void BooleanMarshallerTests()
        {
            Test("BooleanMarshaller.RoundTrip", () =>
            {
                JS.Set(K, true);
                AssertEqual(TypeOfKey(), "boolean", "typeof");
                AssertEqual(StrKey(), "true", "value");
                AssertEqual(JS.Get<bool>(K), true, "true round trip");

                JS.Set(K, false);
                AssertEqual(StrKey(), "false", "value");
                AssertEqual(JS.Get<bool>(K), false, "false round trip");
            });

            Test("BooleanNullableMarshaller.RoundTrip", () =>
            {
                JS.Set(K, (bool?)true);
                AssertEqual(JS.Get<bool?>(K), true, "value round trip");
                JS.Set(K, (bool?)null);
                AssertEqual(DescribeKey(), "object:null", "null bool? must cross as JS null, not false");
                AssertEqual(JS.Get<bool?>(K), null, "null round trip");
                AssertEqual(JS.Get<bool?>("__mt_missing"), null, "absent property as bool?");
            });
        }

        // ==========================================================================================
        // ITupleMarshallerFactory - .Net Tuple, ValueTuple <-> JS Array
        // ==========================================================================================
        static void TupleMarshallerTests()
        {
            Test("TupleMarshaller.ValueTupleRoundTrip", () =>
            {
                (string, int, bool) v = ("Hello", 42, true);
                JS.Set(K, v);
                AssertEqual(CtorOfKey(), "Array", "a ValueTuple must cross as a JS Array");
                AssertEqual(ElementsKey(), "Hello,42,true", "positional elements");
                var back = JS.Get<(string, int, bool)>(K);
                AssertEqual(back.Item1, "Hello", "Item1");
                AssertEqual(back.Item2, 42, "Item2");
                AssertEqual(back.Item3, true, "Item3");
            });

            Test("TupleMarshaller.TupleRoundTrip", () =>
            {
                var v = new Tuple<string, int>("Hello", 42);
                JS.Set(K, v);
                AssertEqual(CtorOfKey(), "Array", "a Tuple must cross as a JS Array");
                var back = JS.Get<Tuple<string, int>>(K)!;
                AssertEqual(back.Item1, "Hello", "Item1");
                AssertEqual(back.Item2, 42, "Item2");
            });

            Test("TupleMarshaller.NullableRoundTrip", () =>
            {
                (int, string)? some = (7, "seven");
                JS.Set(K, some);
                var back = JS.Get<(int, string)?>(K);
                Assert(back.HasValue, "a non-null ValueTuple? must read back with a value");
                AssertEqual(back!.Value.Item1, 7, "Item1");
                AssertEqual(back!.Value.Item2, "seven", "Item2");

                (int, string)? none = null;
                JS.Set(K, none);
                AssertEqual(DescribeKey(), "object:null", "a null ValueTuple? must cross as JS null");
                AssertEqual(JS.Get<(int, string)?>(K), null, "null round trip");
            });

            Test("TupleMarshaller.In", () =>
            {
                // an array Javascript built, read as a tuple
                var back = JS.Call<string, (int, int, int)>("SpawnJSTests.numberArray", "1,2,3");
                AssertEqual(back.Item1, 1, "Item1");
                AssertEqual(back.Item2, 2, "Item2");
                AssertEqual(back.Item3, 3, "Item3");
            });
        }

        // ==========================================================================================
        // SpawnJSObjectReferenceMarshaller - .Net SpawnJSObjectReference <-> JS Any
        // ==========================================================================================
        static void SpawnJSObjectReferenceMarshallerTests()
        {
            Test("SpawnJSObjectReferenceMarshaller.RoundTripIsTheSameObject", () =>
            {
                using var obj = JS.Call<SpawnJSObjectReference>("SpawnJSTests.newObject")!;
                JS.Set(K, obj);
                using var back = JS.Get<SpawnJSObjectReference>(K)!;
                // === on the Javascript side: the reference must carry the SAME object, not a clone
                Assert(JS.Call<SpawnJSObjectReference, SpawnJSObjectReference, bool>("SpawnJSTests.same", obj, back),
                    "a reference round trip produced a different Javascript object");
            });

            Test("SpawnJSObjectReferenceMarshaller.NullIn", () =>
            {
                AssertEqual(JS.Call<SpawnJSObjectReference?>("SpawnJSTests.nullValue"), null, "JS null must read as a null reference");
                AssertEqual(JS.Call<SpawnJSObjectReference?>("SpawnJSTests.undefinedValue"), null, "JS undefined must read as a null reference");
                AssertEqual(JS.Get<SpawnJSObjectReference?>("__mt_missing"), null, "an absent property must read as a null reference");
            });
        }

        // ==========================================================================================
        // ArrayMarshaller - .Net T[] <-> JS Array<>
        // ==========================================================================================
        static void ArrayMarshallerTests()
        {
            Test("ArrayMarshaller.IntArrayRoundTrip", () =>
            {
                var src = new[] { 1, 2, 3, 4 };
                JS.Set(K, src);
                AssertEqual(CtorOfKey(), "Array", "an int[] must cross as a JS Array");
                AssertEqual(ElementsKey(), "1,2,3,4", "elements");
                AssertEqual(Show(JS.Get<int[]>(K)), "1,2,3,4", "round trip");
            });

            Test("ArrayMarshaller.StringArrayRoundTrip", () =>
            {
                var src = new[] { "sign", "verify" };
                JS.Set(K, src);
                AssertEqual(ElementsKey(), "sign,verify", "elements");
                AssertEqual(Show(JS.Get<string[]>(K)), "sign,verify", "round trip");
            });

            Test("ArrayMarshaller.DoubleArrayRoundTrip", () =>
            {
                var src = new[] { 0.5, -1.25, 2.0 };
                JS.Set(K, src);
                AssertEqual(ElementsKey(), "0.5,-1.25,2", "elements as Javascript prints them");
                AssertEqual(Show(JS.Get<double[]>(K)), "0.5,-1.25,2", "round trip");
            });

            Test("ArrayMarshaller.EmptyRoundTrip", () =>
            {
                JS.Set(K, System.Array.Empty<int>());
                AssertEqual(CtorOfKey(), "Array", "constructor");
                AssertEqual(JS.Get<int[]>(K)!.Length, 0, "an empty array must stay empty");
            });

            Test("ArrayMarshaller.NullElements", () =>
            {
                var src = new string?[] { "a", null, "c" };
                JS.Set(K, src);
                AssertEqual(ElementsKey(), "a,null,c", "a null element must cross as JS null");
                var back = JS.Get<string?[]>(K)!;
                AssertEqual(back[1], null, "the null element must come back null");
            });

            Test("ArrayMarshaller.NullOut", () =>
            {
                JS.Set(K, (int[]?)null);
                AssertEqual(DescribeKey(), "object:null", "a null array must cross as JS null");
                AssertEqual(JS.Get<int[]?>(K), null, "null round trip");
            });

            Test("ArrayMarshaller.In", () =>
            {
                var back = JS.Call<string, int[]>("SpawnJSTests.numberArray", "10,20,30")!;
                AssertEqual(Show(back), "10,20,30", "read of a JS array as int[]");
                var strings = JS.Call<string, string[]>("SpawnJSTests.stringArray", "a,b,c")!;
                AssertEqual(Show(strings), "a,b,c", "read of a JS array as string[]");
            });

            Test("ArrayMarshaller.InWithHoles", () =>
            {
                // [1, null, undefined, 4] - both absence shapes must land as null in a nullable element
                var back = JS.Call<int?[]>("SpawnJSTests.arrayWithHoles")!;
                AssertEqual(back.Length, 4, "length");
                AssertEqual(back[0], 1, "element 0");
                AssertEqual(back[1], null, "a JS null element must read as null");
                AssertEqual(back[2], null, "a JS undefined element must read as null");
                AssertEqual(back[3], 4, "element 3");
            });

            Test("ArrayMarshaller.UndefinedIn", () =>
            {
                AssertEqual(JS.Get<int[]?>("__mt_missing"), null, "an absent property read as int[] must be null");
            });
        }

        // ==========================================================================================
        // ListMarshaller - .Net List<> <-> JS Array<>
        // ==========================================================================================
        static void ListMarshallerTests()
        {
            Test("ListMarshaller.RoundTrip", () =>
            {
                var src = new List<int> { 7, 8, 9 };
                JS.Set(K, src);
                AssertEqual(CtorOfKey(), "Array", "a List<> must cross as a JS Array");
                AssertEqual(ElementsKey(), "7,8,9", "elements");
                AssertEqual(Show(JS.Get<List<int>>(K)), "7,8,9", "round trip");
            });

            Test("ListMarshaller.StringRoundTrip", () =>
            {
                var src = new List<string> { "x", "y" };
                JS.Set(K, src);
                AssertEqual(Show(JS.Get<List<string>>(K)), "x,y", "round trip");
            });

            Test("ListMarshaller.NullOut", () =>
            {
                JS.Set(K, (List<int>?)null);
                AssertEqual(DescribeKey(), "object:null", "a null List<> must cross as JS null");
                AssertEqual(JS.Get<List<int>?>(K), null, "null round trip");
            });

            Test("ListMarshaller.In", () =>
            {
                var back = JS.Call<string, List<int>>("SpawnJSTests.numberArray", "4,5,6")!;
                AssertEqual(Show(back), "4,5,6", "read of a JS array as List<int>");
            });
        }

        // ==========================================================================================
        // DictionaryMarshaller - .Net Dictionary <-> JS plain object (record)
        // ==========================================================================================
        public enum Axis { X, Y, Z }

        static void DictionaryMarshallerTests()
        {
            Test("DictionaryMarshaller.StringKeyRoundTrip", () =>
            {
                var src = new Dictionary<string, int> { ["a"] = 1, ["b"] = 22, ["c"] = 333 };
                JS.Set(K, src);
                AssertEqual(CtorOfKey(), "Object", "a Dictionary must cross as a plain JS object");
                AssertEqual(OwnKeysOf(), "a,b,c", "keys");
                AssertEqual(JS.Get<int>($"{K}.b"), 22, "the value must be stored under its own key");
                var back = JS.Get<Dictionary<string, int>>(K)!;
                AssertEqual(back.Count, 3, "count");
                AssertEqual(back["b"], 22, "round trip value");
            });

            Test("DictionaryMarshaller.IntKeyRoundTrip", () =>
            {
                // the SpawnDev.ILGPU WebGL stride-map shape
                var src = new Dictionary<int, int[]> { [0] = new[] { 1, 2, 3 }, [7] = new[] { 40, 50 } };
                JS.Set(K, src);
                AssertEqual(OwnKeysOf(), "0,7", "int keys must stringify the way Javascript does");
                AssertEqual(JS.Get<int>($"{K}.7.0"), 40, "nested value");
                var back = JS.Get<Dictionary<int, int[]>>(K)!;
                AssertEqual(back.Count, 2, "count");
                AssertEqual(Show(back[0]), "1,2,3", "value at key 0");
                AssertEqual(Show(back[7]), "40,50", "value at key 7");
            });

            Test("DictionaryMarshaller.EnumKeyRoundTrip", () =>
            {
                var src = new Dictionary<Axis, double> { [Axis.X] = 1.0, [Axis.Z] = 3.0 };
                JS.Set(K, src);
                AssertEqual(OwnKeysOf(), "X,Z", "enum keys cross by name");
                var back = JS.Get<Dictionary<Axis, double>>(K)!;
                AssertEqual(back[Axis.Z], 3.0, "value at Z");
            });

            Test("DictionaryMarshaller.MixedValueTypes", () =>
            {
                var src = new Dictionary<string, object> { ["n"] = 1, ["s"] = "two", ["b"] = true };
                JS.Set(K, src);
                AssertEqual(JS.TypeOf($"{K}.n"), "number", "number value");
                AssertEqual(JS.TypeOf($"{K}.s"), "string", "string value");
                AssertEqual(JS.TypeOf($"{K}.b"), "boolean", "boolean value");
            });

            Test("DictionaryMarshaller.NullValue", () =>
            {
                var src = new Dictionary<string, string?> { ["present"] = "yes", ["absent"] = null };
                JS.Set(K, src);
                AssertEqual(JS.TypeOf($"{K}.absent"), "object", "a null value must be written as JS null");
                var back = JS.Get<Dictionary<string, string?>>(K)!;
                AssertEqual(back["absent"], null, "null value round trip");
            });

            Test("DictionaryMarshaller.NullOut", () =>
            {
                JS.Set(K, (Dictionary<string, int>?)null);
                AssertEqual(DescribeKey(), "object:null", "a null Dictionary must cross as JS null");
            });

            Test("DictionaryMarshaller.In", () =>
            {
                var back = JS.Call<Dictionary<string, int>>("SpawnJSTests.objectWithNullMember")!;
                AssertEqual(back["present"], 1, "a JS object must read back as a Dictionary");
            });
        }

        // ==========================================================================================
        // HeapViewDescriptorMarshaller - .Net HeapViewDescriptor -> JS ArrayBufferView   (OUT ONLY)
        // ==========================================================================================
        static void HeapViewDescriptorMarshallerTests()
        {
            Test("HeapViewDescriptorMarshaller.OutHonoursViewTypeOnAPropertyKey", () =>
            {
                // A descriptor names the view type it wants. Writing it to a STRING key must produce
                // that type - if the type is dropped the result is a Uint8Array that still looks fine.
                var data = new float[] { 1.5f, -2.5f, 3.5f };
                unsafe
                {
                    fixed (float* p = data)
                    {
                        var descriptor = new HeapViewDescriptor((IntPtr)p, data.Length, JSArrayBufferView.Float32Array, true);
                        JS.Set(K, descriptor);
                    }
                }
                AssertEqual(CtorOfKey(), "Float32Array", "the descriptor's view type was not honoured on a string key");
                AssertEqual(ElementsKey(), "1.5,-2.5,3.5", "elements");
            });

            Test("HeapViewDescriptorMarshaller.OutHonoursViewTypeAsACallArgument", () =>
            {
                // the same descriptor written by INT key (the call-argument overload)
                var data = new float[] { 1.5f, -2.5f, 3.5f };
                string ctor;
                unsafe
                {
                    fixed (float* p = data)
                    {
                        var descriptor = new HeapViewDescriptor((IntPtr)p, data.Length, JSArrayBufferView.Float32Array, true);
                        ctor = JS.Call<HeapViewDescriptor, string>("SpawnJSTests.viewCtor", descriptor);
                    }
                }
                AssertEqual(ctor, "Float32Array", "the descriptor's view type was not honoured on an int key");
            });

            // A value written as a NAMED MEMBER goes through the string-key overload, which is a
            // different method from the int-key one a call argument uses. JS.Set marshals its value as a
            // call argument, so only a member write exercises the string-key path - putting the
            // descriptor in a record is the shortest route to it.
            Test("HeapViewDescriptorMarshaller.OutHonoursViewTypeOnAnObjectMember", () =>
            {
                var data = new float[] { 1.5f, -2.5f, 3.5f };
                unsafe
                {
                    fixed (float* p = data)
                    {
                        var record = new Dictionary<string, HeapViewDescriptor>
                        {
                            ["view"] = new HeapViewDescriptor((IntPtr)p, data.Length, JSArrayBufferView.Float32Array, true),
                        };
                        JS.Set(K, record);
                    }
                }
                AssertEqual(JS.ConstructorName($"{K}.view"), "Float32Array", "the descriptor's view type was dropped on the string-key path");
                using var member = JS.Get($"{K}.view")!;
                AssertEqual(JS.Call<SpawnJSObjectReference, string>("SpawnJSTests.elements", member), "1.5,-2.5,3.5", "elements");
            });

            Test("HeapViewDescriptorMarshaller.OutDefaultsToUint8Array", () =>
            {
                var data = new byte[] { 1, 2, 3 };
                unsafe
                {
                    fixed (byte* p = data)
                    {
                        JS.Set(K, new HeapViewDescriptor((IntPtr)p, data.Length, true));
                    }
                }
                AssertEqual(CtorOfKey(), "Uint8Array", "the default view type must be Uint8Array");
                AssertEqual(ElementsKey(), "1,2,3", "elements");
            });
        }

        // ==========================================================================================
        // CallbackMarshaller - .Net Callback -> JS Function   (OUT ONLY)
        // ==========================================================================================
        static void CallbackMarshallerTests()
        {
            Test("CallbackMarshaller.OutIsCallableFromJavascript", () =>
            {
                var called = false;
                using var cb = Callback.Create(() => { called = true; });
                JS.Set(K, cb);
                AssertEqual(TypeOfKey(), "function", "a Callback must cross as a JS function");
                JS.CallVoid(K);
                Assert(called, "Javascript called the function but the .Net callback did not run");
            });

            Test("CallbackMarshaller.OutReceivesArguments", () =>
            {
                var got = 0;
                using var cb = Callback.Create((int x) => { got = x; });
                JS.Set(K, cb);
                JS.CallVoid(K, 99);
                AssertEqual(got, 99, "the argument Javascript passed did not reach .Net");
            });

            Test("CallbackMarshaller.OutReturnsAValueToJavascript", () =>
            {
                using var cb = Callback.Create((int x) => x * 2);
                JS.Set(K, cb);
                using var raw = JS.Get(K)!;
                var result = JS.Call<SpawnJSObjectReference, int, int>("SpawnJSTests.invoke", raw, 21);
                AssertEqual(result, 42, "the .Net return value did not reach Javascript");
            });

            Test("CallbackMarshaller.NullOut", () =>
            {
                JS.Set(K, (Callback?)null);
                Assert(TypeOfKey() != "function", "a null Callback must not produce a JS function");
            });
        }

        // ==========================================================================================
        // ByteArrayMarshaller - .Net byte[] <-> JS Uint8Array
        // ==========================================================================================
        static void ByteArrayMarshallerTests()
        {
            Test("ByteArrayMarshaller.RoundTrip", () =>
            {
                var src = new byte[] { 0, 1, 127, 128, 255 };
                JS.Set(K, src);
                AssertEqual(CtorOfKey(), "Uint8Array", "a byte[] must cross as a Uint8Array");
                AssertEqual(ElementsKey(), "0,1,127,128,255", "elements");
                AssertEqual(Show(JS.Get<byte[]>(K)), "0,1,127,128,255", "round trip");
            });

            Test("ByteArrayMarshaller.IsACopyNotALiveView", () =>
            {
                // the marshaller writes a COPY: a later .Net mutation must NOT be visible in Javascript,
                // otherwise the JS side holds a pointer into a moving managed array
                var src = new byte[] { 1, 2, 3 };
                JS.Set(K, src);
                src[0] = 200;
                AssertEqual(ElementsKey(), "1,2,3", "the crossing must copy, not alias the managed array");
            });

            Test("ByteArrayMarshaller.Empty", () =>
            {
                JS.Set(K, System.Array.Empty<byte>());
                AssertEqual(CtorOfKey(), "Uint8Array", "constructor");
                AssertEqual(JS.Get<byte[]>(K)!.Length, 0, "an empty byte[] must stay empty");
            });

            Test("ByteArrayMarshaller.Large", () =>
            {
                var src = new byte[64 * 1024];
                for (var i = 0; i < src.Length; i++) src[i] = (byte)(i * 31);
                JS.Set(K, src);
                var back = JS.Get<byte[]>(K)!;
                AssertEqual(back.Length, src.Length, "length");
                for (var i = 0; i < src.Length; i++)
                    if (back[i] != src[i]) throw new Exception($"byte {i} came back as {back[i]}, expected {src[i]}");
            });

            Test("ByteArrayMarshaller.NullOut", () =>
            {
                JS.Set(K, (byte[]?)null);
                AssertEqual(DescribeKey(), "object:null", "a null byte[] must cross as JS null");
                AssertEqual(JS.Get<byte[]?>(K), null, "null round trip");
            });

            Test("ByteArrayMarshaller.In", () =>
            {
                var back = JS.Call<string, string, byte[]>("SpawnJSTests.typedArray", "Uint8Array", "9,8,7")!;
                AssertEqual(Show(back), "9,8,7", "a JS Uint8Array must read back as a byte[]");
            });
        }

        // ==========================================================================================
        // TaskMarshaller - .Net Task, Task<> <-> JS Promise
        // ==========================================================================================
        static async Task TaskMarshallerTests()
        {
            await TestAsync("TaskMarshaller.InResolved", async () =>
            {
                var value = await JS.CallAsync<string, string>("SpawnJSTests.asyncReturn", "done");
                AssertEqual(value, "done", "an awaited JS promise must yield its resolved value");
            });

            await TestAsync("TaskMarshaller.InRejectedWithError", async () =>
            {
                try
                {
                    await JS.CallAsync<string, string>("SpawnJSTests.asyncThrow", "boom");
                    throw new Exception("a rejected promise must surface as a .Net exception");
                }
                catch (Exception ex) when (ex.Message.Contains("boom"))
                {
                    // the rejection reason must survive, not be replaced by a generic message
                }
            });

            // Reading a JS Promise INTO a Task is TaskMarshaller.JSToNet - a different path from the
            // async interop call above, and the one that has to translate every rejection shape.
            await TestAsync("TaskMarshaller.InPromiseAsTaskResolved", async () =>
            {
                var task = JS.Call<string, Task<string>>("SpawnJSTests.resolvedPromise", "ok")!;
                AssertEqual(await task, "ok", "a Promise read as a Task<string> must yield its value");
            });

            await TestAsync("TaskMarshaller.InPromiseAsTaskRejectedWithError", async () =>
            {
                var task = JS.Call<string, Task<string>>("SpawnJSTests.rejectedPromiseError", "err boom")!;
                try
                {
                    await task;
                    throw new Exception("a rejected Promise read as a Task must fault the Task");
                }
                catch (Exception ex) when (ex.Message.Contains("err boom")) { }
            });

            await TestAsync("TaskMarshaller.InRejectedWithTypeError", async () =>
            {
                try
                {
                    await JS.CallAsync<string, string>("SpawnJSTests.rejectedPromiseTypeError", "type boom");
                    throw new Exception("a TypeError rejection must surface as a .Net exception");
                }
                catch (Exception ex) when (ex.Message.Contains("type boom")) { }
            });

            await TestAsync("TaskMarshaller.InRejectedWithString", async () =>
            {
                try
                {
                    await JS.CallAsync<string, string>("SpawnJSTests.rejectedPromiseString", "plain string reason");
                    throw new Exception("a string rejection must surface as a .Net exception");
                }
                catch (Exception ex) when (ex.Message.Contains("plain string reason")) { }
            });

            // A rejection carrying no reason at all still has to become an exception. If the JS side
            // treats "no error object" as "no error", the await resolves as a success and the failure is
            // silently swallowed - which is worse than any message being wrong.
            await TestAsync("TaskMarshaller.InRejectedWithNull", async () =>
            {
                try
                {
                    await JS.CallAsync<string>("SpawnJSTests.rejectedPromiseNull");
                    throw new Exception("a null rejection must still surface as a .Net exception, not succeed");
                }
                catch (Exception ex) when (!ex.Message.Contains("must still surface")) { }
            });

            await TestAsync("TaskMarshaller.InRejectedWithUndefined", async () =>
            {
                try
                {
                    await JS.CallAsync<string>("SpawnJSTests.rejectedPromiseUndefined");
                    throw new Exception("an undefined rejection must still surface as a .Net exception, not succeed");
                }
                catch (Exception ex) when (!ex.Message.Contains("must still surface")) { }
            });

            await TestAsync("TaskMarshaller.InSynchronousThrow", async () =>
            {
                try
                {
                    await JS.CallAsync<string, string>("SpawnJSTests.throwSync", "sync boom");
                    throw new Exception("a synchronous throw in an async call must surface as a .Net exception");
                }
                catch (Exception ex) when (ex.Message.Contains("sync boom")) { }
            });

            await TestAsync("TaskMarshaller.OutCompletedTaskOfT", async () =>
            {
                JS.Set(K, Task.FromResult(42));
                AssertEqual(CtorOfKey(), "Promise", "a Task<T> must cross as a JS Promise");
                using var raw = JS.Get(K)!;
                var outcome = await JS.CallAsync<SpawnJSObjectReference, string>("SpawnJSTests.promiseOutcome", raw);
                AssertEqual(outcome, "resolved:42", "the completed task's RESULT must reach Javascript");
            });

            await TestAsync("TaskMarshaller.OutCompletedTask", async () =>
            {
                JS.Set(K, Task.CompletedTask);
                AssertEqual(CtorOfKey(), "Promise", "a Task must cross as a JS Promise");
                using var raw = JS.Get(K)!;
                var outcome = await JS.CallAsync<SpawnJSObjectReference, string>("SpawnJSTests.promiseOutcome", raw);
                AssertEqual(outcome, "resolved:[undefined]", "a completed void task must resolve, with no value");
            });

            // as above: a Task written as a NAMED MEMBER takes the string-key overload, which is where a
            // completed Task<T> can lose its result and still hand Javascript a valid looking promise
            await TestAsync("TaskMarshaller.OutCompletedTaskOfTOnAnObjectMember", async () =>
            {
                JS.Set(K, new Dictionary<string, Task<int>> { ["work"] = Task.FromResult(42) });
                AssertEqual(JS.ConstructorName($"{K}.work"), "Promise", "a Task<T> member must cross as a JS Promise");
                using var member = JS.Get($"{K}.work")!;
                var outcome = await JS.CallAsync<SpawnJSObjectReference, string>("SpawnJSTests.promiseOutcome", member);
                AssertEqual(outcome, "resolved:42", "the result was dropped on the string-key path");
            });

            await TestAsync("TaskMarshaller.OutPendingTaskOfT", async () =>
            {
                var tcs = new TaskCompletionSource<int>();
                JS.Set(K, tcs.Task);
                AssertEqual(CtorOfKey(), "Promise", "a pending Task<T> must cross as a JS Promise");
                using var raw = JS.Get(K)!;
                var outcomeTask = JS.CallAsync<SpawnJSObjectReference, string>("SpawnJSTests.promiseOutcome", raw);
                tcs.SetResult(7);
                AssertEqual(await outcomeTask, "resolved:7", "resolving the .Net task must resolve the JS promise with its value");
            });

            await TestAsync("TaskMarshaller.OutFaultedTask", async () =>
            {
                var tcs = new TaskCompletionSource<int>();
                JS.Set(K, tcs.Task);
                using var raw = JS.Get(K)!;
                var outcomeTask = JS.CallAsync<SpawnJSObjectReference, string>("SpawnJSTests.promiseOutcome", raw);
                tcs.SetException(new Exception("net side failure"));
                var outcome = await outcomeTask;
                Assert(outcome.StartsWith("rejected:"), $"a faulted .Net task must reject the JS promise, got [{outcome}]");
                Assert(outcome.Contains("net side failure"), $"the .Net exception message must reach Javascript, got [{outcome}]");
            });

            await TestAsync("TaskMarshaller.NullOut", async () =>
            {
                JS.Set(K, (Task<int>?)null);
                AssertEqual(DescribeKey(), "object:null", "a null Task must cross as JS null");
                await Task.CompletedTask;
            });
        }

        // ==========================================================================================
        // BigIntegerMarshaller / BigIntegerNullableMarshaller - .Net BigInteger <-> JS BigInt
        // ==========================================================================================
        static void BigIntegerMarshallerTests()
        {
            Test("BigIntegerMarshaller.RoundTrip", () =>
            {
                var value = BigInteger.Parse("123456789012345678901234567890");
                JS.Set(K, value);
                AssertEqual(TypeOfKey(), "bigint", "a BigInteger must cross as a JS BigInt");
                AssertEqual(StrKey(), "123456789012345678901234567890", "value");
                AssertEqual(JS.Get<BigInteger>(K), value, "round trip");
            });

            Test("BigIntegerMarshaller.ExactPast2Pow53", () =>
            {
                // the reason BigInteger exists here: a value a JS number cannot hold exactly
                var value = new BigInteger(9007199254740993L); // 2^53+1
                JS.Set(K, value);
                AssertEqual(StrKey(), "9007199254740993", "a BigInt must not lose the low bit the way an f64 does");
                AssertEqual(JS.Get<BigInteger>(K), value, "round trip");
            });

            Test("BigIntegerMarshaller.Negative", () =>
            {
                var value = BigInteger.Parse("-98765432109876543210");
                JS.Set(K, value);
                AssertEqual(JS.Get<BigInteger>(K), value, "round trip");
            });

            Test("BigIntegerMarshaller.In", () =>
            {
                var back = JS.Call<string, BigInteger>("SpawnJSTests.bigIntFrom", "170141183460469231731687303715884105727");
                AssertEqual(back, BigInteger.Parse("170141183460469231731687303715884105727"), "read of a JS BigInt");
            });

            Test("BigIntegerNullableMarshaller.RoundTrip", () =>
            {
                BigInteger? some = new BigInteger(42);
                JS.Set(K, some);
                AssertEqual(TypeOfKey(), "bigint", "typeof");
                AssertEqual(JS.Get<BigInteger?>(K), some, "value round trip");

                BigInteger? none = null;
                JS.Set(K, none);
                AssertEqual(DescribeKey(), "object:null", "a null BigInteger? must cross as JS null");
                AssertEqual(JS.Get<BigInteger?>(K), null, "null round trip");
                AssertEqual(JS.Get<BigInteger?>("__mt_missing"), null, "absent property as BigInteger?");
            });
        }

        // ==========================================================================================
        // UnionMarshaller - .Net Union <-> JS Any
        // ==========================================================================================
        static void UnionMarshallerTests()
        {
            Test("UnionMarshaller.NumberArm", () =>
            {
                Union<string, int> u = 42;
                JS.Set(K, u);
                AssertEqual(TypeOfKey(), "number", "the int arm must cross as a JS number");
                var back = JS.Get<Union<string, int>>(K)!;
                Assert(back.Is<int>(), "the arm was not resolved as int");
                AssertEqual((int)back, 42, "value");
            });

            Test("UnionMarshaller.StringArm", () =>
            {
                Union<string, int> u = "hello";
                JS.Set(K, u);
                AssertEqual(TypeOfKey(), "string", "the string arm must cross as a JS string");
                var back = JS.Get<Union<string, int>>(K)!;
                Assert(back.Is<string>(), "the arm was not resolved as string");
                AssertEqual((string)back!, "hello", "value");
            });

            Test("UnionMarshaller.InFromJavascript", () =>
            {
                var back = JS.Call<string, Union<string, int>>("SpawnJSTests.numberFrom", "7")!;
                Assert(back.Is<int>(), "a JS number must resolve to the int arm");
                AssertEqual((int)back, 7, "value");
            });

            Test("UnionMarshaller.NullOut", () =>
            {
                JS.Set(K, (Union<string, int>?)null);
                AssertEqual(DescribeKey(), "object:null", "a null Union must cross as JS null");
            });
        }

        // ==========================================================================================
        // DelegateMarshaller - .Net Action, Action<>, Func<> -> JS Function   (OUT ONLY)
        // ==========================================================================================
        static void DelegateMarshallerTests()
        {
            Test("DelegateMarshaller.ActionOut", () =>
            {
                var called = false;
                Action act = () => { called = true; };
                JS.Set(K, act);
                AssertEqual(TypeOfKey(), "function", "an Action must cross as a JS function");
                JS.CallVoid(K);
                Assert(called, "the Action did not run when Javascript called it");
            });

            Test("DelegateMarshaller.ActionWithArgumentOut", () =>
            {
                var got = 0;
                Action<int> act = x => { got = x; };
                JS.Set(K, act);
                JS.CallVoid(K, 99);
                AssertEqual(got, 99, "the argument did not reach the Action");
            });

            Test("DelegateMarshaller.FuncOut", () =>
            {
                Func<int, int> dbl = x => x * 2;
                JS.Set(K, dbl);
                AssertEqual(TypeOfKey(), "function", "a Func must cross as a JS function");
                AssertEqual(JS.Call<int, int>(K, 21), 42, "the Func's return value did not reach Javascript");
            });

            Test("DelegateMarshaller.NullOut", () =>
            {
                JS.Set(K, (Action?)null);
                Assert(TypeOfKey() != "function", "a null Action must not produce a JS function");
            });
        }

        // ==========================================================================================
        // SpawnJSObjectMarshaller - .Net SpawnJSObject <-> JS Any
        // ==========================================================================================
        static void SpawnJSObjectMarshallerTests()
        {
            Test("SpawnJSObjectMarshaller.RoundTrip", () =>
            {
                using var src = new Uint8Array(new byte[] { 1, 2, 3 });
                JS.Set(K, src);
                AssertEqual(CtorOfKey(), "Uint8Array", "a wrapper must cross as the object it wraps");
                using var back = JS.Get<Uint8Array>(K)!;
                AssertEqual(back.Length, 3L, "length through the wrapper");
                AssertEqual(Show(back.ReadBytes()), "1,2,3", "contents through the wrapper");
            });

            Test("SpawnJSObjectMarshaller.WrapsTheSameJavascriptObject", () =>
            {
                using var src = new Uint8Array(new byte[] { 5 });
                JS.Set(K, src);
                using var back = JS.Get<Uint8Array>(K)!;
                Assert(JS.Call<Uint8Array, Uint8Array, bool>("SpawnJSTests.same", src, back),
                    "the wrapper read back a different Javascript object");
            });

            Test("SpawnJSObjectMarshaller.In", () =>
            {
                using var back = JS.Call<string, string, Uint8Array>("SpawnJSTests.typedArray", "Uint8Array", "1,2")!;
                AssertEqual(back.Length, 2L, "a JS TypedArray must read back through its wrapper");
            });

            Test("SpawnJSObjectMarshaller.NullOut", () =>
            {
                JS.Set(K, (Uint8Array?)null);
                AssertEqual(DescribeKey(), "object:null", "a null wrapper must cross as JS null");
            });

            Test("SpawnJSObjectMarshaller.NullIn", () =>
            {
                AssertEqual(JS.Call<Uint8Array?>("SpawnJSTests.nullValue"), null, "JS null must read as a null wrapper");
                AssertEqual(JS.Call<Uint8Array?>("SpawnJSTests.undefinedValue"), null, "JS undefined must read as a null wrapper");
                AssertEqual(JS.Get<Uint8Array?>("__mt_missing"), null, "an absent property must read as a null wrapper");
            });
        }

        // ==========================================================================================
        // EnumMarshaller / EnumNullableMarshaller - .Net Enum <-> JS Number
        // ==========================================================================================
        [Flags]
        public enum Usage : uint { None = 0, CopySrc = 0x0004, CopyDst = 0x0008, Storage = 0x0080 }
        public enum Small : byte { A = 1, B = 2 }
        public enum Big : long { Low = 1, High = 4294967296 }

        static void EnumMarshallerTests()
        {
            Test("EnumMarshaller.RoundTrip", () =>
            {
                const Usage v = Usage.CopySrc | Usage.Storage;
                JS.Set(K, v);
                AssertEqual(TypeOfKey(), "number", "an enum must cross as a JS number, which is what a web API sees");
                AssertEqual(StrKey(), "132", "the numeric value (0x84)");
                AssertEqual(JS.Get<Usage>(K), v, "round trip");
            });

            Test("EnumMarshaller.NonInt32BackedEnum", () =>
            {
                // a byte-backed enum: a direct unbox to TEnum would throw, Enum.ToObject must be used
                JS.Set(K, Small.B);
                AssertEqual(StrKey(), "2", "value");
                AssertEqual(JS.Get<Small>(K), Small.B, "byte-backed enum round trip");
            });

            Test("EnumMarshaller.NullableRoundTrip", () =>
            {
                Usage? some = Usage.CopyDst;
                JS.Set(K, some);
                AssertEqual(TypeOfKey(), "number", "typeof");
                AssertEqual(JS.Get<Usage?>(K), some, "value round trip");

                Usage? none = null;
                JS.Set(K, none);
                // a wrong write here produces the NUMBER 0, which reads back as Usage.None and looks fine
                AssertEqual(JS.Get<double?>(K), null, "a null enum? must cross as JS null, NOT as the number 0");
                AssertEqual(JS.Get<Usage?>(K), null, "null round trip");
                AssertEqual(JS.Get<Usage?>("__mt_missing"), null, "absent property as an enum?");
            });

            Test("EnumMarshaller.In", () =>
            {
                AssertEqual(JS.Call<string, Usage>("SpawnJSTests.numberFrom", "132"), Usage.CopySrc | Usage.Storage, "read of a JS number as an enum");
            });
        }

        // ==========================================================================================
        // EnumStringMarshaller - .Net EnumString<> <-> JS String?
        // ==========================================================================================
        static void EnumStringMarshallerTests()
        {
            Test("EnumStringMarshaller.RoundTrip", () =>
            {
                EnumString<VideoFacingModeEnum> v = VideoFacingModeEnum.Left;
                JS.Set(K, v);
                AssertEqual(TypeOfKey(), "string", "an EnumString must cross as a JS string");
                AssertEqual(StrKey(), "left", "the Javascript string the member maps to");
                var back = JS.Get<EnumString<VideoFacingModeEnum>>(K)!;
                AssertEqual(back.String, "left", "round trip string");
                Assert(back.IsDefined, "the round tripped value must resolve to a known member");
            });

            Test("EnumStringMarshaller.UnknownStringSurvives", () =>
            {
                // an EnumString carries a value the enum does not know, with IsDefined false - that is
                // what lets a wrapper survive a web API adding a value
                JS.Set(K, "some-future-value");
                var back = JS.Get<EnumString<VideoFacingModeEnum>>(K)!;
                AssertEqual(back.String, "some-future-value", "an unknown string must be carried, not dropped");
                Assert(!back.IsDefined, "an unknown string must report IsDefined false");
            });

            Test("EnumStringMarshaller.NullOut", () =>
            {
                JS.Set(K, (EnumString<VideoFacingModeEnum>?)null);
                AssertEqual(DescribeKey(), "object:null", "a null EnumString must cross as JS null");
            });

            Test("EnumStringMarshaller.UndefinedIn", () =>
            {
                AssertEqual(JS.Get<EnumString<VideoFacingModeEnum>?>("__mt_missing"), null, "an absent property must read as null");
            });
        }

        // ==========================================================================================
        // EpochDateTimeMarshaller - .Net EpochDateTime <-> JS Number?
        // ==========================================================================================
        static void EpochDateTimeMarshallerTests()
        {
            Test("EpochDateTimeMarshaller.RoundTrip", () =>
            {
                var when = new DateTime(2026, 8, 17, 12, 34, 56, DateTimeKind.Utc);
                EpochDateTime v = when;
                JS.Set(K, v);
                AssertEqual(TypeOfKey(), "number", "an EpochDateTime must cross as a JS number");
                var back = JS.Get<EpochDateTime>(K)!;
                AssertEqual(back.ValueEpoch, v.ValueEpoch, "epoch milliseconds round trip");
            });

            Test("EpochDateTimeMarshaller.In", () =>
            {
                var back = JS.Call<string, EpochDateTime>("SpawnJSTests.numberFrom", "1000000000000")!;
                AssertEqual(back.ValueEpoch, 1000000000000L, "read of a JS number as an EpochDateTime");
            });

            Test("EpochDateTimeMarshaller.UndefinedIn", () =>
            {
                AssertEqual(JS.Get<EpochDateTime?>("__mt_missing"), null, "an absent property must read as null");
            });
        }

        // ==========================================================================================
        // DateTimeMarshaller / DateTimeNullableMarshaller - .Net DateTime <-> JS String
        // ==========================================================================================
        static void DateTimeMarshallerTests()
        {
            Test("DateTimeMarshaller.RoundTrip", () =>
            {
                var when = new DateTime(2026, 8, 17, 12, 34, 56, DateTimeKind.Utc);
                JS.Set(K, when);
                AssertEqual(TypeOfKey(), "string", "a DateTime must cross as a round trip ISO 8601 string");
                AssertEqual(JS.Get<DateTime>(K), when, "round trip");
            });

            // Javascript hands a moment back as a Date, an epoch number, or a string depending on the
            // API, so all three shapes must read. An epoch value arrives through EpochTimeToDateTime,
            // which converts to LOCAL time - note this differs in Kind from the ISO string a .Net write
            // produces, so a round trip through JS is not Kind preserving.
            static DateTime ExpectedLocal(long epochMs) => DateTimeOffset.FromUnixTimeMilliseconds(epochMs).UtcDateTime.ToLocalTime();

            Test("DateTimeMarshaller.InFromJavascriptDateObject", () =>
            {
                var back = JS.Call<double, DateTime>("SpawnJSTests.dateFrom", 1000000000000d);
                AssertEqual(back, ExpectedLocal(1000000000000), "read of a JS Date object");
            });

            Test("DateTimeMarshaller.InFromJavascriptNumber", () =>
            {
                var back = JS.Call<string, DateTime>("SpawnJSTests.numberFrom", "1000000000000");
                AssertEqual(back, ExpectedLocal(1000000000000), "read of an epoch number");
            });

            Test("DateTimeNullableMarshaller.RoundTrip", () =>
            {
                DateTime? some = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc);
                JS.Set(K, some);
                AssertEqual(JS.Get<DateTime?>(K), some, "value round trip");

                DateTime? none = null;
                JS.Set(K, none);
                AssertEqual(DescribeKey(), "object:null", "a null DateTime? must cross as JS null");
                AssertEqual(JS.Get<DateTime?>(K), null, "null round trip");
                AssertEqual(JS.Get<DateTime?>("__mt_missing"), null, "absent property as DateTime?");
            });
        }

        // ==========================================================================================
        // HeapViewMarshaller - .Net HeapView -> JS ArrayBufferView   (OUT ONLY)
        // ==========================================================================================
        static void HeapViewMarshallerTests()
        {
            Test("HeapViewMarshaller.Out", () =>
            {
                var data = new byte[] { 10, 20, 30 };
                using var heapView = HeapView.Create(data);
                JS.Set(K, heapView);
                AssertEqual(CtorOfKey(), "Uint8Array", "a HeapView must cross as its view type");
                AssertEqual(ElementsKey(), "10,20,30", "elements");
            });

            Test("HeapViewMarshaller.NullOut", () =>
            {
                JS.Set(K, (HeapView?)null);
                AssertEqual(DescribeKey(), "object:null", "a null HeapView must cross as JS null");
            });
        }

        // ==========================================================================================
        // HeapView - pinned .Net memory seen by Javascript as a real TypedArray
        // ==========================================================================================

        // One live view per element type / view type pair. Every sizing mistake hides in the byte==element
        // case, so each pair asserts BOTH geometries: length in ELEMENTS and byteLength in BYTES.
        // ==========================================================================================
        // JsonElementMarshaller - .Net JsonElement <-> JS any            (ReturnType.Json)
        // ==========================================================================================
        // This is the one marshaller whose MECHANISM is JSON: the JS side JSON.stringify's the value and
        // .Net parses the string (and on the way out the raw JSON text goes over and JS JSON.parse's it).
        // So the suite's rule matters more here, not less - JSON must never be used to VERIFY it, or a
        // marshaller wrong in both directions round trips perfectly. OUT is checked by asking Javascript
        // for typeof / constructor / own keys / String(v) and by reading members back through OTHER
        // marshallers; IN is checked against values Javascript created.
        //
        // The point of the marshaller is that a JsonElement lands as a REAL Javascript value - an object
        // is a JS object with live members, not a string that happens to contain JSON. Asserting typeof
        // and reading members individually is what tells those two apart.

        // Parse rather than JsonSerializer.Deserialize: this app runs with
        // System.Text.Json.JsonSerializer.IsReflectionEnabledByDefault=false (it is in the built
        // runtimeconfig.json), so the reflection-based serializer THROWS at runtime here. That is the
        // same constraint the marshaller is under, so the instrument must not need anything the thing
        // under test is not allowed to use.
        static JsonElement Json(string json)
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.Clone();
        }

        static void JsonElementMarshallerTests()
        {
            Test("JsonElementMarshaller.OutObject", () =>
            {
                JS.Set(K, Json("""{"a":1,"b":"two","c":true}"""));
                AssertEqual(CtorOfKey(), "Object", "a JsonElement object must cross as a plain JS object");
                AssertEqual(OwnKeysOf(), "a,b,c", "own keys");
                // read the members back through OTHER marshallers - this is what proves they are live JS
                // values rather than one JSON string parked at the key
                AssertEqual(JS.TypeOf($"{K}.a"), "number", "member a is a real JS number");
                AssertEqual(JS.TypeOf($"{K}.b"), "string", "member b is a real JS string");
                AssertEqual(JS.TypeOf($"{K}.c"), "boolean", "member c is a real JS boolean");
                AssertEqual(JS.Get<int>($"{K}.a"), 1, "member a value");
                AssertEqual(JS.Get<string>($"{K}.b"), "two", "member b value");
            });

            Test("JsonElementMarshaller.OutArray", () =>
            {
                JS.Set(K, Json("[1,2,3]"));
                AssertEqual(CtorOfKey(), "Array", "a JsonElement array must cross as a JS Array");
                AssertEqual(ElementsKey(), "1,2,3", "elements");
                AssertEqual(JS.Get<int[]>(K)!.Length, 3, "length read through the array marshaller");
            });

            Test("JsonElementMarshaller.OutNested", () =>
            {
                JS.Set(K, Json("""{"outer":{"inner":[10,{"deep":"yes"}]}}"""));
                AssertEqual(JS.TypeOf($"{K}.outer"), "object", "a nested object stays an object");
                AssertEqual(JS.Get<int>($"{K}.outer.inner.0"), 10, "value inside a nested array");
                AssertEqual(JS.Get<string>($"{K}.outer.inner.1.deep"), "yes", "value inside a nested object");
            });

            Test("JsonElementMarshaller.OutString", () =>
            {
                JS.Set(K, Json("\"hi\""));
                AssertEqual(TypeOfKey(), "string", "a JSON string must cross as a JS string primitive");
                AssertEqual(StrKey(), "hi", "value");
            });

            Test("JsonElementMarshaller.OutNumber", () =>
            {
                JS.Set(K, Json("42.5"));
                AssertEqual(TypeOfKey(), "number", "a JSON number must cross as a JS number");
                AssertEqual(StrKey(), "42.5", "value");
            });

            Test("JsonElementMarshaller.OutBoolean", () =>
            {
                JS.Set(K, Json("true"));
                AssertEqual(TypeOfKey(), "boolean", "a JSON true must cross as a JS boolean");
                AssertEqual(StrKey(), "true", "value");
            });

            Test("JsonElementMarshaller.OutNull", () =>
            {
                JS.Set(K, Json("null"));
                AssertEqual(DescribeKey(), "object:null", "a JSON null must cross as JS null");
            });

            // default(JsonElement) is ValueKind.Undefined and has no raw text at all - GetRawText throws
            // on it. It must write JS undefined: that is the symmetric partner of the read (see
            // InUndefinedIsUndefinedKind) and stays distinct from the JSON null above.
            Test("JsonElementMarshaller.OutUndefined", () =>
            {
                JS.Set(K, default(JsonElement));
                // typeof read BY KEY, not DescribeKey: describe() takes the value as an argument, and a
                // JS undefined arrives there as a null reference - so DescribeKey cannot tell undefined
                // from null. typeof is evaluated JS-side on the property itself and can.
                AssertEqual(TypeOfKey(), "undefined", "default(JsonElement) must cross as JS undefined");
                AssertEqual(JS.Get<JsonElement>(K).ValueKind, JsonValueKind.Undefined, "and read back as Undefined");
                // and it must NOT be the same thing a JSON null produces
                JS.Set(K, Json("null"));
                AssertEqual(TypeOfKey(), "object", "a JSON null is JS null, which typeof reports as object");
            });

            // JS.Set marshals its value through the INT-key (call argument) overload. A NAMED MEMBER
            // write is the only thing that reaches NetToJS(parent, string key, ...) - separate code that
            // has drifted from its int-key twin before.
            Test("JsonElementMarshaller.OutStringKeyPath", () =>
            {
                var src = new Dictionary<string, JsonElement>
                {
                    ["obj"] = Json("""{"n":7}"""),
                    ["arr"] = Json("[1,2]"),
                    ["nul"] = Json("null"),
                };
                JS.Set(K, src);
                AssertEqual(OwnKeysOf(), "obj,arr,nul", "own keys");
                AssertEqual(JS.TypeOf($"{K}.obj"), "object", "object written by string key");
                AssertEqual(JS.Get<int>($"{K}.obj.n"), 7, "member of an object written by string key");
                AssertEqual(JS.Get<int>($"{K}.arr.1"), 2, "element of an array written by string key");
                AssertEqual(JS.TypeOf($"{K}.nul"), "object", "a JSON null written by string key is JS null");
            });

            Test("JsonElementMarshaller.InObject", () =>
            {
                // Javascript builds the object, so the read is fed a genuine JS value
                var el = JS.Call<JsonElement>("SpawnJSTests.objectWithNullMember");
                AssertEqual(el.ValueKind, JsonValueKind.Object, "kind");
                AssertEqual(el.GetProperty("present").GetInt32(), 1, "present member");
                AssertEqual(el.GetProperty("absent").ValueKind, JsonValueKind.Null, "a JS null member reads as JSON null");
            });

            Test("JsonElementMarshaller.InArray", () =>
            {
                var el = Js2<string, JsonElement>("numberArray", "1,2,3");
                AssertEqual(el.ValueKind, JsonValueKind.Array, "kind");
                AssertEqual(el.GetArrayLength(), 3, "length");
                AssertEqual(el[2].GetInt32(), 3, "last element");
            });

            Test("JsonElementMarshaller.InPrimitives", () =>
            {
                AssertEqual(Js2<string, JsonElement>("str", "from js").GetString(), "from js", "a JS string");
                AssertEqual(Js2<string, JsonElement>("numberFrom", "42.5").GetDouble(), 42.5, "a JS number");
                AssertEqual(JS.Call<JsonElement>("SpawnJSTests.nullValue").ValueKind, JsonValueKind.Null, "a JS null");
            });

            // JSON.stringify(undefined) yields undefined rather than text, which arrives as a null string
            // and reads back as default(JsonElement). So Undefined is how "there was nothing here" reports,
            // and it stays DISTINCT from a JS null (which stringifies to "null" -> JsonValueKind.Null).
            // That distinction is why JsonElement needs no nullable companion to model absence.
            Test("JsonElementMarshaller.InUndefinedIsUndefinedKind", () =>
            {
                AssertEqual(JS.Call<JsonElement>("SpawnJSTests.undefinedValue").ValueKind, JsonValueKind.Undefined,
                    "JS undefined must read as JsonValueKind.Undefined");
                AssertEqual(JS.Get<JsonElement>("__mt_missing").ValueKind, JsonValueKind.Undefined,
                    "an absent property must read as JsonValueKind.Undefined");
                Assert(JS.Call<JsonElement>("SpawnJSTests.nullValue").ValueKind
                       != JS.Call<JsonElement>("SpawnJSTests.undefinedValue").ValueKind,
                    "JS null and JS undefined must NOT collapse to the same ValueKind");
            });

            // JSON.stringify drops a member whose value is undefined. Pinned because it means a
            // JsonElement read cannot tell "member set to undefined" from "member never written", even
            // though Javascript can - a real limit of this marshaller's mechanism, not a bug in it.
            Test("JsonElementMarshaller.InUndefinedMemberIsDropped", () =>
            {
                var el = JS.Call<JsonElement>("SpawnJSTests.objectWithUndefinedMember");
                AssertEqual(el.ValueKind, JsonValueKind.Object, "kind");
                AssertEqual(el.GetProperty("present").GetInt32(), 1, "the present member survives");
                Assert(!el.TryGetProperty("absent", out _), "a member set to undefined is dropped by JSON.stringify");
                // the member IS there on the JS side - the loss is JSON's, not the crossing's
                using var jsObj = JS.Call<SpawnJSObjectReference>("SpawnJSTests.objectWithUndefinedMember");
                AssertEqual(JS.Call<SpawnJSObjectReference?, string, bool>("SpawnJSTests.hasIn", jsObj, "absent"), true,
                    "Javascript still reports the member as present");
            });

            Test("JsonElementMarshaller.RoundTrip", () =>
            {
                JS.Set(K, Json("""{"n":1,"s":"two","b":false,"arr":[1,2],"obj":{"k":"v"},"nul":null}"""));
                var back = JS.Get<JsonElement>(K);
                AssertEqual(back.ValueKind, JsonValueKind.Object, "kind survives");
                AssertEqual(back.GetProperty("n").GetInt32(), 1, "number");
                AssertEqual(back.GetProperty("s").GetString(), "two", "string");
                AssertEqual(back.GetProperty("b").GetBoolean(), false, "boolean");
                AssertEqual(back.GetProperty("arr").GetArrayLength(), 2, "array length");
                AssertEqual(back.GetProperty("obj").GetProperty("k").GetString(), "v", "nested object");
                AssertEqual(back.GetProperty("nul").ValueKind, JsonValueKind.Null, "null member");
            });

            Test("JsonElementMarshaller.RoundTripNonAscii", () =>
            {
                // an astral-plane pair plus the characters JSON has to escape, written as raw JSON text
                // so both the wire form and the expected .Net string are stated explicitly
                var jsonText = "\"héllo 世界 🚀 \\\" \\\\ \\n\"";
                var value = "héllo 世界 🚀 \" \\ \n";
                JS.Set(K, Json(jsonText));
                AssertEqual(TypeOfKey(), "string", "still a JS string");
                AssertEqual(StrKey(), value, "Javascript sees the unescaped string");
                AssertEqual(JS.Get<JsonElement>(K).GetString(), value, "round trip");
            });
        }

        // Call a SpawnJSTests helper with one argument and read the result as TResult. The existing Js()
        // helper returns string only; JsonElement reads need the typed form.
        static TResult Js2<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TResult>(string fn, T1 arg1)
            => JS.Call<T1, TResult>($"SpawnJSTests.{fn}", arg1);

        static void LiveViewGeometry<TElement, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TView>(
            TElement[] data, int elementSize, string expectedElements)
            where TElement : struct where TView : SpawnJSObject
        {
            using var heapView = HeapView.Create<TElement, TView>(data);
            var name = $"{typeof(TElement).Name}->{typeof(TView).Name}";
            AssertEqual(Js("viewCtor", heapView.View), typeof(TView).Name, $"{name}: view constructor");
            AssertEqual(JS.Call<TView, double>("SpawnJSTests.viewLength", heapView.View), data.Length, $"{name}: length must be the ELEMENT count");
            AssertEqual(JS.Call<TView, double>("SpawnJSTests.viewByteLength", heapView.View), data.Length * elementSize, $"{name}: byteLength must be elements * element size");
            AssertEqual(JS.Call<TView, string>("SpawnJSTests.elements", heapView.View), expectedElements, $"{name}: elements");
            Assert(JS.Call<TView, bool>("SpawnJSTests.isOnWasmHeap", heapView.View), $"{name}: a live view must be backed by the WebAssembly heap itself");
        }

        static void HeapViewTests()
        {
            Test("HeapView.LiveViewGeometryEveryElementType", () =>
            {
                LiveViewGeometry<byte, Uint8Array>(new byte[] { 1, 2, 255 }, 1, "1,2,255");
                LiveViewGeometry<sbyte, Int8Array>(new sbyte[] { -1, 2, -128 }, 1, "-1,2,-128");
                LiveViewGeometry<ushort, Uint16Array>(new ushort[] { 1, 65535 }, 2, "1,65535");
                LiveViewGeometry<short, Int16Array>(new short[] { -1, 32767 }, 2, "-1,32767");
                LiveViewGeometry<uint, Uint32Array>(new uint[] { 1, 4294967295 }, 4, "1,4294967295");
                LiveViewGeometry<int, Int32Array>(new int[] { -1, 2147483647 }, 4, "-1,2147483647");
                LiveViewGeometry<float, Float32Array>(new float[] { 1.5f, -2.5f }, 4, "1.5,-2.5");
                LiveViewGeometry<double, Float64Array>(new double[] { 1.5, -2.5 }, 8, "1.5,-2.5");
                LiveViewGeometry<ulong, BigUint64Array>(new ulong[] { 1, 18446744073709551615 }, 8, "1,18446744073709551615");
                LiveViewGeometry<long, BigInt64Array>(new long[] { -1, 9223372036854775807 }, 8, "-1,9223372036854775807");
            });

            Test("HeapView.Float16ViewGeometry", () =>
            {
                if (JS.Get<SpawnJSObjectReference?>("Float16Array") == null) Skip("Float16Array is not available in this host");
                LiveViewGeometry<Half, Float16Array>(new[] { (Half)1.5f, (Half)(-2.5f) }, 2, "1.5,-2.5");
            });

            Test("HeapView.Uint8ClampedViewGeometry", () =>
                LiveViewGeometry<byte, Uint8ClampedArray>(new byte[] { 0, 128, 255 }, 1, "0,128,255"));

            Test("HeapView.CrossTypeViewIsSizedByTheTargetElement", () =>
            {
                // a double[] seen as BYTES. The view must be sized by the TARGET element size - sizing it
                // by the source builds a view 8x too long, which looks like a valid Uint8Array until its
                // tail is touched. This exact bug has shipped before.
                var data = new double[] { 1.0, 2.0, 3.0 };
                using var heapView = HeapView.Create<double, Uint8Array>(data);
                var expectedBytes = data.Length * sizeof(double);
                AssertEqual(JS.Call<Uint8Array, double>("SpawnJSTests.viewLength", heapView.View), expectedBytes, "a byte view of a double[] must be byteLength elements long");
                AssertEqual(JS.Call<Uint8Array, double>("SpawnJSTests.viewByteLength", heapView.View), expectedBytes, "byteLength");
                // and the bytes must be the doubles' actual little-endian bytes, not some other memory
                var expectedHex = string.Concat(data.SelectMany(BitConverter.GetBytes).Select(b => b.ToString("x2")));
                AssertEqual(JS.Call<Uint8Array, string>("SpawnJSTests.bytesHex", heapView.View), expectedHex, "the bytes Javascript sees are not the .Net doubles' bytes");
            });

            Test("HeapView.LiveViewSeesLaterDotnetWrites", () =>
            {
                var data = new byte[] { 0, 0, 0 };
                using var heapView = HeapView.Create(data);
                data[1] = 77;
                AssertEqual(JS.Call<Uint8Array, string>("SpawnJSTests.elements", heapView.View), "0,77,0",
                    "a live view must show a .Net write made after the view was created");
            });

            Test("HeapView.JavascriptWritesReachTheDotnetArray", () =>
            {
                var data = new byte[3];
                using var heapView = HeapView.Create(data);
                JS.CallVoid<Uint8Array, int, int>("SpawnJSTests.writeElement", heapView.View, 0, 111);
                JS.CallVoid<Uint8Array, int, int>("SpawnJSTests.writeElement", heapView.View, 2, 133);
                AssertEqual(Show(data), "111,0,133", "a Javascript write did not land in the .Net array - the view is a copy");
            });

            Test("HeapView.JavascriptWritesReachTheDotnetArrayFloat", () =>
            {
                var data = new float[3];
                using var heapView = HeapView.Create(data);
                JS.CallVoid<Float32Array, int, double>("SpawnJSTests.writeElement", heapView.View, 1, -9.5);
                AssertEqual(data[1], -9.5f, "a Javascript write to a Float32Array view did not land in the .Net float[]");
            });

            Test("HeapView.CopyIsIndependentOfTheDotnetArray", () =>
            {
                var data = new byte[] { 1, 2, 3 };
                using var heapView = HeapView.Create<byte, Uint8Array>(data, copy: true);
                Assert(!JS.Call<Uint8Array, bool>("SpawnJSTests.isOnWasmHeap", heapView.View), "a copy must NOT be backed by the WebAssembly heap");
                data[0] = 200;
                AssertEqual(JS.Call<Uint8Array, string>("SpawnJSTests.elements", heapView.View), "1,2,3", "a copy must not see later .Net writes");
                JS.CallVoid<Uint8Array, int, int>("SpawnJSTests.writeElement", heapView.View, 1, 99);
                AssertEqual(data[1], (byte)2, "a Javascript write to a copy must not reach the .Net array");
            });

            Test("HeapView.CopyRefreshPicksUpNewData", () =>
            {
                var data = new byte[] { 1, 2, 3 };
                using var heapView = HeapView.Create<byte, Uint8Array>(data, copy: true);
                data[0] = 200;
                heapView.RefreshCopy();
                AssertEqual(JS.Call<Uint8Array, string>("SpawnJSTests.elements", heapView.View), "200,2,3", "RefreshCopy must rebuild the copy from current .Net memory");
            });

            Test("HeapView.LiveViewIsMarkedForReattach", () =>
            {
                var data = new byte[] { 1, 2, 3 };
                using var live = HeapView.Create<byte, Uint8Array>(data, copy: false);
                Assert(JS.Call<Uint8Array, bool>("SpawnJSTests.hasHeapViewInfo", live.View), "a live view must carry the descriptor the reviver reattaches from");
                using var copy = HeapView.Create<byte, Uint8Array>(data, copy: true);
                Assert(!JS.Call<Uint8Array, bool>("SpawnJSTests.hasHeapViewInfo", copy.View), "a copy needs no reattach descriptor and must not carry one");
            });

            Test("HeapView.ArrayBufferCopy", () =>
            {
                var data = new byte[] { 4, 5, 6 };
                using var arrayBuffer = (ArrayBuffer)data;
                AssertEqual(Js("viewCtor", arrayBuffer), "ArrayBuffer", "an ArrayBuffer cast must produce an ArrayBuffer");
                AssertEqual(JS.Call<ArrayBuffer, double>("SpawnJSTests.viewByteLength", arrayBuffer), 3, "byteLength");
                AssertEqual(JS.Call<ArrayBuffer, string>("SpawnJSTests.bytesHex", arrayBuffer), "040506", "bytes");
            });

            Test("HeapView.StringSource", () =>
            {
                // HeapView.Create(string) pins the string's UTF-16 characters and views them as bytes
                var source = "AB";
                using var heapView = HeapView.Create(source);
                AssertEqual(JS.Call<Uint8Array, double>("SpawnJSTests.viewByteLength", heapView.View), source.Length * 2, "a string view spans two bytes per char");
                // 'A' = 0x0041, 'B' = 0x0042, little endian UTF-16
                AssertEqual(JS.Call<Uint8Array, string>("SpawnJSTests.bytesHex", heapView.View), "41004200", "the bytes Javascript sees are not the string's UTF-16 bytes");
            });

            Test("HeapView.EmptySource", () =>
            {
                using var heapView = HeapView.Create(System.Array.Empty<byte>());
                AssertEqual(JS.Call<Uint8Array, double>("SpawnJSTests.viewLength", heapView.View), 0, "an empty source must produce an empty view");
            });

            Test("HeapView.LiveViewSurvivesHeapGrowth", () =>
            {
                // growing the WASM heap DETACHES every existing ArrayBuffer. A live view must reattach on
                // its next use or every zero copy path silently breaks after the first allocation spike.
                var data = new byte[] { 1, 2, 3 };
                using var heapView = HeapView.Create(data);
                var grew = JS.GrowHeap();
                if (grew <= 0) Skip("the heap did not grow, so reattach was not exercised");
                AssertEqual(JS.Call<Uint8Array, string>("SpawnJSTests.elements", heapView.View), "1,2,3",
                    "a live view did not reattach after the heap grew and detached its buffer");
                Assert(JS.Call<Uint8Array, bool>("SpawnJSTests.isOnWasmHeap", heapView.View), "after reattach the view must be on the CURRENT heap buffer");
            });

            // A TElement[] binds to the Memory<TElement> overload, so every test above exercises the
            // Memory lane and NONE of them touched ReadOnlyMemory. The ReadOnlyMemory ctor read the
            // wrong backing field (_memorySource, which it never sets) and threw on EVERY call - the
            // whole lane was dead, and nothing here noticed. These cast explicitly to pin the lane.

            Test("HeapView.ReadOnlyMemoryLiveView", () =>
            {
                var data = new byte[] { 7, 8, 9 };
                using var heapView = HeapView.Create<byte, Uint8Array>((ReadOnlyMemory<byte>)data);
                AssertEqual(JS.Call<Uint8Array, double>("SpawnJSTests.viewLength", heapView.View), data.Length,
                    "a ReadOnlyMemory view must be the ELEMENT count long");
                AssertEqual(JS.Call<Uint8Array, double>("SpawnJSTests.viewByteLength", heapView.View), data.Length,
                    "byteLength");
                AssertEqual(JS.Call<Uint8Array, string>("SpawnJSTests.elements", heapView.View), "7,8,9",
                    "a ReadOnlyMemory view must show the source elements");
                Assert(JS.Call<Uint8Array, bool>("SpawnJSTests.isOnWasmHeap", heapView.View),
                    "a live ReadOnlyMemory view must be backed by the WebAssembly heap itself");
            });

            Test("HeapView.ReadOnlyMemoryLiveViewSeesLaterDotnetWrites", () =>
            {
                var data = new byte[] { 0, 0, 0 };
                using var heapView = HeapView.Create<byte, Uint8Array>((ReadOnlyMemory<byte>)data);
                data[1] = 77;
                AssertEqual(JS.Call<Uint8Array, string>("SpawnJSTests.elements", heapView.View), "0,77,0",
                    "a live ReadOnlyMemory view must show a .Net write made after the view was created");
            });

            Test("HeapView.ReadOnlyMemoryMultiByteElementGeometry", () =>
            {
                // sizing off the wrong field is invisible when sizeof(TElement) == 1
                var data = new double[] { 1.5, -2.5 };
                using var heapView = HeapView.Create<double, Float64Array>((ReadOnlyMemory<double>)data);
                AssertEqual(JS.Call<Float64Array, double>("SpawnJSTests.viewLength", heapView.View), data.Length, "length in ELEMENTS");
                AssertEqual(JS.Call<Float64Array, double>("SpawnJSTests.viewByteLength", heapView.View), data.Length * 8, "byteLength in BYTES");
                AssertEqual(JS.Call<Float64Array, string>("SpawnJSTests.elements", heapView.View), "1.5,-2.5", "elements");
            });

            Test("HeapView.ReadOnlyMemoryCopyIsIndependentOfTheDotnetArray", () =>
            {
                var data = new byte[] { 1, 2, 3 };
                using var heapView = HeapView.Create<byte, Uint8Array>((ReadOnlyMemory<byte>)data, copy: true);
                Assert(!JS.Call<Uint8Array, bool>("SpawnJSTests.isOnWasmHeap", heapView.View), "a copy must NOT be backed by the WebAssembly heap");
                data[0] = 200;
                AssertEqual(JS.Call<Uint8Array, string>("SpawnJSTests.elements", heapView.View), "1,2,3", "a copy must not see later .Net writes");
            });

            Test("HeapView.ReadOnlyMemoryCreateCopyReturnsTheView", () =>
            {
                // CreateCopy is the path Blob(byte[][]) and friends take - it was dead too
                var data = new byte[] { 4, 5, 6 };
                using var view = HeapView.CreateCopy<byte, Uint8Array>((ReadOnlyMemory<byte>)data);
                AssertEqual(Js("viewCtor", view), "Uint8Array", "CreateCopy must return the view itself");
                AssertEqual(JS.Call<Uint8Array, string>("SpawnJSTests.elements", view), "4,5,6", "the copy must hold the source elements");
            });

            Test("HeapView.ReadOnlyMemoryEmptySource", () =>
            {
                using var heapView = HeapView.Create<byte, Uint8Array>((ReadOnlyMemory<byte>)System.Array.Empty<byte>());
                AssertEqual(JS.Call<Uint8Array, double>("SpawnJSTests.viewLength", heapView.View), 0, "an empty ReadOnlyMemory source must produce an empty view");
            });

            Test("HeapView.DisposingAViewWhoseCtorThrewDoesNotThrow", () =>
            {
                // A ctor that throws before assigning View still leaves an instance for the finalizer,
                // and an exception escaping a finalizer is FATAL on the WASM runtime: it exits with 255
                // and EVERY later interop call fails with "Assert failed: .NET runtime already exited
                // with 255". One broken ctor took down a whole browser test run that way.
                // GetUninitializedObject reproduces that state exactly - all fields at their zero value,
                // View null - which is why the dispose flag has to be safe at its DEFAULT and not rely
                // on a field initializer the throwing ctor never got to run.
                var halfBuilt = (HeapView<byte, Uint8Array>)System.Runtime.CompilerServices.RuntimeHelpers
                    .GetUninitializedObject(typeof(HeapView<byte, Uint8Array>));
                halfBuilt.Dispose();
                // and the runtime is still here to say so
                JS.Set(K, 1234);
                AssertEqual(JS.Get<int>(K), 1234, "the runtime must still be alive after disposing a view whose ctor threw");
            });

            // The abstract TypedArray base as a STATIC argument type - the shape every
            // FileSystemWritableFileStream.Write / AsyncFileSystem write goes through. Marshaller
            // selection is by the DECLARED type, so a view that crosses correctly as Uint8Array can
            // still cross wrong when the parameter is typed as its base class.
            Test("HeapView.ViewCrossesCorrectlyWhenTypedAsTheTypedArrayBase", () =>
            {
                using var view = HeapView.CreateCopy(new ReadOnlyMemory<byte>(new byte[] { 1, 2, 3 }));
                TypedArray asBase = view;
                AssertEqual(JS.Call<TypedArray, string>("SpawnJSTests.viewCtor", asBase), "Uint8Array",
                    "a TypedArray-typed argument must still cross as its concrete view");
                AssertEqual(JS.Call<TypedArray, string>("SpawnJSTests.elements", asBase), "1,2,3",
                    "a TypedArray-typed argument must carry its elements");
            });

            Test("HeapView.UnsupportedViewTypeThrowsAndLeavesNothingBehind", () =>
            {
                // Blob is a SpawnJSObject but not an ArrayBufferView, so the ctor throws at the view
                // type lookup - after the field initializers, before View is assigned: the exact state
                // the test above disposes. This one pins that the ctor still reports the real error.
                var threw = false;
                try
                {
                    HeapView.Create<byte, Blob>(new byte[] { 1, 2, 3 });
                }
                catch (NotImplementedException)
                {
                    threw = true;
                }
                Assert(threw, "an unsupported view type must throw NotImplementedException");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                JS.Set(K, 4321);
                AssertEqual(JS.Get<int>(K), 4321, "the runtime must still be alive after the failed construction is collected");
            });
        }

        // ==========================================================================================
        // TypedArray read/write - the methods that take .Net types and go through HeapView
        // ==========================================================================================
        static void TypedArrayHeapViewTests()
        {
            Test("TypedArray.WriteAndReadBytes", () =>
            {
                using var target = new Uint8Array(5);
                target.WriteBytes(new byte[] { 1, 2, 3, 4, 5 });
                AssertEqual(Show(target.ReadBytes()), "1,2,3,4,5", "WriteBytes/ReadBytes round trip");
                AssertEqual(Show(target.ReadBytes(2)), "3,4,5", "ReadBytes at a byte offset");
                AssertEqual(Show(target.ReadBytes(1, 2)), "2,3", "ReadBytes with a byte length");
            });

            Test("TypedArray.WriteAtAByteOffset", () =>
            {
                using var target = new Uint8Array(6);
                target.Write(new byte[] { 7, 8 }, 2);
                AssertEqual(Show(target.ReadBytes()), "0,0,7,8,0,0", "Write must land at the requested byte offset");
            });

            Test("TypedArray.WriteSubRange", () =>
            {
                using var target = new Uint8Array(4);
                target.Write(new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 2, 3);
                AssertEqual(Show(target.ReadBytes()), "3,4,5,0", "Write(src, destByteOffset, srcOffset, length)");
            });

            Test("TypedArray.WriteOutOfBoundsThrows", () =>
            {
                using var target = new Uint8Array(2);
                try
                {
                    target.Write(new byte[] { 1, 2, 3 });
                    throw new Exception("writing past the end must throw, not corrupt neighbouring memory");
                }
                catch (NotImplementedException) { }
            });

            // Every element type through the generic Write<T>/Read<T> path, which builds a cross type
            // Uint8Array HeapView internally - the exact place a wrong element size does damage.
            Test("TypedArray.WriteReadEveryElementType", () =>
            {
                WriteReadRoundTrip(new byte[] { 1, 2, 255 });
                WriteReadRoundTrip(new sbyte[] { -1, 2, -128 });
                WriteReadRoundTrip(new ushort[] { 1, 65535 });
                WriteReadRoundTrip(new short[] { -1, 32767 });
                WriteReadRoundTrip(new uint[] { 1, 4294967295 });
                WriteReadRoundTrip(new int[] { -1, 2147483647 });
                WriteReadRoundTrip(new float[] { 1.5f, -2.5f });
                WriteReadRoundTrip(new double[] { 1.5, -2.5 });
                WriteReadRoundTrip(new ulong[] { 1, 18446744073709551615 });
                WriteReadRoundTrip(new long[] { -1, 9223372036854775807 });
            });

            Test("TypedArray.ToArrayAndFromArrayTyped", () =>
            {
                using var target = new Float32Array(3);
                target.FromArray(new[] { 1.5f, -2.5f, 3.5f });
                AssertEqual(Show(target.ToArray()), "1.5,-2.5,3.5", "FromArray/ToArray on a Float32Array");
                AssertEqual(Show(target.ToArray(1)), "-2.5,3.5", "ToArray from an element index");
                AssertEqual(Show(target.ToArray(0, 2)), "1.5,-2.5", "ToArray with a count");
            });

            Test("TypedArray.ToArrayIntoAnExistingBuffer", () =>
            {
                using var target = new Int32Array(4);
                target.FromArray(new[] { 10, 20, 30, 40 });
                var dest = new int[4];
                var copied = target.ToArray(1, dest, 0, 2);
                AssertEqual(copied, 2L, "the reported element count");
                AssertEqual(Show(dest), "20,30,0,0", "elements copied into the caller's buffer");
            });

            Test("TypedArray.SetFromDotnetArray", () =>
            {
                using var target = new Int32Array(4);
                target.Set(new[] { 5, 6, 7 }, 1);
                AssertEqual(Show(target.ToArray()), "0,5,6,7", "Set(int[], targetOffset)");
            });

            Test("TypedArray.SetFromDotnetArrayEveryOverload", () =>
            {
                using var bytes = new Uint8Array(3);
                bytes.Set(new byte[] { 1, 2, 3 });
                AssertEqual(Show(bytes.ToArray()), "1,2,3", "Set(byte[])");

                using var doubles = new Float64Array(2);
                doubles.Set(new[] { 1.5, 2.5 });
                AssertEqual(Show(doubles.ToArray()), "1.5,2.5", "Set(double[])");

                using var shorts = new Int16Array(2);
                shorts.Set(new short[] { -1, 300 });
                AssertEqual(Show(shorts.ToArray()), "-1,300", "Set(short[])");
            });

            Test("TypedArray.CrossTypeReadReinterpretsBytes", () =>
            {
                // write doubles, read the same memory back as bytes - a byte for byte reinterpretation,
                // not an element conversion
                using var target = new Float64Array(2);
                target.FromArray(new[] { 1.0, 2.0 });
                var asBytes = target.Read<byte>();
                var expected = new[] { 1.0, 2.0 }.SelectMany(BitConverter.GetBytes).ToArray();
                AssertEqual(Show(asBytes), Show(expected), "a byte read of a Float64Array must be its raw bytes");
            });

            Test("TypedArray.ReadPastTheEndIsClamped", () =>
            {
                using var target = new Uint8Array(3);
                target.WriteBytes(new byte[] { 1, 2, 3 });
                AssertEqual(Show(target.ReadBytes(2)), "3", "a read from an offset must return only what is there");
                AssertEqual(target.Read<byte>(3).Length, 0, "a read starting at the end must return nothing");
            });

            Test("TypedArray.NewFromDotnetArray", () =>
            {
                using var view = new Uint8Array(new byte[] { 9, 8, 7 });
                AssertEqual(view.Length, 3L, "length");
                AssertEqual(Show(view.ReadBytes()), "9,8,7", "contents");
                Assert(!JS.Call<Uint8Array, bool>("SpawnJSTests.isOnWasmHeap", view), "a TypedArray built from a .Net array must own its buffer, not view the heap");
            });

            Test("TypedArray.ReCast", () =>
            {
                using var source = new Uint8Array(new byte[] { 1, 0, 0, 0, 2, 0, 0, 0 });
                using var asInts = source.ReCast<Int32Array>(0, 2);
                AssertEqual(Show(asInts.ToArray()), "1,2", "ReCast must reinterpret the same bytes as another element type");
            });
        }

        static void WriteReadRoundTrip<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(T[] data) where T : struct
        {
            var byteLength = data.Length * System.Runtime.InteropServices.Marshal.SizeOf<T>();
            using var target = new Uint8Array(byteLength);
            target.Write(data);
            var back = target.Read<T>();
            AssertEqual(Show(back), Show(data), $"Write<{typeof(T).Name}>/Read<{typeof(T).Name}> round trip");
        }
    }
}
