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
        /// <inheritdoc/>
        /// <remarks>Its decimal string, made a BigInt by stringToBigInt - a Javascript number cannot hold it exactly.</remarks>
        public override void Write(JSTape tape, BigInteger value)
        {
            tape.WriteRevived(InteropMethod.StringToBigInt);
            tape.WriteString(value.ToString());
        }
    }
    public class BigIntegerNullableMarshaller : JSMarshallerFromString<BigInteger?>
    {
        public override BigInteger? JSToNet(string? value)
        {
            if (value == null) return null;
            return BigInteger.Parse(value);
        }
        /// <inheritdoc/>
        public override void Write(JSTape tape, BigInteger? value)
        {
            if (value == null) { tape.WriteNull(); return; }
            tape.WriteRevived(InteropMethod.StringToBigInt);
            tape.WriteString(value.Value.ToString());
        }
    }
}
