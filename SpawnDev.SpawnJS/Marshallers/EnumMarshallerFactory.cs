using SpawnDev.SpawnJS.Marshaller;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class EnumMarshallerFactory : JSMarshaller
    {
        public override bool CanMarshal(Type type)
            => type != null && (Nullable.GetUnderlyingType(type) ?? type).IsEnum;

        [UnconditionalSuppressMessage("Trimming", "IL2055",
    Justification = "MakeGenericType over SpawnJS's own EnumMarshaller<>/EnumNullableMarshaller<>, closed with the requested enum type (constrained struct, Enum).")]
        [UnconditionalSuppressMessage("Trimming", "IL2071",
    Justification = "The closed type argument is an enum (a value type); the marshaller's own generic constraints (struct, Enum) carry no PublicConstructors requirement, so trimming preserves nothing extra.")]
        [UnconditionalSuppressMessage("Trimming", "IL2072",
    Justification = "Activator over SpawnJS's own enum marshaller (parameterless ctor), referenced via typeof here. Verified to survive a trimmed WASM publish.")]
        public override JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            var typeToConvert = typeof(T);
            var underlying = Nullable.GetUnderlyingType(typeToConvert);
            if (underlying != null)
            {
                // MyEnum? -> EnumNullableMarshaller<MyEnum> (JSMarshaller<MyEnum?>).
                var nullableMarshaller = typeof(EnumNullableMarshaller<>).MakeGenericType(underlying);
                return (JSMarshaller<T>)Activator.CreateInstance(nullableMarshaller)!;
            }
            var marshallerTyped = typeof(EnumMarshaller<>).MakeGenericType(typeToConvert);
            return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
        }
    }
}
