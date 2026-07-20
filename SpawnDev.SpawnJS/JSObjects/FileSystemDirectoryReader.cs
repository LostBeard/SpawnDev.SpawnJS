// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The FileSystemDirectoryReader interface of the File and Directory Entries API lets you access the FileSystemFileEntry-based objects (generally FileSystemFileEntry or FileSystemDirectoryEntry) representing each entry in a directory.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/FileSystemDirectoryReader
    /// </summary>
    public class FileSystemDirectoryReader : SpawnJSObject
    {
        /// <inheritdoc />
        public FileSystemDirectoryReader(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The FileSystemDirectoryReader interface's readEntries() method retrieves the directory entries within the directory being read and delivers them in an array to a provided callback function.
        /// </summary>
        /// <param name="successCallback"></param>
        public void ReadEntries(ActionCallback<Array<FileSystemEntry>> successCallback) => JSRef!.CallVoid("readEntries", successCallback);
        /// <summary>
        /// The FileSystemDirectoryReader interface's readEntries() method retrieves the directory entries within the directory being read and delivers them in an array to a provided callback function.
        /// </summary>
        /// <param name="successCallback"></param>
        /// <param name="errorCallback"></param>
        public void ReadEntries(ActionCallback<Array<FileSystemEntry>> successCallback, ActionCallback<DOMException> errorCallback) => JSRef!.CallVoid("readEntries", successCallback, errorCallback);
        /// <summary>
        /// Returns an array containing some number of the directory's entries. Each item in the array is an object based on FileSystemEntry—typically either FileSystemFileEntry or FileSystemDirectoryEntry.
        /// </summary>
        /// <returns></returns>
        public async Task<Array<FileSystemEntry>> ReadEntriesAsync()
        {
            var tcs = new TaskCompletionSource<Array<FileSystemEntry>>();
            using var successCallback = new ActionCallback<Array<FileSystemEntry>>(tcs.SetResult);
            using var errorCallback = new ActionCallback<DOMException>((ex) => tcs.SetException(new Exception(ex.Message)));
            ReadEntries(successCallback, errorCallback);
            return await tcs.Task;
        }
    }
}
