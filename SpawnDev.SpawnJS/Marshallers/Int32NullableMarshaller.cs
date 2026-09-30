using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class Int32NullableMarshaller : JSMarshallerFromInt32Nullable<int?>
    {
        public override int? JSToNet(int? value) => value;
        /// <inheritdoc/>
        public override void Write(JSTape tape, int? value) { if (value is null) tape.WriteNull(); else tape.WriteNumber(value.Value); }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, int? value) => jsParent.PropertySet(jsKey, value);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, int? value) => jsParent.PropertySet(jsKey, value);
    }
}
