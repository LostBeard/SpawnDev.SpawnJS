using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls SpawnJSObjectReference and types that derive from SpawnJSObject
    /// </summary>
    public class SpawnJSObjectReferenceMarshaller : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? type) => type != null && typeof(SpawnJSObjectReference).IsAssignableFrom(type);
        /// <inheritdoc/>
        /// <remarks>
        /// The exact type is constructed DIRECTLY from a handle, so it takes its reference through the slot
        /// table and no JSObject proxy is created - see <see cref="SpawnJSObjectMarshaller.JSToNet"/> for
        /// why that matters. This is the case that actually occurs: asking for a bare
        /// SpawnJSObjectReference, which is what JSRefCopy does.
        /// <br/>
        /// A DERIVED type keeps the JSObject path, because only its own constructor can build it and this
        /// cannot know whether it declares a handle one. Deciding that by reflecting for the constructor
        /// was the obvious alternative and is deliberately not used: the trimmer cannot see through
        /// Type.GetConstructor, so it warns, and under trimming the answer could change to "no" once the
        /// constructor is removed - the reflection would be reasoning about a type the trimmer had already
        /// altered. A static construction is knowable at build time and needs no such reasoning.
        /// </remarks>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle)
        {
            if (type == typeof(SpawnJSObjectReference) && jsHandle.TryTakeOwnedValue(out var owned))
            {
                return owned == null ? null : new SpawnJSObjectReference(owned);
            }
            var value = jsHandle.AsJSObject();
            return value == null ? null : Activator.CreateInstance(type, value);
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is SpawnJSObjectReference jsRef)
            {
                // handle to handle: neither the parent nor the referenced value becomes a .Net proxy, so
                // neither picks up the runtime's enumerable Symbol tag
                jsParent.SetProperty(jsKey, jsRef.JSHandle);
            }
            else
            {
                jsParent.SetProperty(jsKey, (string?)null);
            }
        }
    }
}
