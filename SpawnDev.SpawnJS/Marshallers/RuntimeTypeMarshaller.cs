using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class RuntimeTypeMarshaller<TRuntimeType> : JSMarshallerFromString<TRuntimeType> 
    {
        public override TRuntimeType? JSToNet(string value) => string.IsNullOrEmpty(value) ? default : (TRuntimeType)(object)TypeExtensions.GetType(value);
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TRuntimeType value) => jsParent.PropertySet(jsKey, (value as Type)?.FullName!);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TRuntimeType value) => jsParent.PropertySet(jsKey, (value as Type)?.FullName!);
    }
}
