namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// What an argument-frame slot's eight byte payload means.<br/>
    /// Kept in step with the constants at the top of the argument frame section in
    /// SpawnDev.SpawnJS.lib.module.js - the two sides read the same memory, so a value added here has to
    /// be added there.
    /// </summary>
    public static class ArgTag
    {
        /// <summary>The payload IS the number.</summary>
        public const byte Number = 1;
        /// <summary>The payload is 0 or 1.</summary>
        public const byte Boolean = 2;
        /// <summary>
        /// The payload is a slot id, resolved against the Javascript slot table. Covers objects,
        /// wrappers, handles and interned strings - everything that already lives Javascript side.
        /// </summary>
        public const byte Slot = 3;
        /// <summary>Javascript null.</summary>
        public const byte Null = 4;
        /// <summary>Javascript undefined.</summary>
        public const byte Undefined = 5;
        /// <summary>
        /// The value was BUILT Javascript side already and the payload indexes the scratch array. The
        /// fallback for anything that cannot be one number - a descriptor object, an array - which has to
        /// be constructed there whatever the transport does.
        /// </summary>
        public const byte Scratch = 6;
        /// <summary>
        /// An object built inline, Javascript side, out of a NESTED REGION of the frame. The payload packs
        /// where that region is and how many name/value pairs it holds, via <see cref="PackInline"/>.<br/>
        /// <br/>
        /// This exists so an object passed as an argument costs no slot. Handing one over as a
        /// <see cref="Slot"/> means allocating a slot table entry that only the call needs, and the slot
        /// table holds a STRONG reference - so a temporary that nobody frees is a permanent leak of both
        /// the entry and the Javascript object it names. Built inline there is nothing to free.
        /// </summary>
        public const byte InlineObject = 7;

        /// <summary>
        /// How many name/value pairs an inline object may carry. The pair count occupies the low digits of
        /// the packed payload, so it bounds what can be encoded.
        /// </summary>
        public const int InlinePackLimit = 1 << 20;

        /// <summary>
        /// Packs an inline object's frame position and pair count into one float64.<br/>
        /// <paramref name="f64Index"/> is the region's ABSOLUTE index into HEAPF64, not a reservation
        /// offset, so the Javascript side needs nothing but the payload to find it. Both parts stay whole:
        /// a heap index is well under 2^32 and the count under 2^20, so the product is under 2^52 and a
        /// float64 carries it exactly.
        /// </summary>
        public static double PackInline(double f64Index, int count) => f64Index * InlinePackLimit + count;
    }
}
