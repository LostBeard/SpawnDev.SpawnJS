using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>
    /// Marshals data between .Net and Javascript using SpawnJSObjectReference
    /// </summary>
    public abstract class JSMarshaller
    {
        public virtual ReturnType ReturnType => throw new NotImplementedException();
        /// <summary>
        /// What Javascript writes back for a value this marshaller reads. By default the <see cref="ReturnType"/>'s own
        /// built-in schema; a marshaller that reads a whole object, array or dictionary returns a composite schema, so the
        /// value is read in the call's single crossing.
        /// </summary>
        public virtual JSSchema Schema => JSSchema.Builtin(ReturnType);
        /// <summary>Reads a value written by <see cref="Schema"/>, boxed - for callers that only have a runtime Type.</summary>
        public virtual object? ReadBoxed(ref JSTapeReader reader) => throw new NotImplementedException(GetType().Name);
        /// <summary>
        /// SpawnJSRuntime
        /// </summary>
        protected SpawnJSRuntime JS => SpawnJSRuntime.Instance ?? throw new InvalidOperationException("SpawnJSRuntime has not been created.");
        /// <summary>
        /// Returns true if the data type can be marshalled.<br/>
        /// <paramref name="type"/> may be null when the .Net value being marshalled is null.
        /// </summary>
        public abstract bool CanMarshal(Type type);
        /// <summary>
        /// If this class reported true to CanMarshal, GetMarshaller may be called to get the marshaller to do the marshalling<br/>
        /// </summary>
        /// <returns></returns>
        public virtual JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>() => (JSMarshaller<T>)this;
    }
    /// <summary>
    /// Strongly-typed marshaller for <typeparamref name="TType"/>. This is the layer the no-boxing path
    /// targets: the runtime bridges a value's runtime Type back into <typeparamref name="TType"/> and calls
    /// NetToJS(Type, SpawnJSObjectReference, double, TType) / the <c>JSToNet</c> overloads
    /// directly, so the value never has to be boxed as <c>object</c>. The non-generic base's
    /// <c>JSToNetBoxed</c> overloads simply forward to the typed <c>JSToNet</c> for callers that only have a
    /// <see cref="Type"/>. Each subclass overrides only the one <c>JSToNet</c> overload matching its
    /// <see cref="JSMarshaller.ReturnType"/>; the rest throw <see cref="NotImplementedException"/> by design.
    /// </summary>
    public abstract class JSMarshaller<TType> : JSMarshaller
    {
        public virtual Type RegisteredType => typeof(TType);
        /// <summary>
        /// JS to .Net: reads the value Javascript wrote by <see cref="JSMarshaller.Schema"/>. The default reads the
        /// <see cref="JSMarshaller.ReturnType"/> form and hands it to the matching <c>JSToNet</c> overload - exactly what
        /// the typed return of v2's dispatcher produced.
        /// </summary>
        public virtual TType Read(ref JSTapeReader reader) => ReturnType switch
        {
            ReturnType.Void => JSToNet(),
            ReturnType.Double => JSToNet(reader.ReadDouble()),
            ReturnType.Int32 => JSToNet(reader.ReadInt32()),
            ReturnType.Boolean => JSToNet(reader.ReadBoolean()),
            ReturnType.DoubleNullable => JSToNet(reader.ReadDoubleNullable()),
            ReturnType.Int32Nullable => JSToNet(reader.ReadInt32Nullable()),
            ReturnType.BooleanNullable => JSToNet(reader.ReadBooleanNullable()),
            ReturnType.String or ReturnType.Json => JSToNet(reader.ReadString()!),
            ReturnType.SpawnJSObjectReference => JSToNet(reader.ReadRef(false)!),
            ReturnType.SpawnJSObjectReferenceNonNullable => JSToNet(reader.ReadRef(true)!),
            _ => throw new Exception($"Invalid ReturnType for marshaller: {GetType().Name} {ReturnType}"),
        };
        /// <inheritdoc/>
        public override object? ReadBoxed(ref JSTapeReader reader) => Read(ref reader);
        /// <summary>
        /// Returns true if the data type can be marshalled
        /// </summary>
        public override bool CanMarshal(Type type) => typeof(TType) == type;
        /// <summary>
        /// Not data interop
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual TType JSToNet() => throw new NotImplementedException(this.GetType().Name);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(bool value) => throw new NotImplementedException(this.GetType().Name);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(bool? value) => throw new NotImplementedException(this.GetType().Name);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(double value) => throw new NotImplementedException(this.GetType().Name);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(double? value) => throw new NotImplementedException(this.GetType().Name);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(int value) => throw new NotImplementedException(this.GetType().Name);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(int? value) => throw new NotImplementedException(this.GetType().Name);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(string value) => throw new NotImplementedException(this.GetType().Name);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(SpawnJSObjectReference value) => throw new NotImplementedException(this.GetType().Name);
        /// <summary>
        /// .Net to JS: append <paramref name="value"/> to the call's <see cref="JSTape"/>. Nothing crosses here - the
        /// whole call, every argument and everything nested in them, crosses once when it is sent.
        /// </summary>
        public abstract void Write(JSTape tape, TType value);
    }
}
