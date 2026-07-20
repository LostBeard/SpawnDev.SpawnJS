namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Represents the Javascript Window
    /// </summary>
    public class Window : SpawnJSObject
    {
        /// <summary>
        /// Create a new instance that wraps the specified Window reference
        /// </summary>
        /// <param name="jsObject"></param>
        public Window(SpawnJSObjectReference jsObject) : base(jsObject) { }
        /// <summary>
        /// Window origin
        /// </summary>
        public string Origin => JSRef!.Get<string>("origin");
    }
}
