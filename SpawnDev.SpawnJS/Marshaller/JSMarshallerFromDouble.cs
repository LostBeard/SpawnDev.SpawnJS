namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>Base for marshallers whose JS result is read as a <c>double</c>. See <see cref="ReturnType.Double"/>.</summary>
    public abstract class JSMarshallerFromDouble<TType> : JSMarshaller<TType>
    {
        public override ReturnType ReturnType => ReturnType.Double;
    }
}
