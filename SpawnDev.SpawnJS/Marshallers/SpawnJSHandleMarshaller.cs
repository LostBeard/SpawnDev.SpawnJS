namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls SpawnJSHandle
    /// </summary>
    public class SpawnJSHandleMarshaller : JSMarshaller<SpawnJSHandle>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle)
        {
            return jsHandle.AsJSHandle();
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is SpawnJSHandle jsHandle)
            {
                jsHandle.CopyTo(jsParent, jsKey);
            }
            else
            {
                Reflect.Set(jsParent.JSObject!, jsKey, (string)null!);
            }
        }
    }
}
