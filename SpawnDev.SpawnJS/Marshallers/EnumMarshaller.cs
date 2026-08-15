using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class EnumMarshaller<TEnum> : JSMarshallerFromInt32<TEnum> where TEnum : struct, Enum
    {
        public override TEnum JSToNet(int value) => (TEnum)(object)value;
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TEnum value) => jsParent.PropertySet(jsKey, Convert.ToInt32(value));
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TEnum value) => jsParent.PropertySet(jsKey, Convert.ToInt32(value));
    }
}
