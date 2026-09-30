using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="bool"/> to/from a JS boolean (no conversion needed).</summary>
    public class BooleanMarshaller : JSMarshallerFromBoolean<bool>
    {
        public override bool JSToNet(bool value) => value;
        /// <inheritdoc/>
        public override void Write(JSTape tape, bool value) => tape.WriteBoolean(value);
    }
}
