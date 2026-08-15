namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>Base for marshallers whose JS result is read as a <c>bool?</c>. See <see cref="ReturnType.BooleanNullable"/>.</summary>
    public abstract class JSMarshallerFromBooleanNullable<TType> : JSMarshaller<TType>
    {
        public override ReturnType ReturnType => ReturnType.BooleanNullable;
    }
}
