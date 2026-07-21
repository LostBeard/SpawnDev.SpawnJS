using System.Runtime.InteropServices.JavaScript;
using System.Runtime.InteropServices.Marshalling;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Default Marshaller
    /// </summary>
    public class DefaultMarshaller : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? type)
        {

            
            return true;
            
        }
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle)
        {
            var value = jsHandle.JSValue;
            return value ?? type.GetDefaultValue();
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? obj)
        {
            if (obj == null || type == null)
            {
                jsParent.SetProperty(jsKey, (string?)null);
            }
            else
            {
                // slot native, so the parent is not proxied merely to be written through
                jsParent.SetPropertyAny(jsKey, obj);
            }
        }
    }
}
