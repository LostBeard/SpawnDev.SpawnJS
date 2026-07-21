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
        /// Allocates a slot holding a string, in one crossing. Declared with a string parameter rather
        /// than Any so the runtime uses its string marshaller directly.
        /// </summary>
        [JSImport("globalThis.__sjsAllocString")]
        public static partial double AllocString(string value);

        /// <summary>
        /// Calls a command through the ARGUMENT FRAME: the arguments are already in .Net memory, so only
        /// the name, an offset and a length cross. The result comes back in the caller's own frame slot.
        /// </summary>
        [JSImport("globalThis.__sjsFrameCall")]
        public static partial void FrameCall(string cmd, double offset, double length);

        /// <summary>
        /// Calls a METHOD on a slotted object with its arguments already in the frame - ONE crossing for
        /// the whole call.<br/>
        /// Replaces build-an-array-and-fill-it, which cost one crossing to create the array, one PER
        /// ARGUMENT to fill it, one to invoke and one to free it.
        /// </summary>
        [JSImport("globalThis.__sjsInvokeFrameVoid")]
        public static partial void InvokeFrameVoid(double targetSlot, string name, double offset, double length);

        /// <summary>
        /// As <see cref="InvokeFrameVoid"/>, with the result written back into the caller's own frame
        /// slot - so a number or boolean result moves no data across the boundary either way, and an
        /// object result arrives as a slot id rather than a proxy.
        /// </summary>
        [JSImport("globalThis.__sjsInvokeFrameResult")]
        public static partial void InvokeFrameResult(double targetSlot, string name, double offset, double length);

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

        /// <summary>Reads a property as a byte array, or null if it was null or undefined.</summary>
        [JSImport("globalThis.__sjsGet")]
        public static partial byte[]? GetByteArray(double slot, string key);

        // Numeric-key reads. Same Javascript function, bound with a number parameter so an index does not
        // have to become a string first - the read-side counterpart of the SetAt variants.

        /// <summary>Reads a property at a numeric index as an int.</summary>
        [JSImport("globalThis.__sjsGetAt")]
        public static partial int GetInt32At(double slot, double index);

        /// <summary>Reads a property at a numeric index as a number.</summary>
        [JSImport("globalThis.__sjsGetAt")]
        public static partial double GetDoubleAt(double slot, double index);

        /// <summary>Reads a property at a numeric index as a bool.</summary>
        [JSImport("globalThis.__sjsGetAt")]
        public static partial bool GetBooleanAt(double slot, double index);

        /// <summary>Reads a property at a numeric index as a string, or null.</summary>
        [JSImport("globalThis.__sjsGetAt")]
        public static partial string? GetStringAt(double slot, double index);

        /// <summary>Reads a property at a numeric index as a byte array, or null.</summary>
        [JSImport("globalThis.__sjsGetAt")]
        public static partial byte[]? GetByteArrayAt(double slot, double index);

        /// <summary>
        /// Whether a property exists on the slotted object. <paramref name="useIn"/> selects the
        /// <c>in</c> operator, which walks the prototype chain, over hasOwnProperty.
        /// </summary>
        [JSImport("globalThis.__sjsHas")]
        public static partial bool Has(double slot, string key, bool useIn);

        // The value a slot HOLDS, rather than a property of it - what an owning handle needs to read
        // itself. Same one Javascript function at several .Net return types, as above.

        /// <summary>Reads the slot's own value as a bool.</summary>
        [JSImport("globalThis.__sjsSelf")]
        public static partial bool SelfBoolean(double slot);

        /// <summary>Reads the slot's own value as an int.</summary>
        [JSImport("globalThis.__sjsSelf")]
        public static partial int SelfInt32(double slot);

        /// <summary>Reads the slot's own value as a number.</summary>
        [JSImport("globalThis.__sjsSelf")]
        public static partial double SelfDouble(double slot);

        /// <summary>Reads the slot's own value as a string, or null.</summary>
        [JSImport("globalThis.__sjsSelf")]
        public static partial string? SelfString(double slot);

        /// <summary>Reads the slot's own value as a byte array, or null.</summary>
        [JSImport("globalThis.__sjsSelf")]
        public static partial byte[]? SelfByteArray(double slot);

        /// <summary>Reads the slot's own value as an int, or null.</summary>
        [JSImport("globalThis.__sjsSelf")]
        public static partial int? SelfInt32Nullable(double slot);

        /// <summary>Reads the slot's own value as a number, or null.</summary>
        [JSImport("globalThis.__sjsSelf")]
        public static partial double? SelfDoubleNullable(double slot);

        /// <summary>Reads the slot's own value as a bool, or null.</summary>
        [JSImport("globalThis.__sjsSelf")]
        public static partial bool? SelfBooleanNullable(double slot);

        /// <summary>Reads the slot's own value, letting the runtime decide how it crosses.</summary>
        [JSImport("globalThis.__sjsSelf")]
        [return: JSMarshalAs<JSType.Any>]
        public static partial object? SelfAny(double slot);

        /// <summary>Reads a property as an int, or null. Numeric index.</summary>
        [JSImport("globalThis.__sjsGetAt")]
        public static partial int? GetInt32NullableAt(double slot, double index);

        /// <summary>Reads a property as a number, or null. Numeric index.</summary>
        [JSImport("globalThis.__sjsGetAt")]
        public static partial double? GetDoubleNullableAt(double slot, double index);

        /// <summary>Reads a property as a bool, or null. Numeric index.</summary>
        [JSImport("globalThis.__sjsGetAt")]
        public static partial bool? GetBooleanNullableAt(double slot, double index);

        /// <summary>Reads a property, letting the runtime decide how it crosses.</summary>
        [JSImport("globalThis.__sjsGet")]
        [return: JSMarshalAs<JSType.Any>]
        public static partial object? GetAny(double slot, string key);

        /// <summary>Reads a property at a numeric index, letting the runtime decide how it crosses.</summary>
        [JSImport("globalThis.__sjsGetAt")]
        [return: JSMarshalAs<JSType.Any>]
        public static partial object? GetAnyAt(double slot, double index);

        // PROBE: an argument buffer in .Net's OWN memory. See the notes on the Javascript side.
        // Javascript can view the WebAssembly linear memory directly, so a buffer placed there is free to
        // BOTH sides - .Net writes are plain stores, Javascript reads are DataView reads - which takes an
        // N argument call from N+1 crossings down to 1.

        /// <summary>
        /// Binds the Javascript side to a pinned region of .Net memory. Called once; the view is cached
        /// and rebound automatically if growing the WebAssembly memory detaches it.
        /// </summary>
        [JSImport("globalThis.__sjsBindArgBuffer")]
        public static partial bool BindArgBuffer(double address, double byteLength);

        /// <summary>
        /// Which HEAP views the runtime actually exposes, comma separated. The design reads .Net memory
        /// through the runtime's own TypedArray views, so this reports what is really there rather than
        /// relying on what Emscripten normally exports.
        /// </summary>
        [JSImport("globalThis.__sjsHeapViewNames")]
        public static partial string HeapViewNames();

        /// <summary>
        /// PROBE: reads a .Net string straight out of .Net memory, given the address of its first
        /// character and its length in chars. Nothing is copied .Net side and no string marshaller runs.
        /// </summary>
        [JSImport("globalThis.__sjsReadUtf16")]
        public static partial string ReadUtf16(double address, double length);

        /// <summary>
        /// Builds a Javascript object from name/value pairs in the frame and returns its slot.<br/>
        /// One crossing for the whole object however many members it has, against one PER MEMBER plus
        /// three before.
        /// </summary>
        [JSImport("globalThis.__sjsBuildObject")]
        public static partial double BuildObject(double offset, double count);

        /// <summary>
        /// Builds the object and ASSIGNS it in the same crossing, so a descriptor written onto a slotted
        /// parent costs exactly one - and allocates no temporary slot, so none has to be freed.
        /// </summary>
        [JSImport("globalThis.__sjsBuildObjectInto")]
        public static partial void BuildObjectInto(double parentSlot, string key, double offset, double count);

        /// <summary>
        /// Binds the Javascript side to the TRANSPORT argument frame. Called once, by the runtime.
        /// </summary>
        [JSImport("globalThis.__sjsBindArgFrame")]
        public static partial bool BindArgFrame(double address, double byteLength);

        /// <summary>
        /// Binds a PROBE frame - benchmarks and layout tests only.<br/>
        /// Separate from <see cref="BindArgFrame"/> on purpose. When both shared one address, binding a
        /// probe silently redirected every live transport call to read the probe's memory: nothing threw,
        /// the values simply came from the wrong place.
        /// </summary>
        [JSImport("globalThis.__sjsBindProbeFrame")]
        public static partial bool BindProbeFrame(double address, double byteLength);

        /// <summary>PROBE: sums `count` values from the interleaved frame - one padded slot per argument.</summary>
        [JSImport("globalThis.__sjsFrameSum")]
        public static partial double FrameSum(double count);

        /// <summary>PROBE: the same, reading each slot's inline tag byte - the runtime's own shape.</summary>
        [JSImport("globalThis.__sjsFrameTaggedSum")]
        public static partial double FrameTaggedSum(double count);

        /// <summary>PROBE: interleaved with the tag as a float64 in the slot's padding - one view, one width.</summary>
        [JSImport("globalThis.__sjsFrameTaggedSumF64")]
        public static partial double FrameTaggedSumF64(double count);

        /// <summary>PROBE: decodes `count` strings the frame carries as (address, length) - none crossed.</summary>
        [JSImport("globalThis.__sjsFrameStringLength")]
        public static partial double FrameStringLength(double count);

        /// <summary>PROBE: the same, over strings that crossed the boundary one at a time.</summary>
        [JSImport("globalThis.__sjsSlotStringLength")]
        public static partial double SlotStringLength(double argsSlot, double count);

        /// <summary>
        /// PROBE: the same sum over a Javascript side argument array - the transport in use today. The
        /// Javascript work is identical to <see cref="HeapSum"/>, so an A/B isolates exactly what .Net
        /// paid to deliver the arguments.
        /// </summary>
        [JSImport("globalThis.__sjsSlotSum")]
        public static partial double SlotSum(double argsSlot, double count);

        /// <summary>PROBE: sums `count` float64s .Net wrote at `offset` bytes, delivered in ONE crossing.</summary>
        [JSImport("globalThis.__sjsHeapSum")]
        public static partial double HeapSum(double offset, double count);

        /// <summary>
        /// PROBE: the same for a heterogeneous list - tags in one region, payloads in another, parallel by
        /// index. Structure of arrays rather than interleaved, because an interleaved tag would misalign
        /// the float64 payloads.
        /// </summary>
        [JSImport("globalThis.__sjsHeapTaggedSum")]
        public static partial double HeapTaggedSum(double tagOffset, double valueOffset, double count);

        /// <summary>
        /// Own enumerable keys of the slotted object, for reading a record back.<br/>
        /// Null - not an empty array - when the value is null or undefined, so "no object here" stays
        /// distinguishable from "an object with no keys".
        /// </summary>
        [JSImport("globalThis.__sjsKeys")]
        public static partial string[]? Keys(double slot, bool ownOnly);

        // Writes whose VALUE type the binding decides. These exist for the cases the typed setters do not
        // cover, where the value was never the problem - the PARENT had to become a proxy to be written
        // through. Declared separately per value type for the same reason Reflect.Set is.

        /// <summary>Writes an arbitrary value to a property.</summary>
        [JSImport("globalThis.__sjsSetAny")]
        public static partial void SetAny(double slot, string key, [JSMarshalAs<JSType.Any>] object? value);

        /// <summary>Writes an arbitrary value at a numeric index.</summary>
        [JSImport("globalThis.__sjsSetAnyAt")]
        public static partial void SetAnyAt(double slot, double index, [JSMarshalAs<JSType.Any>] object? value);

        /// <summary>Writes a JSObject the caller genuinely holds to a property.</summary>
        [JSImport("globalThis.__sjsSetAny")]
        public static partial void SetJSObject(double slot, string key, JSObject? value);

        /// <summary>Writes a JSObject at a numeric index.</summary>
        [JSImport("globalThis.__sjsSetAnyAt")]
        public static partial void SetJSObjectAt(double slot, double index, JSObject? value);

        /// <summary>Writes a byte array to a property.</summary>
        [JSImport("globalThis.__sjsSetAny")]
        public static partial void SetBytes(double slot, string key, byte[]? value);

        /// <summary>Writes a byte array at a numeric index.</summary>
        [JSImport("globalThis.__sjsSetAnyAt")]
        public static partial void SetBytesAt(double slot, double index, byte[]? value);

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
        /// As <see cref="GetObjectSlot"/>, but slots a value that is not a reference rather than refusing
        /// it. A slot holds any Javascript value, so a wrapper over a primitive works - it is only a
        /// JSObject PROXY that cannot represent one. Still returns 0 for null and undefined.
        /// </summary>
        [JSImport("globalThis.__sjsGetValueSlot")]
        public static partial double GetValueSlot(double slot, string key);

        /// <summary>As <see cref="CloneObjectSlot"/>, but accepts a non-reference value.</summary>
        [JSImport("globalThis.__sjsCloneValueSlot")]
        public static partial double CloneValueSlot(double slot);

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
