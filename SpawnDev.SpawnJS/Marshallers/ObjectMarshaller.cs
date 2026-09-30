using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals a plain <see cref="object"/> (used for the null case): reads back as null, writes JS null.</summary>
    public class ObjectMarshaller : JSMarshallerFromSpawnJSObjectReference<object>
    {
        public override object JSToNet(SpawnJSObjectReference value) => value;
        /// <inheritdoc/>
        public override void Write(JSTape tape, object value)
        {
            if (value == null) { tape.WriteNull(); return; }
            var valueType = value.GetType();
            if (valueType == typeof(object)) throw new NotImplementedException("TODO");
            tape.WriteValue(value);
        }
    }
}
