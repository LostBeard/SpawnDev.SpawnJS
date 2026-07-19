using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls string
    /// </summary>
    public class StringMarshaller : JSMarshaller<string>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsParent, object jsKey) => jsParent.GetPropertyAsString(jsKey);
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value) => jsParent.SetProperty(jsKey, (string)value!);
    }
}
