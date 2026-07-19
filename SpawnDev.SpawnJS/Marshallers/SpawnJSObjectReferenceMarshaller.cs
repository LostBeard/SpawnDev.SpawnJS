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
        public override object? JSToNet(Type type, SpawnJSHandle jsParent, object jsKey)
        {
            var value = jsParent.GetPropertyAsJSObject(jsKey);
            return value == null ? null : Activator.CreateInstance(type, value);
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is SpawnJSObjectReference jsRef)
            {
                Reflect.Set(jsParent.JSObject, jsKey, jsRef.JSObject);
            }
            else
            {
                Reflect.Set(jsParent.JSObject, jsKey, (string)null!);
            }
        }
    }
}
