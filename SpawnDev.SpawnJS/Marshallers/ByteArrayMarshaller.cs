using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class ByteArrayMarshaller : JSMarshallerFromSpawnJSObjectReference<byte[]?>
    {
        public override byte[]? JSToNet(SpawnJSObjectReference value)
        {
            if (value == null) return null;
            var byteLength = (long)value.PropertyGetDouble("byteLength");
            var ret = new byte[byteLength];
            if (byteLength == 0) return ret;
            unsafe
            {
                fixed (byte* ptr = ret)
                {
                    var address = (double)(IntPtr)ptr;
                    JS.InteropCall<double, SpawnJSObjectReference, double, double, double, VoidType>(InteropMethod.WriteArrayBufferViewToHeap, JS.DotnetInstance.Id, value, 0, address, byteLength);
                }
            }
            return ret;
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
