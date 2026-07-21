using SpawnDev.SpawnJS.Toolbox;
using System.Runtime.InteropServices;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// PROBE: the INTERLEAVED argument frame - one padded slot per argument, which is the shape the .Net
    /// runtime's own marshaller uses.
    /// <br/><br/>
    /// Read out of the shipped runtime (dotnet.runtime.js): a marshaler argument keeps its value at slot
    /// offset 0, a handle at 4, a length at 8 and a TYPE TAG BYTE at 12 - all interleaved inside one
    /// fixed-stride slot, read with <c>HEAPF64[addr&gt;&gt;&gt;3]</c> and <c>HEAPU8[addr]</c>.
    /// <br/><br/>
    /// The alignment problem that pushed <see cref="HeapArgBuffer"/> to structure-of-arrays is solved by
    /// PADDING rather than by splitting: a tag byte next to an eight byte value gives a stride of 9 and
    /// misaligns every payload, but padding the stride to a multiple of 8 puts every value back on an
    /// eight byte boundary while keeping an argument's fields together. The runtime pads to 32 because it
    /// carries six fields; we carry two, so 16 is enough.
    /// <br/><br/>
    /// Layout, per argument, stride 16:
    /// <code>
    ///   +0   value   (float64)   -> HEAPF64[(base + i*16) >>> 3]
    ///   +8   tag     (uint8)     -> HEAPU8 [ base + i*16 + 8   ]
    ///   +9   (padding to keep the next value 8 byte aligned)
    /// </code>
    /// Exists to be measured against <see cref="HeapArgBuffer"/>, not to be assumed better.
    /// </summary>
    public sealed class HeapArgFrame : IDisposable
    {
        /// <summary>Bytes per argument slot. A multiple of 8, so every value stays 8 byte aligned.</summary>
        public const int Stride = 16;
        /// <summary>Byte offset of the tag within a slot.</summary>
        public const int TagOffset = 8;

        /// <summary>A number, carried as itself.</summary>
        public const byte TagNumber = 1;
        /// <summary>A boolean, carried as 0 or 1.</summary>
        public const byte TagBoolean = 2;
        /// <summary>A slot id - Javascript resolves it against the slot table.</summary>
        public const byte TagSlot = 3;

        // backed by double[] rather than byte[] so the pinned base is naturally 8 byte aligned; the tag
        // bytes are written through a byte view over the same memory.
        readonly double[] _frame;
        readonly HeapView<double> _view;

        /// <summary>Address of the frame in the WebAssembly heap. Always 8 byte aligned.</summary>
        public long Address => _view.Address;
        /// <summary>How many arguments the frame holds.</summary>
        public int Capacity { get; }
        /// <summary>Whether the Javascript side has been pointed at this frame.</summary>
        public bool IsBound { get; private set; }

        /// <summary>Allocates and pins the frame.</summary>
        public HeapArgFrame(int capacity = 4096)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
            _frame = new double[capacity * (Stride / 8)];
            _view = new HeapView<double>(_frame);
        }

        /// <summary>
        /// Points the Javascript side at this frame. Throws if the pinned base is not 8 byte aligned,
        /// because HEAPF64 is indexed in elements and a misaligned base reads the wrong slot silently.
        /// </summary>
        public void Bind()
        {
            if (Address % 8 != 0)
                throw new InvalidOperationException($"the pinned frame is at {Address}, which is not 8 byte aligned");
            SlotInterop.BindArgFrame(Address, Capacity * Stride);
            IsBound = true;
        }

        /// <summary>Writes a value at an argument index. No crossing - a plain array store.</summary>
        public void Write(int index, double value) => _frame[index * (Stride / 8)] = value;

        /// <summary>Writes a value and its tag. No crossing.</summary>
        public void WriteTagged(int index, byte tag, double value)
        {
            _frame[index * (Stride / 8)] = value;
            MemoryMarshal.AsBytes(_frame.AsSpan())[index * Stride + TagOffset] = tag;
        }

        /// <summary>
        /// Writes a value and its tag, with the tag stored as a float64 in the slot's PADDING rather than
        /// as a byte.<br/>
        /// <br/>
        /// The padding is there either way, so this costs nothing in space - and it removes two costs the
        /// byte form pays: a <c>MemoryMarshal.AsBytes</c> span per write on the .Net side, and a second
        /// heap view lookup plus a mixed width read on the Javascript side. Measured against the byte
        /// form rather than assumed better.
        /// </summary>
        public void WriteTaggedF64(int index, byte tag, double value)
        {
            var at = index * (Stride / 8);
            _frame[at] = value;
            _frame[at + 1] = tag;
        }

        /// <summary>Reads a float64 stored tag back.</summary>
        public byte ReadTagF64(int index) => (byte)_frame[index * (Stride / 8) + 1];

        /// <summary>Reads a value back.</summary>
        public double Read(int index) => _frame[index * (Stride / 8)];

        /// <summary>Reads a tag back.</summary>
        public byte ReadTag(int index) => MemoryMarshal.AsBytes(_frame.AsSpan())[index * Stride + TagOffset];

        /// <inheritdoc/>
        public void Dispose() => _view.Dispose();
    }
}
