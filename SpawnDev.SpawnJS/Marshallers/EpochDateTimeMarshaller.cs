using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls <see cref="EpochDateTime"/>.<br/>
    /// EpochDateTime exists precisely because a lot of Javascript APIs express a moment in time as
    /// milliseconds since 1970-01-01 rather than as a Date, so it marshals as a plain number.
    /// </summary>
    public class EpochDateTimeMarshaller : JSMarshallerFromDoubleNullable<EpochDateTime?>
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type) => type == typeof(EpochDateTime);
        /// <inheritdoc/>
        public override EpochDateTime? JSToNet(double? value)
        {
            return value == null ? null : new EpochDateTime((long)value.Value);
        }
        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, EpochDateTime? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            jsParent.PropertySet(jsKey, value.ValueEpoch);
        }
        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, EpochDateTime? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            jsParent.PropertySet(jsKey, value.ValueEpoch);
        }
    }
}
