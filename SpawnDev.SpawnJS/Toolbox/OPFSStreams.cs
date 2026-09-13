using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS.Toolbox
{
    public static class OPFSStreams
    {
        public static async Task<Stream> OpenPathStream(this FileSystemDirectoryHandle root, string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, OPFSSyncMode syncMode = OPFSSyncMode.Auto, CancellationToken cancellationToken = default)
        {
            return await OPFSStream.OpenPath(root, path, fileMode, fileAccess, syncMode, cancellationToken);
        }
        public static async Task<Stream> OpenPathStream(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, OPFSSyncMode syncMode = OPFSSyncMode.Auto, CancellationToken cancellationToken = default)
        {
            return await OPFSStream.OpenPath(path, fileMode, fileAccess, syncMode, cancellationToken);
        }
        public static async Task<Stream> OpenStream(this FileSystemDirectoryHandle root, string name, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, OPFSSyncMode syncMode = OPFSSyncMode.Auto, CancellationToken cancellationToken = default)
        {
            return await OPFSStream.Open(root, name, fileMode, fileAccess, syncMode, cancellationToken);
        }
        public static async Task<Stream> OpenStream(string name, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, OPFSSyncMode syncMode = OPFSSyncMode.Auto, CancellationToken cancellationToken = default)
        {
            return await OPFSStream.Open(name, fileMode, fileAccess, syncMode, cancellationToken);
        }
        public static async Task<Stream> OpenStream(this FileSystemFileHandle fileHandle, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, OPFSSyncMode syncMode = OPFSSyncMode.Auto, CancellationToken cancellationToken = default)
        {
            return await OPFSStream.Open(fileHandle, fileMode, fileAccess, syncMode, cancellationToken);
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
