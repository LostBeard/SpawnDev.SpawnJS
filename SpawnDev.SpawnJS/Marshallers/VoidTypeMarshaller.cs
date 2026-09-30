using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="VoidType"/>: writes nothing and reads back null. Used for void calls.</summary>
    public class VoidTypeMarshaller : JSMarshallerFromVoid<VoidType>
    {
        public override VoidType JSToNet() => null!;
        /// <inheritdoc/>
        public override void Write(JSTape tape, VoidType value) => tape.WriteUndefined();
    }
}
