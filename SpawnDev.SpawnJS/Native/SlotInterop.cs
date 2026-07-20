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

        /// <summary>
        /// Reads a number from a property of the slotted object.
        /// </summary>
        [JSImport("globalThis.__sjsGetDouble")]
        public static partial double GetDouble(double slot, string key);

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
