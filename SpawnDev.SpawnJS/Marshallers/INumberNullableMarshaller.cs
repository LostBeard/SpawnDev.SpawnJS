using SpawnDev.SpawnJS.Marshaller;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals <see cref="Nullable{T}"/> for any numeric <typeparamref name="TNumber"/> (a type implementing
    /// <see cref="INumber{TSelf}"/>) to/from a JS number or null/undefined - the nullable companion to
    /// <see cref="INumberMarshaller{TNumber}"/>. Covers <c>long?</c>, <c>ulong?</c>, <c>short?</c>,
    /// <c>nint?</c>, etc.
    /// <para>
    /// The specific nullable marshallers (<see cref="Int32NullableMarshaller"/>, <see cref="DoubleNullableMarshaller"/>,
    /// <see cref="BigIntegerNullableMarshaller"/>) are registered AFTER this one, so - because the registry
    /// scans in reverse - they take precedence for <c>int?</c>/<c>double?</c>/<c>BigInteger?</c>; this catches
    /// every other nullable numeric. Non-nullable numbers were already covered by
    /// <see cref="INumberMarshaller{TNumber}"/>, but <see cref="Nullable{T}"/> does not implement
    /// <see cref="INumber{TSelf}"/>, so <c>long?</c> (and friends) matched no numeric marshaller and read
    /// <c>null</c>. That regressed the BlazorJS-&gt;SpawnJS move: WebGPU adapter/device limits are exposed as
    /// <c>long?</c> (e.g. <c>maxStorageBufferBindingSize</c>, <c>maxBufferSize</c>), so every one read null and
    /// the WebGPU device fell back to the 128 MiB spec-default storage-buffer limit - breaking any model with a
    /// &gt;128 MiB binding.
    /// </para>
    /// <para>
    /// Like <see cref="INumberMarshaller{TNumber}"/>, the value crosses as a JS <c>number</c> (f64), so integers
    /// beyond 2^53 lose precision - use <see cref="System.Numerics.BigInteger"/> (BigInt) for exact 64-bit
    /// values. Every real WebGPU limit is well within 2^53.
    /// </para>
    /// </summary>
    public class INumberNullableMarshaller<TNumber> : JSMarshallerFromDoubleNullable<TNumber?>
        where TNumber : struct, INumber<TNumber>
    {
        [UnconditionalSuppressMessage("Trimming", "IL2070",
            Justification = "Probes whether the Nullable's underlying type implements INumber<>. INumber<> is referenced by this marshaller, so the trimmer preserves that interface implementation on any numeric type it keeps; a type whose INumber<> interface was trimmed would be an unused number type that never reaches this check.")]
        public override bool CanMarshal(Type type)
        {
            if (type.IsGenericTypeDefinition) return false;
            var inner = Nullable.GetUnderlyingType(type);
            return inner != null && inner.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INumber<>));
        }

        public override JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var inner = Nullable.GetUnderlyingType(typeof(T))!;
            var marshallerTyped = typeof(INumberNullableMarshaller<>).MakeGenericType(inner);
            return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
        }

        public override TNumber? JSToNet(double? value)
            => value is null ? null : TNumber.CreateChecked(value.Value);

        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TNumber? value)
            => jsParent.PropertySet(jsKey, value is null ? (double?)null : double.CreateChecked(value.Value));

        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TNumber? value)
            => jsParent.PropertySet(jsKey, value is null ? (double?)null : double.CreateChecked(value.Value));
    }
}
