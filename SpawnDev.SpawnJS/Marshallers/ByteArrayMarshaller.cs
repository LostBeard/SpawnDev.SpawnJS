using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls byte[]
    /// </summary>
    public class ByteArrayMarshaller : JSMarshaller<byte[]>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle) => jsHandle.AsByteArray();
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value) => Reflect.Set(jsParent.JSObjectRequired, jsKey, (byte[])value!);
    }
}
