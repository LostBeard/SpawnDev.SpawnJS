namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>Base for marshallers whose JS result is nothing (undefined). See <see cref="ReturnType.Void"/>.</summary>
    public abstract class JSMarshallerFromVoid<TType> : JSMarshaller<TType>
    {
        public override ReturnType ReturnType => ReturnType.Void;
    }
}
