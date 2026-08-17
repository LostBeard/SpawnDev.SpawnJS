using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class HeapViewDescriptorMarshaller : JSMarshallerFromString<HeapViewDescriptor>
    {
        public override HeapViewDescriptor JSToNet(string value)
        {
            throw new NotImplementedException();
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, HeapViewDescriptor value)
        {
            jsParent.PropertySetHeapView(jsKey, value.Offset, value.Length, value.Type, value.Copy);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, HeapViewDescriptor value)
        {
            // value.Type must be passed: the overload without it defaults to Uint8Array, so a descriptor
            // written to a named member (a POCO member, a record value) silently lost its view type and
            // produced a byte view of the same memory.
            jsParent.PropertySetHeapView(jsKey, value.Offset, value.Length, value.Type, value.Copy);
        }
    }
}
