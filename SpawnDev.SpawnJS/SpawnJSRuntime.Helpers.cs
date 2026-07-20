using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Conveniences the wrappers reach for. These exist in SpawnDev.BlazorJS on BlazorJSRuntime, and the
    /// ported wrappers call them by the same names.
    /// </summary>
    public partial class SpawnJSRuntime
    {
        /// <summary>
        /// Hands a value to Javascript and reads it straight back as T.<br/>
        /// The point is the type change, not the round trip: it re-reads the same live Javascript object
        /// through a different marshaller, which is how a value of one wrapper type is reinterpreted as
        /// another without copying anything.
        /// </summary>
        public T ReturnMe<T>(object? obj) => obj == null ? default! : NetRun<T>("returnMe", new object?[] { obj });

        /// <summary>
        /// Hands a value to Javascript and reads it straight back as the given type
        /// </summary>
        public object? ReturnMe(Type returnType, object? obj) => obj == null ? null : NetRun(returnType, "returnMe", new object?[] { obj });

        /// <summary>
        /// Creates a Javascript TypedArray or DataView directly over .Net heap memory - a real view, not a
        /// copy, so the bytes are shared with the pinned .Net array.<br/>
        /// <paramref name="byteLength"/> must be sized by the TARGET view's element size, not by whatever
        /// the memory was pinned as: a cross type view such as HeapView&lt;double&gt;.As&lt;Uint8Array&gt;()
        /// otherwise builds an oversized view that only throws when its tail is touched.
        /// </summary>
        /// <param name="viewType">A global constructor name: "Uint8Array", "Float32Array", "DataView"...</param>
        /// <param name="address">Address within the WebAssembly heap</param>
        /// <param name="byteLength">Length in BYTES</param>
        public T CreateHeapView<T>(string viewType, long address, long byteLength)
            => NetRun<T>("heapView", new object?[] { viewType, (double)address, (double)byteLength });

        /// <summary>
        /// Creates a Javascript TypedArray or DataView directly over .Net heap memory, returning the given
        /// wrapper type. See <see cref="CreateHeapView{T}"/>.
        /// </summary>
        public object? CreateHeapView(Type returnType, string viewType, long address, long byteLength)
            => NetRun(returnType, "heapView", new object?[] { viewType, (double)address, (double)byteLength });

        /// <summary>
        /// The current Document, or null when there is no document (a worker, or a non browser host)
        /// </summary>
        public Document? GetDocument() => Get<Document?>("document");

        /// <summary>
        /// document.createElement
        /// </summary>
        /// <param name="elementType">The tag name, for example "canvas"</param>
        public SpawnJSObjectReference DocumentCreateElement(string elementType)
            => Call<SpawnJSObjectReference>("document.createElement", elementType)!;

        /// <summary>
        /// document.createElement, returning the typed wrapper
        /// </summary>
        /// <param name="elementType">The tag name, for example "canvas"</param>
        public T DocumentCreateElement<T>(string elementType) where T : Element
            => Call<T>("document.createElement", elementType);

        /// <summary>
        /// document.body.appendChild
        /// </summary>
        public void DocumentBodyAppendChild(Element element) => CallVoid("document.body.appendChild", element);
    }
}
