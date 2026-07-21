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
        /// <inheritdoc/>
        /// <remarks>
        /// Slot native: the bytes were never the problem, but the PARENT had to become a JSObject proxy
        /// purely to be written through.
        /// </remarks>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value) => jsParent.SetProperty(jsKey, (byte[]?)value);
    }
}
