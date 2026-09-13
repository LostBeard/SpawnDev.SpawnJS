using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;

namespace SpawnDev.SpawnJS.WebWorkers.OPFS
{
    /// <summary>
    /// In a DedicatedWorkerGlobalScope access OPFS FileSystemFileHandle as a .Net sync and async Stream using FileSystemSyncAccessHandle.<br/>
    /// If not running in a dedicated worker Open will throw an exception.<br/>
    /// Purpose:<br/>
    /// This allows in-place write access to OPFS files which is only possible via FileSystemSyncAccessHandle.
    /// </summary>
    public class OPFSyncStream : JSStreamBase
    {
        /// <summary>
        /// Returns true if this environment can use this class
        /// </summary>
        public static bool Supported => JS?.IsDedicatedWorkerGlobalScope == true;
        private static SpawnJSRuntime? JS => SpawnJSRuntime.Instance;
        /// <inheritdoc/>
        public override bool CanWriteSync => _canWriteSync;
        private bool _canWriteSync = false;
        /// <inheritdoc/>
        public override bool CanReadSync => _canReadSync;
        private bool _canReadSync = false;
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
        public override long Length => _syncAccessHandle?.GetSize() ?? 0;
        /// <inheritdoc/>
        public override long Position { get => _position; set => Seek(value, SeekOrigin.Begin); }
        private long _position = 0;
        /// <summary>
        /// Returns true of the file is open
        /// </summary>
        public bool IsOpen => _syncAccessHandle != null;
        /// <summary>
        /// Returns true if the file is open and direct sync Stream access is possible
        /// </summary>
        public bool IsSyncOpen => _syncAccessHandle != null;
        /// <summary>
        /// If _handleManager is running in this instance and not a worker this returns the sync handle which enables<br/>
        /// synchronous stream access
        /// </summary>
        private FileSystemSyncAccessHandle? _syncAccessHandle;
        private static Dictionary<string, OPFSyncStream> WorkerStreams { get; } = new Dictionary<string, OPFSyncStream>();
        private OPFSyncStream() { }
        static void ThrowIfNotSupported()
        {
            if (!Supported) throw new NotImplementedException($"{nameof(OPFSyncStream)} requires DedicatedWorkerGlobalScope");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="root">Root directory handle</param>
        /// <param name="path">The file entry name to open</param>
        /// <param name="fileMode">FileMode</param>
        /// <param name="fileAccess">FileAccess</param>
        /// <param name="cancellationToken">FileAccess</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static async Task<OPFSyncStream> OpenPath(FileSystemDirectoryHandle root, string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite, CancellationToken cancellationToken = default)
        {
            ThrowIfNotSupported();
            OPFSyncStream? ret = null;
            try
            {
                ret = new OPFSyncStream();
                await ret.OpenPathInternal(root, path, fileMode, fileAccess, cancellationToken);
            }
            catch
            {
                ret?.Dispose();
                throw;
            }
            return ret;
        }
        /// <summary>
        /// Open by path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<OPFSyncStream> OpenPath(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite, CancellationToken cancellationToken = default)
        {
            using var navigator = JS!.Get<Navigator>("navigator");
            using var root = await navigator.Storage.GetDirectory();
            return await OpenPath(root, path, fileMode, fileAccess, cancellationToken);
        }
        /// <summary>
        /// Open by name
        /// </summary>
        /// <param name="root">Root directory handle</param>
        /// <param name="name">The file entry name to open</param>
        /// <param name="fileMode">FileMode</param>
        /// <param name="fileAccess">FileAccess</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<OPFSyncStream> Open(FileSystemDirectoryHandle root, string name, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite, CancellationToken cancellationToken = default)
        {
            ThrowIfNotSupported();
            OPFSyncStream? ret = null;
            try
            {
                ret = new OPFSyncStream();
                await ret.OpenNameInternal(root, name, fileMode, fileAccess, cancellationToken);
            }
            catch
            {
                ret?.Dispose();
                throw;
            }
            return ret;
        }
        /// <summary>
        /// Open by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<OPFSyncStream> Open(string name, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite, CancellationToken cancellationToken = default)
        {
            using var navigator = JS!.Get<Navigator>("navigator");
            using var root = await navigator.Storage.GetDirectory();
            return await Open(root, name, fileMode, fileAccess, cancellationToken);
        }
        /// <summary>
        /// Open by hanlde
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static async Task<OPFSyncStream> Open(FileSystemFileHandle fileHandle, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite, CancellationToken cancellationToken = default)
        {
            ThrowIfNotSupported();
            OPFSyncStream? ret = null;
            try
            {
                ret = new OPFSyncStream();
                await ret.OpenInternal(fileHandle, fileMode, fileAccess, cancellationToken);
            }
            catch
            {
                ret?.Dispose();
                throw;
            }
            return ret;
        }
        private async Task OpenPathInternal(FileSystemDirectoryHandle root, string path, FileMode fileMode, FileAccess fileAccess, CancellationToken cancellationToken = default)
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
            _canWriteSync = _canWrite;
            _canReadSync = _canRead;
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
            _syncAccessHandle = await fileHandle.CreateSyncAccessHandle(new FileSystemSyncAccessOptions { Mode = "readwrite-unsafe" });
            if (truncate)
            {
                _syncAccessHandle.Truncate(0);
            }
            // if FileMode is append seek to the end of the file
            _position = seekToEnd ? Length : 0;
        }
        private async Task OpenNameInternal(FileSystemDirectoryHandle root, string name, FileMode fileMode, FileAccess fileAccess, CancellationToken cancellationToken = default)
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
            _canWriteSync = _canWrite;
            _canReadSync = _canRead;
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
            _syncAccessHandle = await fileHandle.CreateSyncAccessHandle(new FileSystemSyncAccessOptions { Mode = "readwrite-unsafe" });
            if (truncate)
            {
                _syncAccessHandle.Truncate(0);
            }
            // if FileMode is append seek to the end of the file
            _position = seekToEnd ? Length : 0;
        }
        private async Task OpenInternal(FileSystemFileHandle fileHandle, FileMode fileMode, FileAccess fileAccess, CancellationToken cancellationToken = default)
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
            _canWriteSync = _canWrite;
            _canReadSync = _canRead;
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
            _syncAccessHandle = await fileHandle.CreateSyncAccessHandle(new FileSystemSyncAccessOptions { Mode = "readwrite-unsafe" });
            if (truncate)
            {
                _syncAccessHandle.Truncate(0);
            }
            // if FileMode is append seek to the end of the file
            _position = seekToEnd ? Length : 0;
        }
        /// <inheritdoc/>
        public override async Task FlushAsync(CancellationToken cancellationToken = default)
        {
            Flush();
        }
        /// <summary>
        /// Returns true if this stream has been disposed
        /// </summary>
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            IsDisposed = true;
            Close();
        }

        /// <inheritdoc/>
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => await ReadAsync(new Memory<byte>(buffer, offset, count), cancellationToken);
        /// <inheritdoc/>
        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (_syncAccessHandle == null) throw new Exception("File not open");
            using var uint8ArrayData = ReadUint8Array(buffer.Length);
            using var bufferHeapView = HeapView.Create<byte, Uint8Array>(buffer);
            bufferHeapView.View.Set(uint8ArrayData);
            return (int)uint8ArrayData.Length;
        }
        /// <inheritdoc/>
        public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (_syncAccessHandle == null) throw new Exception("File not open");
            using var heapView = HeapView.Create<byte, Uint8Array>(buffer);
            using var uint8Array = heapView.View;
            WriteUint8Array(uint8Array);
        }
        /// <inheritdoc/>
        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => await WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken);
        /// <inheritdoc/>
        public override async Task<Uint8Array> ReadUint8ArrayAsync(int count, CancellationToken cancellationToken = default)
        {
            return ReadUint8Array(count);
        }
        /// <inheritdoc/>
        public override async Task WriteUint8ArrayAsync(Uint8Array data, CancellationToken cancellationToken = default)
        {
            WriteUint8Array(data);
        }
        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    _position = offset;
                    break;
                case SeekOrigin.Current:
                    _position += offset;
                    break;
                case SeekOrigin.End:
                    _position = Length + offset;
                    break;
            }
            return _position;
        }
        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            if (_syncAccessHandle == null) throw new Exception("File not open");
            _syncAccessHandle.Truncate(value);
        }
        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count) => Write(buffer);
        /// <inheritdoc/>
        public override void WriteUint8Array(Uint8Array data)
        {
            if (_syncAccessHandle == null) throw new Exception("File not open");
            var byteCount = _syncAccessHandle.Write(data, new FileSystemSyncReadWriteOptions { At = _position });
            _position += byteCount;
        }
        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count) => Read(buffer);
        /// <inheritdoc/>
        public override int Read(Span<byte> buffer)
        {
            if (_syncAccessHandle == null) throw new Exception("File not open");
            var count = buffer.Length;
            var bytesLeft = Math.Max(Length - _position, 0);
            var bytesToRead = Math.Max(0, Math.Min(bytesLeft, count));
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    var ptr = (IntPtr)p;
                    using var heapView = new HeapView<byte, Uint8Array>(ptr, bytesToRead);
                    var byteCount = _syncAccessHandle.Read(heapView.View, new FileSystemSyncReadWriteOptions { At = _position });
                    _position += byteCount;
                    return (int)byteCount;
                }
            }
        }
        /// <inheritdoc/>
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            if (_syncAccessHandle == null) throw new Exception("File not open");
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    var ptr = (IntPtr)p;
                    using var heapView = new HeapView<byte, Uint8Array>(ptr, buffer.Length);
                    var byteCount = _syncAccessHandle.Write(heapView.View, new FileSystemSyncReadWriteOptions { At = _position });
                    _position += byteCount;
                }
            }
        }
        /// <inheritdoc/>
        public override Uint8Array ReadUint8Array(int count)
        {
            if (_syncAccessHandle == null) throw new Exception("File not open");
            var bytesLeft = Math.Max(Length - _position, 0);
            var bytesToRead = Math.Max(0, Math.Min(bytesLeft, count));
            var uint8Array = new Uint8Array(bytesToRead);
            var byteCount = _syncAccessHandle.Read(uint8Array, new FileSystemSyncReadWriteOptions { At = _position });
            _position += byteCount;
            return uint8Array;
        }
        /// <inheritdoc/>
        public override void Flush()
        {
            _syncAccessHandle?.Flush();
        }
        /// <inheritdoc/>
        public override void Close()
        {
            _syncAccessHandle?.Close();
            _syncAccessHandle?.Dispose();
            _syncAccessHandle = null;
        }
    }
}
