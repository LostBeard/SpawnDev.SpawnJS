using SpawnDev.SpawnJS.Marshaller;
using System.Text.Json;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals <see cref="JsonElement"/> to/from a JS any.
    /// <para>
    /// A JsonElement already IS parsed JSON, so both directions move the raw JSON text and let the
    /// other side parse it - no serializer, and nothing is encoded twice. Going out, the text is written to the
    /// tape revived by <c>__reviverJson</c>, which JSON.parse's it, so the value lands as a real
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
        /// <inheritdoc/>
        /// <remarks>Its raw JSON text, parsed Javascript side; an Undefined element is JS undefined.</remarks>
        public override void Write(JSTape tape, JsonElement value)
        {
            if (value.ValueKind == JsonValueKind.Undefined) { tape.WriteUndefined(); return; }
            tape.WriteRevived(InteropMethod.ReviverJson);
            tape.WriteString(value.GetRawText());
        }
    }
}
