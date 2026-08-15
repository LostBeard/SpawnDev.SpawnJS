namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>Base for marshallers whose JS result is read as a <c>bool</c>. See <see cref="ReturnType.Boolean"/>.</summary>
    public abstract class JSMarshallerFromBoolean<TType> : JSMarshaller<TType>
    {
        public override ReturnType ReturnType => ReturnType.Boolean;
    }
}
