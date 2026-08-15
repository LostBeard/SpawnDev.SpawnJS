using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="Nullable{Boolean}"/> to/from a JS boolean or null/undefined.</summary>
    public class BooleanNullableMarshaller : JSMarshallerFromBooleanNullable<bool?>
    {
        public override bool? JSToNet(bool? value) => value;
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, bool? value) => jsParent.PropertySet(jsKey, value);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, bool? value) => jsParent.PropertySet(jsKey, value);
    }
}
