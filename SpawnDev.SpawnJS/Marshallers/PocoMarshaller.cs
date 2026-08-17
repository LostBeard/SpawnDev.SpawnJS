using SpawnDev.SpawnJS.Marshaller;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Property-walking marshaller for plain .Net objects (POCOs), <b>class or struct</b>. It clones the
    /// object to/from a plain JS object member by member - NO JSON serialization is used; each member is
    /// marshalled through the normal marshaller graph. Respects the System.Text.Json attributes
    /// <c>[JsonPropertyName]</c> (member name) and <c>[JsonIgnore]</c> (Always / WhenWritingNull /
    /// WhenWritingDefault), plus <c>[JsonInclude]</c> for non-public members and for fields, via
    /// <see cref="ClassMemberJsonInfo"/> / <see cref="TypeExtensions.GetTypeJsonProperties"/>.
    /// <para>
    /// Structs are handled here rather than by a separate marshaller because they differ in exactly one
    /// place: the read has to build into a boxed instance, since SetValue boxes its target (see
    /// <see cref="JSToNet"/>). Everything else - the member walk, the Json attribute handling, the
    /// per-member marshaller resolution and its cache - is identical, and a second near-copy of it would
    /// be free to drift out of step with this one.
    /// <c>Nullable&lt;TStruct&gt;</c> marshals as the underlying struct, or as JS null.
    /// </para>
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
        {
            if (type == null || type == typeof(string) || type.IsArray || type.IsInterface || type.IsAbstract) return false;
            // A Nullable<TStruct> marshals as its underlying struct, or as JS null - so the decision is
            // made on the underlying type.
            var target = Nullable.GetUnderlyingType(type) ?? type;
            if (target.IsClass) return true;
            // Struct POCOs walk their members exactly like a class POCO does; the only difference is on the
            // read, where SetValue needs a boxed target (see JSToNet). Enums and primitives are value types
            // too, but they have their own marshallers - and this marshaller is registered FIRST, so it is
            // scanned LAST and only ever sees what nothing more specific claimed. The explicit exclusions
            // are there to state the intent, not because the ordering needs them.
            return target.IsValueType && !target.IsEnum && !target.IsPrimitive;
        }
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
        /// <remarks>
        /// A JS null/undefined reads as null for a class or a <c>Nullable&lt;TStruct&gt;</c>, and as the
        /// struct's default for a plain struct - a struct has no way to represent absence.
        /// </remarks>
        [UnconditionalSuppressMessage("Trimming", "IL2072",
            Justification = "The Nullable<> branch constructs the underlying type, which is always a value type. A value type needs no constructor to be created (the runtime zero-initializes it), so there is nothing for the trimmer to have removed. The non-nullable branch uses typeof(T), which carries the PublicConstructors requirement.")]
        public override T? JSToNet(SpawnJSObjectReference value)
        {
            if (value == null) return default;
            var underlying = Nullable.GetUnderlyingType(typeof(T));
            var targetType = underlying ?? typeof(T);
            // Build into a BOXED instance. PropertyInfo/FieldInfo.SetValue take their target as object, so
            // for a struct they box whatever is passed, mutate that temporary box, and discard it - setting
            // members on an unboxed local would throw every write away and hand back an all-defaults struct
            // with no error anywhere. Boxing once here and unboxing at the end is also exactly what the
            // class path was already doing, so one walk serves both.
            var obj = underlying == null ? Activator.CreateInstance(typeof(T))! : Activator.CreateInstance(underlying)!;
            foreach (var member in targetType.GetTypeJsonProperties())
            {
                var name = member.GetJsonName();
                var memberType = member.PropertyInfo?.PropertyType ?? member.FieldInfo!.FieldType;
                // runtime Type -> <TMember> so the value goes back through its own strongly typed marshaller
                var read = ((Delegate)readTyped<object>).InvokeGeneric(memberType, name);
                if (read == null) continue;
                member.PropertyInfo?.SetValue(obj, read);
                member.FieldInfo?.SetValue(obj, read);
            }
            // unboxing a boxed TStruct into Nullable<TStruct> is a legal unbox, so this covers both shapes
            return (T)obj;

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
            // The members to walk are the underlying type's. A write normally types itself from the boxed
            // VALUE - and boxing a non-null Nullable<TStruct> already yields a boxed TStruct - so T is
            // rarely Nullable here; when it is, walking Nullable<> itself would marshal HasValue and Value
            // instead of the struct's own members.
            var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            foreach (var member in targetType.GetTypeJsonProperties())
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
