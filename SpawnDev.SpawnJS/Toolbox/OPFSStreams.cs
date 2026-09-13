using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS.Toolbox
{
    public static class OPFSStreams
    {
        public static async Task<Stream> OpenPathAsyncAccess(this FileSystemDirectoryHandle root, string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, CancellationToken cancellationToken = default)
        {
            return await OPFSStream.OpenPath(root, path, fileMode, fileAccess, OPFSSyncMode.Auto, cancellationToken);
        }
        public static async Task<Stream> OpenPathAsyncAccess(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, CancellationToken cancellationToken = default)
        {
            return await OPFSStream.OpenPath(path, fileMode, fileAccess, OPFSSyncMode.Auto, cancellationToken);
        }
        public static async Task<Stream> OpenAsyncAccess(this FileSystemDirectoryHandle root, string name, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, CancellationToken cancellationToken = default)
        {
            return await OPFSStream.Open(root, name, fileMode, fileAccess, OPFSSyncMode.Auto, cancellationToken);
        }
        public static async Task<Stream> OpenAsyncAccess(string name, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, CancellationToken cancellationToken = default)
        {
            return await OPFSStream.Open(name, fileMode, fileAccess, OPFSSyncMode.Auto, cancellationToken);
        }
        public static async Task<Stream> OpenAsyncAccess(this FileSystemFileHandle fileHandle, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, CancellationToken cancellationToken = default)
        {
            return await OPFSStream.Open(fileHandle, fileMode, fileAccess, OPFSSyncMode.Auto, cancellationToken);
        }
        public static async Task<Stream> OpenPathNative(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, CancellationToken cancellationToken = default)
        {
            var ret = new FileStream(path, new FileStreamOptions
            {
                Options = System.IO.FileOptions.Asynchronous,
                Mode = fileMode,
                Access = fileAccess,
            });
            return ret;
        }
    }
}
