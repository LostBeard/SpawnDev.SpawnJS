using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls JSObject
    /// </summary>
    public class JSObjectMarshaller : JSMarshaller<JSObject>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle) => jsHandle.AsJSObject();
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value) => Reflect.Set(jsParent.JSObject, jsKey, (JSObject)value!);
    }
}
