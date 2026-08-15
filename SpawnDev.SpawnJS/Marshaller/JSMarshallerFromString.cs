namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>Base for marshallers whose JS result is read as a <c>string</c>. See <see cref="ReturnType.String"/>.</summary>
    public abstract class JSMarshallerFromString<TType> : JSMarshaller<TType>
    {
        public override ReturnType ReturnType => ReturnType.String;
    }
}
