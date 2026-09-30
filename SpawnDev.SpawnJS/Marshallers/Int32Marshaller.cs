using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class Int32Marshaller : JSMarshallerFromInt32<int>
    {
        public override int JSToNet(int value) => value;
        /// <inheritdoc/>
        public override void Write(JSTape tape, int value) => tape.WriteNumber(value);
    }
}
