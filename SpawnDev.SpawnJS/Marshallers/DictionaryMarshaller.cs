using SpawnDev.SpawnJS.Marshaller;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals a dictionary to/from a plain Javascript object - what the web platform means by a
    /// "record" (e.g. <c>GPUProgrammableStage.constants</c> is <c>record&lt;USVString, GPUPipelineConstantValue&gt;</c>,
    /// and <c>trustedTypes.createPolicy</c> takes a record of create* callbacks). Registered as
    /// <c>DictionaryMarshaller&lt;Dictionary&lt;string, object&gt;&gt;</c>, but when selected it re-specializes to the
    /// requested dictionary type (see <see cref="GetMarshaller{T}"/>) so each value goes through its own
    /// strongly-typed marshaller.
    /// <para>
    /// Without this a <c>Dictionary&lt;,&gt;</c> matches <see cref="PocoMarshaller{T}"/>, which property-walks the
    /// dictionary's OWN members (Comparer, Count, Keys, Values) instead of its contents. That both crosses the wrong
    /// shape AND can fail outright - the comparer/collection members drag in delegates, surfacing as
    /// <c>ArgumentException (Arg_DlgtTargMeth)</c> far from the cause (it broke SpawnDev.ILGPU WebGPU pipeline
    /// creation and SpawnJS.RazorRenderer Trusted Types policy creation). This marshaller must therefore out-rank
    /// PocoMarshaller in the reverse-scan registration order.
    /// </para>
    /// <para>
    /// Any key type is claimed. Javascript object keys are always strings, so a non-string key is written as its
    /// invariant string form (<c>key.ToString()</c> for <c>int</c>/<c>long</c>/<c>enum</c>/etc) - exactly what
    /// <c>obj[1] = x</c> does in JS - and read back by parsing that string into the concrete key type. This is the
    /// same lossless round trip JS itself performs; e.g. SpawnDev.ILGPU's WebGL dispatch sends a
    /// <c>Dictionary&lt;int, int[]&gt;</c> stride map that becomes <c>{"0":[...],"1":[...]}</c> and back.
    /// </para>
    /// </summary>
    public class DictionaryMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TDictionary> : JSMarshallerFromSpawnJSObjectReference<TDictionary?>
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type)
        {
            if (type == null || !type.IsGenericType) return false;
            var definition = type.GetGenericTypeDefinition();
            return definition == typeof(Dictionary<,>)
                || definition == typeof(IDictionary<,>)
                || definition == typeof(IReadOnlyDictionary<,>);
        }

        /// <summary>Re-specializes this marshaller to the concrete requested dictionary type.</summary>
        [UnconditionalSuppressMessage("Trimming", "IL2076", Justification = "See IL2055.")]
        [UnconditionalSuppressMessage("Trimming", "IL2055",
            Justification = "MakeGenericType over SpawnJS's own DictionaryMarshaller<>, closed with the requested dictionary type. The marshaller carries no PublicConstructors requirement on TDictionary beyond its own parameterless ctor. Verified to survive a trimmed WASM publish.")]
        public override JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var typedMarshaller = typeof(DictionaryMarshaller<>).MakeGenericType(typeof(T));
            return (JSMarshaller<T>)Activator.CreateInstance(typedMarshaller)!;
        }

        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TDictionary? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            jsParent.Set(jsKey, WriteToNewObject(value));
        }

        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TDictionary? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            jsParent.Set(jsKey, WriteToNewObject(value));
        }

        SpawnJSObjectReference WriteToNewObject(TDictionary value)
        {
            var outObj = JS.New<SpawnJSObjectReference>("Object");
            if (value is IDictionary dictionary)
            {
                foreach (DictionaryEntry entry in dictionary)
                {
                    // Javascript object keys are strings; stringify any non-string key the same way JS does
                    // (obj[1] = x stores under "1"). null keys can't exist in a Dictionary<,>.
                    var key = KeyToString(entry.Key);
                    if (entry.Value == null) { outObj.PropertySetNull(key); continue; }
                    // runtime Type -> <TValue> so each value goes through its own strongly-typed marshaller with
                    // no boxing (a record can hold numbers, strings, booleans, a JSObject wrapper, a Callback...).
                    var valueType = entry.Value.GetType();
                    ((Delegate)writeTyped<object>).InvokeGeneric(valueType, entry.Value);
                    void writeTyped<TValue>(TValue v) => JS.GetMarshallerForWrite<TValue>().NetToJS(outObj, key, v);
                }
            }
            return outObj;
        }

        /// <inheritdoc/>
        [UnconditionalSuppressMessage("Trimming", "IL2055",
            Justification = "MakeGenericType(Dictionary<,>, keyType, valueType) builds the concrete result dictionary; Dictionary<,> is a framework type whose public constructors are always preserved.")]
        [UnconditionalSuppressMessage("Trimming", "IL2072",
            Justification = "Activator over the closed Dictionary<keyType, valueType> (framework type, parameterless ctor always preserved).")]
        public override TDictionary? JSToNet(SpawnJSObjectReference value)
        {
            if (value == null) return default;
            var typeArgs = typeof(TDictionary).GetGenericArguments();
            var keyType = typeArgs[0];
            var valueType = typeArgs[1];
            var result = (IDictionary)Activator.CreateInstance(
                typeof(Dictionary<,>).MakeGenericType(keyType, valueType))!;
            // Own enumerable keys only - inherited keys belong to the prototype chain, not the record.
            foreach (var key in value.Keys(true))
            {
                result[KeyFromString(key, keyType)] = ((Delegate)readTyped<object>).InvokeGeneric(valueType, key);
            }
            value.Dispose();
            return (TDictionary)result;

            object? readTyped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TValue>(string k) => value.Get<TValue>(k);
        }

        /// <summary>Invariant string form of a dictionary key, matching how Javascript coerces object keys.</summary>
        static string KeyToString(object key)
            => key is string s ? s : Convert.ToString(key, CultureInfo.InvariantCulture) ?? string.Empty;

        /// <summary>Parses a Javascript object key (always a string) back into the concrete .Net key type.</summary>
        [UnconditionalSuppressMessage("Trimming", "IL2067",
            Justification = "keyType is the first generic argument of TDictionary (a Dictionary/IDictionary/IReadOnlyDictionary). Enum key types keep their fields (the runtime never trims enum members) and framework IConvertible key types (int, long, string, Guid...) are preserved.")]
        static object KeyFromString(string key, Type keyType)
        {
            if (keyType == typeof(string)) return key;
            if (keyType.IsEnum) return Enum.Parse(keyType, key);
            return Convert.ChangeType(key, keyType, CultureInfo.InvariantCulture);
        }
    }
}
