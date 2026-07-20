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
