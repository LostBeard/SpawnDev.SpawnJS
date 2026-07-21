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

        /// <summary>
        /// Takes a handle that OWNS the value this one points at, without materialising a JSObject proxy
        /// for it.<br/>
        /// <br/>
        /// A handle handed to a marshaller is the caller's, and is disposed as soon as the marshal
        /// returns - so a wrapper cannot retain it and has to take its own reference. Until now the only
        /// way to do that was to resolve the value to a JSObject and build a handle around it, which is
        /// the single most expensive thing in the library: a proxy costs a GC handle, a proxy-table entry
        /// and an enumerable Symbol tag on the Javascript object, measured at 21us to create an object
        /// through it against 1.3us to create it Javascript side and take its slot.<br/>
        /// <br/>
        /// Returns false when the value cannot be reached through the slot table - a volatile handle whose
        /// parent has no slot of its own, or a value that is not a reference type - and the caller should
        /// take the proxy path, which behaves exactly as it did before. Returns true with a null
        /// <paramref name="owned"/> when the value is null or undefined.
        /// </summary>
        /// <param name="owned">The new handle, or null when the value is null or undefined</param>
        /// <param name="allowNonReference">
        /// When true, a value that is not a reference is slotted rather than refused. A slot holds any
        /// Javascript value, so a wrapper over a PRIMITIVE works - it is only a JSObject proxy that cannot
        /// represent one. Off by default, so <c>JS.Get&lt;Window&gt;("someNumber")</c> still fails loudly
        /// instead of handing back a wrapper whose every property reads undefined; a wrapper type that
        /// genuinely represents a primitive opts in.
        /// </param>
        internal bool TryTakeOwnedValue(out SpawnJSHandle? owned, bool allowNonReference = false)
        {
            owned = null;
            if (IsDisposed) return false;
            double valueSlot;
            if (TryGetSlot(out var ownSlot))
            {
                // this handle IS the value's storage, so take a second reference to the same value
                valueSlot = allowNonReference ? SlotInterop.CloneValueSlot(ownSlot) : SlotInterop.CloneObjectSlot(ownSlot);
            }
            else if (_unownedParent != null && _unownedParent.TryGetSlot(out var parentSlot))
            {
                // volatile: the value lives at parent[key] and the parent is addressable, so the read
                // never leaves Javascript
                valueSlot = JSKey is string name
                    ? (allowNonReference ? SlotInterop.GetValueSlot(parentSlot, name) : SlotInterop.GetObjectSlot(parentSlot, name))
                    : SlotInterop.GetObjectSlotAt(parentSlot, Convert.ToDouble(JSKey, System.Globalization.CultureInfo.InvariantCulture));
            }
            else
            {
                return false;
            }
            // not a reference type: there is nothing for a handle to own. Fall back rather than invent a
            // meaning, so the caller raises the same error for this case that it always has.
            if (valueSlot < 0) return false;
            // null or undefined, and deliberately no slot was allocated
            if (valueSlot == 0) return true;
            owned = new SpawnJSHandle(valueSlot);
            return true;
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
        /// Writes a byte array to a property, through the slot table when possible.
        /// </summary>
        internal void SetProperty(object key, byte[]? value)
        {
            if (TryGetSlot(out var slot))
            {
                if (key is string name) SlotInterop.SetBytes(slot, name, value);
                else SlotInterop.SetBytesAt(slot, Convert.ToDouble(key), value);
                return;
            }
            Reflect.Set(JSObjectRequired, key, value);
        }

        /// <summary>
        /// Writes a JSObject the caller genuinely holds to a property, through the slot table when
        /// possible.<br/>
        /// The VALUE is already a proxy here and that is the caller's choice; what this avoids is
        /// materialising a second one for the PARENT merely to write through it.
        /// </summary>
        internal void SetProperty(object key, System.Runtime.InteropServices.JavaScript.JSObject? value)
        {
            if (TryGetSlot(out var slot))
            {
                if (key is string name) SlotInterop.SetJSObject(slot, name, value);
                else SlotInterop.SetJSObjectAt(slot, Convert.ToDouble(key), value);
                return;
            }
            Reflect.Set(JSObjectRequired, key, value);
        }

        /// <summary>
        /// Writes an arbitrary value to a property, through the slot table when possible. This is the
        /// fallback shape - the runtime decides how the value crosses.
        /// </summary>
        internal void SetPropertyAny(object key, object? value)
        {
            if (TryGetSlot(out var slot))
            {
                if (key is string name) SlotInterop.SetAny(slot, name, value);
                else SlotInterop.SetAnyAt(slot, Convert.ToDouble(key), value);
                return;
            }
            Reflect.SetObject(JSObjectRequired, key, value);
        }

        /// <summary>
        /// Own enumerable keys of this handle's value, read through the slot table.<br/>
        /// Only call this when <see cref="CanReadBySlot"/> is true; a null then means the value itself is
        /// null or undefined, which is a different answer from "no slot path available" and the caller
        /// has to be able to tell them apart.
        /// </summary>
        internal string[]? GetOwnKeysBySlot()
            => TryGetSlot(out var slot) ? SlotInterop.Keys(slot, true) : null;

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
