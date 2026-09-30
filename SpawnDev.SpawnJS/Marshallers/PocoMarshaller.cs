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

        PocoReadPlan? _readPlan;
        PocoReadPlan ReadPlan => _readPlan ??= PocoReadPlan.For(Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
        /// <inheritdoc/>
        /// <remarks>An Object schema: every member, by its declared type, in the call's one crossing.</remarks>
        public override JSSchema Schema => ReadPlan.Schema;
        /// <inheritdoc/>
        public override T? Read(ref JSTapeReader reader)
        {
            var plan = ReadPlan;
            if (plan is PocoReadPlan<T> typed) return typed.Read(ref reader, out var value) ? value : default;
            // Nullable<TStruct>: read the struct, then unbox it into the nullable
            var boxed = plan.ReadBoxed(ref reader);
            return boxed == null ? default : (T)boxed;
        }

        PocoWritePlan<T>? _plan;
        /// <inheritdoc/>
        /// <remarks>
        /// Written as what the value IS: a subclass of T is walked as the subclass (a Pbkdf2Params passed as its
        /// KeyDeriveParams base keeps salt / hash / iterations), and a boxed Nullable&lt;TStruct&gt; as its struct.
        /// </remarks>
        public override void Write(JSTape tape, T? value)
        {
            if (value == null) { tape.WriteNull(); return; }
            // a plain struct cannot be anything else, and asking would box it
            if (typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) == null)
            {
                (_plan ??= (PocoWritePlan<T>)PocoWritePlan.For(typeof(T))).Write(tape, value);
                return;
            }
            var type = value.GetType();
            if (type == typeof(T)) (_plan ??= (PocoWritePlan<T>)PocoWritePlan.For(type)).Write(tape, value);
            else PocoWritePlan.For(type).WriteBoxed(tape, value);
        }



    }
}
