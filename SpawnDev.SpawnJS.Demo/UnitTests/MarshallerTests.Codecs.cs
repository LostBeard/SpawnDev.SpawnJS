using SpawnDev.SpawnJS.Marshaller;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.Demo.UnitTests
{
    // Generated POCO codecs (SpawnDev.SpawnJS.Generators): the gate that keeps them identical to the reflection plan.
    //
    // The suite runs twice - as it is, and with ?nocodecs (JSPocoCodecs.UseGenerated = false, every POCO by
    // reflection) - and every test must pass both ways. On top of that, the tests here compare the two directly for
    // EVERY generated codec: the names, their order, and what Javascript receives.
    public static partial class MarshallerTests
    {
        // ---- edge cases the generator must mirror exactly ----

        // setters generated code cannot name: an [UnsafeAccessor] each
        public class CgPrivateSetter
        {
            public string? Name { get; private set; }
            public int Count { get; protected set; }
            public static CgPrivateSetter Make(string name, int count) => new CgPrivateSetter { Name = name, Count = count };
        }
        // members only [JsonInclude] / [JsonPropertyName] bring in, at every access level
        public class CgHidden
        {
            [JsonInclude]
            protected int Level { get; set; }
            [JsonPropertyName("tag")]
            internal string? Tag { get; set; }
            // public, get-only: written, never read
            public string? Secret => _secret;
            [JsonInclude]
            private string? _secret;
            public CgHidden() { }
            public CgHidden(string secret, int level, string tag) { _secret = secret; Level = level; Tag = tag; }
            public int GetLevel() => Level;
        }
        // names that are C# keywords
        public class CgKeywords
        {
            public int @class { get; set; }
            [JsonInclude]
            public string? @event;
        }
        // a struct: members through `ref`, a private field through a by-ref accessor
        public struct CgStruct
        {
            [JsonInclude]
            private int _hidden;
            public int Visible { get; set; }
            public int Hidden => _hidden;
            public CgStruct(int hidden, int visible) { _hidden = hidden; Visible = visible; }
        }
        // inherited members: the derived type's first, a base's protected member through the base
        public class CgBase
        {
            [JsonInclude]
            protected string? BaseNote { get; set; }
            public string? BaseName { get; set; }
            public string? Note => BaseNote;
            public void SetNote(string note) => BaseNote = note;
        }
        public class CgDerived : CgBase
        {
            public int Extra { get; set; }
        }
        // init-only and required members: set through [UnsafeAccessor]s, as reflection sets them
        public class CgInit
        {
            public string? Name { get; init; }
            public int Size { get; init; }
        }
        public class CgRequired
        {
            public required string Name { get; set; }
            public int Size { get; set; }
        }
        public class CgReadonlyField
        {
            [JsonInclude]
            public readonly int Value;
            public CgReadonlyField() { }
            public CgReadonlyField(int value) => Value = value;
        }
        // an override: reflection merges it with its base declaration, so the generator leaves it to reflection
        public class CgVirtualBase
        {
            public virtual string? Kind { get; set; }
        }
        public class CgOverride : CgVirtualBase
        {
            public override string? Kind { get; set; }
        }
        // only ever held in an `object`: found by the attribute, and only by the attribute
        [SpawnJSPoco]
        public class CgAttributeOnly
        {
            public int A { get; set; }
        }
        public class CgObjectOnly
        {
            public int B { get; set; }
        }
        public class CgObjectHolder
        {
            public object? Value { get; set; }
        }

        static readonly bool Codecs = JSPocoCodecs.UseGenerated;
        static IJSPocoCodec? CodecOf(Type type) => JSPocoCodecs.All.FirstOrDefault(c => c.Type == type);

        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgPrivateSetter))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgHidden))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgKeywords))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgStruct))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgBase))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgDerived))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgInit))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgRequired))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgReadonlyField))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgOverride))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgAttributeOnly))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgObjectOnly))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CgObjectHolder))]
        static void CodecTests()
        {
            Test("Codec.Mode", () =>
            {
                // the runner's --nocodecs must actually reach the app, or the second pass proves nothing
                Console.WriteLine($"Codec mode: {(Codecs ? "generated" : "reflection (nocodecs)")}");
                Assert(JSPocoCodecs.All.Count > 0, "no codec registered: the generator did not run");
            });

            // what the generator must find, and what it must leave to reflection
            Test("Codec.Coverage", () =>
            {
                void Reads(Type type) => Assert(CodecOf(type)?.ReadNames != null, $"{type.Name}: expected a codec that reads");
                void WritesOnly(Type type)
                {
                    var codec = CodecOf(type);
                    Assert(codec != null && codec.ReadNames == null, $"{type.Name}: expected a write-only codec");
                }
                void None(Type type) => Assert(CodecOf(type) == null, $"{type.Name}: expected no codec");
                Reads(typeof(Person));
                Reads(typeof(CgPrivateSetter));
                Reads(typeof(CgHidden));
                Reads(typeof(CgKeywords));
                Reads(typeof(CgStruct));
                Reads(typeof(CgDerived));
                Reads(typeof(CgAttributeOnly));
                Reads(typeof(CgObjectHolder));
                Reads(typeof(CgInit));
                Reads(typeof(CgRequired));
                // a readonly field: reflection can set it, generated code leaves the read to reflection
                WritesOnly(typeof(CgReadonlyField));
                None(typeof(CgOverride));
                None(typeof(CgObjectOnly));
                // the library's own descriptors, generated in the library
                Reads(typeof(SpawnDev.SpawnJS.JSObjects.ArrayBufferOptions));
            });

            // every codec: the same names, in the same order, as the reflection plan
            Test("Codec.Parity.Names", () =>
            {
                var mismatches = new List<string>();
                foreach (var codec in JSPocoCodecs.All)
                {
                    var members = codec.Type.GetTypeJsonProperties();
                    var write = members.Select(m => m.GetJsonName()).ToList();
                    if (!write.SequenceEqual(codec.WriteNames))
                        mismatches.Add($"{codec.Type.FullName} write [{string.Join(",", codec.WriteNames)}] vs reflection [{string.Join(",", write)}]");
                    if (codec.ReadNames == null) continue;
                    var read = members.Where(m => m.FieldInfo != null || (m.PropertyInfo!.GetSetMethod(true) != null && m.PropertyInfo.GetIndexParameters().Length == 0))
                        .Select(m => m.GetJsonName()).ToList();
                    if (!read.SequenceEqual(codec.ReadNames))
                        mismatches.Add($"{codec.Type.FullName} read [{string.Join(",", codec.ReadNames)}] vs reflection [{string.Join(",", read)}]");
                }
                Console.WriteLine($"Codec.Parity.Names: {JSPocoCodecs.All.Count} codecs");
                Assert(mismatches.Count == 0, $"{mismatches.Count} codec(s) differ: {string.Join(" | ", mismatches.Take(5))}");
            });

            // every codec that can be built: what Javascript receives, empty and filled, codec against reflection
            Test("Codec.Parity.Write", () =>
            {
                var compared = 0;
                var mismatches = new List<string>();
                var saved = JSPocoCodecs.UseGenerated;
                try
                {
                    foreach (var codec in JSPocoCodecs.All)
                    {
                        foreach (var fill in new[] { false, true })
                        {
                            object? value;
                            try
                            {
                                value = Create(codec.Type);
                                if (value != null && fill) Fill(value);
                            }
                            catch
                            {
                                continue;
                            }
                            if (value == null) continue;
                            JSPocoCodecs.UseGenerated = true;
                            var generated = Js<object>("shape", value);
                            JSPocoCodecs.UseGenerated = false;
                            var reflected = Js<object>("shape", value);
                            compared++;
                            if (generated != reflected) mismatches.Add($"{codec.Type.FullName}: {generated} vs {reflected}");
                        }
                    }
                }
                finally
                {
                    JSPocoCodecs.UseGenerated = saved;
                }
                Console.WriteLine($"Codec.Parity.Write: {compared} values compared");
                Assert(compared > JSPocoCodecs.All.Count, $"only {compared} values compared");
                Assert(mismatches.Count == 0, $"{mismatches.Count} differ: {string.Join(" | ", mismatches.Take(3))}");
            });

            // ---- round trips: the same result generated and by reflection (the ?nocodecs pass) ----

            Test("Codec.RoundTrip.PrivateSetters", () =>
            {
                JS.Set(K, CgPrivateSetter.Make("n", 3));
                AssertEqual(ShapeKey(), "{name:string(\"n\"),count:number(3)}", "shape");
                var back = JS.Get<CgPrivateSetter>(K)!;
                AssertEqual(back.Name, "n", "private setter");
                AssertEqual(back.Count, 3, "protected setter");
            });

            Test("Codec.RoundTrip.HiddenMembers", () =>
            {
                JS.Set(K, new CgHidden("s", 2, "t"));
                AssertEqual(ShapeKey(), "{level:number(2),tag:string(\"t\"),secret:string(\"s\"),_secret:string(\"s\")}", "shape");
                var back = JS.Get<CgHidden>(K)!;
                AssertEqual(back.GetLevel(), 2, "protected [JsonInclude] property");
                AssertEqual(back.Tag, "t", "internal [JsonPropertyName] property");
                AssertEqual(back.Secret, "s", "private [JsonInclude] field");
            });

            Test("Codec.RoundTrip.KeywordNames", () =>
            {
                JS.Set(K, new CgKeywords { @class = 4, @event = "e" });
                AssertEqual(ShapeKey(), "{class:number(4),event:string(\"e\")}", "shape");
                var back = JS.Get<CgKeywords>(K)!;
                AssertEqual(back.@class, 4, "@class");
                AssertEqual(back.@event, "e", "@event");
            });

            Test("Codec.RoundTrip.Struct", () =>
            {
                JS.Set(K, new CgStruct(7, 8));
                AssertEqual(ShapeKey(), "{visible:number(8),hidden:number(7),_hidden:number(7)}", "shape");
                var back = JS.Get<CgStruct>(K);
                AssertEqual(back.Hidden, 7, "private field of a struct");
                AssertEqual(back.Visible, 8, "struct property");
                var nullable = JS.Get<CgStruct?>(K);
                AssertEqual(nullable?.Hidden, 7, "Nullable<struct>");
            });

            Test("Codec.RoundTrip.Inherited", () =>
            {
                var value = new CgDerived { Extra = 1, BaseName = "b" };
                value.SetNote("note");
                JS.Set(K, value);
                AssertEqual(ShapeKey(), "{extra:number(1),baseNote:string(\"note\"),baseName:string(\"b\"),note:string(\"note\")}", "shape");
                var back = JS.Get<CgDerived>(K)!;
                AssertEqual(back.Extra, 1, "own member");
                AssertEqual(back.BaseName, "b", "inherited member");
                AssertEqual(back.Note, "note", "inherited protected [JsonInclude] member");
            });

            Test("Codec.RoundTrip.InitAndRequired", () =>
            {
                JS.Set(K, new CgInit { Name = "i", Size = 5 });
                AssertEqual(ShapeKey(), "{name:string(\"i\"),size:number(5)}", "init shape");
                var init = JS.Get<CgInit>(K)!;
                AssertEqual(init.Name, "i", "init-only member");
                AssertEqual(init.Size, 5, "init-only member");
                JS.Set(K, new CgRequired { Name = "r", Size = 6 });
                AssertEqual(ShapeKey(), "{name:string(\"r\"),size:number(6)}", "required shape");
                var required = JS.Get<CgRequired>(K)!;
                AssertEqual(required.Name, "r", "required member");
                AssertEqual(required.Size, 6, "member of a type with a required member");
                JS.Set(K, new CgReadonlyField(9));
                AssertEqual(ShapeKey(), "{value:number(9)}", "readonly field shape");
                AssertEqual(JS.Get<CgReadonlyField>(K)!.Value, 9, "readonly field, read by reflection");
            });

            Test("Codec.RoundTrip.Override", () =>
            {
                JS.Set(K, new CgOverride { Kind = "k" });
                AssertEqual(ShapeKey(), "{kind:string(\"k\")}", "shape");
                AssertEqual(JS.Get<CgOverride>(K)!.Kind, "k", "override");
            });

            Test("Codec.RoundTrip.RuntimeTyped", () =>
            {
                // an [SpawnJSPoco] type and a plain one, each known only at run time
                JS.Set(K, new CgObjectHolder { Value = new CgAttributeOnly { A = 1 } });
                AssertEqual(ShapeKey(), "{value:{a:number(1)}}", "attribute type");
                JS.Set(K, new CgObjectHolder { Value = new CgObjectOnly { B = 2 } });
                AssertEqual(ShapeKey(), "{value:{b:number(2)}}", "reflection type");
            });
        }

        /// <summary>Gives every settable member of a simple type a non-default value, so a written value is compared
        /// as well as a skipped one.</summary>
        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Test-only reflection over types the generated codecs root.")]
        static void Fill(object value)
        {
            var index = 1;
            foreach (var member in value.GetType().GetTypeJsonProperties())
            {
                var type = member.PropertyInfo?.PropertyType ?? member.FieldInfo!.FieldType;
                var sample = Sample(Nullable.GetUnderlyingType(type) ?? type, index++);
                if (sample == null) continue;
                try
                {
                    if (member.PropertyInfo != null)
                    {
                        if (member.PropertyInfo.GetSetMethod(true) is { } setter) setter.Invoke(value, new[] { sample });
                    }
                    else if (!member.FieldInfo!.IsInitOnly) member.FieldInfo.SetValue(value, sample);
                }
                catch
                {
                    // a setter that validates: the member keeps its value, in both writes
                }
            }
        }

        [UnconditionalSuppressMessage("Trimming", "IL2067", Justification = "Test-only: a type the trimmer left no constructor on is skipped.")]
        static object? Create(Type type) => Activator.CreateInstance(type);

        static object? Sample(Type type, int index)
        {
            if (type == typeof(string)) return "s" + index;
            if (type == typeof(bool)) return true;
            if (type.IsEnum)
            {
                var values = Enum.GetValues(type);
                return values.Length > 1 ? values.GetValue(1) : values.Length == 1 ? values.GetValue(0) : null;
            }
            if (type.IsPrimitive && type != typeof(char) && type != typeof(IntPtr) && type != typeof(UIntPtr))
                return Convert.ChangeType(index, type);
            return null;
        }
    }
}
