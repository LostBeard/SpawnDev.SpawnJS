namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// A SpawnJSInterop static method that a call names by index. The index is resolved from the method map once, on
    /// first use, and kept: the map only ever appends, so an index never changes. Looking the name up per call cost a
    /// dictionary lookup (~160 ns interpreted) on every interop call.
    /// </summary>
    internal sealed class InteropMethod
    {
        public static readonly InteropMethod PropertyCall = new("propertyCall");
        public static readonly InteropMethod PropertyCallApply = new("propertyCallApply");
        public static readonly InteropMethod PropertyNew = new("propertyNew");
        public static readonly InteropMethod PropertyNewApply = new("propertyNewApply");
        public static readonly InteropMethod PropertyGet = new("propertyGet");
        public static readonly InteropMethod PropertySet = new("propertySet");
        public static readonly InteropMethod PropertySetNewPromise = new("propertySetNewPromise");
        public static readonly InteropMethod PropertySetResolvedPromise = new("propertySetResolvedPromise");
        public static readonly InteropMethod PropertySetRejectedPromise = new("propertySetRejectedPromise");
        public static readonly InteropMethod WriteArrayBufferViewToHeap = new("writeArrayBufferViewToHeap");
        public static readonly InteropMethod ReturnMe = new("returnMe");
        public static readonly InteropMethod ObjectEquals = new("objectEquals");
        public static readonly InteropMethod GetHeapSize = new("getHeapSize");

        /// <summary>The SpawnJSInterop static method's name.</summary>
        public string Name { get; }
        int _index = -1;
        InteropMethod(string name) => Name = name;
        /// <summary>
        /// The method's index in <paramref name="js"/>'s method map. Statics are per .Net runtime and so is the
        /// SpawnJSRuntime, so caching it here is per instance.
        /// </summary>
        public int IndexIn(SpawnJSRuntime js) => _index >= 0 ? _index : (_index = js.InteropMethodIndex(Name));
    }
}
