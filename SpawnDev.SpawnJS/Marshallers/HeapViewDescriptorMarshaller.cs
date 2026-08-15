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
            jsParent.PropertySetHeapView(jsKey, value.Offset, value.Length, value.Copy);
        }
    }
}
