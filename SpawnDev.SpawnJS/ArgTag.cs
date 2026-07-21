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
    }
}
