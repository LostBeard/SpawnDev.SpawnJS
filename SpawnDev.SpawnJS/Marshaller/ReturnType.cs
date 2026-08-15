namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>
    /// How a JS call result should be shaped and read back. The value is passed to JS as the call's
    /// returnType so it can prepare the result (see the JS <c>_serializeToNet</c> switch), and it selects
    /// which typed [JSImport] the .Net side uses to receive it. The numeric order MUST match the JS switch.
    /// </summary>
    public enum ReturnType
    {
        /// <summary>Nothing is returned (JS undefined).</summary>
        Void,
        /// <summary>The JS number is returned as a <c>double</c>.</summary>
        Double,
        /// <summary>The JS boolean is returned as a <c>bool</c>.</summary>
        Boolean,
        /// <summary>The JS number (or null/undefined) is returned as a <c>double?</c>.</summary>
        DoubleNullable,
        /// <summary>The JS boolean (or null/undefined) is returned as a <c>bool?</c>.</summary>
        BooleanNullable,
        /// <summary>The JS string is returned as a <c>string</c>.</summary>
        String,
        /// <summary>The value is held on the JS side and its object-table id returned, or null if the value is null/undefined.</summary>
        SpawnJSObjectReference,
        /// <summary>The value is always held on the JS side and its object-table id returned, even if null/undefined.</summary>
        SpawnJSObjectReferenceNonNullable,
        /// <summary>The value is JSON-stringified on the JS side (JSON.stringify) and returned as a string to deserialize.</summary>
        Json,
        /// <summary>The JS number is returned as a <c>int</c>.</summary>
        Int32,
        /// <summary>The JS number (or null/undefined) is returned as a <c>int?</c>.</summary>
        Int32Nullable,
    }
}
