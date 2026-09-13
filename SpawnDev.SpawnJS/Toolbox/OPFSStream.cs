using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.WebWorkers.OPFS;

namespace SpawnDev.SpawnJS.Toolbox
{
    /// <summary>
    /// Access OPFS asynchronously using the synchronous API from Window, DedicatedWorker, and SharedWorker scopes.<br/>
    /// If running in a DedicatedWorkerGlobalScope synschronous Stream access will also be available.
    /// </summary>
    public class OPFSStream : JSStreamBase
    {
        /// <summary>
        /// IF true, the Stream is accessing the file directly on disk, not a copy, in-memory or otherwise.
        /// </summary>
        public bool InPlace { get; private set; }
        /// <summary>
        /// Returns true if sync access is supported
        /// </summary>
        public static bool SyncSupported => JS?.IsDedicatedWorkerGlobalScope == true;
        private static SpawnJSRuntime? JS => SpawnJSRuntime.Instance;
        /// <inheritdoc/>
        public override bool CanWriteSync => CanWrite && (_writeStream?.CanWriteSync ?? false);
        /// <inheritdoc/>
        public override bool CanReadSync => CanRead && (_readStream?.CanReadSync ?? false);
        /// <inheritdoc/>
        public override bool CanRead => _canRead;
        private bool _canRead = false;
        /// <inheritdoc/>
        public override bool CanWrite => _canWrite;
        private bool _canWrite = false;
        /// <inheritdoc/>
        public override bool CanSeek => _canSeek;
        private bool _canSeek = true;
        /// <inheritdoc/>
        public override long Length => _stream?.Length ?? 0;
        /// <inheritdoc/>
        public override long Position { get => _stream?.Position ?? 0; set => Seek(value, SeekOrigin.Begin); }
        public bool IsOpen => _stream != null;
        /// <summary>
        /// The stream being used
        /// </summary>
        Stream? _stream = null;
        IJSWriteStream? _writeStream => _stream as IJSWriteStream;
        IJSReadStream? _readStream => _stream as IJSReadStream;
        private OPFSStream() { }
        /// <summary>
        /// Open FileSystemFileHandle as a Stream by path
        /// </summary>
        /// <param name="root">Root directory handle</param>
        /// <param name="path">The file entry name to open</param>
        /// <param name="fileMode">FileMode</param>
        /// <param name="fileAccess">FileAccess</param>
        /// <param name="syncMode"></param>
        /// <param name="cancellationToken">FileAccess</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static async Task<OPFSStream> OpenPath(FileSystemDirectoryHandle root, string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, OPFSSyncMode syncMode = OPFSSyncMode.Auto, CancellationToken cancellationToken = default)
        {
            OPFSStream? ret = null;
            try
            {
                ret = new OPFSStream();
                await ret.OpenPathInternal(root, path, fileMode, fileAccess, syncMode, cancellationToken);
            }
            catch
            {
                ret?.Dispose();
                throw;
            }
            return ret;
        }
        /// <summary>
        /// Open FileSystemFileHandle as a Stream by path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="syncMode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<OPFSStream> OpenPath(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, OPFSSyncMode syncMode = OPFSSyncMode.Auto, CancellationToken cancellationToken = default)
        {
            using var navigator = JS!.Get<Navigator>("navigator");
            using var root = await navigator.Storage.GetDirectory();
            return await OpenPath(root, path, fileMode, fileAccess, syncMode, cancellationToken);
        }
        /// <summary>
        /// Open FileSystemFileHandle as a Stream by name
        /// </summary>
        /// <param name="root">Root directory handle</param>
        /// <param name="name">The file entry name to open</param>
        /// <param name="fileMode">FileMode</param>
        /// <param name="fileAccess">FileAccess</param>
        /// <param name="syncMode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<OPFSStream> Open(FileSystemDirectoryHandle root, string name, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, OPFSSyncMode syncMode = OPFSSyncMode.Auto, CancellationToken cancellationToken = default)
        {
            OPFSStream? ret = null;
            try
            {
                ret = new OPFSStream();
                await ret.OpenNameInternal(root, name, fileMode, fileAccess, syncMode, cancellationToken);
            }
            catch
            {
                ret?.Dispose();
                throw;
            }
            return ret;
        }
        /// <summary>
        /// Open FileSystemFileHandle as a Stream by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="syncMode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<OPFSStream> Open(string name, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, OPFSSyncMode syncMode = OPFSSyncMode.Auto, CancellationToken cancellationToken = default)
        {
            using var navigator = JS!.Get<Navigator>("navigator");
            using var root = await navigator.Storage.GetDirectory();
            return await Open(root, name, fileMode, fileAccess, syncMode, cancellationToken);
        }
        /// <summary>
        /// Open a FileSystemFileHandle as a Stream
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="syncMode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static async Task<OPFSStream> Open(FileSystemFileHandle fileHandle, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read, OPFSSyncMode syncMode = OPFSSyncMode.Auto, CancellationToken cancellationToken = default)
        {
            OPFSStream? ret = null;
            try
            {
                ret = new OPFSStream();
                await ret.OpenInternal(fileHandle, fileMode, fileAccess, syncMode, cancellationToken);
            }
            catch
            {
                ret?.Dispose();
                throw;
            }
            return ret;
        }
        private async Task OpenPathInternal(FileSystemDirectoryHandle root, string path, FileMode fileMode, FileAccess fileAccess, OPFSSyncMode syncMode, CancellationToken cancellationToken)
        {
            switch (fileAccess)
            {
                case FileAccess.Read:
                    _canRead = true;
                    _canWrite = false;
                    break;
                case FileAccess.Write:
                    _canRead = false;
                    _canWrite = true;
                    break;
                case FileAccess.ReadWrite:
                    _canRead = true;
                    _canWrite = true;
                    break;
            }
            var truncate = false;
            var seekToEnd = false;
            var fileHandle = await root.GetPathFileHandle(path, false);
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
                    if (fileHandle == null)
                    {
                        fileHandle = await root.GetPathFileHandle(path, true);
                    }
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
                    truncate = true;
                    break;
                case FileMode.Append:
                    // Opens the file if it exists and seeks to the end.  Otherwise,
                    // creates a new file.
                    if (fileHandle == null)
                    {
                        fileHandle = await root.GetPathFileHandle(path, true);
                    }
                    seekToEnd = true;
                    break;
            }
            if (fileHandle == null)
            {
                throw new FileNotFoundException();
            }
            if (JS?.IsDedicatedWorkerGlobalScope == true && syncMode != OPFSSyncMode.Disabled)
            {
                try
                {
                    _stream = await OPFSyncStream.Open(fileHandle, fileMode, fileAccess, cancellationToken);
                    InPlace = true;
                }
                catch
                {
                    // continue (fallback to async)
                }
            }
            if (_stream == null)
            {
                if (syncMode == OPFSSyncMode.RequiredOnDisk)
                {
                    throw new NotSupportedException($"{nameof(OPFSSyncMode.RequiredOnDisk)} failed");
                }
                if (syncMode == OPFSSyncMode.Required)
                {
                    if (fileAccess == FileAccess.Read)
                    {
                        var arrayBuffer = await fileHandle.ReadArrayBuffer();
                        _stream = new ArrayBufferStream(arrayBuffer);
                    } 
                    else
                    {
                        throw new NotSupportedException($"{nameof(OPFSSyncMode.Required)} failed");
                    }
                }
            }
            if (_stream == null)
            {
                switch (fileAccess)
                {
                    case FileAccess.Read:
                        _stream = await fileHandle.ReadBlobStream();
                        InPlace = true;
                        break;
                    case FileAccess.Write:
                        _stream = await FileSystemHandleWritableStream.Create(fileHandle, seekToEnd, truncate);
                        break;
                    default:
                        throw new NotSupportedException($"{nameof(OPFSStream)} only supports Read and Write access. ReadWrite access is not supported.");
                }
            }
            _stream!.Position = seekToEnd ? Length : 0;
        }
        private async Task OpenNameInternal(FileSystemDirectoryHandle root, string name, FileMode fileMode, FileAccess fileAccess, OPFSSyncMode syncMode, CancellationToken cancellationToken)
        {
            switch (fileAccess)
            {
                case FileAccess.Read:
                    _canRead = true;
                    _canWrite = false;
                    break;
                case FileAccess.Write:
                    _canRead = false;
                    _canWrite = true;
                    break;
                case FileAccess.ReadWrite:
                    _canRead = true;
                    _canWrite = true;
                    break;
            }
            var truncate = false;
            var seekToEnd = false;
            FileSystemFileHandle? fileHandle = null;
            try
            {
                fileHandle = await root.GetFileHandle(name, false);
            }
            catch { }
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
                    if (fileHandle == null)
                    {
                        fileHandle = await root.GetFileHandle(name, true);
                    }
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
                    truncate = true;
                    break;
                case FileMode.Append:
                    // Opens the file if it exists and seeks to the end.  Otherwise,
                    // creates a new file.
                    if (fileHandle == null)
                    {
                        fileHandle = await root.GetFileHandle(name, true);
                    }
                    seekToEnd = true;
                    break;
            }
            if (fileHandle == null)
            {
                throw new FileNotFoundException();
            }
            if (JS?.IsDedicatedWorkerGlobalScope == true && syncMode != OPFSSyncMode.Disabled)
            {
                try
                {
                    _stream = await OPFSyncStream.Open(fileHandle, fileMode, fileAccess, cancellationToken);
                    InPlace = true;
                }
                catch
                {
                    // continue (fallback to async)
                }
            }
            if (_stream == null)
            {
                if (syncMode == OPFSSyncMode.RequiredOnDisk)
                {
                    throw new NotSupportedException($"{nameof(OPFSSyncMode.RequiredOnDisk)} failed");
                }
                if (syncMode == OPFSSyncMode.Required)
                {
                    if (fileAccess == FileAccess.Read)
                    {
                        var arrayBuffer = await fileHandle.ReadArrayBuffer();
                        _stream = new ArrayBufferStream(arrayBuffer);
                    }
                    else
                    {
                        throw new NotSupportedException($"{nameof(OPFSSyncMode.Required)} failed");
                    }
                }
            }
            if (_stream == null)
            {
                switch (fileAccess)
                {
                    case FileAccess.Read:
                        _stream = await fileHandle.ReadBlobStream();
                        InPlace = true;
                        break;
                    case FileAccess.Write:
                        _stream = await FileSystemHandleWritableStream.Create(fileHandle, seekToEnd, truncate);
                        break;
                    default:
                        throw new NotSupportedException($"{nameof(OPFSStream)} only supports Read and Write access. ReadWrite access is not supported.");
                }
            }
            _stream!.Position = seekToEnd ? Length : 0;
        }
        private async Task OpenInternal(FileSystemFileHandle fileHandle, FileMode fileMode, FileAccess fileAccess, OPFSSyncMode syncMode, CancellationToken cancellationToken)
        {
            if (fileHandle == null)
            {
                throw new FileNotFoundException();
            }
            switch (fileAccess)
            {
                case FileAccess.Read:
                    _canRead = true;
                    _canWrite = false;
                    break;
                case FileAccess.Write:
                    _canRead = false;
                    _canWrite = true;
                    break;
                case FileAccess.ReadWrite:
                    _canRead = true;
                    _canWrite = true;
                    break;
            }
            var truncate = false;
            var seekToEnd = false;
            switch (fileMode)
            {
                case FileMode.CreateNew:
                    // Creates a new file. An exception is raised if the file already exists.
                    // This is being called directly on a handle that already exists, we can assume it was created new already.
                    truncate = true;
                    break;
                case FileMode.Create:
                    // Creates a new file. If the file already exists, it is overwritten.
                    truncate = true;
                    break;
                case FileMode.Open:
                    // Opens an existing file. An exception is raised if the file does not exist.
                    break;
                case FileMode.OpenOrCreate:
                    // Opens the file if it exists. Otherwise, creates a new file.
                    break;
                case FileMode.Truncate:
                    // Opens an existing file. Once opened, the file is truncated so that its
                    // size is zero bytes. The calling process must open the file with at least
                    // WRITE access. An exception is raised if the file does not exist.
                    truncate = true;
                    break;
                case FileMode.Append:
                    // Opens the file if it exists and seeks to the end.  Otherwise,
                    // creates a new file.
                    seekToEnd = true;
                    break;
            }
            if (JS?.IsDedicatedWorkerGlobalScope == true && syncMode != OPFSSyncMode.Disabled)
            {
                try
                {
                    _stream = await OPFSyncStream.Open(fileHandle, fileMode, fileAccess, cancellationToken);
                    InPlace = true;
                }
                catch
                {
                    // continue (fallback to async)
                }
            }
            if (_stream == null)
            {
                if (syncMode == OPFSSyncMode.RequiredOnDisk)
                {
                    throw new NotSupportedException($"{nameof(OPFSSyncMode.RequiredOnDisk)} failed");
                }
                if (syncMode == OPFSSyncMode.Required)
                {
                    if (fileAccess == FileAccess.Read)
                    {
                        var arrayBuffer = await fileHandle.ReadArrayBuffer();
                        _stream = new ArrayBufferStream(arrayBuffer);
                    }
                    else
                    {
                        throw new NotSupportedException($"{nameof(OPFSSyncMode.Required)} failed");
                    }
                }
            }
            if (_stream == null)
            {
                switch (fileAccess)
                {
                    case FileAccess.Read:
                        _stream = await fileHandle.ReadBlobStream();
                        InPlace = true;
                        break;
                    case FileAccess.Write:
                        _stream = await FileSystemHandleWritableStream.Create(fileHandle, seekToEnd, truncate);
                        break;
                    default:
                        throw new NotSupportedException($"{nameof(OPFSStream)} only supports Read and Write access. ReadWrite access is not supported.");
                }
            }
            _stream!.Position = seekToEnd ? Length : 0;
        }
        /// <inheritdoc/>
        public override async Task FlushAsync(CancellationToken cancellationToken = default)
        {
            await _stream!.FlushAsync();
        }
        /// <summary>
        /// Returns true if this stream has been disposed
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <inheritdoc/>
        public override async ValueTask DisposeAsync()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            if (_stream != null)
            {
                try
                {
                    await _stream.DisposeAsync();
                }
                catch { }
                _stream = null;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            Async.Run(async () =>
            {
                try
                {
                    await DisposeAsync();
                }
                catch { }
            });
        }
        /// <inheritdoc/>
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (_stream == null) throw new Exception("File not open");
            return await _stream.ReadAsync(buffer, offset, count, cancellationToken);
        }
        /// <inheritdoc/>
        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (_stream == null) throw new Exception("File not open");
            return await _stream.ReadAsync(buffer, cancellationToken);
        }
        /// <inheritdoc/>
        public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (_stream == null) throw new Exception("File not open");
            await _stream.WriteAsync(buffer, cancellationToken);
        }
        /// <inheritdoc/>
        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (_stream == null) throw new Exception("File not open");
            await _stream.WriteAsync(buffer, offset, count, cancellationToken);
        }
        /// <inheritdoc/>
        public override async Task<Uint8Array> ReadUint8ArrayAsync(int count, CancellationToken cancellationToken = default)
        {
            if (_readStream == null) throw new Exception("File not open");
            return await _readStream.ReadUint8ArrayAsync(count, cancellationToken);
        }
        /// <inheritdoc/>
        public override async Task WriteUint8ArrayAsync(Uint8Array data, CancellationToken cancellationToken = default)
        {
            if (_writeStream == null) throw new Exception("File not open");
            await _writeStream.WriteUint8ArrayAsync(data, cancellationToken);
        }
        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream == null ? 0 : _stream.Seek(offset, origin);
        }
        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            _stream?.SetLength(value);
        }
        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_writeStream?.CanWriteSync != true) throw new NotSupportedException("Sync access not available");
            _stream!.Write(buffer, offset, count);
        }
        /// <inheritdoc/>
        public override void WriteUint8Array(Uint8Array data)
        {
            if (_writeStream?.CanWriteSync != true) throw new NotSupportedException("Sync access not available");
            _writeStream.WriteUint8Array(data);
        }
        /// <inheritdoc/>
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            if (_writeStream?.CanWriteSync != true) throw new NotSupportedException("Sync access not available");
            _stream!.Write(buffer);
        }
        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_readStream?.CanReadSync != true) throw new NotSupportedException("Sync access not available");
            return _stream!.Read(buffer, offset, count);
        }
        /// <inheritdoc/>
        public override int Read(Span<byte> buffer)
        {
            if (_readStream?.CanReadSync != true) throw new NotSupportedException("Sync access not available");
            return _stream!.Read(buffer);
        }
        /// <inheritdoc/>
        public override Uint8Array ReadUint8Array(int count)
        {
            if (_readStream?.CanReadSync != true) throw new NotSupportedException("Sync access not available");
            return _readStream.ReadUint8Array(count);
        }
        /// <inheritdoc/>
        public override void Flush() => _stream?.Flush();
        /// <inheritdoc/>
        public override void Close() => _stream?.Close();
    }
}
