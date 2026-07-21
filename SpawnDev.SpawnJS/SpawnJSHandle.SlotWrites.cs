namespace SpawnDev.SpawnJS
{
    public sealed partial class SpawnJSHandle
    {
        /// <summary>
        /// True when this handle's value lives in the shared Javascript slot table, which means its
        /// properties can be written without ever materialising a JSObject proxy for it.<br/>
        /// A volatile handle borrows its parent's storage and has no slot of its own, so it falls back.
        /// </summary>
        internal bool TryGetSlot(out double slot)
        {
            if (!IsDisposed && _ownsSlot && JSKey is double ownSlot)
            {
                slot = ownSlot;
                return true;
            }
            slot = 0;
            return false;
        }

        // Javascript object keys are strings, and an array index behaves identically written as one
        // (arr["0"] and arr[0] address the same element), so a numeric key converts rather than needing
        // a second binding per type.
        static string KeyToString(object key) => key as string ?? Convert.ToString(key, System.Globalization.CultureInfo.InvariantCulture)!;

        /// <summary>
        /// Writes a number to a property, through the slot table when possible.<br/>
        /// The proxy path costs a JSObject for the PARENT even when the value is a plain number - and a
        /// descriptor is mostly plain numbers, so that proxy is created purely to be written through.
        /// </summary>
        internal void SetProperty(object key, double value)
        {
            if (TryGetSlot(out var slot))
            {
                if (key is string name) SlotInterop.SetDouble(slot, name, value);
                else SlotInterop.SetDoubleAt(slot, Convert.ToDouble(key), value);
                return;
            }
            Reflect.Set(JSObjectRequired, key, value);
        }

        /// <summary>
        /// Writes a string to a property, through the slot table when possible.
        /// </summary>
        internal void SetProperty(object key, string? value)
        {
            if (TryGetSlot(out var slot))
            {
                if (key is string name) SlotInterop.SetString(slot, name, value);
                else SlotInterop.SetStringAt(slot, Convert.ToDouble(key), value);
                return;
            }
            Reflect.Set(JSObjectRequired, key, value);
        }

        /// <summary>
        /// Writes a boolean to a property, through the slot table when possible.
        /// </summary>
        internal void SetProperty(object key, bool value)
        {
            if (TryGetSlot(out var slot))
            {
                if (key is string name) SlotInterop.SetBoolean(slot, name, value);
                else SlotInterop.SetBooleanAt(slot, Convert.ToDouble(key), value);
                return;
            }
            Reflect.Set(JSObjectRequired, key, value);
        }

        /// <summary>
        /// Assigns another handle's value as a property of this one.<br/>
        /// When both sides are slotted this is one call between two Javascript values and NEITHER becomes
        /// a .Net proxy - which is the whole point for a nested descriptor, where the old path had to
        /// materialise a proxy for the parent and for every child just to link them together.
        /// </summary>
        internal void SetProperty(object key, SpawnJSHandle value)
        {
            if (TryGetSlot(out var slot) && value.TryGetSlot(out var valueSlot))
            {
                if (key is string name) SlotInterop.SetSlot(slot, name, valueSlot);
                else SlotInterop.SetSlotAt(slot, Convert.ToDouble(key), valueSlot);
                return;
            }
            Reflect.Set(JSObjectRequired, key, value.JSObjectRequired);
        }
    }
}
