using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="bool"/> to/from a JS boolean (no conversion needed).</summary>
    public class BooleanMarshaller : JSMarshallerFromBoolean<bool>
    {
        public override bool JSToNet(bool value) => value;
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, bool value) => jsParent.PropertySet(jsKey, value);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, bool value) => jsParent.PropertySet(jsKey, value);
    }
}
