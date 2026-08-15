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
        public override TTuple JSToNet(SpawnJSObjectReference value)
        {
            if (value == null) return default!;
            var items = new object?[GenericTypes.Length];
            for (var i = 0; i < GenericTypes.Length; i++)
            {
                // Read slot i back through the typed Get<T> path, closed over the item's runtime type.
                items[i] = ((Delegate)readTyped<object>).InvokeGeneric(GenericTypes[i]);
                T readTyped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>() => value.Get<T>(i);
            }
            var tuple = Activator.CreateInstance(TypeT, items)!;
            value.Dispose();
            return (TTuple)tuple;
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TTuple value) => Write(jsParent, jsKey, value);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TTuple value) => Write(jsParent, jsKey, value);
        void Write(SpawnJSObjectReference jsParent, int jsKey, TTuple value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            using var array = JS.NewJSArray();
            for (var i = 0; i < value.Length; i++) array.Set(i, value[i]);
            jsParent.PropertySet(jsKey, array);
        }
        void Write(SpawnJSObjectReference jsParent, string jsKey, TTuple value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            using var array = JS.NewJSArray();
            for (var i = 0; i < value.Length; i++) array.Set(i, value[i]);
            jsParent.PropertySet(jsKey, array);
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
        public override TTuple? JSToNet(SpawnJSObjectReference value)
        {
            if (value == null) return null;
            return inner.JSToNet(value);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TTuple? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            inner.NetToJS(jsParent, jsKey, value.Value);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TTuple? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            inner.NetToJS(jsParent, jsKey, value.Value);
        }
    }
}
