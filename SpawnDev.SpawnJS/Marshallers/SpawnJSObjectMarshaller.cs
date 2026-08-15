
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.Marshaller;
using System.Diagnostics.CodeAnalysis;

// DAM(PublicConstructors) on TSpawnJSObject makes the trimmer preserve the wrapper's
// .ctor(SpawnJSObjectReference). The requirement flows in from the generic interop entry points
// (As<T>/Get<T>/Call<T>...), so a consumer's own wrapper is preserved automatically when named
// concretely, and a consumer flowing an abstract generic T gets an actionable warning to annotate it.
public class SpawnJSObjectMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TSpawnJSObject> : JSMarshallerFromSpawnJSObjectReference<TSpawnJSObject?> where TSpawnJSObject : SpawnJSObject
{
    /// <inheritdoc/>
    public override bool CanMarshal(Type type) => typeof(SpawnJSObject).IsAssignableFrom(type);

    /// <inheritdoc/>
    public override JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
    {
        if (this is JSMarshaller<T> _this) return _this;
        var marshallerTyped = typeof(SpawnJSObjectMarshaller<>).MakeGenericType(typeof(T));
        return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
    }
    /// <inheritdoc/>
    public override TSpawnJSObject? JSToNet(SpawnJSObjectReference? value)
    {
        return value == null ? null : (TSpawnJSObject)Activator.CreateInstance(typeof(TSpawnJSObject), value)!;
    }
    /// <inheritdoc/>
    public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TSpawnJSObject? value)
    {
        jsParent.Set(jsKey, value?.JSRef);
    }
    /// <inheritdoc/>
    public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TSpawnJSObject? value)
    {
        jsParent.Set(jsKey, value?.JSRef);
    }
}