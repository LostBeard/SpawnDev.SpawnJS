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
                    JS.InteropCall<double, SpawnJSObjectReference, double, double, double, VoidType>("writeArrayBufferViewToHeap", JS.DotnetInstance.Id, value, 0, address, byteLength);
                }
            }
            return ret;
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, byte[]? value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            unsafe
            {
                fixed (byte* ptr = value)
                {
                    IntPtr address = (IntPtr)ptr;
                    var heapViewDescriptor = new HeapViewDescriptor(address, value.Length, true);
                    jsParent.PropertySet(jsKey, heapViewDescriptor);
                }
            }
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, byte[]? value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            unsafe
            {
                fixed (byte* ptr = value)
                {
                    IntPtr address = (IntPtr)ptr;
                    var heapViewDescriptor = new HeapViewDescriptor(address, value.Length, true);
                    jsParent.PropertySet(jsKey, heapViewDescriptor);
                }
            }
        }
    }
}
