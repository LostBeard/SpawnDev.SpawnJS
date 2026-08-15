using SpawnDev.SpawnJS.Marshaller;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Property-walking marshaller for plain .Net objects (POCOs). It clones the object to/from a plain JS
    /// object member by member - NO JSON serialization is used; each member is marshalled through the normal
    /// marshaller graph. Respects the System.Text.Json attributes <c>[JsonPropertyName]</c> (member name) and
    /// <c>[JsonIgnore]</c> (Always / WhenWritingNull / WhenWritingDefault), plus <c>[JsonInclude]</c> for
    /// non-public members, via <see cref="ClassMemberJsonInfo"/> / <see cref="TypeExtensions.GetTypeJsonProperties"/>.
    /// <para>
    /// This is the most generic marshaller, so it is registered FIRST (lowest priority - resolution scans in
    /// reverse) and only wins when no more specific marshaller (wrapper, array, string, primitive, ...) matches.
    /// </para>
    /// <para>
    /// Trimming: the type parameter carries <see cref="DynamicallyAccessedMemberTypes.PublicConstructors"/> so
    /// the parameterless ctor survives. A consumer marshalling their own POCO in a trimmed app is responsible
    /// for preserving that type's property/field accessors (e.g. by using them, a <c>[DynamicDependency]</c>, or
    /// a trimmer descriptor) - the same contract as reflection-based object mapping.
    /// </para>
    /// </summary>
    public class PocoMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T> : JSMarshallerFromSpawnJSObjectReference<T?>
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type)
            => type != null
               && type.IsClass
               && !type.IsInterface
               && !type.IsAbstract
               && !type.IsArray
               && type != typeof(string);
        // NOTE: no need to exclude SpawnJSObject wrappers here - SpawnJSObjectMarshaller is registered later
        // (higher priority in the reverse scan) and wins for those. SpawnJSObject also lives in the JSObjects
        // assembly, which depends on Core, so it is not referenceable from here anyway.

        /// <inheritdoc/>
        public override JSMarshaller<TT> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TT>()
        {
            if (this is JSMarshaller<TT> _this) return _this;
            var marshallerType = typeof(PocoMarshaller<>).MakeGenericType(typeof(TT));
            return (JSMarshaller<TT>)Activator.CreateInstance(marshallerType)!;
        }

        /// <inheritdoc/>
        public override T? JSToNet(SpawnJSObjectReference value)
        {
            if (value == null) return default;
            var obj = (T)Activator.CreateInstance(typeof(T))!;
            foreach (var member in typeof(T).GetTypeJsonProperties())
            {
                var name = member.GetJsonName();
                var memberType = member.PropertyInfo?.PropertyType ?? member.FieldInfo!.FieldType;
                // runtime Type -> <TMember> so the value goes back through its own strongly typed marshaller
                var read = ((Delegate)readTyped<object>).InvokeGeneric(memberType, name);
                if (read == null) continue;
                member.PropertyInfo?.SetValue(obj, read);
                member.FieldInfo?.SetValue(obj, read);
            }
            return obj;

            object? readTyped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMember>(string key) => value.Get<TMember>(key);
        }

        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, T? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            jsParent.Set(jsKey, WriteToNewObject(value));
        }

        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, T? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            jsParent.Set(jsKey, WriteToNewObject(value));
        }

        SpawnJSObjectReference WriteToNewObject(T value)
        {
            var outObj = JS.New<SpawnJSObjectReference>("Object");
            foreach (var member in typeof(T).GetTypeJsonProperties())
            {
                var memberValue = member.PropertyInfo != null
                    ? member.PropertyInfo.GetValue(value)
                    : member.FieldInfo!.GetValue(value);
                if (!member.GetShouldWrite(memberValue)) continue; // honours [JsonIgnore] Always/WhenWritingNull/WhenWritingDefault
                var name = member.GetJsonName();
                if (memberValue == null) { outObj.PropertySetNull(name); continue; }
                // runtime Type -> <TMember> write with no boxing, straight into the new JS object by name
                var memberType = memberValue.GetType();
                ((Delegate)writeTyped<object>).InvokeGeneric(memberType, memberValue);
                void writeTyped<TMember>(TMember v)
                {
                    // When the member's runtime type is fixed (value type or sealed), resolve its marshaller
                    // once and reuse it - the per-member Type->marshaller lookup is otherwise repaid on every
                    // marshal. Otherwise a base-typed member may hold any subclass, so it must resolve per value.
                    var marshaller = member.RuntimeTypeIsKnown
                        ? (JSMarshaller<TMember>)(member.CachedMarshaller ??= JS.GetMarshallerForWrite<TMember>())
                        : JS.GetMarshallerForWrite<TMember>();
                    marshaller.NetToJS(outObj, name, v);
                }
            }
            return outObj;
        }
    }
}
