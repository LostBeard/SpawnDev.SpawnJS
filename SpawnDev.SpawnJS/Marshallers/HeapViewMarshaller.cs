using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class HeapViewMarshaller : JSMarshallerFromString<HeapView?>
    {
        public override bool CanMarshal(Type type) => type.IsAssignableTo(typeof(HeapView));
        public override HeapView? JSToNet(string value)
        {
            throw new NotImplementedException();
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, HeapView? value)
        {
            if (value?.RefreshCopyOnMarshal == true) value.RefreshCopy();
            jsParent.Set(jsKey, value?._View);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, HeapView? value)
        {
            if (value?.RefreshCopyOnMarshal == true) value.RefreshCopy();
            jsParent.Set(jsKey, value?._View);
        }
    }
}
