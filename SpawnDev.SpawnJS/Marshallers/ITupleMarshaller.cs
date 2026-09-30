using SpawnDev.SpawnJS.Marshaller;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals a .Net tuple (<see cref="Tuple{T1}"/> or <see cref="ValueTuple{T1}"/> and their higher
    /// arities) to/from a JS array - one JS array slot per tuple item, each item going through its own
    /// strongly-typed marshaller. The nullable case (<c>ValueTuple&lt;...&gt;?</c>) is handled by
    /// <see cref="ITupleNullableMarshaller{TTuple}"/>; <see cref="Tuple{T1}"/> is a reference type and is
    /// never <see cref="Nullable{T}"/>-wrapped.
    /// </summary>
    public class ITupleMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTuple> : JSMarshallerFromSpawnJSObjectReference<TTuple> where TTuple : ITuple
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        readonly Type TypeT;
        readonly Type[] GenericTypes;
        public ITupleMarshaller()
        {
            TypeT = typeof(TTuple);
            GenericTypes = TypeT.GenericTypeArguments;
        }
        JSMarshaller[]? _itemMarshallers;
        JSSchema? _schema;
        JSMarshaller[] ItemMarshallers => _itemMarshallers ??= GenericTypes.Select(t => JS.GetMarshaller(t)).ToArray();
        /// <inheritdoc/>
        /// <remarks>A Tuple: read positionally, each item by its own type's schema.</remarks>
        public override JSSchema Schema
        {
            get
            {
                if (_schema != null) return _schema;
                var schema = JSSchema.Tuple(GenericTypes.Length);
                var items = ItemMarshallers;
                for (var i = 0; i < items.Length; i++) schema.Members![i] = items[i].Schema;
                return _schema = schema;
            }
        }
        /// <inheritdoc/>
        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "As JSToNet: the tuple's own constructor.")]
        public override TTuple Read(ref JSTapeReader reader)
        {
            var count = reader.ReadCount();
            if (count < 0) return default!;
            var markers = ItemMarshallers;
            var items = new object?[markers.Length];
            for (var i = 0; i < markers.Length; i++) items[i] = markers[i].ReadBoxed(ref reader);
            return (TTuple)Activator.CreateInstance(TypeT, items)!;
        }
        /// <inheritdoc/>
        /// <remarks>A Javascript array, positional; each item by what it IS.</remarks>
        public override void Write(JSTape tape, TTuple value)
        {
            if (value == null) { tape.WriteNull(); return; }
            tape.WriteArray(value.Length);
            for (var i = 0; i < value.Length; i++) tape.WriteValue(value[i]);
        }
    }
    /// <summary>
    /// Marshals a nullable value tuple (<c>ValueTuple&lt;...&gt;?</c>). A <see cref="Nullable{T}"/> of a
    /// value tuple does NOT itself implement <see cref="ITuple"/>, so it cannot flow through
    /// <see cref="ITupleMarshaller{TTuple}"/> directly (that was the <c>ValueTuple?</c> failure); this
    /// wrapper handles the null case and delegates the underlying tuple to an inner
    /// <see cref="ITupleMarshaller{TTuple}"/>.
    /// </summary>
    public class ITupleNullableMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTuple> : JSMarshallerFromSpawnJSObjectReference<TTuple?> where TTuple : struct, ITuple
    {
        readonly ITupleMarshaller<TTuple> inner = new();
        /// <inheritdoc/>
        public override JSSchema Schema => inner.Schema;
        /// <inheritdoc/>
        public override TTuple? Read(ref JSTapeReader reader)
        {
            // the inner read returns default for null; null has to stay null here
            if (reader.PeekNull()) { reader.SkipNull(); return null; }
            return inner.Read(ref reader);
        }
        /// <inheritdoc/>
        public override void Write(JSTape tape, TTuple? value)
        {
            if (value == null) { tape.WriteNull(); return; }
            var tuple = value.Value;
            tape.WriteArray(tuple.Length);
            for (var i = 0; i < tuple.Length; i++) tape.WriteValue(tuple[i]);
        }
    }
}
