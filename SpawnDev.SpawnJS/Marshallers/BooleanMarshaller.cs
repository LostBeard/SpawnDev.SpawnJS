using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls Boolean
    /// </summary>
    public class BooleanMarshaller : JSMarshaller<bool>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle) => jsHandle.AsBoolean();
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value) => jsParent.SetProperty(jsKey, (bool)value!);
        /// <inheritdoc/>
        public override bool TryWriteArg(Type? typeToConvert, object value, out byte tag, out double payload)
        {
            tag = ArgTag.Boolean;
            payload = (bool)value ? 1 : 0;
            return true;
        }
    }
}
