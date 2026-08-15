using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals a <see cref="SpawnJSObjectReference"/> itself: a JS value held on the JS side and carried
    /// across the boundary as its numeric object-table id. A non-positive id (0, or the null/undefined
    /// sentinels) is treated as no reference and returns null.
    /// </summary>
    public class SpawnJSObjectReferenceMarshaller : JSMarshallerFromSpawnJSObjectReference<SpawnJSObjectReference>
    {
        public override SpawnJSObjectReference JSToNet(SpawnJSObjectReference value) => value;
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, SpawnJSObjectReference value) => jsParent.PropertySet(jsKey, value);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, SpawnJSObjectReference value) => jsParent.PropertySet(jsKey, value);
    }
}
