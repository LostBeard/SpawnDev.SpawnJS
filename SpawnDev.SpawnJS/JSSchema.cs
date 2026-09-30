using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// What Javascript writes back onto the tape for a value .Net reads. The first eleven kinds are exactly v2's
    /// <see cref="ReturnType"/> - same numbers, same conversions - and need no definition; the composite kinds describe
    /// a whole object, array, dictionary or tuple, so Javascript writes all of it in the call's single crossing instead
    /// of .Net reading it back one property at a time.
    /// </summary>
    public enum JSSchemaKind
    {
        /// <summary>Nothing is written.</summary>
        Void = 0,
        /// <summary>A number (null/undefined read as 0).</summary>
        Double = 1,
        /// <summary>Truthiness.</summary>
        Boolean = 2,
        /// <summary>A number, or null.</summary>
        DoubleNullable = 3,
        /// <summary>A boolean, or null.</summary>
        BooleanNullable = 4,
        /// <summary>A string (a non-string is converted), or null.</summary>
        String = 5,
        /// <summary>The value, held Javascript side; null/undefined read as a null reference.</summary>
        Ref = 6,
        /// <summary>The value, held Javascript side, even when null/undefined.</summary>
        RefNonNullable = 7,
        /// <summary>JSON.stringify of the value.</summary>
        Json = 8,
        /// <summary>A number through ToInt32 (null/undefined read as 0).</summary>
        Int32 = 9,
        /// <summary>An Int32, or null.</summary>
        Int32Nullable = 10,
        /// <summary>An object: each member read by its own schema. Null/undefined read as null.</summary>
        Object = 16,
        /// <summary>An array-like: its length, then each element by the element schema.</summary>
        Array = 17,
        /// <summary>An array-like of numbers in one copy - Int32 or Double elements.</summary>
        Numbers = 18,
        /// <summary>An object's own keys, each with its value by the element schema (a dictionary).</summary>
        Record = 19,
        /// <summary>An array-like read positionally, each item by its own schema.</summary>
        Tuple = 20,
        /// <summary>The bytes of an ArrayBuffer or ArrayBufferView, in one copy.</summary>
        Bytes = 21,
    }

    /// <summary>
    /// Something the tape defines inline, once per runtime, and only trusts Javascript to know after a call that carried
    /// the definition has completed (see <see cref="JSShape"/> for why).
    /// </summary>
    public abstract class JSDefinition
    {
        internal int Id = -1;
        internal bool Confirmed;
        // the frame that last carried the definition, so one frame carries it once
        internal long Frame = -1;
    }

    /// <summary>
    /// The schema a marshaller reads with - see <see cref="JSSchemaKind"/>. A marshaller builds its composite schema once;
    /// the tape defines it inline, after the arguments of the first call that returns it.
    /// </summary>
    public sealed class JSSchema : JSDefinition
    {
        /// <summary>What Javascript writes.</summary>
        public JSSchemaKind Kind { get; }
        /// <summary><see cref="JSSchemaKind.Object"/>: the Javascript member names read, in order.</summary>
        public IReadOnlyList<string>? Names { get; }
        /// <summary><see cref="JSSchemaKind.Object"/> / <see cref="JSSchemaKind.Tuple"/>: one schema per member. Filled
        /// after the schema is created, so a type that contains itself can refer to its own schema.</summary>
        public JSSchema?[]? Members { get; }
        /// <summary><see cref="JSSchemaKind.Array"/> / <see cref="JSSchemaKind.Record"/>: the element schema.</summary>
        public JSSchema? Element { get; internal set; }
        /// <summary><see cref="JSSchemaKind.Numbers"/>: <see cref="JSSchemaKind.Int32"/> or <see cref="JSSchemaKind.Double"/>.</summary>
        public JSSchemaKind NumberKind { get; }

        JSSchema(JSSchemaKind kind, IReadOnlyList<string>? names, int memberCount, JSSchemaKind numberKind)
        {
            Kind = kind;
            Names = names;
            Members = memberCount > 0 || kind == JSSchemaKind.Object || kind == JSSchemaKind.Tuple ? new JSSchema?[memberCount] : null;
            NumberKind = numberKind;
        }

        /// <summary>An object with these member names; set <see cref="Members"/> before it is used.</summary>
        public static JSSchema Object(IReadOnlyList<string> names) => new JSSchema(JSSchemaKind.Object, names, names.Count, default);
        /// <summary>A positional array of <paramref name="count"/> items; set <see cref="Members"/> before it is used.</summary>
        public static JSSchema Tuple(int count) => new JSSchema(JSSchemaKind.Tuple, null, count, default);
        /// <summary>An array of <paramref name="element"/>.</summary>
        public static JSSchema Array(JSSchema element) => new JSSchema(JSSchemaKind.Array, null, 0, default) { Element = element };
        /// <summary>A dictionary-like object whose values are <paramref name="element"/>.</summary>
        public static JSSchema Record(JSSchema element) => new JSSchema(JSSchemaKind.Record, null, 0, default) { Element = element };
        /// <summary>An array of numbers read in one copy, as Int32 or Double.</summary>
        public static JSSchema Numbers(JSSchemaKind numberKind)
        {
            if (numberKind != JSSchemaKind.Int32 && numberKind != JSSchemaKind.Double) throw new ArgumentException("Numbers are Int32 or Double", nameof(numberKind));
            return new JSSchema(JSSchemaKind.Numbers, null, 0, numberKind);
        }
        /// <summary>The bytes of an ArrayBuffer or view.</summary>
        public static JSSchema Bytes() => new JSSchema(JSSchemaKind.Bytes, null, 0, default);

        static readonly JSSchema[] _builtins = Enumerable.Range(0, 11).Select(i => new JSSchema((JSSchemaKind)i, null, 0, default) { Id = i, Confirmed = true }).ToArray();
        /// <summary>The schema of a v2 <see cref="ReturnType"/> - built in, never defined on the tape.</summary>
        public static JSSchema Builtin(ReturnType returnType) => _builtins[(int)returnType];
        /// <summary>True for the eleven ReturnType kinds.</summary>
        public bool IsBuiltin => (int)Kind <= 10;
    }
}
