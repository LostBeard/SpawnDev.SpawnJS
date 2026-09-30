using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpawnDev.SpawnJS.Generators
{
    /// <summary>How generated code reaches one accessor or field.</summary>
    internal enum Access
    {
        /// <summary>By name: public or internal.</summary>
        Direct,
        /// <summary>Through an [UnsafeAccessor] to the accessor method: private or protected.</summary>
        Method,
        /// <summary>Through an [UnsafeAccessor] to the field: private or protected.</summary>
        Field,
    }

    internal sealed class MemberModel
    {
        public string Name = "";
        public ITypeSymbol Type = null!;
        public string TypeName = "";
        /// <summary>The Json name: [JsonPropertyName], or null for the camelCased <see cref="Name"/>.</summary>
        public string? JsonName;
        /// <summary>JsonIgnoreCondition: 0 Never, 2 WhenWritingDefault, 3 WhenWritingNull (Always is left out).</summary>
        public int Condition;
        public bool IsField;
        /// <summary>The type that declares the member - what an [UnsafeAccessor] targets.</summary>
        public INamedTypeSymbol DeclaringType = null!;
        public Access Get;
        public string? GetMethodName;
        public Access Set;
        public string? SetMethodName;
        public bool CanBeNull => Type.IsReferenceType || Type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T
            || Type.TypeKind == TypeKind.TypeParameter;
    }

    /// <summary>
    /// One POCO's generated codec. <see cref="Build"/> mirrors TypeExtensions.GetTypeJsonProperties (the members written)
    /// and PocoReadPlan (the members read) - and returns null, leaving the type to those reflection plans, for anything
    /// it cannot reproduce exactly.
    /// </summary>
    internal sealed class CodecModel
    {
        const string JsonIgnore = "System.Text.Json.Serialization.JsonIgnoreAttribute";
        const string JsonPropertyName = "System.Text.Json.Serialization.JsonPropertyNameAttribute";
        const string JsonInclude = "System.Text.Json.Serialization.JsonIncludeAttribute";

        public INamedTypeSymbol Type = null!;
        public string TypeName = "";
        public string ClassName = "";
        public List<MemberModel> WriteMembers = new List<MemberModel>();
        /// <summary>Null when reading is left to the reflection plan.</summary>
        public List<MemberModel>? ReadMembers;
        /// <summary>A required member: C# will not <c>new</c> the type without setting it, so the codec constructs it
        /// through an [UnsafeAccessor] - the same public parameterless constructor Activator.CreateInstance runs.</summary>
        public bool HasRequired;

        static readonly SymbolDisplayFormat TypeFormat = SymbolDisplayFormat.FullyQualifiedFormat;

        public static CodecModel? Build(INamedTypeSymbol type)
        {
            var model = new CodecModel
            {
                Type = type,
                TypeName = type.ToDisplayString(TypeFormat),
                ClassName = "Codec_" + Sanitize(PocoCodecGenerator.FullName(type)),
            };
            var properties = new List<MemberModel>();
            var fields = new List<MemberModel>();
            var readable = true;
            var names = new HashSet<string>();
            // the base declarations of the overrides already seen: reflection returns only the most derived
            var overridden = new HashSet<IPropertySymbol>(SymbolEqualityComparer.Default);
            // Type.GetProperties / GetFields (Instance | Public | NonPublic): the type's own members, then each base's -
            // a base's PRIVATE members are not returned
            for (var t = type; t != null && t.SpecialType != SpecialType.System_Object && t.SpecialType != SpecialType.System_ValueType; t = t.BaseType)
            {
                var inherited = !SymbolEqualityComparer.Default.Equals(t, type);
                if (!IsNameable(t)) return null;
                foreach (var symbol in t.GetMembers())
                {
                    if (symbol.IsStatic) continue;
                    if (inherited && symbol.DeclaredAccessibility == Accessibility.Private) continue;
                    if (symbol is IPropertySymbol property)
                    {
                        if (overridden.Contains(property)) continue;
                        for (var o = property.OverriddenProperty; o != null; o = o.OverriddenProperty) overridden.Add(o);
                        // two members reflection would both return under one name (a `new` redeclaration): reflection's
                        // own rules decide which it keeps, so it keeps the type
                        if (!names.Add("p:" + property.MetadataName)) return null;
                        var ignore = Condition(property);
                        if (ignore == 1) continue;
                        var isPublic = property.GetMethod?.DeclaredAccessibility == Accessibility.Public
                            || property.SetMethod?.DeclaredAccessibility == Accessibility.Public;
                        var jsonName = JsonNameOf(property);
                        if (!isPublic && jsonName == null && !Has(property, JsonInclude)) continue;
                        // an indexer or a write-only property throws in the reflection write; an override's accessors and
                        // attributes are merged with its base's by reflection; an explicit implementation has no plain name
                        if (property.IsIndexer || property.GetMethod == null || property.IsOverride
                            || property.ExplicitInterfaceImplementations.Length > 0) return null;
                        if (!IsNameable(property.Type) || property.Type.IsRefLikeType || property.ReturnsByRef || property.ReturnsByRefReadonly) return null;
                        var member = new MemberModel
                        {
                            Name = Identifier(property.Name),
                            Type = property.Type,
                            TypeName = property.Type.ToDisplayString(TypeFormat),
                            JsonName = jsonName,
                            Condition = ignore,
                            DeclaringType = t,
                            GetMethodName = property.GetMethod.MetadataName,
                        };
                        // a base's private accessor is invisible through the derived type's PropertyInfo
                        if (inherited && property.GetMethod.DeclaredAccessibility == Accessibility.Private) return null;
                        member.Get = Accessible(property.GetMethod) ? Access.Direct : Access.Method;
                        properties.Add(member);
                        // read: a property with a setter (PocoReadPlan), set the way generated code can match exactly
                        if (property.SetMethod == null) continue;
                        member.SetMethodName = property.SetMethod.MetadataName;
                        if (inherited && property.SetMethod.DeclaredAccessibility == Accessibility.Private) readable = false;
                        if (property.IsRequired) model.HasRequired = true;
                        // an init-only setter is an ordinary method to the runtime: an [UnsafeAccessor] calls it where
                        // C# would not, as the reflection plan's setter delegate does
                        member.Set = Accessible(property.SetMethod) && !property.SetMethod.IsInitOnly ? Access.Direct : Access.Method;
                    }
                    else if (symbol is IFieldSymbol field)
                    {
                        if (field.IsConst) continue;
                        if (field.IsImplicitlyDeclared)
                        {
                            // a backing field with [field: JsonInclude] - generated code cannot name it
                            if (field.GetAttributes().Any(a => IsJson(a))) return null;
                            continue;
                        }
                        if (!names.Add("f:" + field.MetadataName)) return null;
                        var ignore = Condition(field);
                        if (ignore == 1) continue;
                        var jsonName = JsonNameOf(field);
                        if (jsonName == null && !Has(field, JsonInclude)) continue;
                        if (!IsNameable(field.Type) || field.Type.IsRefLikeType || field.IsFixedSizeBuffer || field.RefKind != RefKind.None) return null;
                        var access = Accessible(field) ? Access.Direct : Access.Field;
                        var member = new MemberModel
                        {
                            Name = Identifier(field.Name),
                            Type = field.Type,
                            TypeName = field.Type.ToDisplayString(TypeFormat),
                            JsonName = jsonName,
                            Condition = ignore,
                            IsField = true,
                            DeclaringType = t,
                            Get = access,
                            Set = access,
                        };
                        if (field.IsReadOnly) readable = false;
                        if (field.IsRequired) model.HasRequired = true;
                        fields.Add(member);
                    }
                }
            }
            model.WriteMembers.AddRange(properties);
            model.WriteMembers.AddRange(fields);
            // PocoReadPlan builds with Activator.CreateInstance(Type): a public parameterless constructor, or a zeroed struct
            if (type.TypeKind == TypeKind.Class && !type.InstanceConstructors.Any(c => c.Parameters.Length == 0 && c.DeclaredAccessibility == Accessibility.Public))
                readable = false;
            if (readable)
                model.ReadMembers = model.WriteMembers.Where(m => m.IsField || m.SetMethodName != null).ToList();
            return model;
        }

        /// <summary>1 Always, 2 WhenWritingDefault, 3 WhenWritingNull, 0 Never or no [JsonIgnore].</summary>
        static int Condition(ISymbol symbol)
        {
            var attribute = symbol.GetAttributes().FirstOrDefault(a => Is(a, JsonIgnore));
            if (attribute == null) return 0;
            foreach (var named in attribute.NamedArguments)
                if (named.Key == "Condition" && named.Value.Value is int condition) return condition;
            return 1;
        }

        static string? JsonNameOf(ISymbol symbol)
        {
            var attribute = symbol.GetAttributes().FirstOrDefault(a => Is(a, JsonPropertyName));
            return attribute != null && attribute.ConstructorArguments.Length == 1 ? attribute.ConstructorArguments[0].Value as string : null;
        }

        static bool Has(ISymbol symbol, string attribute) => symbol.GetAttributes().Any(a => Is(a, attribute));
        static bool Is(AttributeData attribute, string name) => attribute.AttributeClass != null && PocoCodecGenerator.FullName(attribute.AttributeClass) == name;
        static bool IsJson(AttributeData attribute) => Is(attribute, JsonIgnore) || Is(attribute, JsonPropertyName) || Is(attribute, JsonInclude);

        /// <summary>Whether code elsewhere in the assembly can use the member by name.</summary>
        static bool Accessible(ISymbol symbol)
            => symbol.DeclaredAccessibility == Accessibility.Public || symbol.DeclaredAccessibility == Accessibility.Internal
                || symbol.DeclaredAccessibility == Accessibility.ProtectedOrInternal;

        /// <summary>Whether generated code elsewhere in the assembly can write the type's name.</summary>
        static bool IsNameable(ITypeSymbol type)
        {
            switch (type)
            {
                case IArrayTypeSymbol array: return IsNameable(array.ElementType);
                case IPointerTypeSymbol _:
                case IFunctionPointerTypeSymbol _:
                case ITypeParameterSymbol _:
                case IErrorTypeSymbol _:
                    return false;
                case INamedTypeSymbol named:
                    for (var t = named; t != null; t = t.ContainingType)
                    {
                        if (t.IsAnonymousType || !Accessible(t) || !t.TypeArguments.All(IsNameable)) return false;
                    }
                    return true;
                default:
                    return type.TypeKind == TypeKind.Dynamic;
            }
        }

        /// <summary>The name as C# source: a keyword escaped with @.</summary>
        static string Identifier(string name)
            => Microsoft.CodeAnalysis.CSharp.SyntaxFacts.GetKeywordKind(name) != Microsoft.CodeAnalysis.CSharp.SyntaxKind.None
                || Microsoft.CodeAnalysis.CSharp.SyntaxFacts.GetContextualKeywordKind(name) != Microsoft.CodeAnalysis.CSharp.SyntaxKind.None ? "@" + name : name;

        static string Sanitize(string name)
        {
            var result = new StringBuilder(name.Length);
            foreach (var c in name) result.Append(char.IsLetterOrDigit(c) ? c : '_');
            return result.ToString();
        }

        public void Emit(StringBuilder source)
        {
            var writeNames = string.Join(", ", WriteMembers.Select(NameExpression));
            var readNames = ReadMembers == null ? "null" : "new string[] { " + string.Join(", ", ReadMembers.Select(NameExpression)) + " }";
            var isStruct = Type.IsValueType;
            var target = isStruct ? "ref value" : "value";
            source.AppendLine($"    internal sealed class {ClassName} : global::SpawnDev.SpawnJS.JSPocoCodec<{TypeName}>");
            source.AppendLine("    {");
            source.AppendLine($"        public {ClassName}() : base(new string[] {{ {writeNames} }}, {readNames}) {{ }}");
            // write
            for (var i = 0; i < WriteMembers.Count; i++)
                source.AppendLine($"        readonly global::SpawnDev.SpawnJS.JSPocoMember<{WriteMembers[i].TypeName}> _w{i} = new global::SpawnDev.SpawnJS.JSPocoMember<{WriteMembers[i].TypeName}>();");
            source.AppendLine($"        public override void WriteMembers(global::SpawnDev.SpawnJS.JSTape tape, {TypeName} value)");
            source.AppendLine("        {");
            for (var i = 0; i < WriteMembers.Count; i++)
            {
                var member = WriteMembers[i];
                var get = member.Get == Access.Direct ? $"value.{member.Name}"
                    : member.Get == Access.Method ? $"Get{i}({target})"
                    : $"Field{i}({target})";
                source.AppendLine($"            var v{i} = {get};");
                var skip = member.Condition == 2 ? $"global::System.Collections.Generic.EqualityComparer<{member.TypeName}>.Default.Equals(v{i}, default)"
                    : member.Condition == 3 && member.CanBeNull ? $"v{i} is null"
                    : null;
                var prefix = "            ";
                if (skip != null) { source.AppendLine($"{prefix}if ({skip}) tape.WriteAbsent();"); prefix += "else "; }
                // v2's walk wrote a null member as JS null (unless [JsonIgnore] already skipped it)
                if (member.CanBeNull && member.Condition != 2 && member.Condition != 3)
                {
                    source.AppendLine($"{prefix}if (v{i} is null) tape.WriteNull();");
                    prefix = "            else ";
                }
                source.AppendLine($"{prefix}_w{i}.Write(tape, v{i});");
            }
            source.AppendLine("        }");
            // read
            if (ReadMembers != null)
            {
                for (var i = 0; i < ReadMembers.Count; i++)
                    source.AppendLine($"        readonly global::SpawnDev.SpawnJS.JSPocoMember<{ReadMembers[i].TypeName}> _r{i} = new global::SpawnDev.SpawnJS.JSPocoMember<{ReadMembers[i].TypeName}>();");
                source.AppendLine($"        protected override global::SpawnDev.SpawnJS.JSSchema[] ReadMemberSchemas() => new global::SpawnDev.SpawnJS.JSSchema[] {{ {string.Join(", ", ReadMembers.Select((m, i) => $"_r{i}.Schema"))} }};");
                source.AppendLine($"        public override {TypeName} ReadMembers(ref global::SpawnDev.SpawnJS.JSTapeReader reader)");
                source.AppendLine("        {");
                source.AppendLine(isStruct ? $"            {TypeName} value = default;"
                    : HasRequired ? "            var value = Construct();"
                    : $"            var value = new {TypeName}();");
                for (var i = 0; i < ReadMembers.Count; i++)
                {
                    var member = ReadMembers[i];
                    var set = member.Set == Access.Direct ? $"value.{member.Name} = r{i};"
                        : member.Set == Access.Method ? $"Set{i}({target}, r{i});"
                        : $"Field{WriteMembers.IndexOf(member)}({target}) = r{i};";
                    source.AppendLine($"            var r{i} = _r{i}.Read(ref reader);");
                    // PocoReadPlan: a member read as null is not set - it keeps its initializer
                    source.AppendLine(member.CanBeNull ? $"            if (r{i} is not null) {set}" : $"            {set}");
                }
                source.AppendLine("            return value;");
                source.AppendLine("        }");
            }
            // private and protected members
            for (var i = 0; i < WriteMembers.Count; i++)
            {
                var member = WriteMembers[i];
                var declaring = member.DeclaringType.ToDisplayString(TypeFormat);
                var self = isStruct ? $"ref {declaring} obj" : $"{declaring} obj";
                if (member.Get == Access.Method)
                {
                    source.AppendLine($"        [global::System.Runtime.CompilerServices.UnsafeAccessor(global::System.Runtime.CompilerServices.UnsafeAccessorKind.Method, Name = \"{member.GetMethodName}\")]");
                    source.AppendLine($"        static extern {member.TypeName} Get{i}({self});");
                }
                if (member.IsField && member.Get == Access.Field)
                {
                    source.AppendLine($"        [global::System.Runtime.CompilerServices.UnsafeAccessor(global::System.Runtime.CompilerServices.UnsafeAccessorKind.Field, Name = \"{member.Name.TrimStart('@')}\")]");
                    source.AppendLine($"        static extern ref {member.TypeName} Field{i}({self});");
                }
            }
            if (ReadMembers != null)
            {
                if (HasRequired && !isStruct)
                {
                    source.AppendLine("        [global::System.Runtime.CompilerServices.UnsafeAccessor(global::System.Runtime.CompilerServices.UnsafeAccessorKind.Constructor)]");
                    source.AppendLine($"        static extern {TypeName} Construct();");
                }
                for (var i = 0; i < ReadMembers.Count; i++)
                {
                    var member = ReadMembers[i];
                    if (member.IsField || member.Set != Access.Method) continue;
                    var declaring = member.DeclaringType.ToDisplayString(TypeFormat);
                    var self = isStruct ? $"ref {declaring} obj" : $"{declaring} obj";
                    source.AppendLine($"        [global::System.Runtime.CompilerServices.UnsafeAccessor(global::System.Runtime.CompilerServices.UnsafeAccessorKind.Method, Name = \"{member.SetMethodName}\")]");
                    source.AppendLine($"        static extern void Set{i}({self}, {member.TypeName} value);");
                }
            }
            source.AppendLine("    }");
        }

        static string NameExpression(MemberModel member)
            => member.JsonName != null
                ? Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(member.JsonName, true)
                // STJ's own camelCase, so the name cannot drift from the reflection plan's GetJsonName()
                : $"global::System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName({Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(member.Name.TrimStart('@'), true)})";
    }
}
