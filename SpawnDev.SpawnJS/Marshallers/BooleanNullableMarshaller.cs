using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="Nullable{Boolean}"/> to/from a JS boolean or null/undefined.</summary>
    public class BooleanNullableMarshaller : JSMarshallerFromBooleanNullable<bool?>
    {
        public override bool? JSToNet(bool? value) => value;
        /// <inheritdoc/>
        public override void Write(JSTape tape, bool? value) { if (value is null) tape.WriteNull(); else tape.WriteBoolean(value.Value); }
    }
}
