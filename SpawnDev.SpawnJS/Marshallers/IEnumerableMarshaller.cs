using System.Collections;
using System.Diagnostics.CodeAnalysis;
using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls <see cref="IEnumerable{T}"/> to a Javascript array.
    /// <para>
    /// Also claims any concrete <see cref="IEnumerable"/> (except <see cref="string"/>) so collection-expression
    /// values such as compiler-generated single-element lists are written as JS arrays. Those concrete types are
    /// specialized via <see cref="IEnumerableConcreteWriteMarshaller{TCollection}"/>. Do not cast
    /// <c>IEnumerableMarshaller&lt;E&gt;</c> to <c>JSMarshaller&lt;ConcreteList&gt;</c> - that InvalidCastException
    /// broke Serial.requestPort / USB.requestDevice filter POCOs.
    /// </para>
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class IEnumerableMarshaller<TElement> : JSMarshallerFromSpawnJSObjectReference<IEnumerable<TElement>?>
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type)
            => type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);

        /// <summary>
        /// Specializes to <see cref="IEnumerableMarshaller{TElement}"/> when <typeparamref name="T"/> is
        /// <c>IEnumerable&lt;E&gt;</c>, or to <see cref="IEnumerableConcreteWriteMarshaller{TCollection}"/>
        /// when <typeparamref name="T"/> is a concrete collection that implements <see cref="IEnumerable"/>.
        /// </summary>
        [UnconditionalSuppressMessage("Trimming", "IL2055", Justification = "Element type from GetGenericArguments cannot carry DynamicallyAccessedMembers; write path only.")]
        [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "MakeGenericType over SpawnJS's own marshaller types.")]
        [UnconditionalSuppressMessage("Trimming", "IL2076", Justification = "See IL2055.")]
        public override JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            if (this is JSMarshaller<T> _this) return _this;

            var t = typeof(T);
            var underlying = Nullable.GetUnderlyingType(t) ?? t;

            // Exact IEnumerable<E> (declared POCO members often use this)
            if (underlying.IsGenericType && underlying.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var elementType = underlying.GetGenericArguments()[0];
                var marshallerTyped = typeof(IEnumerableMarshaller<>).MakeGenericType(elementType);
                return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
            }

            // Concrete IEnumerable (collection expressions, custom collections, ...).
            // do NOT use GetGenericArguments()[0] + cast IEnumerableMarshaller<E> -> JSMarshaller<T>:
            // that cast throws InvalidCastException for <>z__ReadOnlySingleElementList<E>.
            if (typeof(IEnumerable).IsAssignableFrom(underlying) && underlying != typeof(string))
            {
                var concrete = typeof(IEnumerableConcreteWriteMarshaller<>).MakeGenericType(typeof(T));
                return (JSMarshaller<T>)Activator.CreateInstance(concrete)!;
            }

            throw new NotSupportedException($"IEnumerableMarshaller cannot specialize for {typeof(T).FullName}");
        }

        /// <inheritdoc/>
        public override IEnumerable<TElement>? JSToNet(SpawnJSObjectReference? value)
        {
            throw new NotImplementedException();
        }

        ValueWriter<TElement>? _elements;
        /// <inheritdoc/>
        public override void Write(JSTape tape, IEnumerable<TElement>? value)
        {
            if (value == null) { tape.WriteNull(); return; }
            TapeCollections.WriteEnumerable(tape, value, _elements ??= new ValueWriter<TElement>());
        }


    }

    /// <summary>
    /// Write-only marshaller for a concrete collection type that implements <see cref="IEnumerable"/>.
    /// Used when Poco/arg marshalling sees a runtime collection-expression list rather than
    /// <c>IEnumerable&lt;E&gt;</c> itself.
    /// </summary>
    public class IEnumerableConcreteWriteMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TCollection>
        : JSMarshallerFromSpawnJSObjectReference<TCollection?>
    {
        /// <inheritdoc/>
        /// <remarks>Only constructed via <see cref="IEnumerableMarshaller{TElement}.GetMarshaller{T}"/>; never selected by CanMarshal scan.</remarks>
        public override bool CanMarshal(Type type) => false;

        /// <inheritdoc/>
        public override JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var concrete = typeof(IEnumerableConcreteWriteMarshaller<>).MakeGenericType(typeof(T));
            return (JSMarshaller<T>)Activator.CreateInstance(concrete)!;
        }

        /// <inheritdoc/>
        public override TCollection? JSToNet(SpawnJSObjectReference? value)
            => throw new NotImplementedException($"{nameof(IEnumerableConcreteWriteMarshaller<TCollection>)} is write-only.");

        /// <inheritdoc/>
        /// <remarks>Each element by what it IS, as the element-by-element v2 path wrote it.</remarks>
        public override void Write(JSTape tape, TCollection? value)
        {
            if (value == null) { tape.WriteNull(); return; }
            var token = tape.BeginArray();
            var count = 0;
            foreach (var item in (IEnumerable)value)
            {
                tape.WriteValue(item);
                count++;
            }
            tape.EndArray(token, count);
        }



    }
}
