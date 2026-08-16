namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// A view of the dotnet heap
    /// </summary>
    public readonly struct HeapViewDescriptor
    {
        /// <summary>
        /// Memory offset
        /// </summary>
        public long Offset { get; }
        /// <summary>
        /// Number of JSArrayBufferView elements
        /// </summary>
        public long Length { get; }
        /// <summary>
        /// Revive as this global type<br/>
        /// Revives as a Uint8Array by default
        /// </summary>
        public JSArrayBufferView Type { get; } = JSArrayBufferView.Uint8Array;
        /// <summary>
        /// If true the heap view wil be a copy of the heap and not a live view.<br/>
        /// </summary>
        public bool Copy { get; }
        /// <summary>
        /// New instance
        /// </summary>
        /// <param name="offset">byte offset</param>
        /// <param name="length">Number of JSArrayBufferView elements</param>
        /// <param name="copy">Copy if true and a live heap view if false</param>
        public HeapViewDescriptor(long offset, long length, bool copy = false)
        {
            Offset = offset;
            Length = length;
            Copy = copy;
        }
        /// <summary>
        /// New instance
        /// </summary>
        /// <param name="offset">byte offset</param>
        /// <param name="length">Number of JSArrayBufferView elements</param>
        /// <param name="type">ArrayBufferView type</param>
        /// <param name="copy">Copy if true and a live heap view if false</param>
        public HeapViewDescriptor(long offset, long length, JSArrayBufferView type, bool copy = false)
        {
            Offset = offset;
            Length = length;
            Type = type;
            Copy = copy;
        }
    }
}
