using SpawnDev.SpawnJS.Marshaller;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS.Marshallers
{

    /// <summary>
    /// Marshalls <see cref="EnumString{T}"/>.<br/>
    /// An EnumString is a .Net enum paired with the Javascript string that names it, so on the wire it is
    /// nothing but that string. Marshalling it is therefore a string read or write plus the enum lookup
    /// EnumString already does in its own constructor<br/>
    /// A Javascript string that matches no enum member is NOT an error: EnumString is built to carry it
    /// with IsDefined false, which is what lets a wrapper survive a value the enum does not know yet.
    /// </summary>
    public class EnumStringMarshallerFactory : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type) => typeof(EnumString).IsAssignableFrom(type);
        /// <summary>
        /// Builds an <see cref="ArrayMarshaller{T}"/> bound to the concrete element type of
        /// <typeparamref name="T"/> (e.g. selecting for <c>int[]</c> yields an <c>ArrayMarshaller&lt;int&gt;</c>).
        /// </summary>
        [UnconditionalSuppressMessage("Trimming", "IL2055",
    Justification = "MakeGenericType over SpawnJS's own EnumStringMarshaller<>, closed with the requested enum type (constrained struct, Enum).")]
        [UnconditionalSuppressMessage("Trimming", "IL2071",
    Justification = "The closed type argument is the EnumString's enum type (a value type); the marshaller's own generic constraints (struct, Enum) carry no PublicConstructors requirement, so trimming preserves nothing extra.")]
        [UnconditionalSuppressMessage("Trimming", "IL2072",
    Justification = "Activator over SpawnJS's own EnumStringMarshaller<> (parameterless ctor), referenced via typeof here. Verified to survive a trimmed WASM publish.")]
        public override JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var T1 = typeof(T).GetGenericArguments()[0];
            var marshallerTyped = typeof(EnumStringMarshaller<>).MakeGenericType(T1);
            return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
        }
    }
    /// <summary>
    /// Marshalls <see cref="EnumString{T}"/>.<br/>
    /// An EnumString is a .Net enum paired with the Javascript string that names it, so on the wire it is
    /// nothing but that string. Marshalling it is therefore a string read or write plus the enum lookup
    /// EnumString already does in its own constructor<br/>
    /// A Javascript string that matches no enum member is NOT an error: EnumString is built to carry it
    /// with IsDefined false, which is what lets a wrapper survive a value the enum does not know yet.
    /// </summary>
    public class EnumStringMarshaller<TEnum> : JSMarshallerFromString<EnumString<TEnum>?> where TEnum : struct, Enum
    {
        /// <inheritdoc/>
        public override EnumString<TEnum>? JSToNet(string value)
        {
            if (value == null) return null;
            return new EnumString<TEnum>(value);
        }
        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, EnumString<TEnum>? value)
        {
            // write the Javascript string the enum member maps to, which is all Javascript ever sees
            jsParent.PropertySet(jsKey, value?.String!);
        }
        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, EnumString<TEnum>? value)
        {
            // write the Javascript string the enum member maps to, which is all Javascript ever sees
            jsParent.PropertySet(jsKey, value?.String!);
        }
    }
}
