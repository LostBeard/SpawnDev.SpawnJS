using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class RuntimeTypeMarshaller<TRuntimeType> : JSMarshallerFromString<TRuntimeType> 
    {
        public override TRuntimeType? JSToNet(string value) => string.IsNullOrEmpty(value) ? default : (TRuntimeType)(object)TypeExtensions.GetType(value);
        public override void Write(JSTape tape, TRuntimeType value) => tape.WriteString((value as Type)?.FullName);
    }
}
