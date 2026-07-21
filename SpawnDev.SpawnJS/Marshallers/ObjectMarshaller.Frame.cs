namespace SpawnDev.SpawnJS.Marshallers
{
    public partial class ObjectMarshaller
    {
        /// <summary>
        /// Writes an object's members into the argument frame as name/value pairs and returns how many
        /// were written. The caller owns the frame region and must release it.<br/>
        /// <br/>
        /// A member whose value cannot be one number - a nested object, an array - takes the scratch path
        /// exactly as before, so this is never worse than what it replaces.
        /// </summary>
        int WriteMembersToFrame(Type type, object obj, int offset)
        {
            var written = 0;
            foreach (var member in type.GetTypeJsonProperties())
            {
                var value = member.PropertyInfo != null ? member.PropertyInfo.GetValue(obj)
                    : member.FieldInfo != null ? member.FieldInfo.GetValue(obj)
                    : null;
                if (!member.GetShouldWrite(value)) continue;
                // intern the property name once per process - it is a fixed literal for this type
                if (member.NameSlot == 0) member.NameSlot = SlotInterop.AllocString(member.GetJsonName());
                JS.WriteMemberToFrame(offset, written, member.NameSlot, value);
                written++;
            }
            return written;
        }

        /// <summary>
        /// How many members this type could write, so the frame region can be reserved before any value is
        /// read. An over-reservation is harmless - the top unwinds to where it started either way.
        /// </summary>
        static int MaxMemberCount(Type type) => type.GetTypeJsonProperties().Count;

        /// <summary>
        /// Builds the object and assigns it to <c>jsParent[jsKey]</c> in ONE crossing.<br/>
        /// Returns false when the parent cannot be addressed by slot, in which case the caller takes the
        /// per-member path it always had.
        /// </summary>
        internal bool TryWriteObjectInto(Type type, SpawnJSHandle jsParent, object jsKey, object obj)
        {
            if (jsKey is not string key) return false;
            if (!jsParent.TryGetSlot(out var parentSlot)) return false;
            var offset = JS.ReserveMemberPairs(MaxMemberCount(type));
            try
            {
                var count = WriteMembersToFrame(type, obj, offset);
                SlotInterop.BuildObjectInto(parentSlot, key, offset, count);
                return true;
            }
            finally
            {
                JS.ReleaseFrameArgs(offset);
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// An object offered as a single frame value: it is built Javascript side and its SLOT is the
        /// payload. This is what lets a nested descriptor, or an object passed as a call argument, cost
        /// one crossing rather than one per member of every level.
        /// </remarks>
        public override bool TryWriteArg(Type? typeToConvert, object value, out byte tag, out double payload)
        {
            tag = ArgTag.Slot;
            payload = 0;
            if (typeToConvert == null) return false;
            var offset = JS.ReserveMemberPairs(MaxMemberCount(typeToConvert));
            try
            {
                var count = WriteMembersToFrame(typeToConvert, value, offset);
                payload = SlotInterop.BuildObject(offset, count);
                return true;
            }
            finally
            {
                JS.ReleaseFrameArgs(offset);
            }
        }
    }
}
