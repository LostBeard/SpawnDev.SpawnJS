using SpawnDev.SpawnJS.Marshaller;
using System.Numerics;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class BigIntegerMarshaller : JSMarshallerFromString<BigInteger>
    {
        public override BigInteger JSToNet(string? value)
        {
            if (value == null) return new BigInteger();
            return BigInteger.Parse(value);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, BigInteger value)
        {
            jsParent.PropertySetWithReviver("BigInt", jsKey, value.ToString());
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, BigInteger value)
        {
            jsParent.PropertySetWithReviver("BigInt", jsKey, value.ToString());
        }
    }
    public class BigIntegerNullableMarshaller : JSMarshallerFromString<BigInteger?>
    {
        public override BigInteger? JSToNet(string? value)
        {
            if (value == null) return null;
            return BigInteger.Parse(value);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, BigInteger? value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            jsParent.PropertySetWithReviver("BigInt", jsKey, value.Value.ToString());
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, BigInteger? value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            jsParent.PropertySetWithReviver("BigInt", jsKey, value.Value.ToString());
        }
    }
}
