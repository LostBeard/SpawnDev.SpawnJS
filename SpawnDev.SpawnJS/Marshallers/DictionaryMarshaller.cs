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
                // Slot to slot, so neither object is ever proxied.
                //
                // This used to call a JS assignRecord command that rebuilt the object from its string
                // keys. That existed because the .Net runtime tags every JS object it proxies with an
                // enumerable Symbol, and a record-typed web API enumerates every own key and converts it
                // to a string - which a Symbol cannot be, so WebGPU threw "Cannot convert a Symbol value
                // to a string". Rebuilding stripped the tag.
                //
                // Not being proxied removes the tag at its source instead: there is nothing to strip.
                jsParent.SetProperty(jsKey, outObj);
            }
            else
            {
                jsParent.SetProperty(jsKey, (string?)null);
            }
        }

        /// <inheritdoc/>
        public override object? JSToNet(Type typeToConvert, SpawnJSHandle jsHandle)
        {
            var valueType = GetValueType(typeToConvert);
            if (valueType == null) return null;
            // Own keys only. Inherited keys belong to the prototype chain and are not part of the record.
            //
            // Take the record into a handle that OWNS it first, and then both halves are slot native: the
            // key enumeration, and every value read, because each value is addressed against a parent that
            // has a slot. Reading them against the handle we were given would not be - a marshalled handle
            // borrows its parent's storage, so every member read would resolve a proxy for the record.
            //
            // That proxy also tagged the record with the enumerable Symbol that record-typed web APIs
            // choke on, so the READ path was reintroducing the exact hazard the write path was cleared of.
            IReadOnlyList<string>? keys;
            SpawnJSHandle? owned = null;
            if (jsHandle.TryTakeOwnedValue(out owned))
            {
                // a null owned handle means the value is null or undefined - the same answer the proxy
                // path gave by handing back a null JSObject. An empty dictionary would turn a null record
                // into a present but empty one.
                if (owned == null) return null;
            }
            using (owned)
            {
                var source = owned ?? jsHandle;
                if (owned != null)
                {
                    keys = owned.GetOwnKeysBySlot();
                    if (keys == null) return null;
                }
                else
                {
                    var jsObject = jsHandle.JSObject;
                    if (jsObject == null) return null;
                    keys = JS.NetRun<List<string>>("objectKeys", new object?[] { jsObject, true });
                }
                return BuildDictionary(valueType, keys, source);
            }
        }

        /// <summary>
        /// Fills the dictionary, reading each value against <paramref name="source"/> - which is the record
        /// itself, so the reads are slot native whenever it is slotted.
        /// </summary>
        object BuildDictionary(Type valueType, IReadOnlyList<string>? keys, SpawnJSHandle source)
        {
            var result = (IDictionary)Activator.CreateInstance(
                typeof(Dictionary<,>).MakeGenericType(typeof(string), valueType))!;
            if (keys == null) return result;
            foreach (var key in keys)
            {
                result[key] = JS.MarshallJSToNet(valueType, source, key);
            }
            return result;
        }
    }
}
