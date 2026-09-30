using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="Nullable{Double}"/> to/from a JS number or null/undefined.</summary>
    public class DoubleNullableMarshaller : JSMarshallerFromDoubleNullable<double?>
    {
        public override double? JSToNet(double? value) => value;
        /// <inheritdoc/>
        public override void Write(JSTape tape, double? value) { if (value is null) tape.WriteNull(); else tape.WriteNumber(value.Value); }
    }
}
