using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class TypeMarshaller : JSMarshallerFromString<Type?>
    {
        public override Type? JSToNet(string value) => string.IsNullOrEmpty(value) ? null : TypeExtensions.GetType(value);
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, Type? value) => jsParent.PropertySet(jsKey, value?.FullName!);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, Type? value) => jsParent.PropertySet(jsKey, value?.FullName!);
    }
}
