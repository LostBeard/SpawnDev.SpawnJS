using SpawnDev.SpawnJS.Toolbox;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// The outbound argument frame - one padded slot per argument, in .Net's OWN memory, which Javascript
    /// views directly through the runtime's HEAPF64 view. A .Net write is a plain array store and costs
    /// NOTHING to deliver; only the call itself - a command name, an offset and a length - crosses.
    /// <br/><br/>
    /// One argument is a value and a tag, both carried as float64 so there is one heap view and one read
    /// width. The tag lives in the slot's PADDING rather than as a separate byte: a byte tag next to an
    /// eight byte value gives a stride of 9 and misaligns every payload, so the stride is padded to a
    /// multiple of 8, which keeps every value 8 byte aligned while keeping an argument's fields together.
    /// <br/><br/>
    /// Layout, per argument, stride 16:
    /// <code>
    ///   +0   value   (float64)   -> HEAPF64[(base + i*16) >>> 3]
    ///   +8   tag     (float64)   -> HEAPF64[(base + i*16) >>> 3 + 1]
    /// </code>
    /// </summary>
    public sealed class HeapArgFrame : IDisposable
    {
        /// <summary>Bytes per argument slot. A multiple of 8, so every value stays 8 byte aligned.</summary>
        public const int Stride = 16;

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
        /// <param name="ctxId">
        /// The owning runtime's context id. Passed rather than looked up because Bind runs INSIDE the
        /// runtime's constructor, before the singleton is assigned - reaching for it there would
        /// construct a second runtime and recurse.
        /// </param>
        public void Bind(double ctxId)
        {
            if (Address % 8 != 0)
                throw new InvalidOperationException($"the pinned frame is at {Address}, which is not 8 byte aligned");
            SlotInterop.BindArgFrame(ctxId, Address, Capacity * Stride);
            IsBound = true;
        }

        /// <summary>
        /// Binds this frame for BENCHMARKS AND LAYOUT TESTS, which read it through their own address.<br/>
        /// Never bind a probe frame with <see cref="Bind"/>: that address belongs to the live transport,
        /// and pointing it at another frame redirects every call's arguments without any error.
        /// </summary>
        public void BindProbe(double ctxId)
        {
            if (Address % 8 != 0)
                throw new InvalidOperationException($"the pinned frame is at {Address}, which is not 8 byte aligned");
            SlotInterop.BindProbeFrame(ctxId, Address, Capacity * Stride);
            IsBound = true;
        }

        /// <summary>Writes a value at an argument index. No crossing - a plain array store.</summary>
        public void Write(int index, double value) => _frame[index * (Stride / 8)] = value;

        /// <summary>
        /// Writes a value and its tag. No crossing.<br/>
        /// Both are float64 - the tag in the slot's PADDING, which exists either way - so writing and
        /// reading an argument needs one heap view and one read width.
        /// </summary>
        public void WriteTagged(int index, byte tag, double value)
        {
            var at = index * (Stride / 8);
            _frame[at] = value;
            _frame[at + 1] = tag;
        }

        /// <summary>Reads the tag back.</summary>
        public byte ReadTag(int index) => (byte)_frame[index * (Stride / 8) + 1];

        /// <summary>Reads a value back.</summary>
        public double Read(int index) => _frame[index * (Stride / 8)];

        /// <inheritdoc/>
        public void Dispose() => _view.Dispose();
    }
}
