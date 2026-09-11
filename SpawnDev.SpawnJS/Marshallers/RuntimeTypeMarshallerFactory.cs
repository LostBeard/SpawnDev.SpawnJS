using SpawnDev.SpawnJS.Marshaller;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class RuntimeTypeMarshallerFactory : JSMarshaller
    {
        public override bool CanMarshal(Type type)
        {
            if (type.Name == "RuntimeType")
            {
                return true;
            }
            return false;
        }
        public override JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            return new RuntimeTypeMarshaller<T>() as JSMarshaller<T>;
        }
    }
}
