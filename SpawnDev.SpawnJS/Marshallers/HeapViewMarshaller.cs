using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class HeapViewMarshaller : JSMarshallerFromString<HeapView?>
    {
        public override HeapView? JSToNet(string value)
        {
            throw new NotImplementedException();
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, HeapView? value)
        {
            jsParent.Set(jsKey, value?.ToNativeView());
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, HeapView? value)
        {
            jsParent.Set(jsKey, value?.ToNativeView());
        }
    }
}
