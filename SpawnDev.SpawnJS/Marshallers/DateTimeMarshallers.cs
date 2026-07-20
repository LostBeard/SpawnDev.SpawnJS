using System.Globalization;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls <see cref="EpochDateTime"/>.<br/>
    /// EpochDateTime exists precisely because a lot of Javascript APIs express a moment in time as
    /// milliseconds since 1970-01-01 rather than as a Date, so it marshals as a plain number.
    /// </summary>
    public class EpochDateTimeMarshaller : JSMarshaller<EpochDateTime>
    {
        /// <inheritdoc/>
        public override void NetToJS(Type? typeToConvert, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is EpochDateTime epoch)
            {
                Reflect.Set(jsParent.JSObjectRequired, jsKey, (double)epoch.ValueEpoch);
                return;
            }
            Reflect.Set(jsParent.JSObjectRequired, jsKey, (string?)null);
        }

        /// <inheritdoc/>
        public override object? JSToNet(Type typeToConvert, SpawnJSHandle jsHandle)
        {
            var value = Reflect.GetDoubleNullable(jsHandle.JSParent, jsHandle.JSKey);
            return value == null ? null : new EpochDateTime((long)value.Value);
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
    public class DateTimeMarshaller : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? typeToConvert)
            => typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?);

        /// <inheritdoc/>
        public override void NetToJS(Type? typeToConvert, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is DateTime dateTime)
            {
                Reflect.Set(jsParent.JSObjectRequired, jsKey, dateTime.ToString("O", CultureInfo.InvariantCulture));
                return;
            }
            Reflect.Set(jsParent.JSObjectRequired, jsKey, (string?)null);
        }

        /// <inheritdoc/>
        public override object? JSToNet(Type typeToConvert, SpawnJSHandle jsHandle)
        {
            switch (jsHandle.JSType)
            {
                case "undefined":
                    return null;
                case "number":
                    {
                        var ms = Reflect.GetDoubleNullable(jsHandle.JSParent, jsHandle.JSKey);
                        return ms == null ? null : ((long)ms.Value).EpochTimeToDateTime();
                    }
                case "string":
                    {
                        var text = Reflect.GetString(jsHandle.JSParent, jsHandle.JSKey);
                        if (string.IsNullOrEmpty(text)) return null;
                        return DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsed)
                            ? parsed
                            : throw new Exception($"{nameof(DateTimeMarshaller)}: '{text}' is not a parsable date");
                    }
            }
            // a Javascript Date. getTime() is milliseconds since the epoch, the same shape as the number
            // case above, so read it through the object rather than guessing at its string form.
            if (System.Array.IndexOf(jsHandle.ConstructorNames, "Date") >= 0)
            {
                using var date = jsHandle.AsJSHandle();
                if (date == null) return null;
                var ms = date.InvokePropertyDouble("getTime");
                return ((long)ms).EpochTimeToDateTime();
            }
            return null;
        }
    }
}
