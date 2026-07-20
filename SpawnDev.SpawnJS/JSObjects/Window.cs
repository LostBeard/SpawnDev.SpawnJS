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
        /// <summary>
        /// Shows a directory picker and returns the chosen directory, or null if the user cancelled.<br/>
        /// https://developer.mozilla.org/en-US/docs/Web/API/Window/showDirectoryPicker
        /// </summary>
        public Task<FileSystemDirectoryHandle?> ShowDirectoryPicker(ShowDirectoryPickerOptions options)
            => JSRef!.CallAsync<FileSystemDirectoryHandle?>("showDirectoryPicker", options);
        /// <summary>
        /// Shows a directory picker and returns the chosen directory, or null if the user cancelled
        /// </summary>
        public Task<FileSystemDirectoryHandle?> ShowDirectoryPicker()
            => JSRef!.CallAsync<FileSystemDirectoryHandle?>("showDirectoryPicker");
    }
}
