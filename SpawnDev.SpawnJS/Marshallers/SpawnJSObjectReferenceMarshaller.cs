using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals a <see cref="SpawnJSObjectReference"/> itself: a JS value held on the JS side and carried
    /// across the boundary as its numeric object-table id. A non-positive id (0, or the null/undefined
    /// sentinels) is treated as no reference and returns null.
    /// </summary>
    public class SpawnJSObjectReferenceMarshaller : JSMarshallerFromSpawnJSObjectReference<SpawnJSObjectReference>
    {
        public override SpawnJSObjectReference JSToNet(SpawnJSObjectReference value) => value;
        /// <inheritdoc/>
        public override void Write(JSTape tape, SpawnJSObjectReference value) { if (value == null) tape.WriteNull(); else tape.WriteRef(value.Id); }
    }
}
