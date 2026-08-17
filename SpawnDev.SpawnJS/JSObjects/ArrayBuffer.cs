using System.Diagnostics.CodeAnalysis;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ArrayBuffer object is used to represent a generic raw binary data buffer.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/ArrayBuffer
    /// </summary>
    [Transferable]
    public class ArrayBuffer : SpawnJSObject
    {
        /// <summary>
        /// Returns true if the Javascript object is a SharedArrayBuffer,<br/>
        /// This property aids in using a SharedArrayBuffer as an ArrayBuffer for APIs that may only have methods for ArrayBuffer but can use a SharedArrayBuffer.<br/>
        /// </summary>
        public bool IsSharedArrayBuffer => JSRef!.Get<string>("constructor.name") == nameof(SharedArrayBuffer);
        /// <summary>
        /// Returns true if the Javascript object is an ArrayBuffer.<br/>
        /// This property aids in using a SharedArrayBuffer as an ArrayBuffer for APIs that may only have methods for ArrayBuffer but can use a SharedArrayBuffer.<br/>
        /// </summary>
        public bool IsArrayBuffer => JSRef!.Get<string>("constructor.name") == nameof(ArrayBuffer);
        /// <summary>
        /// Explicit conversion to ArrayBuffer<br/>
        /// This cast uses the HeapView class for a fast copy into a new ArrayBuffer.
        /// </summary>
        /// <param name="data">Data to copy to a new ArrayBuffer</param>
        public static explicit operator ArrayBuffer(byte[] data) => HeapView.CreateCopy<byte, ArrayBuffer>(data);
        /// <summary>
        /// Explicit conversion to ArrayBuffer<br/>
        /// This cast uses the HeapView class for a fast copy into a new ArrayBuffer.
        /// </summary>
        /// <param name="data">Data to copy to a new ArrayBuffer</param>
        public static explicit operator ArrayBuffer(ushort[] data) => HeapView.CreateCopy<ushort, ArrayBuffer>(data);
        /// <summary>
        /// Explicit conversion to ArrayBuffer<br/>
        /// This cast uses the HeapView class for a fast copy into a new ArrayBuffer.
        /// </summary>
        /// <param name="data">Data to copy to a new ArrayBuffer</param>
        public static explicit operator ArrayBuffer(uint[] data) => HeapView.CreateCopy<uint, ArrayBuffer>(data);
        /// <summary>
        /// Explicit conversion to ArrayBuffer<br/>
        /// This cast uses the HeapView class for a fast copy into a new ArrayBuffer.
        /// </summary>
        /// <param name="data">Data to copy to a new ArrayBuffer</param>
        public static explicit operator ArrayBuffer(ulong[] data) => HeapView.CreateCopy<ulong, ArrayBuffer>(data);
        /// <summary>
        /// Explicit conversion to ArrayBuffer<br/>
        /// This cast uses the HeapView class for a fast copy into a new ArrayBuffer.
        /// </summary>
        /// <param name="data">Data to copy to a new ArrayBuffer</param>
        public static explicit operator ArrayBuffer(sbyte[] data) => HeapView.CreateCopy<sbyte, ArrayBuffer>(data);
        /// <summary>
        /// Explicit conversion to ArrayBuffer<br/>
        /// This cast uses the HeapView class for a fast copy into a new ArrayBuffer.
        /// </summary>
        /// <param name="data">Data to copy to a new ArrayBuffer</param>
        public static explicit operator ArrayBuffer(short[] data) => HeapView.CreateCopy<short, ArrayBuffer>(data);
        /// <summary>
        /// Explicit conversion to ArrayBuffer<br/>
        /// This cast uses the HeapView class for a fast copy into a new ArrayBuffer.
        /// </summary>
        /// <param name="data">Data to copy to a new ArrayBuffer</param>
        public static explicit operator ArrayBuffer(int[] data) => HeapView.CreateCopy<int, ArrayBuffer>(data);
        /// <summary>
        /// Explicit conversion to ArrayBuffer<br/>
        /// This cast uses the HeapView class for a fast copy into a new ArrayBuffer.
        /// </summary>
        /// <param name="data">Data to copy to a new ArrayBuffer</param>
        public static explicit operator ArrayBuffer(long[] data) => HeapView.CreateCopy<long, ArrayBuffer>(data);
        /// <summary>
        /// Explicit conversion to ArrayBuffer<br/>
        /// This cast uses the HeapView class for a fast copy into a new ArrayBuffer.
        /// </summary>
        /// <param name="data">Data to copy to a new ArrayBuffer</param>
        public static explicit operator ArrayBuffer(Half[] data) => HeapView.CreateCopy<Half, ArrayBuffer>(data);
        /// <summary>
        /// Explicit conversion to ArrayBuffer<br/>
        /// This cast uses the HeapView class for a fast copy into a new ArrayBuffer.
        /// </summary>
        /// <param name="data">Data to copy to a new ArrayBuffer</param>
        public static explicit operator ArrayBuffer(float[] data) => HeapView.CreateCopy<float, ArrayBuffer>(data);
        /// <summary>
        /// Explicit conversion to ArrayBuffer<br/>
        /// This cast uses the HeapView class for a fast copy into a new ArrayBuffer.
        /// </summary>
        /// <param name="data">Data to copy to a new ArrayBuffer</param>
        public static explicit operator ArrayBuffer(double[] data) => HeapView.CreateCopy<double, ArrayBuffer>(data);
        /// <summary>
        /// Returns a copy of the struct array as a new ArrayBuffer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static ArrayBuffer Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(T[] data, int offset = 0) where T : struct
        {
            using var heapView = HeapView.CreateCopy<T, ArrayBuffer>(new Memory<T>(data, offset, data.Length));
            return heapView;
        }
        /// <summary>
        /// Returns a copy of the struct array as a new ArrayBuffer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static ArrayBuffer Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(T[] data, int offset, int length) where T : struct
        {
            using var heapView = HeapView.CreateCopy<T, ArrayBuffer>(new Memory<T>(data, offset, length));
            return heapView;
        }
        /// <summary>
        /// Returns a copy of the Javascript ArrayBuffer as a .Net byte[]
        /// </summary>
        /// <param name="arrayBuffer"></param>
        public static explicit operator byte[]?(ArrayBuffer? arrayBuffer) => arrayBuffer?.ReadBytes();
        /// <summary>
        /// The size, in bytes, of the ArrayBuffer. This is established when the array is constructed and can only be changed using the ArrayBuffer.prototype.resize() method if the ArrayBuffer is resizable.
        /// </summary>
        public long ByteLength => JSRef!.Get<long>("byteLength");
        /// <summary>
        /// The read-only maximum length, in bytes, that the ArrayBuffer can be resized to. This is established when the array is constructed and cannot be changed.
        /// </summary>
        public long MaxByteLength => JSRef!.Get<long>("maxByteLength");
        /// <summary>
        /// Read-only. Returns true if the ArrayBuffer has been detached (transferred), or false if not.
        /// </summary>
        public bool Detached => JSRef!.Get<bool>("detached");
        /// <summary>
        /// Read-only. Returns true if the ArrayBuffer can be resized, or false if not.
        /// </summary>
        public bool Resizable => JSRef!.Get<bool>("resizable");
        /// <summary>
        /// The ArrayBuffer() constructor creates ArrayBuffer objects.
        /// </summary>
        /// <param name="length">The size, in bytes, of the array buffer to create.</param>
        public ArrayBuffer(long length) : base(JS.New(nameof(ArrayBuffer), length)) { }
        /// <summary>
        /// The ArrayBuffer() constructor creates ArrayBuffer objects.
        /// </summary>
        /// <param name="length">The size, in bytes, of the array buffer to create.</param>
        /// <param name="options">Additional options</param>
        public ArrayBuffer(long length, ArrayBufferOptions options) : base(JS.New(nameof(ArrayBuffer), length, options)) { }
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public ArrayBuffer(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Non-standard implementation. Creates a Uint8Array from the ArrayBuffer and reads it into .Net as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] ReadBytes()
        {
            using var uint8array = new Uint8Array(this);
            return uint8array.ReadBytes();
        }
        /// <summary>
        /// Returns true if arg is one of the ArrayBuffer views, such as typed array objects or a DataView. Returns false otherwise.
        /// </summary>
        /// <returns></returns>
        public static bool IsView(object value) => JS.Call<bool>("ArrayBuffer.isView");
        /// <summary>
        /// Resizes the ArrayBuffer to the specified size, in bytes.
        /// </summary>
        /// <param name="newLength"></param>
        public void Resize(long newLength) => JSRef!.CallVoid("resize", newLength);
        /// <summary>
        /// Creates a new ArrayBuffer with the same byte content as this buffer, then detaches this buffer.
        /// </summary>
        /// <param name="newByteLength"></param>
        /// <returns></returns>
        public ArrayBuffer Transfer(long newByteLength) => JSRef!.Call<long, ArrayBuffer>("transfer", newByteLength);
        /// <summary>
        /// Creates a new ArrayBuffer with the same byte content as this buffer, then detaches this buffer.
        /// </summary>
        /// <returns></returns>
        public ArrayBuffer Transfer() => JSRef!.Call<ArrayBuffer>("transfer");
        /// <summary>
        /// Returns a new ArrayBuffer whose contents are a copy of this ArrayBuffer's bytes from begin (inclusive) up to end (exclusive). If either begin or end is negative, it refers to an index from the end of the array, as opposed to from the beginning.
        /// </summary>
        /// <returns></returns>
        public ArrayBuffer Slice() => JSRef!.Call<ArrayBuffer>("slice");
        /// <summary>
        /// Returns a new ArrayBuffer whose contents are a copy of this ArrayBuffer's bytes from begin (inclusive) up to end (exclusive). If either begin or end is negative, it refers to an index from the end of the array, as opposed to from the beginning.
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public ArrayBuffer Slice(long start) => JSRef!.Call<long, ArrayBuffer>("slice", start);
        /// <summary>
        /// Returns a new ArrayBuffer whose contents are a copy of this ArrayBuffer's bytes from begin (inclusive) up to end (exclusive). If either begin or end is negative, it refers to an index from the end of the array, as opposed to from the beginning.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public ArrayBuffer Slice(long start, long end) => JSRef!.Call<long, long, ArrayBuffer>("slice", start, end);
        /// <summary>
        /// Creates a new non-resizable ArrayBuffer with the same byte content as this buffer, then detaches this buffer.
        /// </summary>
        /// <returns></returns>
        public ArrayBuffer TransferToFixedLength() => JSRef!.Call<ArrayBuffer>("transferToFixedLength");
        /// <summary>
        /// Creates a new non-resizable ArrayBuffer with the same byte content as this buffer, then detaches this buffer.
        /// </summary>
        /// <param name="newByteLength"></param>
        /// <returns></returns>
        public ArrayBuffer TransferToFixedLength(long newByteLength) => JSRef!.Call<long, ArrayBuffer>("transferToFixedLength", newByteLength);
    }
}
