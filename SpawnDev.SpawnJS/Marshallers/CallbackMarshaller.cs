using System.Diagnostics.CodeAnalysis;
using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class CallbackMarshaller<TCallback> : JSMarshallerFromString<TCallback?> where TCallback : Callback
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type) => typeof(Callback).IsAssignableFrom(type);
        /// <summary>
        /// Builds an <see cref="ArrayMarshaller{T}"/> bound to the concrete element type of
        /// <typeparamref name="T"/> (e.g. selecting for <c>int[]</c> yields an <c>ArrayMarshaller&lt;int&gt;</c>).
        /// </summary>
        public override JSMarshaller<T> GetMarshaller<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var marshallerTyped = typeof(CallbackMarshaller<>).MakeGenericType(typeof(T));
            return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
        }
        public override TCallback? JSToNet(string value)
        {
            throw new NotImplementedException();
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TCallback? value)
        {
            jsParent.PropertySet(jsKey, value);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TCallback? value)
        {
            jsParent.PropertySet(jsKey, value);
        }
    }
}
