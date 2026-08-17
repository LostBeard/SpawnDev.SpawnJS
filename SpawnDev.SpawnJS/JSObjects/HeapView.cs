using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpawnDev.SpawnJS.JSObjects
{
    public abstract class HeapView : IDisposable
    {
        /// <summary>
        /// Explicit conversion to HeapView
        /// </summary>
        /// <param name="data">Data to pin</param>
        public static explicit operator HeapView(string data) => Create(data);
        /// <summary>
        /// Explicit conversion to HeapView
        /// </summary>
        /// <param name="data">Data to pin</param>
        public static explicit operator HeapView(byte[] data) => Create(data, true);
        /// <summary>
        /// Explicit conversion to HeapView
        /// </summary>
        /// <param name="data">Data to pin</param>
        public static explicit operator HeapView(ushort[] data) => Create(data, true);
        /// <summary>
        /// Explicit conversion to HeapView
        /// </summary>
        /// <param name="data">Data to pin</param>
        public static explicit operator HeapView(uint[] data) => Create(data, true);
        /// <summary>
        /// Explicit conversion to HeapView
        /// </summary>
        /// <param name="data">Data to pin</param>
        public static explicit operator HeapView(ulong[] data) => Create(data, true);
        /// <summary>
        /// Explicit conversion to HeapView
        /// </summary>
        /// <param name="data">Data to pin</param>
        public static explicit operator HeapView(sbyte[] data) => Create(data, true);
        /// <summary>
        /// Explicit conversion to HeapView
        /// </summary>
        /// <param name="data">Data to pin</param>
        public static explicit operator HeapView(short[] data) => Create(data, true);
        /// <summary>
        /// Explicit conversion to HeapView
        /// </summary>
        /// <param name="data">Data to pin</param>
        public static explicit operator HeapView(int[] data) => Create(data, true);
        /// <summary>
        /// Explicit conversion to HeapView
        /// </summary>
        /// <param name="data">Data to pin</param>
        public static explicit operator HeapView(long[] data) => Create(data, true);
        /// <summary>
        /// Explicit conversion to HeapView
        /// </summary>
        /// <param name="data">Data to pin</param>
        public static explicit operator HeapView(Half[] data) => Create(data, true);
        /// <summary>
        /// Explicit conversion to HeapView
        /// </summary>
        /// <param name="data">Data to pin</param>
        public static explicit operator HeapView(float[] data) => Create(data, true);
        /// <summary>
        /// Explicit conversion to HeapView
        /// </summary>
        /// <param name="data">Data to pin</param>
        public static explicit operator HeapView(double[] data) => Create(data, true);

        internal abstract SpawnJSObject _View { get; }
        public IntPtr Pointer { get; protected set; }
        public long ByteLength { get; protected set; }
        public long ElementCount { get; protected set; }
        public JSArrayBufferView ViewType { get; protected set; }
        public bool RefreshCopyOnMarshal { get; protected set; }
        public bool IsDisposed { get; protected set; }
        public bool Copy { get; protected set; }
        /// <summary>
        /// Creates a fresh view copy (no-op if Copy == false)
        /// </summary>
        public abstract void RefreshCopy();
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<byte, Uint8Array> Create(nint source, int length, bool copy = false)
            => new HeapView<byte, Uint8Array>(source, length, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<char, Uint8Array> Create(string source)
            => new HeapView<char, Uint8Array>(source);

        #region Create-ReadOnlyMemory-Copy
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Uint8Array CreateCopy(ReadOnlyMemory<byte> source)
            => Create<byte, Uint8Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Uint16Array CreateCopy(ReadOnlyMemory<ushort> source)
            => Create<ushort, Uint16Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Uint32Array CreateCopy(ReadOnlyMemory<uint> source)
            => Create<uint, Uint32Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static BigUint64Array CreateCopy(ReadOnlyMemory<ulong> source)
            => Create<ulong, BigUint64Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Int8Array CreateCopy(ReadOnlyMemory<sbyte> source)
            => Create<sbyte, Int8Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Int16Array CreateCopy(ReadOnlyMemory<short> source)
            => Create<short, Int16Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Int32Array CreateCopy(ReadOnlyMemory<int> source)
            => Create<int, Int32Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static BigInt64Array CreateCopy(ReadOnlyMemory<long> source)
            => Create<long, BigInt64Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Float16Array CreateCopy(ReadOnlyMemory<Half> source)
            => Create<Half, Float16Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Float32Array CreateCopy(ReadOnlyMemory<float> source)
            => Create<float, Float32Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Float64Array CreateCopy(ReadOnlyMemory<double> source)
            => Create<double, Float64Array>(source, true);
        #endregion
        #region Create-Memory-Copy
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Uint8Array CreateCopy(Memory<byte> source)
            => Create<byte, Uint8Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Uint16Array CreateCopy(Memory<ushort> source)
            => Create<ushort, Uint16Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Uint32Array CreateCopy(Memory<uint> source)
            => Create<uint, Uint32Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static BigUint64Array CreateCopy(Memory<ulong> source)
            => Create<ulong, BigUint64Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Int8Array CreateCopy(Memory<sbyte> source)
            => Create<sbyte, Int8Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Int16Array CreateCopy(Memory<short> source)
            => Create<short, Int16Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Int32Array CreateCopy(Memory<int> source)
            => Create<int, Int32Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static BigInt64Array CreateCopy(Memory<long> source)
            => Create<long, BigInt64Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Float16Array CreateCopy(Memory<Half> source)
            => Create<Half, Float16Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Float32Array CreateCopy(Memory<float> source)
            => Create<float, Float32Array>(source, true);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static Float64Array CreateCopy(Memory<double> source)
            => Create<double, Float64Array>(source, true);
        #endregion
        #region Create-ReadOnlyMemory
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<byte, Uint8Array> Create(ReadOnlyMemory<byte> source, bool copy = false)
            => Create<byte, Uint8Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<ushort, Uint16Array> Create(ReadOnlyMemory<ushort> source, bool copy = false)
            => Create<ushort, Uint16Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<uint, Uint32Array> Create(ReadOnlyMemory<uint> source, bool copy = false)
            => Create<uint, Uint32Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<ulong, BigUint64Array> Create(ReadOnlyMemory<ulong> source, bool copy = false)
            => Create<ulong, BigUint64Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<sbyte, Int8Array> Create(ReadOnlyMemory<sbyte> source, bool copy = false)
            => Create<sbyte, Int8Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<short, Int16Array> Create(ReadOnlyMemory<short> source, bool copy = false)
            => Create<short, Int16Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<int, Int32Array> Create(ReadOnlyMemory<int> source, bool copy = false)
            => Create<int, Int32Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<long, BigInt64Array> Create(ReadOnlyMemory<long> source, bool copy = false)
            => Create<long, BigInt64Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<Half, Float16Array> Create(ReadOnlyMemory<Half> source, bool copy = false)
            => Create<Half, Float16Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<float, Float32Array> Create(ReadOnlyMemory<float> source, bool copy = false)
            => Create<float, Float32Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<double, Float64Array> Create(ReadOnlyMemory<double> source, bool copy = false)
            => Create<double, Float64Array>(source, copy);
        #endregion
        #region Create-Memory
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<byte, Uint8Array> Create(Memory<byte> source, bool copy = false)
            => Create<byte, Uint8Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<ushort, Uint16Array> Create(Memory<ushort> source, bool copy = false)
            => Create<ushort, Uint16Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<uint, Uint32Array> Create(Memory<uint> source, bool copy = false)
            => Create<uint, Uint32Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<ulong, BigUint64Array> Create(Memory<ulong> source, bool copy = false)
            => Create<ulong, BigUint64Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<sbyte, Int8Array> Create(Memory<sbyte> source, bool copy = false)
            => Create<sbyte, Int8Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<short, Int16Array> Create(Memory<short> source, bool copy = false)
            => Create<short, Int16Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<int, Int32Array> Create(Memory<int> source, bool copy = false)
            => Create<int, Int32Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<long, BigInt64Array> Create(Memory<long> source, bool copy = false)
            => Create<long, BigInt64Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<Half, Float16Array> Create(Memory<Half> source, bool copy = false)
            => Create<Half, Float16Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<float, Float32Array> Create(Memory<float> source, bool copy = false)
            => Create<float, Float32Array>(source, copy);
        /// <summary>
        /// Create a HeapView
        /// </summary>
        public static HeapView<double, Float64Array> Create(Memory<double> source, bool copy = false)
            => Create<double, Float64Array>(source, copy);
        #endregion

        /// <summary>
        /// Create a copy or live view of Memory as a TypedArray, DataView, ArrayBuffer, or SharedArrayBuffer adn return the HeapView
        /// </summary>
        public static HeapView<TElement, TTypedArray> Create<TElement, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTypedArray>(ReadOnlyMemory<TElement> source, bool copy = false)
            where TTypedArray : SpawnJSObject where TElement : struct
            => new HeapView<TElement, TTypedArray>(source, copy);
        /// <summary>
        /// Create a copy or live view of Memory as a TypedArray, DataView, ArrayBuffer, or SharedArrayBuffer adn return the HeapView
        /// </summary>
        public static HeapView<TElement, TTypedArray> Create<TElement, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTypedArray>(Memory<TElement> source, bool copy = false)
            where TTypedArray : SpawnJSObject where TElement : struct
            => new HeapView<TElement, TTypedArray>(source, copy);
        /// <summary>
        /// Create a copy of Memory as a TypedArray, DataView, ArrayBuffer, or SharedArrayBuffer and reutrn the JS view
        /// </summary>
        public static TTypedArray CreateCopy<TElement, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTypedArray>(Memory<TElement> source)
            where TTypedArray : SpawnJSObject
            where TElement : struct
        {
            var view = new HeapView<TElement, TTypedArray>(source, true);
            return view.TakeViewAndDispose();
        }
        /// <summary>
        /// Create a copy of Memory as a TypedArray, DataView, ArrayBuffer, or SharedArrayBuffer and reutrn the JS view
        /// </summary>
        public static TTypedArray CreateCopy<TElement, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTypedArray>(ReadOnlyMemory<TElement> source)
            where TTypedArray : SpawnJSObject
            where TElement : struct
        {
            var view = new HeapView<TElement, TTypedArray>(source, true);
            return view.TakeViewAndDispose();
        }
        public abstract void Dispose();
    }
    public class HeapView<TElement, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TView> : HeapView, IDisposable
        where TView : SpawnJSObject
        where TElement : struct
    {
        public static implicit operator TView(HeapView<TElement, TView> value) => value.View;
        static SpawnJSRuntime JS => SpawnJSRuntime.Instance;
        private static readonly Dictionary<Type, JSArrayBufferView> JSArrayBufferViewTypes = new()
        {
            // big
            { typeof(BigInt64Array),  JSArrayBufferView.BigInt64Array },
            { typeof(BigUint64Array),  JSArrayBufferView.BigUint64Array },
            // float
            { typeof(Float16Array),  JSArrayBufferView.Float16Array },
            { typeof(Float32Array),  JSArrayBufferView.Float32Array },
            { typeof(Float64Array),  JSArrayBufferView.Float64Array },
            // int
            { typeof(Int16Array),  JSArrayBufferView.Int16Array },
            { typeof(Int32Array),  JSArrayBufferView.Int32Array },
            { typeof(Int8Array),  JSArrayBufferView.Int8Array },
            // uint
            { typeof(Uint16Array),  JSArrayBufferView.Uint16Array },
            { typeof(Uint32Array),  JSArrayBufferView.Uint32Array },
            { typeof(Uint8Array),  JSArrayBufferView.Uint8Array },
            { typeof(Uint8ClampedArray),  JSArrayBufferView.Uint8ClampedArray },
            // data view
            { typeof(DataView),  JSArrayBufferView.DataView },
            // non-view
            { typeof(ArrayBuffer),  JSArrayBufferView.ArrayBuffer },
            { typeof(SharedArrayBuffer),  JSArrayBufferView.SharedArrayBuffer },
        };
        internal override SpawnJSObject _View => View;
        /// <summary>
        /// The size of one <typeparamref name="TView"/> element in bytes.<br/>
        /// DataView, ArrayBuffer and SharedArrayBuffer are not element-typed and are measured in bytes,
        /// which <see cref="TypedArray.GetTypedArrayElementSize(Type)"/> reports as 0 - so they size at 1.
        /// </summary>
        static readonly int TargetElementSize = TypedArray.GetTypedArrayElementSize(typeof(TView)) is int size && size > 0 ? size : 1;
        /// <summary>
        /// The length to hand the Javascript side.<br/>
        /// A TypedArray constructor takes a count of ELEMENTS, while DataView, ArrayBuffer and
        /// SharedArrayBuffer take BYTES. Passing the byte length for a typed view builds a view
        /// TargetElementSize times too long: it is still a well formed TypedArray, so nothing complains
        /// until its tail is read (garbage past the end of the source) or the extra length runs off the
        /// end of the heap and throws "RangeError: offset is out of bounds".<br/>
        /// Note this is the TARGET view's element size, not <typeparamref name="TElement"/>'s - a
        /// HeapView&lt;double, Uint8Array&gt; is a byte view of doubles and is ByteLength elements long.
        /// </summary>
        long ViewLength => ByteLength / TargetElementSize;
        /// <summary>
        /// The created view
        /// </summary>
        public TView View { get; private set; }
        /// <summary>
        /// Returns true if the view has been taken
        /// </summary>
        private bool _disposeView = true;
        /// <summary>
        /// Returns the view and unpin the data
        /// </summary>
        /// <returns></returns>
        public TView TakeViewAndDispose()
        {
            if (!Copy) throw new NotImplementedException("TakeViewAndDispose only works only heap view copies");
            _disposeView = false;
            Dispose();
            return View;
        }
        /// <summary>
        /// The source Memory object
        /// </summary>
        private Memory<TElement>? _memorySource { get; set; }
        private ReadOnlyMemory<TElement>? _memoryReadOnlySource { get; set; }
        private MemoryHandle? _memoryHandle;

        GCHandle? _stringHandle;
        /// <summary>
        /// Creates a fresh view copy (no-op if Copy == false)
        /// </summary>
        public override void RefreshCopy()
        {
            if (!Copy) return;
            if (_stringHandle != null)
            {
                // no point in refreshing string copies as the data does not change (they are immutable)
                return;
            }
            // pin, get pointer, copy to JS, unpin
            if (_memorySource != null)
            {
                _memoryHandle = _memorySource.Value.Pin();
                unsafe
                {
                    Pointer = new nint(_memoryHandle.Value.Pointer);
                }
            }
            else if (_memoryReadOnlySource != null)
            {
                _memoryHandle = _memoryReadOnlySource.Value.Pin();
                unsafe
                {
                    Pointer = new nint(_memoryHandle.Value.Pointer);
                }
            }
            View.Dispose();
            View = JS.ReturnAs<HeapViewDescriptor, TView>(new HeapViewDescriptor(Pointer, ViewLength, ViewType, Copy));
            ReleaseHandle();
        }
        public HeapView(IntPtr source, long elementCount, bool copy = false)
        {
            Copy = copy;
            Pointer = source;
            ElementCount = elementCount;
            ByteLength = elementCount * Unsafe.SizeOf<TElement>();
            ViewType = JSArrayBufferViewTypes.TryGetValue(typeof(TView), out var viewFn) ? viewFn : throw new NotImplementedException($"Unsupported view type: {typeof(TView).Name}");
            View = JS.ReturnAs<HeapViewDescriptor, TView>(new HeapViewDescriptor(Pointer, ViewLength, ViewType, Copy));
        }
        public HeapView(string source)
        {
            Copy = true;
            _stringHandle = GCHandle.Alloc(source, GCHandleType.Pinned);
            // AddrOfPinnedObject on a pinned string is the address of its first character. Without this
            // Pointer stays IntPtr.Zero and the view spans the START of the heap instead of the string.
            Pointer = _stringHandle.Value.AddrOfPinnedObject();
            ElementCount = source.Length;
            ByteLength = ElementCount * Unsafe.SizeOf<TElement>();
            ViewType = JSArrayBufferViewTypes.TryGetValue(typeof(TView), out var viewFn) ? viewFn : throw new NotImplementedException($"Unsupported view type: {typeof(TView).Name}");
            View = JS.ReturnAs<HeapViewDescriptor, TView>(new HeapViewDescriptor(Pointer, ViewLength, ViewType, Copy));
            if (Copy)
            {
                ReleaseHandle();
            }
        }
        void ReleaseHandle()
        {
            _memoryHandle?.Dispose();
            _stringHandle?.Free();
        }
        public HeapView(ReadOnlyMemory<TElement> source, bool copy = false)
        {
            Copy = copy;
            _memoryReadOnlySource = source;
            _memoryHandle = _memoryReadOnlySource.Value.Pin();
            unsafe
            {
                Pointer = new nint(_memoryHandle.Value.Pointer);
            }
            ElementCount = _memorySource.Value.Length;
            ByteLength = ElementCount * Unsafe.SizeOf<TElement>();
            ViewType = JSArrayBufferViewTypes.TryGetValue(typeof(TView), out var viewFn) ? viewFn : throw new NotImplementedException($"Unsupported view type: {typeof(TView).Name}");
            View = JS.ReturnAs<HeapViewDescriptor, TView>(new HeapViewDescriptor(Pointer, ViewLength, ViewType, Copy));
            if (Copy)
            {
                // if this is a copy view we can release the pinned data
                ReleaseHandle();
            }
        }
        public HeapView(Memory<TElement> source, bool copy = false)
        {
            Copy = copy;
            _memorySource = source;
            _memoryHandle = _memorySource.Value.Pin();
            unsafe
            {
                Pointer = new nint(_memoryHandle.Value.Pointer);
            }
            ElementCount = _memorySource.Value.Length;
            ByteLength = ElementCount * Unsafe.SizeOf<TElement>();
            ViewType = JSArrayBufferViewTypes.TryGetValue(typeof(TView), out var viewFn) ? viewFn : throw new NotImplementedException($"Unsupported view type: {typeof(TView).Name}");
            View = JS.ReturnAs<HeapViewDescriptor, TView>(new HeapViewDescriptor(Pointer, ViewLength, ViewType, Copy));
            if (Copy)
            {
                // if this is a copy view we can release the pinned data
                ReleaseHandle();
            }
        }
        private void Dispose(bool dispose)
        {
            if (IsDisposed) return;
            IsDisposed = true;
            Pointer = IntPtr.Zero;
            ReleaseHandle();
            if (_disposeView) View.Dispose();
        }
        public override void Dispose()
        {
            if (IsDisposed) return;
            // Do NOT set IsDisposed here: Dispose(bool) guards on it and would short-circuit,
            // so setting it first made this method free NOTHING - the View (a TypedArray slot)
            // stayed held and ReleaseHandle() never ran, leaving the pinned MemoryHandle/GCHandle
            // pinned for the life of the app. Dispose(bool) owns the flag.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~HeapView()
        {
            Dispose(false);
            if (SpawnJSRuntime.EnableIDisposableWatcher && _creationStackTrace != null) SpawnJSRuntime.IDisposableFinalizerAlert(this, _creationStackTrace);
        }
        private readonly string? _creationStackTrace = SpawnJSRuntime.EnableIDisposableWatcher ? new StackTrace(true).ToString() : null;
    }
}
