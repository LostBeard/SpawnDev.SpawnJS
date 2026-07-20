using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls .Net enums.<br/>
    /// Without this an enum matches no marshaller but <see cref="DefaultMarshaller"/>, which hands the
    /// boxed value across as Any - and what arrives is not a number, so any web API expecting one
    /// rejects it. That is not a rare corner: WebGPU passes its bitflags this way, and the failure looks
    /// like <c>Failed to read the 'usage' property from 'GPUBufferDescriptor': Value is not of type
    /// 'unsigned long'</c>, which reads like a caller bug rather than a marshalling gap.<br/>
    /// <br/>
    /// A .Net enum is numeric on the wire, because that is what the web IDL means by an enum-typed
    /// bitflag. The exception is an enum marked <see cref="JsonStringEnumConverter"/>, which is how this
    /// library spells "this one is a Javascript string enum" - those cross as their string, taken from
    /// <see cref="JsonPropertyNameAttribute"/> so the Javascript spelling ("key") is used rather than
    /// the .Net member name ("Key").<br/>
    /// <br/>
    /// Distinct from <see cref="EnumStringMarshaller"/>, which handles the <see cref="EnumString{T}"/>
    /// wrapper type rather than enums themselves.
    /// </summary>
    public class EnumMarshaller : JSMarshaller
    {
        /// <summary>
        /// Javascript string names per string-enum type, or null for the numeric majority. Resolving this
        /// walks attributes over every member, so it is worth caching per type rather than per value.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, StringEnumNames?> _stringEnums = new();

        private sealed class StringEnumNames
        {
            public Dictionary<string, object> ToValue { get; } = new(StringComparer.Ordinal);
            public Dictionary<object, string> ToName { get; } = new();
        }

        private static StringEnumNames? GetStringEnum(Type enumType) => _stringEnums.GetOrAdd(enumType, type =>
        {
            if (type.GetCustomAttribute<JsonConverterAttribute>()?.ConverterType != typeof(JsonStringEnumConverter))
                return null;
            var names = new StringEnumNames();
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var value = field.GetValue(null);
                if (value == null) continue;
                var name = field.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? field.Name;
                names.ToValue[name] = value;
                names.ToName[value] = name;
            }
            return names;
        });

        /// <inheritdoc/>
        public override bool CanMarshal(Type? typeToConvert)
            => typeToConvert != null && (Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert).IsEnum;

        /// <inheritdoc/>
        public override void NetToJS(Type? typeToConvert, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (typeToConvert == null || value == null)
            {
                Reflect.Set(jsParent.JSObjectRequired, jsKey, (string?)null);
                return;
            }
            var enumType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
            var stringEnum = GetStringEnum(enumType);
            if (stringEnum != null)
            {
                // An undeclared combination has no name; sending its .Net spelling is closer to right
                // than sending nothing, and Javascript will reject it loudly either way.
                Reflect.Set(jsParent.JSObjectRequired, jsKey,
                    stringEnum.ToName.TryGetValue(value, out var name) ? name : value.ToString());
                return;
            }
            // Javascript numbers are doubles, so this is the honest width. Enum values live far inside
            // the exactly-representable range - WebGPU's flags are 32 bit.
            jsParent.SetProperty(jsKey, Convert.ToDouble(value));
        }

        /// <inheritdoc/>
        public override object? JSToNet(Type typeToConvert, SpawnJSHandle jsHandle)
        {
            var enumType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
            var stringEnum = GetStringEnum(enumType);
            if (stringEnum != null)
            {
                var name = Reflect.GetString(jsHandle.JSParent, jsHandle.JSKey);
                if (name == null) return null;
                return stringEnum.ToValue.TryGetValue(name, out var value) ? value : null;
            }
            var number = Reflect.GetDoubleNullable(jsHandle.JSParent, jsHandle.JSKey);
            if (number == null) return null;
            return Enum.ToObject(enumType, Convert.ChangeType(number.Value, Enum.GetUnderlyingType(enumType)));
        }
    }
}
