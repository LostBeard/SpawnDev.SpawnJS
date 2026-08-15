namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>
    /// Base for marshallers whose JS result is held on the JS side and returned as an object-table id, or
    /// null when the value is null/undefined. See <see cref="ReturnType.SpawnJSObjectReference"/>.
    /// </summary>
    public abstract class JSMarshallerFromSpawnJSObjectReference<TType> : JSMarshaller<TType>
    {
        public override ReturnType ReturnType => ReturnType.SpawnJSObjectReference;
    }
}
