using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals a nullable enum (<c>TEnum?</c>) to/from a JS number, mapping a null .Net value to
    /// null/undefined on the JS side. Parametrized by the non-nullable <c>struct, Enum</c> type; the
    /// marshalled type (<typeparamref name="TEnum"/>?) is what is registered.
    /// </summary>
    /// <typeparam name="TEnum">The underlying enum type (non-nullable).</typeparam>
    public class EnumNullableMarshaller<TEnum> : JSMarshallerFromInt32Nullable<TEnum?> where TEnum : struct, Enum
    {
        // Enum.ToObject coerces to TEnum's underlying type before boxing (a direct unbox throws for any
        // non-Int32-backed enum). A null JS value maps to a null nullable-enum.
        public override TEnum? JSToNet(int? value) => value.HasValue ? (TEnum)Enum.ToObject(typeof(TEnum), value.Value) : null;
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TEnum? value)
            => jsParent.PropertySet(jsKey, value.HasValue ? (int?)Convert.ToInt32(value.Value) : null);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TEnum? value)
            => jsParent.PropertySet(jsKey, value.HasValue ? (int?)Convert.ToInt32(value.Value) : null);
    }
}
