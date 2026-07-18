using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls string
    /// </summary>
    public class StringMarshaller : JSMarshaller<string>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, JSObject jsParent, object jsKey, SpawnJSRuntime runtime) => Reflect.GetString(jsParent, jsKey);
        /// <inheritdoc/>
        public override void NetToJS(Type? type, JSObject jsParent, object jsKey, object? value, SpawnJSRuntime runtime) => Reflect.Set(jsParent, jsKey, (string)value!);
    }
}
