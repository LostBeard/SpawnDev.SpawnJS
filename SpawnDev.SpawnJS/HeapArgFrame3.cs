using SpawnDev.SpawnJS.Toolbox;
using System.Runtime.InteropServices;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// PROBE: the interleaved frame with a third field, which is what a STRING argument needs.
    /// <br/><br/>
    /// A string cannot be described by one number: Javascript needs to know where its characters are AND
    /// how many there are. That is precisely why the .Net runtime's own marshaler slot carries a length
    /// field alongside the value, and why it is wider than the two-field frame measured in
    /// <see cref="HeapArgFrame"/>.
    /// <br/><br/>
    /// Layout, per argument, stride 24 - every field a float64 so there is one heap view and one read
    /// width, which is what made the two-field frame win its A/B:
    /// <code>
    ///   +0    value   - the number, the slot id, or a pinned string's ADDRESS
    ///   +8    tag     - what the value is
    ///   +16   aux     - a string's LENGTH in characters; unused otherwise
    /// </code>
    /// Exists to answer one question with a measurement: whether carrying strings by pinned address in a
    /// wider frame beats letting them cross one at a time into a slot. The answer decides whether the
    /// production frame is 16 bytes or 24.
    /// </summary>
    public sealed class HeapArgFrame3 : IDisposable
    {
        /// <summary>Bytes per argument slot.</summary>
        public const int Stride = 24;
        /// <summary>Float64 elements per argument slot.</summary>
        public const int Elements = Stride / 8;

        /// <summary>A number, carried as itself.</summary>
        public const byte TagNumber = 1;
        /// <summary>A boolean, carried as 0 or 1.</summary>
        public const byte TagBoolean = 2;
        /// <summary>A slot id - Javascript resolves it against the slot table.</summary>
        public const byte TagSlot = 3;
        /// <summary>A pinned .Net string: value is its address, aux is its length in characters.</summary>
        public const byte TagString = 4;

        readonly double[] _frame;
        readonly HeapView<double> _view;
        // one pin per live string argument, released when the frame is reset. A pinned handle is what
        // keeps the characters at the address Javascript was handed.
        readonly List<GCHandle> _pins = new();

        /// <summary>Address of the frame in the WebAssembly heap. Always 8 byte aligned.</summary>
        public long Address => _view.Address;
        /// <summary>How many arguments the frame holds.</summary>
        public int Capacity { get; }

        /// <summary>Allocates and pins the frame.</summary>
        public HeapArgFrame3(int capacity = 4096)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
            _frame = new double[capacity * Elements];
            _view = new HeapView<double>(_frame);
        }

        /// <summary>Points the Javascript side at this frame.</summary>
        public void Bind()
        {
            if (Address % 8 != 0)
                throw new InvalidOperationException($"the pinned frame is at {Address}, which is not 8 byte aligned");
            SlotInterop.BindArgFrame(Address, Capacity * Stride);
        }

        /// <summary>Writes a tagged value. No crossing.</summary>
        public void WriteTagged(int index, byte tag, double value)
        {
            var at = index * Elements;
            _frame[at] = value;
            _frame[at + 1] = tag;
        }

        /// <summary>
        /// Pins a string and writes its ADDRESS and LENGTH. No crossing, and the characters are never
        /// copied .Net side.<br/>
        /// The pin is held until <see cref="ReleasePins"/>, which must run after the call returns -
        /// safe because interop here is synchronous, so nothing can move while Javascript reads.
        /// </summary>
        public void WriteString(int index, string value)
        {
            var handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            _pins.Add(handle);
            var at = index * Elements;
            _frame[at] = handle.AddrOfPinnedObject().ToInt64();
            _frame[at + 1] = TagString;
            _frame[at + 2] = value.Length;
        }

        /// <summary>Releases every string pin taken since the last release. Call after the call returns.</summary>
        public void ReleasePins()
        {
            for (var i = 0; i < _pins.Count; i++) _pins[i].Free();
            _pins.Clear();
        }

        /// <summary>Reads a value back.</summary>
        public double Read(int index) => _frame[index * Elements];
        /// <summary>Reads a tag back.</summary>
        public byte ReadTag(int index) => (byte)_frame[index * Elements + 1];
        /// <summary>Reads the aux field back.</summary>
        public double ReadAux(int index) => _frame[index * Elements + 2];

        /// <inheritdoc/>
        public void Dispose()
        {
            ReleasePins();
            _view.Dispose();
        }
    }
}
