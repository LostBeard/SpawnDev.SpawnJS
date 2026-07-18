using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls JSObject
    /// </summary>
    public class JSObjectMarshaller : JSMarshaller<JSObject>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, JSObject jsParent, object jsKey, SpawnJSRuntime runtime) => Reflect.GetJSObject(jsParent, jsKey);
        /// <inheritdoc/>
        public override void NetToJS(Type? type, JSObject jsParent, object jsKey, object? value, SpawnJSRuntime runtime) => Reflect.SetJSObject(jsParent, jsKey, (JSObject)value!);
    }
}
