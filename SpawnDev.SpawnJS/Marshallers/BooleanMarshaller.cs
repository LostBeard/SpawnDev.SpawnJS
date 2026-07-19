using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls Boolean
    /// </summary>
    public class BooleanMarshaller : JSMarshaller<bool>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsParent, object jsKey) => Reflect.GetBoolean(jsParent.JSObject, jsKey);
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value) => Reflect.Set(jsParent.JSObject, jsKey, (bool)value!);
    }
}
