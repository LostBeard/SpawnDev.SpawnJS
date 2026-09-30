using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="double"/> to/from a JS number (no conversion needed).</summary>
    public class DoubleMarshaller : JSMarshallerFromDouble<double>
    {
        public override double JSToNet(double value) => value;
        /// <inheritdoc/>
        public override void Write(JSTape tape, double value) => tape.WriteNumber(value);
    }
}
