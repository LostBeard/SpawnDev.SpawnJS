using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls SpawnJSObjectReference and types that derive from SpawnJSObject
    /// </summary>
    public class SpawnJSObjectReferenceMarshaller : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? type) => type != null && typeof(SpawnJSObjectReference).IsAssignableFrom(type);
        /// <inheritdoc/>
        public override object? JSToNet(Type type, JSObject jsParent, object jsKey, SpawnJSRuntime runtime)
        {
            var value = Reflect.GetJSObject(jsParent, jsKey);
            return value == null ? null : new SpawnJSObjectReference(value);
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, JSObject jsParent, object jsKey, object? value, SpawnJSRuntime runtime)
        {
            if (value is SpawnJSObjectReference jsRef)
            {
                Reflect.SetJSObject(jsParent, jsKey, jsRef.JSObject);
            }
            else
            {
                Reflect.SetJSObject(jsParent, jsKey, null!);
            }
        }
    }
}
