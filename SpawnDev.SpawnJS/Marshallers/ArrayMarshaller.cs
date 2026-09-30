using System.Diagnostics.CodeAnalysis;
using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals a .Net array to/from a JS array. Registered as <c>ArrayMarshaller&lt;object&gt;</c>, but
    /// when selected it re-specializes to the concrete element type (see <see cref="GetMarshaller{T}"/>) so
    /// each element goes through its own strongly-typed marshaller with no boxing.
    /// </summary>
    public class ArrayMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TElement> : JSMarshallerFromSpawnJSObjectReference<TElement[]?>
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type)
        {
            if (type == null) return false;
            var ret = type.IsArray && type.HasElementType;
            return ret;
        }
        /// <summary>
        /// Builds an <see cref="ArrayMarshaller{T}"/> bound to the concrete element type of
        /// <typeparamref name="T"/> (e.g. selecting for <c>int[]</c> yields an <c>ArrayMarshaller&lt;int&gt;</c>).
        /// </summary>
        [UnconditionalSuppressMessage("Trimming", "IL2055", Justification = "See IL2076.")]
        [UnconditionalSuppressMessage("Trimming", "IL2076",
            Justification = "GetMarshaller specializes to the element type obtained by reflection (GetElementType), which cannot carry DynamicallyAccessedMembers. Built-in wrapper element ctors are preserved by the embedded ILLink.Descriptors.xml; a consumer using a custom SpawnJSObject wrapper as an array element in a trimmed app must preserve that type's ctor itself (reflection boundary).")]
        public override JSMarshaller<T> GetMarshaller<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var typeOfT = typeof(T);
            var elementType = typeOfT.GetElementType();
            Type openType = typeof(ArrayMarshaller<>);
            Type tyepdArrayMarshaller = openType.MakeGenericType(elementType!);
            return (JSMarshaller<T>)Activator.CreateInstance(tyepdArrayMarshaller)!;
        }
        JSMarshaller<TElement>? _elementMarshaller;
        JSSchema? _schema;
        JSMarshaller<TElement> ElementMarshaller => _elementMarshaller ??= JS.GetMarshaller<TElement>();
        /// <inheritdoc/>
        /// <remarks>Numbers in one copy when each element is an Int32 or Double; an Array of the element schema otherwise.</remarks>
        public override JSSchema Schema => _schema ??= TapeReads.CollectionSchema(ElementMarshaller.Schema);
        /// <inheritdoc/>
        public override TElement[]? Read(ref JSTapeReader reader)
        {
            var count = reader.ReadCount();
            if (count < 0) return null;
            var array = new TElement[count];
            TapeReads.ReadElements(ref reader, Schema, ElementMarshaller, array);
            return array;
        }
        ValueWriter<TElement>? _elements;
        /// <inheritdoc/>
        /// <remarks>A number array crosses in one copy; anything else element by element, each by what it IS.</remarks>
        public override void Write(JSTape tape, TElement[]? value)
        {
            if (value == null) { tape.WriteNull(); return; }
            TapeCollections.Write(tape, value, _elements ??= new ValueWriter<TElement>());
        }
    }
}
