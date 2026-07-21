using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls JSObject
    /// </summary>
    public class JSObjectMarshaller : JSMarshaller<JSObject>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle) => jsHandle.AsJSObject();
        /// <inheritdoc/>
        /// <inheritdoc/>
        /// <remarks>
        /// The VALUE is a JSObject because that is what the caller asked for - this marshaller is opt in
        /// by definition. What is avoided is proxying the PARENT as well, just to write through it.
        /// </remarks>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value) => jsParent.SetProperty(jsKey, (JSObject?)value);
    }
}
