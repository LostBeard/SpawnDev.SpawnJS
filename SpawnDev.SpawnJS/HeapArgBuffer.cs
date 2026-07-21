using SpawnDev.SpawnJS.Toolbox;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// PROBE: an argument buffer that lives in .Net's OWN memory rather than in Javascript's.
    /// <br/><br/>
    /// Javascript can view the WebAssembly linear memory directly, so a buffer placed THERE is free to
    /// both sides: a .Net write is a plain array store, and a Javascript read is an element read off the
    /// runtime's own HEAPF64 view. The buffers in use today live Javascript side, which is free for
    /// Javascript but costs .Net a boundary crossing PER ARGUMENT - so an N argument call pays N+1
    /// crossings, at a measured 1.5 to 1.8us each. Through here it pays ONE, for the signal that says
    /// "go, at this offset".
    /// <br/><br/>
    /// Read through the runtime's published HEAP views (HEAPF64, HEAPU8), which are ordinary TypedArrays
    /// over the whole heap and therefore use the platform's byte order - the same order .Net writes and
    /// Javascript reads everywhere else. There is no byte order parameter anywhere in this design and so
    /// nothing to get wrong at one call site out of fifty.
    /// <br/><br/>
    /// STRUCTURE OF ARRAYS, not interleaved. A tag byte followed by an eight byte payload gives a stride
    /// of 9, and HEAPF64 is indexed in ELEMENTS - so an interleaved payload would be misaligned and the
    /// index arithmetic would silently read the wrong slot. Tags live in their own region and values in
    /// theirs, parallel by index, each naturally aligned for its own type.
    /// <br/><br/>
    /// The memory is pinned by <see cref="HeapView{TElement}"/>, which takes a pinned GCHandle, so the
    /// collector cannot move it out from under Javascript.
    /// </summary>
    public sealed class HeapArgBuffer : IDisposable
    {
        /// <summary>A number, carried as itself.</summary>
        public const byte TagNumber = 1;
        /// <summary>A boolean, carried as 0 or 1.</summary>
        public const byte TagBoolean = 2;
        /// <summary>A slot id - Javascript resolves it against the slot table.</summary>
        public const byte TagSlot = 3;

        readonly double[] _values;
        readonly byte[] _tags;
        readonly HeapView<double> _valueView;
        readonly HeapView<byte> _tagView;

        /// <summary>Address of the value region in the WebAssembly heap. Always 8 byte aligned.</summary>
        public long ValueAddress => _valueView.Address;
        /// <summary>Address of the tag region in the WebAssembly heap.</summary>
        public long TagAddress => _tagView.Address;
        /// <summary>How many arguments the buffer holds.</summary>
        public int Capacity => _values.Length;
        /// <summary>Whether the Javascript side has been pointed at this buffer.</summary>
        public bool IsBound { get; private set; }

        /// <summary>
        /// Allocates and pins the buffer. One per process is the intent - binding costs a crossing, and
        /// not paying crossings is the entire point.
        /// </summary>
        public HeapArgBuffer(int capacity = 4096)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            _values = new double[capacity];
            _tags = new byte[capacity];
            _valueView = new HeapView<double>(_values);
            _tagView = new HeapView<byte>(_tags);
        }

        /// <summary>
        /// Points the Javascript side at this buffer.<br/>
        /// Throws if the pinned value region is not 8 byte aligned: HEAPF64 is indexed in elements, so a
        /// misaligned address would read the wrong element rather than fail, and a loud throw here is far
        /// better than silently shifted arguments later.
        /// </summary>
        public void Bind()
        {
            if (ValueAddress % 8 != 0)
                throw new InvalidOperationException($"the pinned value region is at {ValueAddress}, which is not 8 byte aligned");
            SlotInterop.BindArgBuffer(ValueAddress, _values.Length * 8);
            IsBound = true;
        }

        /// <summary>Writes a value at an argument index. No crossing - this is a plain array store.</summary>
        public void Write(int index, double value) => _values[index] = value;

        /// <summary>Reads a value back, which is what a Javascript written RESULT would look like.</summary>
        public double Read(int index) => _values[index];

        /// <summary>Writes a tagged argument. No crossing.</summary>
        public void WriteTagged(int index, byte tag, double value)
        {
            _tags[index] = tag;
            _values[index] = value;
        }

        /// <summary>Reads an argument's tag.</summary>
        public byte ReadTag(int index) => _tags[index];

        /// <summary>Byte offset of the tag region relative to the value region, for the Javascript side.</summary>
        public long TagOffsetFromValues => TagAddress - ValueAddress;

        /// <inheritdoc/>
        public void Dispose()
        {
            _valueView.Dispose();
            _tagView.Dispose();
        }
    }
}
