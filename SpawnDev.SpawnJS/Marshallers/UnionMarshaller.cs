using SpawnDev.SpawnJS.Marshaller;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class UnionMarshallerFactory : JSMarshaller
    {
        public override bool CanMarshal(Type type) => type != null && type.IsAssignableTo(typeof(Union));
        [UnconditionalSuppressMessage("Trimming", "IL2055",
            Justification = "MakeGenericType over SpawnJS's own UnionMarshaller<>, closed with the requested Union<...> type.")]
        [UnconditionalSuppressMessage("Trimming", "IL2072",
            Justification = "Activator over SpawnJS's own UnionMarshaller<> (parameterless ctor), referenced via typeof here. Verified to survive a trimmed WASM publish.")]
        public override JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            var marshallerTyped = typeof(UnionMarshaller<>).MakeGenericType(typeof(T));
            return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
        }
    }
    /// <summary>
    /// Marshals a <see cref="Union{T1, T2}"/> (any arity) to/from JS. Write hands the contained value to JS
    /// by its runtime type. Read inspects the JS value (constructor name + <c>typeof</c>) to pick the union
    /// arm it belongs to, marshals it as that arm's .Net type, and constructs the Union.
    /// </summary>
    public class UnionMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TUnion> : JSMarshallerFromSpawnJSObjectReference<TUnion> where TUnion : Union
    {
        readonly Type[] ArmTypes;
        public UnionMarshaller()
        {
            ArmTypes = GetUnionArmTypes(typeof(TUnion)) ?? Array.Empty<Type>();
        }
        static readonly List<Type> SupportedGenericTypes = new List<Type> {
            typeof(Union<,>), typeof(Union<,,>), typeof(Union<,,,>), typeof(Union<,,,,>),
            typeof(Union<,,,,,>), typeof(Union<,,,,,,>), typeof(Union<,,,,,,,>),
            typeof(Union<,,,,,,,,>), typeof(Union<,,,,,,,,,>),
        };
        static Type[]? GetUnionArmTypes(Type unionType)
        {
            var t = unionType;
            while (t != null && t != typeof(object))
            {
                if (t.IsGenericType && SupportedGenericTypes.Contains(t.GetGenericTypeDefinition()))
                    return t.GenericTypeArguments;
                t = t.BaseType;
            }
            return null;
        }
        static readonly HashSet<Type> NumberTypes = new HashSet<Type> {
            typeof(byte), typeof(sbyte), typeof(short), typeof(ushort),
            typeof(int), typeof(uint), typeof(long), typeof(ulong),
            typeof(float), typeof(double), typeof(decimal),
        };
        static readonly HashSet<Type> StringTypes = new HashSet<Type> {
            typeof(string), typeof(DateTime),
        };
        public override TUnion JSToNet(SpawnJSObjectReference value)
        {
            if (value == null) return default!;
            var armType = SelectArm(value);
            if (armType == null)
            {
                var arms = string.Join(", ", ArmTypes.Select(a => a.Name));
                var typeInfo = value.TypeInfo();
                value.Dispose();
                throw new Exception($"Union arm not found for JS value (typeof={typeInfo.TypeOf}, constructor={typeInfo.ConstructorName}) in Union<{arms}>");
            }
            // Re-marshal the JS value as the selected arm's .Net type, then build the Union around it.
            var netValue = JS.ReturnAs(armType, value);
            value.Dispose();
            return (TUnion)Activator.CreateInstance(typeof(TUnion), new object?[] { netValue })!;
        }
        /// <summary>
        /// Picks the union arm that matches the JS value. Constructor-name match wins first (wrapper types,
        /// typed arrays, "Array"), then a <c>typeof</c>-category match against the primitive/collection arms.
        /// </summary>
        Type? SelectArm(SpawnJSObjectReference value)
        {
            var ctorName = value.ConstructorName();
            if (!string.IsNullOrEmpty(ctorName))
            {
                foreach (var arm in ArmTypes)
                {
                    var name = arm.Name.Split('`')[0];
                    if (ctorName.Equals(name, StringComparison.OrdinalIgnoreCase)) return arm;
                }
                // A JS Uint8Array is how a .Net byte[] arrives.
                if (ctorName == "Uint8Array" && ArmTypes.Contains(typeof(byte[]))) return typeof(byte[]);
            }
            var typeOf = value.TypeOf();
            switch (typeOf)
            {
                case "string":
                    foreach (var arm in ArmTypes) if (StringTypes.Contains(arm)) return arm;
                    break;
                case "boolean":
                    if (ArmTypes.Contains(typeof(bool))) return typeof(bool);
                    break;
                case "number":
                    foreach (var arm in ArmTypes) if (NumberTypes.Contains(arm)) return arm;
                    break;
                case "bigint":
                    if (ArmTypes.Contains(typeof(BigInteger))) return typeof(BigInteger);
                    break;
                case "object":
                case "function":
                    if (ctorName == "Array")
                    {
                        var enumerableArms = ArmTypes.Where(a => a != typeof(string) && typeof(IEnumerable).IsAssignableFrom(a)).ToList();
                        if (enumerableArms.Count == 1) return enumerableArms[0];
                    }
                    var objectArms = ArmTypes.Where(a => a.IsClass && a != typeof(string)).ToList();
                    if (objectArms.Count == 1) return objectArms[0];
                    break;
            }
            return null;
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TUnion value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            // value.Value is object?; the ObjectMarshaller re-dispatches it by its runtime type.
            jsParent.Set(jsKey, value.Value);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TUnion value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            jsParent.Set(jsKey, value.Value);
        }
    }
}
