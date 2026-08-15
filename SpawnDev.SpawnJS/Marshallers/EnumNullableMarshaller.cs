using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// TODO finish (needs nullable support)
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public class EnumNullableMarshaller<TEnum> : JSMarshallerFromInt32<TEnum> where TEnum : Enum?
    {
        public override TEnum JSToNet(int value) => (TEnum)(object)value;
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TEnum value) => jsParent.PropertySet(jsKey, Convert.ToInt32(value));
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TEnum value) => jsParent.PropertySet(jsKey, Convert.ToInt32(value));
    }
}
