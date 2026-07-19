using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls Func`string, SpawnJSHandle, SpawnJSHandle` to JS
    /// </summary>
    public class JSToNetInvokerMarshaller : JSMarshaller<Func<string, SpawnJSHandle, SpawnJSHandle>>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsParent, object jsKey)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            Reflect.Set(jsParent.JSObject, jsKey, (Func<string, JSObject, JSObject>)value!);
        }
    }
}
