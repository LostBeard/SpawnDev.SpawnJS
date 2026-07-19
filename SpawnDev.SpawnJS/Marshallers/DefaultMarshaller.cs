using System.Runtime.InteropServices.JavaScript;

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
        public override object? JSToNet(Type type, SpawnJSHandle jsParent, object jsKey)
        {
            var value = Reflect.GetObject(jsParent.JSObject, jsKey);
            return value ?? type.GetDefaultValue();
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? obj)
        {
            if (obj == null || type == null)
            {
                Reflect.Set(jsParent.JSObject, jsKey, (string)null!);
            }
            else
            {
                Reflect.SetObject(jsParent.JSObject, jsKey, obj);
            }
        }
    }
}
