using System.Runtime.InteropServices;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Reads a value Javascript wrote onto the tape by a <see cref="JSSchema"/>. A ref struct: a marshaller's Read can
    /// re-enter interop (a wrapper's constructor may call Javascript), so each read has its own position on its own
    /// stack, and nothing is allocated to read.
    /// <para>
    /// Every value is 8 byte cells. Scalars are one cell; a nullable is its tag (null) or its tag and value; a string is
    /// null, or tag, length and UTF-16 code units; a composite is null, or its tag and then its contents.
    /// </para>
    /// </summary>
    public ref struct JSTapeReader
    {
        readonly ReadOnlySpan<double> _cells;
        int _p;

        internal JSTapeReader(ReadOnlySpan<double> cells)
        {
            _cells = cells;
            _p = 0;
        }

        /// <summary><see cref="JSSchemaKind.Double"/>.</summary>
        public double ReadDouble() => _cells[_p++];
        /// <summary><see cref="JSSchemaKind.Int32"/> - written through an Int32Array, so it carries ToInt32's wrap.</summary>
        public int ReadInt32() => MemoryMarshal.Cast<double, int>(_cells.Slice(_p++, 1))[0];
        /// <summary><see cref="JSSchemaKind.Boolean"/>.</summary>
        public bool ReadBoolean() => _cells[_p++] != 0;
        /// <summary><see cref="JSSchemaKind.DoubleNullable"/>.</summary>
        public double? ReadDoubleNullable() => IsNull() ? null : _cells[_p++];
        /// <summary><see cref="JSSchemaKind.Int32Nullable"/>.</summary>
        public int? ReadInt32Nullable() => IsNull() ? null : ReadInt32();
        /// <summary><see cref="JSSchemaKind.BooleanNullable"/>.</summary>
        public bool? ReadBooleanNullable() => IsNull() ? null : _cells[_p++] != 0;
        /// <summary><see cref="JSSchemaKind.String"/> / <see cref="JSSchemaKind.Json"/>.</summary>
        public string? ReadString() => IsNull() ? null : ReadChars();
        /// <summary><see cref="JSSchemaKind.Ref"/> / <see cref="JSSchemaKind.RefNonNullable"/>.</summary>
        public SpawnJSObjectReference? ReadRef(bool nonNullable) => SpawnJSObjectReference.FromID(_cells[_p++], nonNullable);

        /// <summary><see cref="JSSchemaKind.Object"/>: false for null, else each member follows in schema order.</summary>
        public bool ReadObjectStart() => !IsNull();
        /// <summary>
        /// <see cref="JSSchemaKind.Array"/>, <see cref="JSSchemaKind.Tuple"/>, <see cref="JSSchemaKind.Record"/>,
        /// <see cref="JSSchemaKind.Numbers"/>, <see cref="JSSchemaKind.Bytes"/>: -1 for null, else the count.
        /// </summary>
        public int ReadCount() => IsNull() ? -1 : (int)_cells[_p++];
        /// <summary><see cref="JSSchemaKind.Record"/>: a key; its value follows.</summary>
        public string ReadKey() => ReadChars();
        /// <summary><see cref="JSSchemaKind.Numbers"/> / <see cref="JSSchemaKind.Bytes"/>: the raw elements after <see cref="ReadCount"/>.</summary>
        public ReadOnlySpan<T> ReadRaw<T>(int count) where T : unmanaged
        {
            var bytes = count * System.Runtime.CompilerServices.Unsafe.SizeOf<T>();
            var cells = (bytes + 7) >> 3;
            var raw = MemoryMarshal.Cast<double, T>(_cells.Slice(_p, cells)).Slice(0, count);
            _p += cells;
            return raw;
        }

        /// <summary>True when the next value is null, without reading it.</summary>
        public bool PeekNull() => _cells[_p] == JSTape.TagNull;
        /// <summary>Reads past a null (after <see cref="PeekNull"/> returned true).</summary>
        public void SkipNull() => _p++;

        // the leading cell of anything nullable: the null tag, or the value's own tag
        bool IsNull()
        {
            if (_cells[_p] == JSTape.TagNull) { _p++; return true; }
            _p++;
            return false;
        }

        string ReadChars()
        {
            var length = (int)_cells[_p++];
            var value = new string(MemoryMarshal.Cast<double, char>(_cells.Slice(_p)).Slice(0, length));
            _p += (length + 3) >> 2;
            return value;
        }
    }
}
