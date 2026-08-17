using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Marshaller;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals a <see cref="HeapView"/> out to Javascript as the ArrayBufferView it created.
    /// <para>
    /// Registered as <c>HeapViewMarshaller&lt;HeapView&gt;</c>, but a caller almost always holds the
    /// concrete <c>HeapView&lt;TElement, TView&gt;</c> that <c>HeapView.Create</c> returns, so this
    /// re-specializes to the requested type (see <see cref="GetMarshaller{T}"/>). Without that the
    /// default <c>(JSMarshaller&lt;T&gt;)this</c> cast fails with InvalidCastException on every typed
    /// HeapView - the ordinary usage.
    /// </para>
    /// Out only: a Javascript ArrayBufferView cannot become a .Net HeapView, since a HeapView IS pinned
    /// .Net memory rather than a wrapper around a Javascript buffer.
    /// </summary>
    public class HeapViewMarshaller<THeapView> : JSMarshallerFromString<THeapView?> where THeapView : HeapView
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type) => type.IsAssignableTo(typeof(HeapView));
        /// <inheritdoc/>
        [UnconditionalSuppressMessage("Trimming", "IL2055",
            Justification = "MakeGenericType over SpawnJS's own HeapViewMarshaller<>, closed with the requested HeapView type (constrained to HeapView).")]
        [UnconditionalSuppressMessage("Trimming", "IL2071",
            Justification = "The closed type argument is a HeapView subclass; the marshaller's own constraint carries no PublicConstructors requirement, so trimming preserves nothing extra.")]
        [UnconditionalSuppressMessage("Trimming", "IL2072",
            Justification = "Activator over SpawnJS's own HeapViewMarshaller<> (parameterless ctor), referenced via typeof here.")]
        public override JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var marshallerTyped = typeof(HeapViewMarshaller<>).MakeGenericType(typeof(T));
            return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
        }
        /// <inheritdoc/>
        public override THeapView? JSToNet(string value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, THeapView? value)
        {
            if (value?.RefreshCopyOnMarshal == true) value.RefreshCopy();
            jsParent.Set(jsKey, value?._View);
        }
        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, THeapView? value)
        {
            if (value?.RefreshCopyOnMarshal == true) value.RefreshCopy();
            jsParent.Set(jsKey, value?._View);
        }
    }
}
