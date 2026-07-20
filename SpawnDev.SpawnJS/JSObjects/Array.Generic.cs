using System.Collections;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A Javascript Array whose items are read as <typeparamref name="TItem"/>.<br/>
    /// Items are marshalled one at a time on access rather than up front, so enumerating a large JS
    /// array never materialises the whole thing on the .Net side.
    /// </summary>
    /// <typeparam name="TItem">The type each item is marshalled to</typeparam>
    public class Array<TItem> : Array, IEnumerable<TItem>
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public Array(SpawnJSObjectReference jsObject) : base(jsObject) { }
        /// <summary>
        /// The number of items in the array
        /// </summary>
        public int Length => JSRef!.Get<int>("length");
        /// <summary>
        /// Reads the item at the given index
        /// </summary>
        public TItem this[int index]
        {
            get => JSRef!.Get<TItem>(index);
            set => JSRef!.Set(index, value);
        }
        /// <summary>
        /// Copies the array into a .Net array
        /// </summary>
        public TItem[] ToArray()
        {
            var length = Length;
            var ret = new TItem[length];
            for (var i = 0; i < length; i++) ret[i] = this[i];
            return ret;
        }
        /// <inheritdoc/>
        public IEnumerator<TItem> GetEnumerator()
        {
            // read length once per enumeration, then pull items lazily
            var length = Length;
            for (var i = 0; i < length; i++) yield return this[i];
        }
        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
