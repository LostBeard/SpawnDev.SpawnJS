using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class EnumMarshaller<TEnum> : JSMarshallerFromInt32<TEnum> where TEnum : struct, Enum
    {
        // Enum.ToObject coerces the int to TEnum's underlying type before boxing, so this works for enums
        // backed by byte/short/uint/long/etc. A direct (TEnum)(object)value unbox throws InvalidCastException
        // whenever the underlying type is not Int32 (e.g. a uint-backed [Flags] enum).
        public override TEnum JSToNet(int value) => (TEnum)Enum.ToObject(typeof(TEnum), value);
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TEnum value) => jsParent.PropertySet(jsKey, Convert.ToInt32(value));
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TEnum value) => jsParent.PropertySet(jsKey, Convert.ToInt32(value));
    }
}
