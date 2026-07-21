using System.Collections.Concurrent;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls <see cref="EnumString{T}"/>.<br/>
    /// An EnumString is a .Net enum paired with the Javascript string that names it, so on the wire it is
    /// nothing but that string. Marshalling it is therefore a string read or write plus the enum lookup
    /// EnumString already does in its own constructor - no transport of its own, same as
    /// <see cref="UnionMarshaller"/>.<br/>
    /// A Javascript string that matches no enum member is NOT an error: EnumString is built to carry it
    /// with IsDefined false, which is what lets a wrapper survive a value the enum does not know yet.
    /// </summary>
    public class EnumStringMarshaller : JSMarshaller
    {
        /// <summary>
        /// Constructor taking the Javascript string, per concrete EnumString type
        /// </summary>
        private static readonly ConcurrentDictionary<Type, System.Reflection.ConstructorInfo> _constructors = new();

        /// <inheritdoc/>
        public override bool CanMarshal(Type? typeToConvert)
            => typeToConvert != null && typeof(EnumString).IsAssignableFrom(typeToConvert) && !typeToConvert.IsAbstract;

        /// <inheritdoc/>
        public override void NetToJS(Type? typeToConvert, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            // write the Javascript string the enum member maps to, which is all Javascript ever sees
            jsParent.SetProperty(jsKey, value is EnumString enumString ? enumString.String : null);
        }

        /// <inheritdoc/>
        public override object? JSToNet(Type typeToConvert, SpawnJSHandle jsHandle)
        {
            var value = jsHandle.ReadSelfString();
            if (value == null) return null;
            var constructor = _constructors.GetOrAdd(typeToConvert, t =>
                t.GetConstructor(new[] { typeof(string) })
                ?? throw new Exception($"{nameof(EnumStringMarshaller)}: {t.Name} has no constructor taking a string"));
            return constructor.Invoke(new object?[] { value });
        }
    }
}
