namespace SpawnDev.SpawnJS.SpawnJSObjects
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
    }
}
