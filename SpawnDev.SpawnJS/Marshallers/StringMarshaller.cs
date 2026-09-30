using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="string"/> to/from a JS string (no conversion needed).</summary>
    public class StringMarshaller : JSMarshallerFromString<string>
    {
        public override string JSToNet(string value) => value;
        /// <inheritdoc/>
        public override void Write(JSTape tape, string value) => tape.WriteString(value);
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, string value) => jsParent.PropertySet(jsKey, value);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, string value) => jsParent.PropertySet(jsKey, value);
    }
}
