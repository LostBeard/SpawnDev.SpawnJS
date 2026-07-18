using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls byte[]
    /// </summary>
    public class ByteArrayMarshaller : JSMarshaller<byte[]>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, JSObject jsParent, object jsKey, SpawnJSRuntime runtime)=> Reflect.GetByteArray(jsParent, jsKey);
        /// <inheritdoc/>
        public override void NetToJS(Type? type, JSObject jsParent, object jsKey, object? value, SpawnJSRuntime runtime) => Reflect.Set(jsParent, jsKey, (byte[])value!);
    }
}
