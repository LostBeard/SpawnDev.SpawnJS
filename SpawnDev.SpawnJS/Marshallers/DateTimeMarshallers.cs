using SpawnDev.SpawnJS.Marshaller;
using System.Globalization;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls <see cref="DateTime"/>.<br/>
    /// Outbound writes the round trip ISO 8601 string, which is what System.Text.Json produced in
    /// SpawnDev.BlazorJS - parity by default, so code moving over behaves the same. A wrapper that needs
    /// a real Javascript Date object uses the Date wrapper, which converts explicitly.<br/>
    /// Inbound accepts whatever the API actually hands back: a Date object, an epoch number, or a string.
    /// Javascript is inconsistent about this across APIs and the caller asked for a DateTime either way.
    /// </summary>
    public class DateTimeMarshaller : JSMarshallerFromSpawnJSObjectReference<DateTime>
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? typeToConvert) => typeToConvert == typeof(DateTime);
        /// <inheritdoc/>
        public override DateTime JSToNet(SpawnJSObjectReference? value)
        {
            if (value == null) return default;
            switch (value.TypeOf())
            {
                case "undefined":
                    return default;
                case "number":
                    {
                        var ms = value.As<double?>();
                        return ms == null ? default : ((long)ms.Value).EpochTimeToDateTime();
                    }
                case "string":
                    {
                        var text = value.As<string?>();
                        if (string.IsNullOrEmpty(text)) return default;
                        return DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsed)
                            ? parsed
                            : throw new Exception($"{nameof(DateTimeMarshaller)}: '{text}' is not a parsable date");
                    }
            }
            // a Javascript Date. getTime() is milliseconds since the epoch, the same shape as the number
            // case above, so read it through the object rather than guessing at its string form.
            if (value.ConstructorNames().IndexOf("Date") >= 0)
            {
                var ms = value.Call<double>("getTime");
                return ((long)ms).EpochTimeToDateTime();
            }
            return default;
        }
        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, DateTime value)
        {
            jsParent.PropertySet(jsKey, value.ToString("O", CultureInfo.InvariantCulture));
        }
        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, DateTime value)
        {
            jsParent.PropertySet(jsKey, value.ToString("O", CultureInfo.InvariantCulture));
        }
    }
    /// <summary>
    /// Marshalls <see cref="DateTime"/>.<br/>
    /// Outbound writes the round trip ISO 8601 string, which is what System.Text.Json produced in
    /// SpawnDev.BlazorJS - parity by default, so code moving over behaves the same. A wrapper that needs
    /// a real Javascript Date object uses the Date wrapper, which converts explicitly.<br/>
    /// Inbound accepts whatever the API actually hands back: a Date object, an epoch number, or a string.
    /// Javascript is inconsistent about this across APIs and the caller asked for a DateTime either way.
    /// </summary>
    public class DateTimeNullableMarshaller : JSMarshallerFromSpawnJSObjectReference<DateTime?>
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? typeToConvert) => typeToConvert == typeof(DateTime?);
        /// <inheritdoc/>
        public override DateTime? JSToNet(SpawnJSObjectReference? value)
        {
            if (value == null) return default;
            switch (value.TypeOf())
            {
                case "undefined":
                    return default;
                case "number":
                    {
                        var ms = value.As<double?>();
                        return ms == null ? default : ((long)ms.Value).EpochTimeToDateTime();
                    }
                case "string":
                    {
                        var text = value.As<string?>();
                        if (string.IsNullOrEmpty(text)) return default;
                        return DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsed)
                            ? parsed
                            : throw new Exception($"{nameof(DateTimeMarshaller)}: '{text}' is not a parsable date");
                    }
            }
            // a Javascript Date. getTime() is milliseconds since the epoch, the same shape as the number
            // case above, so read it through the object rather than guessing at its string form.
            if (value.ConstructorNames().IndexOf("Date") >= 0)
            {
                var ms = value.Call<double>("getTime");
                return ((long)ms).EpochTimeToDateTime();
            }
            return default;
        }
        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, DateTime? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            jsParent.PropertySet(jsKey, value.Value.ToString("O", CultureInfo.InvariantCulture));
        }
        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, DateTime? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            jsParent.PropertySet(jsKey, value.Value.ToString("O", CultureInfo.InvariantCulture));
        }
    }
}
