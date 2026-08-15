using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="Nullable{Double}"/> to/from a JS number or null/undefined.</summary>
    public class DoubleNullableMarshaller : JSMarshallerFromDoubleNullable<double?>
    {
        public override double? JSToNet(double? value) => value;
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, double? value) => jsParent.PropertySet(jsKey, value);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, double? value) => jsParent.PropertySet(jsKey, value);
    }
}
