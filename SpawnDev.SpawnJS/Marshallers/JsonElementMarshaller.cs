using SpawnDev.SpawnJS.Marshaller;
using System.Text.Json;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals <see cref="JsonElement"/> to/from a JS any.
    /// <para>
    /// A JsonElement already IS parsed JSON, so both directions move the raw JSON text and let the
    /// other side parse it - no serializer, and nothing is encoded twice. Going out, the text goes
    /// through <c>PropertySetRawJson</c>, whose JS half JSON.parse's it, so the value lands as a real
    /// Javascript object/array/primitive rather than as a string containing JSON. Coming in, the JS
    /// side JSON.stringify's the value and <see cref="JsonDocument"/> parses the result.
    /// </para>
    /// <para>
    /// Using <see cref="JsonElement.GetRawText"/> and <see cref="JsonDocument.Parse(string, JsonDocumentOptions)"/>
    /// rather than <see cref="JsonSerializer"/> also keeps this marshaller free of reflection-based
    /// System.Text.Json, so it carries no trimming warning.
    /// </para>
    /// <para>
    /// A JS <c>undefined</c> (an absent property, or a function that returns nothing) JSON.stringify's
    /// to <c>undefined</c> rather than to text, which arrives here as a null string and reads back as
    /// <c>default</c> - <see cref="JsonValueKind.Undefined"/>. A JS <c>null</c> stringifies to "null"
    /// and reads back as <see cref="JsonValueKind.Null"/>. The two stay distinguishable, which is why
    /// there is no nullable companion to this marshaller: JsonElement models absence itself.
    /// </para>
    /// </summary>
    public class JsonElementMarshaller : JSMarshallerFromJson<JsonElement>
    {
        public override JsonElement JSToNet(string value)
        {
            if (value == null) return default;
            // RootElement is only valid while its JsonDocument lives, so clone it out before disposing.
            using var doc = JsonDocument.Parse(value);
            return doc.RootElement.Clone();
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, JsonElement value)
        {
            // default(JsonElement) has no raw text to take. Undefined is what a JS undefined reads
            // back AS, so writing undefined keeps the round trip symmetric (and distinct from null).
            if (value.ValueKind == JsonValueKind.Undefined) { jsParent.PropertySetUndefined(jsKey); return; }
            jsParent.PropertySetRawJson(jsKey, value.GetRawText());
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, JsonElement value)
        {
            // default(JsonElement) has no raw text to take. Undefined is what a JS undefined reads
            // back AS, so writing undefined keeps the round trip symmetric (and distinct from null).
            if (value.ValueKind == JsonValueKind.Undefined) { jsParent.PropertySetUndefined(jsKey); return; }
            jsParent.PropertySetRawJson(jsKey, value.GetRawText());
        }
    }
}
