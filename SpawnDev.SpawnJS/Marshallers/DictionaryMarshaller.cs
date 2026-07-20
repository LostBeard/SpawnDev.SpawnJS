using System.Collections;
using System.Collections.Concurrent;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls a string-keyed dictionary as a plain Javascript object, which is what the web platform
    /// means by a "record" - <c>GPUProgrammableStage.constants</c>, for example, is
    /// <c>record&lt;USVString, GPUPipelineConstantValue&gt;</c>.<br/>
    /// <br/>
    /// Without this a Dictionary matches <see cref="ObjectMarshaller"/>, which reflects over the
    /// dictionary's OWN members - Comparer, Count, Keys, Values - instead of its contents. What crosses is
    /// then an object describing the dictionary rather than the record the API wanted, and walking those
    /// members can fail outright: SpawnDev.ILGPU could not create a single WebGPU compute pipeline
    /// because of it, surfacing as ArgumentException (Arg_DlgtTargMeth) far from the cause.<br/>
    /// <br/>
    /// Only STRING keys are claimed. Javascript object keys are strings, so a dictionary keyed by
    /// anything else would have to be stringified to cross, and silently reinterpreting a caller's keys
    /// is worse than not claiming the type.
    /// </summary>
    public class DictionaryMarshaller : JSMarshaller
    {
        /// <summary>
        /// Value type per dictionary type, or null when the type is not a string-keyed dictionary.
        /// Resolving it walks the interface list, so it is cached per type.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Type?> _valueTypes = new();

        private static Type? GetValueType(Type type) => _valueTypes.GetOrAdd(type, t =>
        {
            if (!t.IsGenericType) return null;
            var definition = t.GetGenericTypeDefinition();
            if (definition != typeof(Dictionary<,>)
                && definition != typeof(IDictionary<,>)
                && definition != typeof(IReadOnlyDictionary<,>))
                return null;
            var args = t.GetGenericArguments();
            return args[0] == typeof(string) ? args[1] : null;
        });

        /// <inheritdoc/>
        public override bool CanMarshal(Type? typeToConvert)
            => typeToConvert != null && GetValueType(typeToConvert) != null;

        /// <inheritdoc/>
        public override void NetToJS(Type? typeToConvert, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is IDictionary dictionary)
            {
                using var outObj = JS.NewJSObject();
                foreach (DictionaryEntry entry in dictionary)
                {
                    // Each value goes through the registry on its own, so a record can hold whatever the
                    // API accepts - numbers, strings, booleans, or a JSObject.
                    if (entry.Key is string key) JS.MarshallNetToJS(outObj, key, entry.Value);
                }
                // Assign through Javascript rather than Reflect.Set. The .Net runtime tags every JS
                // object it proxies with an enumerable Symbol key, and a record-typed web API enumerates
                // every own key and converts it to a string - which a Symbol cannot be. assignRecord
                // rebuilds the object from its string keys on the JS side so what the API receives is
                // clean. Detail in the JS command.
                JS.NetRunVoid("assignRecord", new object?[] { jsParent.JSObjectRequired, jsKey, outObj.JSObjectRequired });
            }
            else
            {
                Reflect.Set(jsParent.JSObjectRequired, jsKey, (string?)null);
            }
        }

        /// <inheritdoc/>
        public override object? JSToNet(Type typeToConvert, SpawnJSHandle jsHandle)
        {
            var valueType = GetValueType(typeToConvert);
            if (valueType == null) return null;
            var jsObject = jsHandle.JSObject;
            if (jsObject == null) return null;

            // Own keys only. Inherited keys belong to the prototype chain and are not part of the record.
            var keys = JS.NetRun<List<string>>("objectKeys", new object?[] { jsObject, true });
            var result = (IDictionary)Activator.CreateInstance(
                typeof(Dictionary<,>).MakeGenericType(typeof(string), valueType))!;
            if (keys == null) return result;
            foreach (var key in keys)
            {
                result[key] = JS.MarshallJSToNet(valueType, jsHandle, key);
            }
            return result;
        }
    }
}
