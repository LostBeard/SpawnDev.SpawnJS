using SpawnDev.SpawnJS.Marshaller;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class EnumMarshallerFactory : JSMarshaller
    {
        public override bool CanMarshal(Type type)
            => type != null && (Nullable.GetUnderlyingType(type) ?? type).IsEnum;

        [UnconditionalSuppressMessage("Trimming", "IL2055",
    Justification = "MakeGenericType over SpawnJS's own ITupleMarshaller<>/ITupleNullableMarshaller<>, closed with the requested tuple type.")]
        [UnconditionalSuppressMessage("Trimming", "IL2071",
    Justification = "The closed tuple type is Tuple<>/ValueTuple<> (framework types whose public constructors are always preserved); its PublicConstructors requirement on the marshaller's TTuple is satisfied at runtime.")]
        [UnconditionalSuppressMessage("Trimming", "IL2072",
    Justification = "Activator over SpawnJS's own tuple marshaller (parameterless ctor), referenced via typeof here. Verified to survive a trimmed WASM publish.")]
        public override JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            var typeToConvert = typeof(T);
            var genericType = typeToConvert.IsGenericType ? typeToConvert.GetGenericTypeDefinition() : null;
            if (genericType == typeof(Nullable<>))
            {
                // ValueTuple<...>? -> ITupleNullableMarshaller<ValueTuple<...>> (JSMarshaller<ValueTuple<...>?>).
                var underlying = typeToConvert.GenericTypeArguments[0];
                var nullableMarshaller = typeof(ITupleNullableMarshaller<>).MakeGenericType(underlying);
                return (JSMarshaller<T>)Activator.CreateInstance(nullableMarshaller)!;
            }
            var marshallerTyped = typeof(EnumMarshaller<>).MakeGenericType(typeToConvert);
            return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
        }
    }
}
