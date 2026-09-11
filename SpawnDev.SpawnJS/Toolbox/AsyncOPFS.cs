using SpawnDev.SpawnJS.JSObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpawnDev.SpawnJS.Toolbox
{
    public static class AsyncOPFS
    {
        static SpawnJSRuntime JS => SpawnJSRuntime.Instance;
        static StorageManager? _storage;
        static FileSystemDirectoryHandle? _root;
        static Task? _ready = null;
        static Task Ready => _ready ??= InitAsync();
        static async Task InitAsync()
        {
            if (OperatingSystem.IsBrowser())
            {
                using var navigator = JS!.Get<Navigator>("navigator");
                _storage = navigator?.Storage;
                if (_storage != null)
                {
                    _root = await _storage.GetDirectory();
                }
            }
        }
        public static async Task<Stream> OpenPathAsyncAccess(this FileSystemDirectoryHandle root, string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, CancellationToken cancellationToken = default)
        {
            if (!OperatingSystem.IsBrowser()) throw new PlatformNotSupportedException();
            var fileHandle = await root.GetPathFileHandle(path);
            var truncate = false;
            var seekToEnd = false;
            switch (fileMode)
            {
                case FileMode.CreateNew:
                    // Creates a new file. An exception is raised if the file already exists.
                    if (fileHandle != null)
                    {
                        fileHandle.Dispose();
                        throw new Exception($"Failed: {fileMode}. Already exists.");
                    }
                    fileHandle = await root.GetPathFileHandle(path, true);
                    break;
                case FileMode.Create:
                    // Creates a new file. If the file already exists, it is overwritten.
                    truncate = true;
                    if (fileHandle != null)
                    {
                        await root.RemovePath(path);
                        fileHandle.Dispose();
                    }
                    fileHandle = await root.GetPathFileHandle(path, true);
                    break;
                case FileMode.Open:
                    // Opens an existing file. An exception is raised if the file does not exist.
                    if (fileHandle == null)
                    {
                        throw new FileNotFoundException();
                    }
                    break;
                case FileMode.OpenOrCreate:
                    // Opens the file if it exists. Otherwise, creates a new file.
                    if (fileHandle == null)
                    {
                        fileHandle = await root.GetPathFileHandle(path, true);
                    }
                    break;
                case FileMode.Truncate:
                    // Opens an existing file. Once opened, the file is truncated so that its
                    // size is zero bytes. The calling process must open the file with at least
                    // WRITE access. An exception is raised if the file does not exist.
                    if (fileHandle == null)
                    {
                        throw new FileNotFoundException();
                    }
                    await root.RemovePath(path);
                    fileHandle.Dispose();
                    fileHandle = await root.GetPathFileHandle(path, true);
                    truncate = true;
                    break;
                case FileMode.Append:
                    // Opens the file if it exists and seeks to the end.  Otherwise,
                    // creates a new file.
                    seekToEnd = true;
                    break;
            }
            if (fileHandle == null)
            {
                throw new FileNotFoundException();
            }
            switch (fileAccess)
            {
                case FileAccess.Read:
                    {
                        var stream = await fileHandle.ReadBlobStream();
                        stream.Position = seekToEnd ? stream.Length : 0;
                        return stream;
                    }
                case FileAccess.Write:
                    {
                        var stream = await fileHandle.GetWritableStream(!truncate);
                        stream.Position = seekToEnd ? stream.Length : 0;
                        return stream;
                    }
                default: throw new NotImplementedException();
            }
        }
        public static async Task<Stream> OpenPathAsyncAccess(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, CancellationToken cancellationToken = default)
        {
            if (OperatingSystem.IsBrowser())
            {
                await Ready;
                if (_root == null) throw new PlatformNotSupportedException();
                return await OpenPathAsyncAccess(_root, path, fileMode, fileAccess, cancellationToken);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
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
        public static async Task<Stream> OpenAsyncAccess(this FileSystemDirectoryHandle root, string name, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, CancellationToken cancellationToken = default)
        {
            if (!OperatingSystem.IsBrowser()) throw new PlatformNotSupportedException();
            var fileHandle = await root.GetFileHandle(name);
            var truncate = false;
            var seekToEnd = false;
            switch (fileMode)
            {
                case FileMode.CreateNew:
                    // Creates a new file. An exception is raised if the file already exists.
                    if (fileHandle != null)
                    {
                        fileHandle.Dispose();
                        throw new Exception($"Failed: {fileMode}. Already exists.");
                    }
                    fileHandle = await root.GetFileHandle(name, true);
                    break;
                case FileMode.Create:
                    // Creates a new file. If the file already exists, it is overwritten.
                    truncate = true;
                    if (fileHandle != null)
                    {
                        await root.RemoveEntry(name);
                        fileHandle.Dispose();
                    }
                    fileHandle = await root.GetFileHandle(name, true);
                    break;
                case FileMode.Open:
                    // Opens an existing file. An exception is raised if the file does not exist.
                    if (fileHandle == null)
                    {
                        throw new FileNotFoundException();
                    }
                    break;
                case FileMode.OpenOrCreate:
                    // Opens the file if it exists. Otherwise, creates a new file.
                    if (fileHandle == null)
                    {
                        fileHandle = await root.GetFileHandle(name, true);
                    }
                    break;
                case FileMode.Truncate:
                    // Opens an existing file. Once opened, the file is truncated so that its
                    // size is zero bytes. The calling process must open the file with at least
                    // WRITE access. An exception is raised if the file does not exist.
                    if (fileHandle == null)
                    {
                        throw new FileNotFoundException();
                    }
                    await root.RemoveEntry(name);
                    fileHandle.Dispose();
                    fileHandle = await root.GetFileHandle(name, true);
                    truncate = true;
                    break;
                case FileMode.Append:
                    // Opens the file if it exists and seeks to the end.  Otherwise,
                    // creates a new file.
                    seekToEnd = true;
                    break;
            }
            if (fileHandle == null)
            {
                throw new FileNotFoundException();
            }
            switch (fileAccess)
            {
                case FileAccess.Read:
                    {
                        var stream = await fileHandle.ReadBlobStream();
                        stream.Position = seekToEnd ? stream.Length : 0;
                        return stream;
                    }
                case FileAccess.Write:
                    {
                        var stream = await fileHandle.GetWritableStream(!truncate);
                        stream.Position = seekToEnd ? stream.Length : 0;
                        return stream;
                    }
                default: throw new NotImplementedException();
            }
        }
        public static async Task<Stream> OpenAsyncAccess(string name, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, CancellationToken cancellationToken = default)
        {
            if (!OperatingSystem.IsBrowser()) throw new PlatformNotSupportedException();
            await Ready;
            if (_root == null) throw new PlatformNotSupportedException();
            return await OpenAsyncAccess(_root, name, fileMode, fileAccess, cancellationToken);
        }
        public static async Task<Stream> OpenAsyncAccess(this FileSystemFileHandle fileHandle, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, CancellationToken cancellationToken = default)
        {
            var truncate = false;
            var seekToEnd = false;
            switch (fileMode)
            {
                case FileMode.CreateNew:
                    // Creates a new file. An exception is raised if the file already exists.
                    if (fileHandle != null)
                    {
                        throw new NotSupportedException("File already exists");
                    }
                    throw new NotSupportedException("Cannot create file handle");
                case FileMode.Create:
                    // Creates a new file. If the file already exists, it is overwritten.
                    truncate = true;
                    break;
                case FileMode.Open:
                    // Opens an existing file. An exception is raised if the file does not exist.
                    if (fileHandle == null)
                    {
                        throw new FileNotFoundException();
                    }
                    break;
                case FileMode.OpenOrCreate:
                    // Opens the file if it exists. Otherwise, creates a new file.
                    break;
                case FileMode.Truncate:
                    // Opens an existing file. Once opened, the file is truncated so that its
                    // size is zero bytes. The calling process must open the file with at least
                    // WRITE access. An exception is raised if the file does not exist.
                    if (fileHandle == null)
                    {
                        throw new FileNotFoundException();
                    }
                    truncate = true;
                    break;
                case FileMode.Append:
                    // Opens the file if it exists and seeks to the end.  Otherwise,
                    // creates a new file.
                    seekToEnd = true;
                    break;
            }
            if (fileHandle == null)
            {
                throw new FileNotFoundException();
            }
            switch (fileAccess)
            {
                case FileAccess.Read:
                    {
                        var stream = await fileHandle.ReadBlobStream();
                        stream.Position = seekToEnd ? stream.Length : 0;
                        return stream;
                    }
                case FileAccess.Write:
                    {
                        var stream = await fileHandle.GetWritableStream(!truncate);
                        stream.Position = seekToEnd ? stream.Length : 0;
                        return stream;
                    }
                default: throw new NotImplementedException();
            }
        }
    }
}
