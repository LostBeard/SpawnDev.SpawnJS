namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Reading a property of a slotted value, without materialising a JSObject proxy for the object being
    /// read FROM.<br/>
    /// <br/>
    /// These mirror the SetProperty family and are used the same way: each tries the slot table and falls
    /// back to the proxy path when the value cannot be addressed that way, so a caller never has to ask
    /// which path it is on and no call site can be worse than it was.<br/>
    /// <br/>
    /// The proxy is the expensive thing - a GC handle, a proxy-table entry and a permanent enumerable
    /// Symbol tag on the Javascript object, measured at 21us to create an object through it against 1.3us
    /// for the slot path. Resolving one merely to READ an integer out of an object is the shape this
    /// removes, and it was on every property access in the library.
    /// </summary>
    public sealed partial class SpawnJSHandle
    {
        /// <summary>Reads a property as a bool.</summary>
        internal bool GetPropertyBoolean(object key)
            => TryGetSlot(out var slot)
                ? (key is string name ? SlotInterop.GetBoolean(slot, name) : SlotInterop.GetBooleanAt(slot, ToIndex(key)))
                : Reflect.GetBoolean(JSObjectRequired, key);

        /// <summary>Reads a property as an int.</summary>
        internal int GetPropertyInt32(object key)
            => TryGetSlot(out var slot)
                ? (key is string name ? SlotInterop.GetInt32(slot, name) : SlotInterop.GetInt32At(slot, ToIndex(key)))
                : Reflect.GetInt32(JSObjectRequired, key);

        /// <summary>Reads a property as a number.</summary>
        internal double GetPropertyDouble(object key)
            => TryGetSlot(out var slot)
                ? (key is string name ? SlotInterop.GetDouble(slot, name) : SlotInterop.GetDoubleAt(slot, ToIndex(key)))
                : Reflect.GetDouble(JSObjectRequired, key);

        /// <summary>Reads a property as a string, or null if it was null or undefined.</summary>
        internal string? GetPropertyString(object key)
            => TryGetSlot(out var slot)
                ? (key is string name ? SlotInterop.GetString(slot, name) : SlotInterop.GetStringAt(slot, ToIndex(key)))
                : Reflect.GetString(JSObjectRequired, key);

        /// <summary>Reads a property as a byte array, or null if it was null or undefined.</summary>
        internal byte[]? GetPropertyByteArray(object key)
            => TryGetSlot(out var slot)
                ? (key is string name ? SlotInterop.GetByteArray(slot, name) : SlotInterop.GetByteArrayAt(slot, ToIndex(key)))
                : Reflect.GetByteArray(JSObjectRequired, key);

        /// <summary>
        /// Whether a property exists. <paramref name="useIn"/> selects the <c>in</c> operator, which walks
        /// the prototype chain, over hasOwnProperty.<br/>
        /// The slot binding takes a string key because <c>in</c> requires one; a numeric key converts,
        /// which is what the proxy path did for every key anyway.
        /// </summary>
        internal bool HasPropertyValue(object key, bool useIn = true)
            => TryGetSlot(out var slot)
                ? SlotInterop.Has(slot, KeyToString(key), useIn)
                : Reflect.Has(JSObjectRequired, key);

        /// <summary>
        /// Reads a property into a handle that OWNS it, creating no proxy.<br/>
        /// Returns null when the value is null or undefined, or when it cannot be addressed through the
        /// slot table - the caller falls back in that case, so a null here is not on its own proof that
        /// the property was absent.
        /// </summary>
        internal SpawnJSHandle? TryGetPropertyHandle(object key)
        {
            if (!TryGetSlot(out var slot)) return null;
            var valueSlot = key is string name
                ? SlotInterop.GetObjectSlot(slot, name)
                : SlotInterop.GetObjectSlotAt(slot, ToIndex(key));
            return valueSlot > 0 ? new SpawnJSHandle(valueSlot) : null;
        }

        /// <summary>
        /// Whether a property can be reached through the slot table at all, which tells a caller whether a
        /// null from <see cref="TryGetPropertyHandle"/> means "absent" or "take the other path".
        /// </summary>
        internal bool CanReadBySlot => TryGetSlot(out _);

        static double ToIndex(object key)
            => Convert.ToDouble(key, System.Globalization.CultureInfo.InvariantCulture);

        /// <summary>
        /// Prepares a slot-native method invocation on this handle's value.<br/>
        /// <br/>
        /// The old path resolved a proxy for the FUNCTION and another for the TARGET, purely to hand them
        /// to Reflect.apply - two per call, on a surface whose whole purpose is calling methods. With both
        /// sides slotted the boundary carries a slot number, a name and a slot number.<br/>
        /// <br/>
        /// Returns false when the target or the argument array cannot be addressed that way, and the
        /// caller takes the path it always had. The caller owns <paramref name="argsHandle"/> and must
        /// dispose it.
        /// </summary>
        internal bool TryPrepareSlotInvoke(object?[] args, out double targetSlot, out double argsSlot, out SpawnJSHandle? argsHandle)
        {
            argsSlot = 0;
            argsHandle = null;
            if (!TryGetSlot(out targetSlot)) return false;
            var handle = JS.MarshallNetArrayToJSArray(args);
            if (handle == null || !handle.TryGetSlot(out argsSlot))
            {
                handle?.Dispose();
                return false;
            }
            argsHandle = handle;
            return true;
        }

        #region Reading this handle's OWN value
        /// <summary>
        /// Where this handle's value lives, if the slot table can reach it.<br/>
        /// <br/>
        /// Two shapes. An OWNING handle is its own storage, so its value IS the slot. A VOLATILE handle
        /// borrows its parent's storage, so its value is at parent[key] - and reading that used to go
        /// through <see cref="JSParent"/>, which is a JSObject, so every read of a number or a string out
        /// of a borrowed handle resolved a proxy for the object holding it. That is one proxy per element
        /// of every array and per member of every record read back.
        /// </summary>
        bool TryLocateValue(out double ownSlot, out double parentSlot, out object? key)
        {
            ownSlot = parentSlot = 0;
            key = null;
            if (IsDisposed) return false;
            if (TryGetSlot(out ownSlot)) return true;
            if (_unownedParent != null && _unownedParent.TryGetSlot(out parentSlot))
            {
                key = JSKey;
                return true;
            }
            return false;
        }

        /// <summary>Reads this handle's own value as a bool, through the slot table when possible.</summary>
        internal bool ReadSelfBoolean()
            => TryLocateValue(out var own, out var parent, out var key)
                ? (key == null ? SlotInterop.SelfBoolean(own)
                    : key is string n ? SlotInterop.GetBoolean(parent, n) : SlotInterop.GetBooleanAt(parent, ToIndex(key)))
                : Reflect.GetBoolean(JSParent, JSKey);

        /// <summary>Reads this handle's own value as an int.</summary>
        internal int ReadSelfInt32()
            => TryLocateValue(out var own, out var parent, out var key)
                ? (key == null ? SlotInterop.SelfInt32(own)
                    : key is string n ? SlotInterop.GetInt32(parent, n) : SlotInterop.GetInt32At(parent, ToIndex(key)))
                : Reflect.GetInt32(JSParent, JSKey);

        /// <summary>Reads this handle's own value as a number.</summary>
        internal double ReadSelfDouble()
            => TryLocateValue(out var own, out var parent, out var key)
                ? (key == null ? SlotInterop.SelfDouble(own)
                    : key is string n ? SlotInterop.GetDouble(parent, n) : SlotInterop.GetDoubleAt(parent, ToIndex(key)))
                : Reflect.GetDouble(JSParent, JSKey);

        /// <summary>Reads this handle's own value as a string, or null.</summary>
        internal string? ReadSelfString()
            => TryLocateValue(out var own, out var parent, out var key)
                ? (key == null ? SlotInterop.SelfString(own)
                    : key is string n ? SlotInterop.GetString(parent, n) : SlotInterop.GetStringAt(parent, ToIndex(key)))
                : Reflect.GetString(JSParent, JSKey);

        /// <summary>Reads this handle's own value as a byte array, or null.</summary>
        internal byte[]? ReadSelfByteArray()
            => TryLocateValue(out var own, out var parent, out var key)
                ? (key == null ? SlotInterop.SelfByteArray(own)
                    : key is string n ? SlotInterop.GetByteArray(parent, n) : SlotInterop.GetByteArrayAt(parent, ToIndex(key)))
                : Reflect.GetByteArray(JSParent, JSKey);

        /// <summary>Reads this handle's own value as an int, or null.</summary>
        internal int? ReadSelfInt32Nullable()
            => TryLocateValue(out var own, out var parent, out var key)
                ? (key == null ? SlotInterop.SelfInt32Nullable(own)
                    : key is string n ? SlotInterop.GetInt32Nullable(parent, n) : SlotInterop.GetInt32NullableAt(parent, ToIndex(key)))
                : Reflect.GetInt32Nullable(JSParent, JSKey);

        /// <summary>Reads this handle's own value as a number, or null.</summary>
        internal double? ReadSelfDoubleNullable()
            => TryLocateValue(out var own, out var parent, out var key)
                ? (key == null ? SlotInterop.SelfDoubleNullable(own)
                    : key is string n ? SlotInterop.GetDoubleNullable(parent, n) : SlotInterop.GetDoubleNullableAt(parent, ToIndex(key)))
                : Reflect.GetDoubleNullable(JSParent, JSKey);

        /// <summary>Reads this handle's own value as a bool, or null.</summary>
        internal bool? ReadSelfBooleanNullable()
            => TryLocateValue(out var own, out var parent, out var key)
                ? (key == null ? SlotInterop.SelfBooleanNullable(own)
                    : key is string n ? SlotInterop.GetBooleanNullable(parent, n) : SlotInterop.GetBooleanNullableAt(parent, ToIndex(key)))
                : Reflect.GetBooleanNullable(JSParent, JSKey);

        /// <summary>Reads this handle's own value, letting the runtime decide how it crosses.</summary>
        internal object? ReadSelfAny()
            => TryLocateValue(out var own, out var parent, out var key)
                ? (key == null ? SlotInterop.SelfAny(own)
                    : key is string n ? SlotInterop.GetAny(parent, n) : SlotInterop.GetAnyAt(parent, ToIndex(key)))
                : Reflect.GetObject(JSParent, JSKey);
        #endregion
    }
}
