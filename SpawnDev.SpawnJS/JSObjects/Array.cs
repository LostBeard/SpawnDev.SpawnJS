namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Represents a Javascript Array
    /// </summary>
    public class Array : SpawnJSObject
    {
        /// <summary>
        /// Create a new instance that wraps the specified Javascript Array reference
        /// </summary>
        /// <param name="jsObject"></param>
        public Array(SpawnJSObjectReference jsObject) : base(jsObject) { }

        /// <summary>
        /// Removes the first element from the array and returns it as type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Shift<T>() => JSRef!.Call<T>("shift");

        /// <summary>
        /// Appends the value to the end of the array
        /// </summary>
        /// <param name="value"></param>
        public void Push(object value) => JSRef!.CallVoid("push", value);
    }
}
