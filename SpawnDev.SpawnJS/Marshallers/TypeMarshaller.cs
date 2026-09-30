using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class TypeMarshaller : JSMarshallerFromString<Type?>
    {
        public override Type? JSToNet(string value) => string.IsNullOrEmpty(value) ? null : TypeExtensions.GetType(value);
        public override void Write(JSTape tape, Type? value) => tape.WriteString(value?.FullName);
    }
}
