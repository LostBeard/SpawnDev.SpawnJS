namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>Base for marshallers whose JS result is read as a <c>double?</c>. See <see cref="ReturnType.DoubleNullable"/>.</summary>
    public abstract class JSMarshallerFromDoubleNullable<TType> : JSMarshaller<TType>
    {
        public override ReturnType ReturnType => ReturnType.DoubleNullable;
    }
}
