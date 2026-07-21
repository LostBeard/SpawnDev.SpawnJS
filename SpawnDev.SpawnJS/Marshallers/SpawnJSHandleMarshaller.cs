namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls SpawnJSHandle
    /// </summary>
    public class SpawnJSHandleMarshaller : JSMarshaller<SpawnJSHandle>
    {
        /// <inheritdoc/>
        /// <remarks>
        /// Takes its own slot on the value rather than resolving it to a JSObject and wrapping that. A
        /// handle is precisely the thing that does not need a proxy, so creating one to build it was the
        /// most backwards read in the library.
        /// </remarks>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle)
        {
            if (jsHandle.TryTakeOwnedValue(out var owned)) return owned;
            return jsHandle.AsJSHandle();
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is SpawnJSHandle jsHandle)
            {
                // slot to slot when both sides are slotted, so neither becomes a proxy. CopyTo is the
                // general path and still handles a handle that borrows its parent's storage.
                if (jsParent.TryGetSlot(out _) && jsHandle.TryGetSlot(out _)) jsParent.SetProperty(jsKey, jsHandle);
                else jsHandle.CopyTo(jsParent, jsKey);
            }
            else
            {
                jsParent.SetProperty(jsKey, (string?)null);
            }
        }
        /// <inheritdoc/>
        public override bool TryWriteArg(Type? typeToConvert, object value, out byte tag, out double payload)
        {
            tag = ArgTag.Slot;
            payload = 0;
            return value is SpawnJSHandle handle && handle.TryGetSlot(out payload);
        }
    }
}
