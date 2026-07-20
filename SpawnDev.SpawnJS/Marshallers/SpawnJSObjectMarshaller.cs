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
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle)
        {
            var value = jsHandle.AsJSObject();
            return value == null ? null : Activator.CreateInstance(type, new SpawnJSObjectReference(value));
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is SpawnJSObject jsRef && jsRef.JSRef != null)
            {
                Reflect.Set(jsParent.JSObject, jsKey, jsRef.JSRef.JSObject);
            }
            else
            {
                Reflect.Set(jsParent.JSObject, jsKey, (string)null!);
            }
        }
    }
}
