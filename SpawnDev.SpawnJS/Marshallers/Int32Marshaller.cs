using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class Int32Marshaller : JSMarshallerFromInt32<int>
    {
        public override int JSToNet(int value) => value;
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, int value) => jsParent.PropertySet(jsKey, value);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, int value) => jsParent.PropertySet(jsKey, value);
    }
}
