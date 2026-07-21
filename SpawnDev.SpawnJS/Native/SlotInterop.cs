using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// SPIKE: referencing Javascript objects by an integer slot instead of a .Net <see cref="JSObject"/>.
    /// <br/><br/>
    /// A JSObject is a runtime PROXY. Creating one allocates a GC handle, registers an entry in the proxy
    /// table and tags the Javascript object with an enumerable Symbol. Measured on this machine: creating
    /// an object costs 21us and wrapping one in a handle costs 7.4us, against 1.4us for a scalar property
    /// write - so a WebGPU dispatch spends most of its time creating references rather than moving values.
    /// <br/><br/>
    /// The marshallers already move one value at a time, so nothing outside startup needs the proxy. The
    /// object can live in a Javascript-side table and .Net can hold the integer that addresses it, which
    /// is an ordinary number across the boundary - no proxy, no Symbol tag, and no dependence on the
    /// runtime's proxy interning (the source of the shared-dispose quirk).
    /// <br/><br/>
    /// These bindings are written by hand against globals the interop module defines, which is the same
    /// thing the JSImport source generator emits - it just does not have to be a JSObject on the way in.
    /// </summary>
    public static partial class SlotInterop
    {
        /// <summary>
        /// Creates <c>{}</c> in Javascript and returns its slot.
        /// </summary>
        [JSImport("globalThis.__sjsNewObject")]
        public static partial double NewObject();

        /// <summary>
        /// Creates <c>[]</c> in Javascript and returns its slot.
        /// </summary>
        [JSImport("globalThis.__sjsNewArray")]
        public static partial double NewArray();

        /// <summary>
        /// Reserves a slot holding undefined, for a handle that will write its own value into it.
        /// </summary>
        [JSImport("globalThis.__sjsAllocEmpty")]
        public static partial double AllocEmpty();

        /// <summary>
        /// Allocates a slot already holding the given object, in one crossing.
        /// </summary>
        [JSImport("globalThis.__sjsAllocValue")]
        public static partial double AllocValue([JSMarshalAs<JSType.Any>] object value);

        /// <summary>
        /// Releases a slot so it can be reused. Lifetime is explicit here - there is no proxy for the
        /// runtime to collect, which is the trade for not paying to create one.
        /// </summary>
        [JSImport("globalThis.__sjsFree")]
        public static partial void Free(double slot);

        /// <summary>
        /// Writes a number to a property of the slotted object.
        /// </summary>
        [JSImport("globalThis.__sjsSetDouble")]
        public static partial void SetDouble(double slot, string key, double value);

        /// <summary>
        /// Writes a string to a property of the slotted object.
        /// </summary>
        [JSImport("globalThis.__sjsSetString")]
        public static partial void SetString(double slot, string key, string? value);

        /// <summary>
        /// Writes a boolean to a property of the slotted object.
        /// </summary>
        [JSImport("globalThis.__sjsSetBoolean")]
        public static partial void SetBoolean(double slot, string key, bool value);

        /// <summary>Writes a number at a numeric index, with no string key conversion.</summary>
        [JSImport("globalThis.__sjsSetDoubleAt")]
        public static partial void SetDoubleAt(double slot, double index, double value);

        /// <summary>Writes a string at a numeric index.</summary>
        [JSImport("globalThis.__sjsSetStringAt")]
        public static partial void SetStringAt(double slot, double index, string? value);

        /// <summary>Writes a boolean at a numeric index.</summary>
        [JSImport("globalThis.__sjsSetBooleanAt")]
        public static partial void SetBooleanAt(double slot, double index, bool value);

        /// <summary>Assigns a slotted value at a numeric index.</summary>
        [JSImport("globalThis.__sjsSetSlotAt")]
        public static partial void SetSlotAt(double slot, double index, double valueSlot);

        /// <summary>
        /// Reads a property of the slotted object as a number.<br/>
        /// This and the typed reads below all bind the SAME Javascript function and differ only in the
        /// .Net return type, which is what performs the conversion - exactly how the Reflect.get bindings
        /// are declared. The point of them is that reading a typed property no longer needs a JSObject for
        /// the object being read FROM, which the fast path used to resolve on every single property access.
        /// </summary>
        [JSImport("globalThis.__sjsGet")]
        public static partial double GetDouble(double slot, string key);

        /// <summary>Reads a property as an int.</summary>
        [JSImport("globalThis.__sjsGet")]
        public static partial int GetInt32(double slot, string key);

        /// <summary>Reads a property as a bool.</summary>
        [JSImport("globalThis.__sjsGet")]
        public static partial bool GetBoolean(double slot, string key);

        /// <summary>Reads a property as a string, or null if it was null or undefined.</summary>
        [JSImport("globalThis.__sjsGet")]
        public static partial string? GetString(double slot, string key);

        /// <summary>Reads a property as an int, or null if it was null or undefined.</summary>
        [JSImport("globalThis.__sjsGet")]
        public static partial int? GetInt32Nullable(double slot, string key);

        /// <summary>Reads a property as a number, or null if it was null or undefined.</summary>
        [JSImport("globalThis.__sjsGet")]
        public static partial double? GetDoubleNullable(double slot, string key);

        /// <summary>Reads a property as a bool, or null if it was null or undefined.</summary>
        [JSImport("globalThis.__sjsGet")]
        public static partial bool? GetBooleanNullable(double slot, string key);

        /// <summary>
        /// Reads a property of the slotted object into a NEW slot, which the caller owns.<br/>
        /// This is the read half of the slot table. Building a descriptor without a proxy was only ever
        /// half the path: every value read back out of Javascript - every <c>JS.Get&lt;Window&gt;</c>, every
        /// wrapper returned from a call - still materialised one.<br/>
        /// Returns <c>0</c> when the value is null or undefined, and <c>-1</c> when it is not a reference
        /// type. Neither is a valid slot, because allocation starts at 1, so one crossing both answers
        /// "is there an object here" and hands back the reference - with no proxy created, and no slot
        /// allocated that the caller would only have to free.
        /// </summary>
        [JSImport("globalThis.__sjsGetObjectSlot")]
        public static partial double GetObjectSlot(double slot, string key);

        /// <summary>
        /// Reads a property addressed by numeric index into a NEW slot. Same sentinels as
        /// <see cref="GetObjectSlot"/>; separate because the shared call buffer is an array and its reads
        /// must not pay a string key conversion per element.
        /// </summary>
        [JSImport("globalThis.__sjsGetObjectSlotAt")]
        public static partial double GetObjectSlotAt(double slot, double index);

        /// <summary>
        /// Takes a SECOND, independent slot on the value a slot already holds, so one handle can hand
        /// ownership of what it points at to another without either becoming a proxy. Same sentinels as
        /// <see cref="GetObjectSlot"/>.
        /// </summary>
        [JSImport("globalThis.__sjsCloneObjectSlot")]
        public static partial double CloneObjectSlot(double slot);

        /// <summary>
        /// Calls a method on the slotted object, discarding the result. Nothing becomes a .Net proxy:
        /// the target, the method and the arguments all stay in Javascript.
        /// </summary>
        [JSImport("globalThis.__sjsInvokeVoid")]
        public static partial void InvokeVoid(double slot, string name, double argsSlot);

        /// <summary>
        /// Calls a method on the slotted object and returns its number result.
        /// </summary>
        [JSImport("globalThis.__sjsInvokeDouble")]
        public static partial double InvokeDouble(double slot, string name, double argsSlot);

        /// <summary>
        /// Calls a method on the slotted object and returns its string result.
        /// </summary>
        [JSImport("globalThis.__sjsInvokeString")]
        public static partial string? InvokeString(double slot, string name, double argsSlot);

        /// <summary>
        /// Calls a method on the slotted object and returns its boolean result.
        /// </summary>
        [JSImport("globalThis.__sjsInvokeBoolean")]
        public static partial bool InvokeBoolean(double slot, string name, double argsSlot);

        /// <summary>
        /// Calls a method on the slotted object and puts the result in a NEW slot, so an
        /// object-returning call still creates no proxy. The caller owns the returned slot.
        /// </summary>
        [JSImport("globalThis.__sjsInvokeSlot")]
        public static partial double InvokeSlot(double slot, string name, double argsSlot);

        /// <summary>
        /// typeof the slotted value ("null" for null), so .Net can branch without moving the value.
        /// </summary>
        [JSImport("globalThis.__sjsTypeOf")]
        public static partial string TypeOf(double slot);

        /// <summary>
        /// Assigns one slotted object as a property of another - the nesting a descriptor needs, without
        /// either object ever becoming a .Net proxy.
        /// </summary>
        [JSImport("globalThis.__sjsSetSlot")]
        public static partial void SetSlot(double slot, string key, double valueSlot);
    }
}
