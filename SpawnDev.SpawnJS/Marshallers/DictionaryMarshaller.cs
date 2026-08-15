using SpawnDev.SpawnJS.Marshaller;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals a STRING-keyed dictionary to/from a plain Javascript object - what the web platform means by a
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
    /// Only STRING keys are claimed. Javascript object keys are strings, so a dictionary keyed by anything else
    /// would have to be stringified to cross, and silently reinterpreting a caller's keys is worse than not
    /// claiming the type - it falls through to the general object handling instead.
    /// </para>
    /// </summary>
    public class DictionaryMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TDictionary> : JSMarshallerFromSpawnJSObjectReference<TDictionary?>
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type)
        {
            if (type == null || !type.IsGenericType) return false;
            var definition = type.GetGenericTypeDefinition();
            if (definition != typeof(Dictionary<,>)
                && definition != typeof(IDictionary<,>)
                && definition != typeof(IReadOnlyDictionary<,>)) return false;
            return type.GetGenericArguments()[0] == typeof(string);
        }

        /// <summary>Re-specializes this marshaller to the concrete requested dictionary type.</summary>
        [UnconditionalSuppressMessage("Trimming", "IL2076", Justification = "See IL2055.")]
        [UnconditionalSuppressMessage("Trimming", "IL2055",
            Justification = "MakeGenericType over SpawnJS's own DictionaryMarshaller<>, closed with the requested (string-keyed) dictionary type. The marshaller carries no PublicConstructors requirement on TDictionary beyond its own parameterless ctor. Verified to survive a trimmed WASM publish.")]
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
                    if (entry.Key is not string key) continue;
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
            Justification = "MakeGenericType(Dictionary<,>, string, valueType) builds the concrete result dictionary; Dictionary<,> is a framework type whose public constructors are always preserved.")]
        [UnconditionalSuppressMessage("Trimming", "IL2072",
            Justification = "Activator over the closed Dictionary<string, valueType> (framework type, parameterless ctor always preserved).")]
        public override TDictionary? JSToNet(SpawnJSObjectReference value)
        {
            if (value == null) return default;
            var valueType = typeof(TDictionary).GetGenericArguments()[1];
            var result = (IDictionary)Activator.CreateInstance(
                typeof(Dictionary<,>).MakeGenericType(typeof(string), valueType))!;
            // Own enumerable keys only - inherited keys belong to the prototype chain, not the record.
            foreach (var key in value.Keys(true))
            {
                result[key] = ((Delegate)readTyped<object>).InvokeGeneric(valueType, key);
            }
            value.Dispose();
            return (TDictionary)result;

            object? readTyped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TValue>(string key) => value.Get<TValue>(key);
        }
    }
}
