namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls Delegate types
    /// </summary>
    public class IMarshalOutByJSHandleMarshaller : JSMarshaller
    {

        ///<inheritdoc/>
        public override bool CanMarshal(Type? typeToConvert)
        {
            return typeToConvert != null && typeToConvert.IsAssignableTo(typeof(IMarshalOutByJSHandle));
        }
        /// <inheritdoc/>
        public override object? JSToNet(Type typeToConvert, SpawnJSHandle jsHandle)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? typeToConvert, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is IMarshalOutByJSHandle byJSHandle)
            {
                // the handle, not its JSObject: handle to handle is slot native and resolves no proxy
                jsParent.SetProperty(jsKey, byJSHandle.JSHandle);
            } else
            {
                jsParent.SetProperty(jsKey, (string)null!);
            }
        }
    }
}
