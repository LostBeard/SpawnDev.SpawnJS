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
        int WriteMembersToFrame(List<ClassMemberJsonInfo> members, object obj, int offset)
        {
            var written = 0;
            foreach (var member in members)
            {
                var value = member.PropertyInfo != null ? member.PropertyInfo.GetValue(obj)
                    : member.FieldInfo != null ? member.FieldInfo.GetValue(obj)
                    : null;
                if (!member.GetShouldWrite(value)) continue;
                // intern the property name once per process - it is a fixed literal for this type
                if (member.NameSlot == 0) member.NameSlot = SlotInterop.AllocString(member.GetJsonName());
                JS.WriteMemberToFrame(offset, written, member, value);
                written++;
            }
            return written;
        }

        /// <summary>
        /// Builds the object and assigns it to <c>jsParent[jsKey]</c> in ONE crossing.<br/>
        /// Returns false when the parent cannot be addressed by slot, in which case the caller takes the
        /// per-member path it always had.
        /// </summary>
        internal bool TryWriteObjectInto(Type type, SpawnJSHandle jsParent, object jsKey, object obj)
        {
            if (jsKey is not string key) return false;
            if (!jsParent.TryGetSlot(out var parentSlot)) return false;
            // resolved once - the reservation and the write both need it, and it is a dictionary lookup
            var members = type.GetTypeJsonProperties();
            var offset = JS.ReserveMemberPairs(members.Count);
            try
            {
                var count = WriteMembersToFrame(members, obj, offset);
                SlotInterop.BuildObjectInto(JS.CtxId, parentSlot, key, offset, count);
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
            var members = typeToConvert.GetTypeJsonProperties();
            var offset = JS.ReserveMemberPairs(members.Count);
            try
            {
                var count = WriteMembersToFrame(members, value, offset);
                payload = SlotInterop.BuildObject(JS.CtxId, offset, count);
                return true;
            }
            finally
            {
                JS.ReleaseFrameArgs(offset);
            }
        }
    }
}
