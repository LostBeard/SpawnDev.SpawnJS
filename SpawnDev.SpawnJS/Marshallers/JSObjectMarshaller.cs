using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls JSObject
    /// </summary>
    public class JSObjectMarshaller : JSMarshaller<JSObject>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsParent, object jsKey) => Reflect.GetJSObject(jsParent.JSObject, jsKey);
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value) => Reflect.Set(jsParent.JSObject, jsKey, (JSObject)value!);
    }
    /// <summary>
    /// Marshalls SpawnJSHandle
    /// </summary>
    public class SpawnJSHandleMarshaller : JSMarshaller<SpawnJSHandle>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsParent, object jsKey)
        {
            var value = Reflect.GetJSObject(jsParent.JSObject, jsKey);
            return value == null ? null : new SpawnJSHandle(value);
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is SpawnJSHandle jsHandle) Reflect.Set(jsParent.JSObject, jsKey, jsHandle.JSObject);
            else Reflect.Set(jsParent.JSObject, jsKey, (string)null!);
        }
    }
}
