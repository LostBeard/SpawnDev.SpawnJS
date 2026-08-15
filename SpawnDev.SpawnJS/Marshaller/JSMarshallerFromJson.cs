namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>Base for marshallers whose JS result is JSON-stringified on the JS side and deserialized here. See <see cref="ReturnType.Json"/>.</summary>
    public abstract class JSMarshallerFromJson<TType> : JSMarshaller<TType>
    {
        public override ReturnType ReturnType => ReturnType.Json;
    }
}
