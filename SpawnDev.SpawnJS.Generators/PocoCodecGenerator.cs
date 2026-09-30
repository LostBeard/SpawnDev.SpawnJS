using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SpawnDev.SpawnJS.Generators
{
    /// <summary>
    /// Generates a SpawnDev.SpawnJS.JSPocoCodec for each POCO type this assembly passes to (or reads from) SpawnJS, and for
    /// every type marked [SpawnJSPoco]: its members read and set directly, with [JsonIgnore] / [JsonPropertyName] /
    /// [JsonInclude] resolved here. No reflection builds it at run time and the trimmer sees every member it uses.
    /// <para>
    /// The members, their Javascript names, their order and their [JsonIgnore] rules must be exactly what the runtime's
    /// reflection plan (TypeExtensions.GetTypeJsonProperties) produces - the test suite compares the two for every
    /// generated type. Anything this generator cannot mirror exactly is left to the reflection plan: it only generates
    /// what it is sure of.
    /// </para>
    /// </summary>
    [Generator]
    public sealed class PocoCodecGenerator : IIncrementalGenerator
    {
        const string AttributeName = "SpawnDev.SpawnJS.SpawnJSPocoAttribute";
        const string SpawnJSAssembly = "SpawnDev.SpawnJS";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var marked = context.SyntaxProvider.ForAttributeWithMetadataName(AttributeName,
                static (node, _) => node is TypeDeclarationSyntax,
                static (ctx, _) => (ITypeSymbol)ctx.TargetSymbol);
            // what crosses: the type arguments of every SpawnJS method called (Call<...>, Get<...>, New<...>, inferred or
            // written) and of every SpawnJS type named (ActionCallback<...>), and the type of every argument passed
            var used = context.SyntaxProvider.CreateSyntaxProvider(
                static (node, _) => node is InvocationExpressionSyntax || node is GenericNameSyntax,
                static (ctx, _) => UsedTypes(ctx));
            var all = marked.Collect().Combine(used.Collect()).Combine(context.CompilationProvider);
            context.RegisterSourceOutput(all, static (spc, source) =>
            {
                var ((markedTypes, usedTypes), compilation) = source;
                var roots = markedTypes.Concat(usedTypes.SelectMany(t => t));
                Emit(spc, compilation, roots);
            });
        }

        static ImmutableArray<ITypeSymbol> UsedTypes(GeneratorSyntaxContext ctx)
        {
            var model = ctx.SemanticModel;
            if (ctx.Node is InvocationExpressionSyntax invocation)
            {
                if (model.GetSymbolInfo(invocation).Symbol is not IMethodSymbol method || !FromSpawnJS(method.ContainingAssembly)) return ImmutableArray<ITypeSymbol>.Empty;
                var types = ImmutableArray.CreateBuilder<ITypeSymbol>();
                types.AddRange(method.TypeArguments);
                foreach (var argument in invocation.ArgumentList.Arguments)
                {
                    var type = model.GetTypeInfo(argument.Expression).Type;
                    if (type != null) types.Add(type);
                }
                return types.ToImmutable();
            }
            return model.GetSymbolInfo(ctx.Node).Symbol is INamedTypeSymbol named && FromSpawnJS(named.ContainingAssembly)
                ? named.TypeArguments
                : ImmutableArray<ITypeSymbol>.Empty;
        }

        static bool FromSpawnJS(IAssemblySymbol? assembly) => assembly != null && assembly.Name == SpawnJSAssembly;

        static void Emit(SourceProductionContext spc, Compilation compilation, IEnumerable<ITypeSymbol> roots)
        {
            var visited = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
            var codecs = new List<CodecModel>();
            var queue = new Queue<ITypeSymbol>(roots.Where(r => r != null));
            while (queue.Count > 0)
            {
                var type = queue.Dequeue();
                if (!visited.Add(type)) continue;
                switch (type)
                {
                    case IArrayTypeSymbol array:
                        queue.Enqueue(array.ElementType);
                        continue;
                    case INamedTypeSymbol named when named.IsGenericType:
                        // List<T>, IEnumerable<T>, Nullable<T>, Union<...>, Task<T>, Dictionary<K, V>, ActionCallback<...>
                        foreach (var argument in named.TypeArguments) queue.Enqueue(argument);
                        continue;
                }
                if (type is not INamedTypeSymbol poco || !IsPoco(poco, compilation)) continue;
                var model = CodecModel.Build(poco);
                if (model == null) continue;
                codecs.Add(model);
                foreach (var member in model.WriteMembers.Concat(model.ReadMembers ?? Enumerable.Empty<MemberModel>())) queue.Enqueue(member.Type);
            }
            if (codecs.Count == 0) return;
            var source = new StringBuilder();
            source.AppendLine("// <auto-generated/> SpawnDev.SpawnJS.Generators.PocoCodecGenerator");
            source.AppendLine("#nullable disable");
            source.AppendLine("#pragma warning disable CA2255 // a module initializer is how each assembly registers its codecs");
            source.AppendLine("namespace SpawnDev.SpawnJS.GeneratedCodecs");
            source.AppendLine("{");
            foreach (var codec in codecs) codec.Emit(source);
            source.AppendLine("    internal static class SpawnJSPocoCodecRegistration");
            source.AppendLine("    {");
            source.AppendLine("        [global::System.Runtime.CompilerServices.ModuleInitializer]");
            source.AppendLine("        internal static void Register()");
            source.AppendLine("        {");
            foreach (var codec in codecs) source.AppendLine($"            global::SpawnDev.SpawnJS.JSPocoCodecs.Register(new {codec.ClassName}());");
            source.AppendLine("        }");
            source.AppendLine("    }");
            source.AppendLine("}");
            spc.AddSource("SpawnJSPocoCodecs.g.cs", source.ToString());
        }

        // Types SpawnJS marshals with a marshaller of their own, not as a POCO
        static readonly HashSet<string> ExcludedTypes = new HashSet<string>
        {
            "SpawnDev.SpawnJS.HeapViewDescriptor", "SpawnDev.SpawnJS.EpochDateTime", "SpawnDev.SpawnJS.Marshaller.VoidType",
            "System.Type", "System.DateTime", "System.Text.Json.JsonElement", "System.Numerics.BigInteger",
        };
        static readonly HashSet<string> ExcludedBases = new HashSet<string>
        {
            "SpawnDev.SpawnJS.SpawnJSObject", "SpawnDev.SpawnJS.SpawnJSObjectReference", "SpawnDev.SpawnJS.Callback",
            "SpawnDev.SpawnJS.Union", "SpawnDev.SpawnJS.EnumString", "SpawnDev.SpawnJS.JSObjects.HeapView",
            "System.Delegate", "System.Threading.Tasks.Task", "System.Attribute", "System.Exception",
        };

        /// <summary>A class or struct of THIS assembly that the runtime marshals as a POCO, and that generated code can reach.</summary>
        static bool IsPoco(INamedTypeSymbol type, Compilation compilation)
        {
            if (!SymbolEqualityComparer.Default.Equals(type.ContainingAssembly, compilation.Assembly)) return false;
            if (type.TypeKind != TypeKind.Class && type.TypeKind != TypeKind.Struct) return false;
            if (type.IsAnonymousType || type.IsImplicitlyDeclared || type.IsAbstract || type.IsStatic || type.IsGenericType || type.IsRefLikeType || type.SpecialType != SpecialType.None) return false;
            for (var t = type.ContainingType; t != null; t = t.ContainingType) if (t.IsGenericType) return false;
            if (ExcludedTypes.Contains(FullName(type))) return false;
            for (var t = type.BaseType; t != null; t = t.BaseType) if (ExcludedBases.Contains(FullName(t))) return false;
            if (ExcludedBases.Contains(FullName(type))) return false;
            // anything enumerable is an array to the runtime (IEnumerableMarshaller)
            if (type.AllInterfaces.Any(i => FullName(i) == "System.Collections.IEnumerable")) return false;
            // numbers (NumberMarshaller) and tuples (ITupleMarshallerFactory) have their own marshallers
            if (type.AllInterfaces.Any(i => FullName(i) == "System.Numerics.INumber`1" || FullName(i) == "System.Runtime.CompilerServices.ITuple")) return false;
            for (var t = type; t != null; t = t.ContainingType)
            {
                if (t.DeclaredAccessibility != Accessibility.Public && t.DeclaredAccessibility != Accessibility.Internal
                    && t.DeclaredAccessibility != Accessibility.ProtectedOrInternal) return false;
            }
            return true;
        }

        /// <summary>Namespace, containing types and metadata name: Ns.Outer.Inner`1.</summary>
        internal static string FullName(ITypeSymbol type)
        {
            var name = type.MetadataName;
            for (var t = type.ContainingType; t != null; t = t.ContainingType) name = t.MetadataName + "." + name;
            return type.ContainingNamespace == null || type.ContainingNamespace.IsGlobalNamespace
                ? name
                : type.ContainingNamespace.ToDisplayString() + "." + name;
        }
    }
}
