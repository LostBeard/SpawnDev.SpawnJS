using System.Collections;
using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls IEnumerable to Javascript
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class IEnumerableMarshaller<TElement> : JSMarshallerFromSpawnJSObjectReference<IEnumerable<TElement>?>
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type) => typeof(IEnumerable).IsAssignableFrom(type);
        /// <summary>
        /// Builds an <see cref="ArrayMarshaller{T}"/> bound to the concrete element type of
        /// <typeparamref name="T"/> (e.g. selecting for <c>int[]</c> yields an <c>ArrayMarshaller&lt;int&gt;</c>).
        /// </summary>
        public override JSMarshaller<T> GetMarshaller<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var elementType = typeof(T).GetGenericArguments()[0];
            var marshallerTyped = typeof(IEnumerableMarshaller<>).MakeGenericType(elementType);
            return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
        }
        public override IEnumerable<TElement>? JSToNet(SpawnJSObjectReference? value)
        {
            throw new NotImplementedException();
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, IEnumerable<TElement>? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            using var outArray = JS.NewJSArray();
            var i = 0;
            foreach (var item in value)
            {
                outArray.Set(i, item);
                i++;
            }
            jsParent.PropertySet(jsKey, outArray);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, IEnumerable<TElement>? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            using var outArray = JS.NewJSArray();
            var i = 0;
            foreach (var item in value)
            {
                outArray.Set(i, item);
                i++;
            }
            jsParent.PropertySet(jsKey, outArray);
        }
    }
}
