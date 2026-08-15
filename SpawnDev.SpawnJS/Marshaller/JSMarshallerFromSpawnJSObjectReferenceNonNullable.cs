namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>
    /// Base for marshallers whose JS result is always held and returned as an object-table id, even when the
    /// value is null/undefined. See <see cref="ReturnType.SpawnJSObjectReferenceNonNullable"/>.
    /// </summary>
    public abstract class JSMarshallerFromSpawnJSObjectReferenceNonNullable<TType> : JSMarshaller<TType>
    {
        public override ReturnType ReturnType => ReturnType.SpawnJSObjectReferenceNonNullable;
    }
}
