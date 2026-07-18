using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls Boolean
    /// </summary>
    public class BooleanMarshaller : JSMarshaller<bool>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, JSObject jsParent, object jsKey, SpawnJSRuntime runtime) => Reflect.GetBoolean(jsParent, jsKey);
        /// <inheritdoc/>
        public override void NetToJS(Type? type, JSObject jsParent, object jsKey, object? value, SpawnJSRuntime runtime) => Reflect.Set(jsParent, jsKey, (bool)value!);
    }
}
