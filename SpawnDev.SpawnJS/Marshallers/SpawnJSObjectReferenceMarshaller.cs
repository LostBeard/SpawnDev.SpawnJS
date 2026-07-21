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
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle)
        {
            var value = jsHandle.AsJSObject();
            return value == null ? null : Activator.CreateInstance(type, value);
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is SpawnJSObjectReference jsRef)
            {
                // handle to handle: neither the parent nor the referenced value becomes a .Net proxy, so
                // neither picks up the runtime's enumerable Symbol tag
                jsParent.SetProperty(jsKey, jsRef.JSHandle);
            }
            else
            {
                jsParent.SetProperty(jsKey, (string?)null);
            }
        }
    }
}
