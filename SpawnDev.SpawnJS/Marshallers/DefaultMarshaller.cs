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
        public override object? JSToNet(Type type, JSObject jsParent, object jsKey, SpawnJSRuntime runtime)
        {
            var value = Reflect.GetObject(jsParent, jsKey);
            return value ?? type.GetDefaultValue();
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, JSObject jsParent, object jsKey, object? obj, SpawnJSRuntime runtime)
        {
            if (obj == null || type == null)
            {
                Reflect.Set(jsParent, jsKey, (string)null!);
            }
            else
            {
                Reflect.SetObject(jsParent, jsKey, obj);
            }
        }
    }
}
