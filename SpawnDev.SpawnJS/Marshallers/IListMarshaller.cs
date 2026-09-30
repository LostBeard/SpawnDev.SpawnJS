using System.Diagnostics.CodeAnalysis;
using SpawnDev.SpawnJS.Marshaller;
using System.Collections;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals a .Net IList to/from a JS array. When selected it re-specializes to the concrete element type (see
    /// <see cref="GetMarshaller{T}"/>) so each element goes through its own strongly-typed marshaller with no boxing.
    /// <para>
    /// ⚠️ NOT REGISTERED (neither in v2 nor here): a value declared IList&lt;T&gt; is marshalled by its runtime type
    /// (a List&lt;T&gt; by ListMarshaller). Its CanMarshal used to test for List&lt;&gt; - ListMarshaller's type -
    /// and now tests IList&lt;&gt;, so it is correct if it is ever registered.
    /// </para>
    /// </summary>
    public class IListMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TElement> : JSMarshallerFromSpawnJSObjectReference<IList<TElement>?>
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type)
        {
            var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
            return genericType == typeof(IList<>);
        }
        /// <summary>
        /// Builds an <see cref="ArrayMarshaller{T}"/> bound to the concrete element type of
        /// <typeparamref name="T"/> (e.g. selecting for <c>int[]</c> yields an <c>ArrayMarshaller&lt;int&gt;</c>).
        /// </summary>
        [UnconditionalSuppressMessage("Trimming", "IL2076", Justification = "See IL2055.")]
        [UnconditionalSuppressMessage("Trimming", "IL2055",
            Justification = "GetMarshaller specializes to the element type obtained by reflection (GetGenericArguments), which cannot carry DynamicallyAccessedMembers. Built-in wrapper element ctors are preserved by the embedded ILLink.Descriptors.xml; a consumer using a custom SpawnJSObject wrapper as a list element in a trimmed app must preserve that type's ctor itself (reflection boundary).")]
        public override JSMarshaller<T> GetMarshaller<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var type = typeof(T);
            var elementType = type.GetGenericArguments()[0];
            Type openType = typeof(IListMarshaller<>);
            Type typedMarshaller = openType.MakeGenericType(elementType!);
            return (JSMarshaller<T>)Activator.CreateInstance(typedMarshaller)!;
        }
        JSMarshaller<TElement>? _elementMarshaller;
        JSSchema? _schema;
        JSMarshaller<TElement> ElementMarshaller => _elementMarshaller ??= JS.GetMarshaller<TElement>();
        /// <inheritdoc/>
        /// <remarks>Numbers in one copy when each element is an Int32 or Double; an Array of the element schema otherwise.</remarks>
        public override JSSchema Schema => _schema ??= TapeReads.CollectionSchema(ElementMarshaller.Schema);
        /// <inheritdoc/>
        public override IList<TElement>? Read(ref JSTapeReader reader)
        {
            var count = reader.ReadCount();
            return count < 0 ? null : TapeReads.ReadList(ref reader, Schema, ElementMarshaller, count);
        }
        ValueWriter<TElement>? _elements;
        /// <inheritdoc/>
        public override void Write(JSTape tape, IList<TElement>? value)
        {
            if (value == null) { tape.WriteNull(); return; }
            TapeCollections.WriteEnumerable(tape, value, _elements ??= new ValueWriter<TElement>());
        }
    }
}
