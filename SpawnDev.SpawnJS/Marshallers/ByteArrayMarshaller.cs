using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class ByteArrayMarshaller : JSMarshallerFromSpawnJSObjectReference<byte[]?>
    {
        static readonly JSSchema _bytes = JSSchema.Bytes();
        /// <inheritdoc/>
        /// <remarks>The bytes of an ArrayBuffer or any ArrayBufferView, in one copy.</remarks>
        public override JSSchema Schema => _bytes;
        /// <inheritdoc/>
        public override byte[]? Read(ref JSTapeReader reader)
        {
            var count = reader.ReadCount();
            return count < 0 ? null : reader.ReadRaw<byte>(count).ToArray();
        }
        /// <inheritdoc/>
        /// <remarks>A Uint8Array copy, made by Javascript straight out of the pinned array.</remarks>
        public override void Write(JSTape tape, byte[]? value)
        {
            if (value == null) { tape.WriteNull(); return; }
            tape.WriteArrayCopy(value, JSArrayBufferView.Uint8Array);
        }
    }
}
