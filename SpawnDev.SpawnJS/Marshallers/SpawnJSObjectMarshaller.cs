namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls SpawnJSObject and types that derive from SpawnJSObject
    /// </summary>
    public class SpawnJSObjectMarshaller : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? type) => type != null && typeof(SpawnJSObject).IsAssignableFrom(type);
        /// <inheritdoc/>
        /// <remarks>
        /// The wrapper takes its own reference through the slot table, so reading one creates no JSObject
        /// proxy. This was the last routine proxy materialisation in the library: every
        /// <c>JS.Get&lt;Window&gt;</c> and every wrapper returned from a call paid for one, at a measured
        /// 21us against 1.3us for the slot path - and the proxy also tags the Javascript object with an
        /// enumerable Symbol, which is what a record-typed web API chokes on.
        /// <br/>
        /// The proxy path remains as a fallback for a value the slot table cannot address, so this is
        /// never worse than what it replaced.
        /// </remarks>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle)
        {
            // A wrapper that represents a PRIMITIVE opts in to being backed by one. A slot holds any
            // Javascript value, so this works; it is only a JSObject proxy that cannot represent a
            // primitive, and that restriction is what made StringPrimitive - and with it the string
            // HeapView path - unusable in this library until the reads became slot native.
            var allowNonReference = typeof(IJSPrimitiveWrapper).IsAssignableFrom(type);
            if (jsHandle.TryTakeOwnedValue(out var owned, allowNonReference))
            {
                return owned == null ? null : Activator.CreateInstance(type, new SpawnJSObjectReference(owned));
            }
            var value = jsHandle.AsJSObject();
            return value == null ? null : Activator.CreateInstance(type, new SpawnJSObjectReference(value));
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is SpawnJSObject jsRef && jsRef.JSRef != null)
            {
                // handle to handle: neither the parent nor the wrapper's value becomes a .Net proxy, so
                // neither picks up the runtime's enumerable Symbol tag
                jsParent.SetProperty(jsKey, jsRef.JSRef.JSHandle);
            }
            else
            {
                jsParent.SetProperty(jsKey, (string?)null);
            }
        }
    }
}
