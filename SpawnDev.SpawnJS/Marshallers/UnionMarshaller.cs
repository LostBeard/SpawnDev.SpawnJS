using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls <see cref="Union"/> types.<br/>
    /// .Net ➡️ Javascript simply unwraps the union and lets the marshaller graph handle the value's real
    /// type, so a union never adds a layer on the way out.<br/>
    /// Javascript ➡️ .Net picks which arm of the union the Javascript value belongs to by looking at the
    /// live Javascript value (typeof, class name, and if needed the prototype chain), then hands the read
    /// off to the marshaller registered for that arm. Nothing about a union needs its own transport.
    /// </summary>
    public class UnionMarshaller : JSMarshaller
    {
        /// <summary>
        /// The concrete Union type this instance is bound to, or null for the unbound registry instance
        /// </summary>
        private readonly Type? _unionType;
        /// <summary>
        /// The union's arm types, in declaration order
        /// </summary>
        private readonly Type[] _armTypes;
        /// <summary>
        /// Per concrete Union type instances, so the arm types and constructors are resolved once
        /// </summary>
        private static readonly ConcurrentDictionary<Type, UnionMarshaller> _bound = new();
        /// <summary>
        /// Constructor per union type and arm type, resolved on first use. Keyed on both because the
        /// unbound registry instance can be asked to marshal more than one union type, and two different
        /// unions can share an arm type.
        /// </summary>
        private static readonly ConcurrentDictionary<(Type UnionType, Type ArmType), ConstructorInfo> _armConstructors = new();

        /// <summary>
        /// Creates the unbound instance used for registration
        /// </summary>
        public UnionMarshaller()
        {
            _unionType = null;
            _armTypes = Type.EmptyTypes;
        }

        private UnionMarshaller(Type unionType, Type[] armTypes)
        {
            _unionType = unionType;
            _armTypes = armTypes;
        }

        /// <inheritdoc/>
        public override bool CanMarshal(Type? typeToConvert)
            => typeToConvert != null && typeof(Union).IsAssignableFrom(typeToConvert) && GetArmTypes(typeToConvert) != null;

        /// <inheritdoc/>
        public override JSMarshaller GetMarshaller(Type? typeToConvert)
        {
            if (typeToConvert == null) return this;
            if (_unionType == typeToConvert) return this;
            return _bound.GetOrAdd(typeToConvert, t => new UnionMarshaller(t, GetArmTypes(t)!));
        }

        /// <summary>
        /// Walks up from a concrete union type to the closed generic Union&lt;...&gt; and returns its arms.
        /// Returns null if the type is not a generic Union.
        /// </summary>
        public static Type[]? GetArmTypes(Type unionType)
        {
            if (!typeof(Union).IsAssignableFrom(unionType)) return null;
            var testType = (Type?)unionType;
            while (testType != null && testType != typeof(object))
            {
                if (testType.IsGenericType && SupportedGenericTypes.Contains(testType.GetGenericTypeDefinition()))
                {
                    return testType.GenericTypeArguments;
                }
                testType = testType.BaseType;
            }
            return null;
        }

        /// <summary>
        /// The open generic Union types this marshaller understands
        /// </summary>
        public static IReadOnlyList<Type> SupportedGenericTypes { get; } = new List<Type>
        {
            typeof(Union<,>),
            typeof(Union<,,>),
            typeof(Union<,,,>),
            typeof(Union<,,,,>),
            typeof(Union<,,,,,>),
            typeof(Union<,,,,,,>),
            typeof(Union<,,,,,,,>),
            typeof(Union<,,,,,,,,>),
            typeof(Union<,,,,,,,,,>),
        };

        /// <summary>
        /// .Net types a Javascript string can be read into
        /// </summary>
        private static readonly Type[] StringTypes = { typeof(string), typeof(DateTime) };

        /// <summary>
        /// .Net types a Javascript number can be read into
        /// </summary>
        private static readonly Type[] NumberTypes =
        {
            typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint),
            typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal),
        };

        /// <inheritdoc/>
        public override void NetToJS(Type? typeToConvert, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            // A union carries no representation of its own - write the value it holds and let the
            // marshaller registered for that value's real type do the work.
            if (value is Union union)
            {
                JS.MarshallNetToJS(jsParent, jsKey, union.Value);
                return;
            }
            Reflect.Set(jsParent.JSObjectRequired, jsKey, (string?)null);
        }

        /// <inheritdoc/>
        public override object? JSToNet(Type typeToConvert, SpawnJSHandle jsHandle)
        {
            var armTypes = _unionType == typeToConvert ? _armTypes : GetArmTypes(typeToConvert);
            if (armTypes == null || armTypes.Length == 0)
                throw new Exception($"{nameof(UnionMarshaller)}: {typeToConvert.Name} is not a generic Union type");
            var armType = SelectArmType(typeToConvert, armTypes, jsHandle);
            if (armType == null) return null;
            // Hand the actual read to whichever marshaller owns the arm type. The union adds selection,
            // never transport.
            var value = JS.MarshallJSToNet(armType, jsHandle);
            if (value == null) return null;
            return CreateUnion(typeToConvert, armType, value);
        }

        /// <summary>
        /// Decides which arm of the union the live Javascript value belongs to.
        /// Returns null when the Javascript value is null or undefined.
        /// </summary>
        private Type? SelectArmType(Type unionType, Type[] armTypes, SpawnJSHandle jsHandle)
        {
            // One round trip gets typeof and the whole prototype chain. Everything below reads these two.
            var jsType = jsHandle.JSType;
            var chain = jsHandle.ConstructorNames;
            switch (jsType)
            {
                case "undefined":
                    return null;
                case "string":
                    return FirstArmIn(armTypes, StringTypes)
                        ?? throw UnionMismatch(unionType, armTypes, jsType, chain);
                case "boolean":
                    return System.Array.IndexOf(armTypes, typeof(bool)) >= 0
                        ? typeof(bool)
                        : throw UnionMismatch(unionType, armTypes, jsType, chain);
                case "number":
                    return FirstArmIn(armTypes, NumberTypes)
                        ?? throw UnionMismatch(unionType, armTypes, jsType, chain);
                case "bigint":
                    return FirstArmIn(armTypes, new[] { typeof(long), typeof(ulong) })
                        ?? throw UnionMismatch(unionType, armTypes, jsType, chain);
            }
            // typeof null is "object". null and undefined are the only values with no prototype chain,
            // and undefined was already handled, so an empty chain here means null.
            if (chain.Length == 0) return null;

            // Walk the prototype chain, most derived first, and take the first arm any link names. Going
            // outward from the value is what makes the most derived arm win: a union declaring both Error
            // and TypeError must resolve a TypeError to TypeError.
            //
            // The chain is used rather than the value's class name because Object.prototype.toString
            // reports "Error" for every Error subclass - a class name cannot identify a derived type, and
            // trusting it silently resolves TypeError to the Error arm.
            //
            // Walking the chain also subsumes the per-type special casing the Blazor converter needed: it
            // carried a hardcoded list of every typed array class so a TypedArray arm could match, but
            // TypedArray is genuinely in Uint8Array's prototype chain.
            foreach (var chainName in chain)
            {
                var match = MatchArmByJSClassName(armTypes, chainName);
                if (match != null) return match;
            }

            // Last resort: an unrecognised plain object with exactly one class arm is unambiguous.
            var classArms = armTypes.Where(o => o.IsClass && o != typeof(string)).ToList();
            if (classArms.Count == 1) return classArms[0];

            throw UnionMismatch(unionType, armTypes, jsType, chain);
        }

        /// <summary>
        /// Matches a single Javascript class name against the union's arms
        /// </summary>
        private static Type? MatchArmByJSClassName(Type[] armTypes, string jsClassName)
        {
            if (string.IsNullOrEmpty(jsClassName)) return null;
            foreach (var arm in armTypes)
            {
                // strip the generic arity marker: Array`1 -> Array
                var armName = arm.Name.Split('`')[0];
                if (jsClassName.Equals(armName, StringComparison.OrdinalIgnoreCase)) return arm;
            }
            // .Net types whose name cannot match the Javascript class name but which are the natural
            // target for it.
            if (jsClassName == "Uint8Array" && System.Array.IndexOf(armTypes, typeof(byte[])) >= 0)
            {
                return typeof(byte[]);
            }
            if (jsClassName == "Array")
            {
                var enumerableArms = armTypes
                    .Where(o => o != typeof(string) && typeof(IEnumerable).IsAssignableFrom(o))
                    .ToList();
                if (enumerableArms.Count == 1) return enumerableArms[0];
            }
            return null;
        }

        /// <summary>
        /// Returns the first arm of the union that appears in candidates, preserving arm declaration order
        /// </summary>
        private static Type? FirstArmIn(Type[] armTypes, Type[] candidates)
        {
            foreach (var arm in armTypes)
            {
                if (System.Array.IndexOf(candidates, arm) >= 0) return arm;
            }
            return null;
        }

        /// <summary>
        /// Creates the union instance holding value, using the constructor for the selected arm.<br/>
        /// The arm is selected explicitly rather than by overload resolution on the value, because a union
        /// like Union&lt;string, object&gt; has more than one constructor a given value would satisfy.
        /// </summary>
        private object CreateUnion(Type unionType, Type armType, object value)
        {
            var constructor = _armConstructors.GetOrAdd((unionType, armType), key =>
                key.UnionType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { key.ArmType }, null)
                ?? throw new Exception($"{nameof(UnionMarshaller)}: {key.UnionType.Name} has no constructor taking {key.ArmType.Name}"));
            return constructor.Invoke(new[] { value });
        }

        private static Exception UnionMismatch(Type unionType, Type[] armTypes, string jsType, string[] chain)
            => new Exception($"{nameof(UnionMarshaller)}: Javascript value ({jsType}{(chain.Length > 0 ? " " + string.Join(" -> ", chain) : "")}) matches no arm of {unionType.Name.Split('`')[0]}<{string.Join(", ", armTypes.Select(o => o.Name))}>");
    }
}
