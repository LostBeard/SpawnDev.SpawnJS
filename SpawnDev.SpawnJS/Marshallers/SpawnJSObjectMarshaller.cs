using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls SpawnJSObject and types that derive from SpawnJSObject
    /// </summary>
    public class SpawnJSObjectMarshaller : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? type) => type != null && typeof(SpawnJSObject).IsAssignableFrom(type);
        /// <inheritdoc/>
        public override object? JSToNet(Type type, JSObject jsParent, object jsKey, SpawnJSRuntime runtime)
        {
            var value = Reflect.GetJSObject(jsParent, jsKey);
            return value == null ? null : new SpawnJSObject(new SpawnJSObjectReference(value));
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, JSObject jsParent, object jsKey, object? value, SpawnJSRuntime runtime)
        {
            if (value is SpawnJSObject jsRef && jsRef.JSRef != null)
            {
                Reflect.SetJSObject(jsParent, jsKey, jsRef.JSRef.JSObject);
            }
            else
            {
                Reflect.SetJSObject(jsParent, jsKey, null!);
            }
        }
    }
}
