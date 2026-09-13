using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS.Toolbox
{
    /// <summary>
    /// FileSystemHandleWritableStream extensions
    /// </summary>
    public static partial class FileSystemHandleWritableStreamExtensions
    {
        /// <summary>
        /// Returns a async-only writable Stream
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="seekToEnd"></param>
        /// <param name="truncate"></param>
        /// <returns></returns>
        public static Task<FileSystemHandleWritableStream> GetWritableStream(this FileSystemFileHandle fileHandle, bool seekToEnd, bool truncate)
        {
            return FileSystemHandleWritableStream.Create(fileHandle, seekToEnd, truncate);
        }
        public static Task<FileSystemHandleWritableStream> GetWritableStream(this FileSystemFileHandle fileHandle, bool appendMode = false)
        {
            return FileSystemHandleWritableStream.Create(fileHandle, appendMode);
        }
    }
    /// <summary>
    /// Creates an asynchronously writable Stream from a writable FileSystemHandle<br/>
    /// IMPORTANT: Only asynchronous writes will work, synchronous writes will throw an exception<br/>
    /// </summary>
    public class FileSystemHandleWritableStream : JSWriteStreamBase
    {
        /// <summary>
        /// False - an OPFS/disk <c>FileSystemWritableFileStream.write()</c> is async, so synchronous
        /// <see cref="WriteUint8Array(Uint8Array)"/> is not supported (it throws). Use
        /// <see cref="WriteUint8ArrayAsync(Uint8Array, CancellationToken)"/>.
        /// </summary>
        public override bool CanWriteSync => false;
        /// <summary>
        /// Returns true when the Writer is ready and no longer applying back pressure
        /// </summary>
        public Task Ready => Writer?.Ready ?? Task.FromException(new Exception("Invalid state"));
        /// <summary>
        /// Returns a new instance of FileSystemHandleWritableStream
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="seekToEnd"></param>
        /// <param name="truncate"></param>
        /// <returns></returns>
        public static async Task<FileSystemHandleWritableStream> Create(FileSystemFileHandle fileHandle, bool seekToEnd, bool truncate)
        {
            var size = truncate ? 0 : await fileHandle.GetSize();
            var startPos = seekToEnd ? size : 0;
            var fsStream = await fileHandle.CreateWritable(new FileSystemCreateWritableOptions { KeepExistingData = !truncate });
            var ret = new FileSystemHandleWritableStream(fileHandle, fsStream, size, startPos);
            return ret;
        }
        public static Task<FileSystemHandleWritableStream> Create(FileSystemFileHandle fileHandle, bool appendMode = false)
            => Create(fileHandle, appendMode, !appendMode);
        /// <summary>
        /// Private constructor. Used by the static Create method
        /// </summary>
        private FileSystemHandleWritableStream(FileSystemFileHandle fileHandle, FileSystemWritableFileStream fsStream, long size, long startPos)
        {
            FileHandle = fileHandle;
            FSStream = fsStream;
            StartSize = size;
            _Length = size;
            // if appendMode, set the position to the end of the file
            // the actual seek will be done during the next write
            _Position = startPos;
        }
        /// <summary>
        /// The FileSystemFileHandle that this stream is writing to
        /// </summary>
        public FileSystemFileHandle FileHandle { get; private set; }
        /// <summary>
        /// The FileSystemWritableFileStream that this stream is writing to
        /// </summary>
        FileSystemWritableFileStream? FSStream { get; set; }
        /// <summary>
        /// The WritableStreamDefaultWriter that this stream is writing to
        /// </summary>
        WritableStreamDefaultWriter? Writer { get; set; }
        ///<inheritdoc/>
        public override bool CanRead => false;
        ///<inheritdoc/>
        public override bool CanSeek => true;
        ///<inheritdoc/>
        public override bool CanWrite => true;
        /// <summary>
        /// This size of the stream when this instance was created.
        /// </summary>
        public long StartSize { get; private set; } = -1;
        long _Length = 0;
        ///<inheritdoc/>
        public override long Length
        {
            get
            {
                return _Length;
            }
        }
        long _Position = 0;
        ///<inheritdoc/>
        public override long Position { get => _Position; set => Seek(value, SeekOrigin.Begin); }
        /// <summary>
        /// Flush and wait for all queued commands to finish.<br/>
        /// Use after a SetLength call to await the Length change (not required as other async calls will wait for it to complete before running.)
        /// </summary>
        public override async Task FlushAsync(CancellationToken cancellationToken = default)
        {
            // calling FlushAsync allows waiting for all queued commands to run
            await Exclusive(cancellationToken);
        }
        ///<inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    _Position = offset;
                    break;
                case SeekOrigin.Current:
                    _Position = _Position + offset;
                    break;
                case SeekOrigin.End:
                    _Position = Length + offset;
                    break;
            }
            return _Position;
        }
        /// <summary>
        /// Queues an async call to resize the file.<br/>
        /// SetLength change can be awaited by calling FlushAsync but it is not required to continue using the stream as all changes are sequential and exclusive.<br/>
        /// </summary>
        public override void SetLength(long value)
        {
            _ = Exclusive(async () =>
            {
                try
                {
                    if (FSStream == null) return;
                    await FSStream.Truncate(value);
                    _Length = value;
                }
                catch { }
            });
        }
        public async Task SetLengthAsync(long value, CancellationToken cancellationToken = default)
        {
            await Exclusive(async () =>
            {
                if (FSStream == null) throw new Exception("Invalid state");
                await FSStream.Truncate(value);
                _Length = value;
            }, cancellationToken);
        }
        long _PositionReal = 0;
        /// <inheritdoc/>
        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
            => await WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken);
        public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            await Exclusive(async () =>
            {
                if (FSStream == null) throw new Exception("Invalid state");
                // seek if needed
                var position = _Position;
                if (_PositionReal != position)
                {
                    if (Writer != null)
                    {
                        Writer.ReleaseLock();
                        Writer.Dispose();
                        Writer = null;
                    }
                    await FSStream.Seek(position);
                    _PositionReal = position;
                }
                Writer ??= FSStream.GetWriter();
                // use HeapView to create fast copy of the buffer source region to a new Uint8Array
                using var heapView = HeapView.Create<byte, Uint8Array>(buffer);
                using var uint8Array = heapView.View;
                await Writer.Write(uint8Array);
                _Position += buffer.Length;
                _PositionReal = _Position;
                if (_Position > _Length)
                {
                    _Length = _Position;
                }
            }, cancellationToken);
        }
        /// <summary>
        /// Write a Uint8Array to the stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task WriteAsync(Uint8Array buffer, CancellationToken cancellationToken = default)
        {
            await Exclusive(async () =>
            {
                if (FSStream == null) throw new Exception("Invalid state");
                // seek if needed
                var position = _Position;
                if (_PositionReal != position)
                {
                    if (Writer != null)
                    {
                        Writer.ReleaseLock();
                        Writer.Dispose();
                        Writer = null;
                    }
                    await FSStream.Seek(position);
                    _PositionReal = position;
                }
                Writer ??= FSStream.GetWriter();
                await Writer.Write(buffer);
                _Position += buffer.Length;
                _PositionReal = _Position;
                if (_Position > _Length)
                {
                    _Length = _Position;
                }
            }, cancellationToken);
        }
        /// <inheritdoc/>
        public override Task WriteUint8ArrayAsync(Uint8Array data, CancellationToken cancellationToken = default)
            => WriteAsync(data, cancellationToken);
        /// <summary>
        /// Not supported - an OPFS/disk write is async (<see cref="CanWriteSync"/> is false). Use
        /// <see cref="WriteUint8ArrayAsync(Uint8Array, CancellationToken)"/>.
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public override void WriteUint8Array(Uint8Array data)
            => throw new NotSupportedException($"{nameof(FileSystemHandleWritableStream)}.WriteUint8Array not supported (the OPFS/disk write is async). Use WriteUint8ArrayAsync.");
        bool _committed = false;
        /// <summary>
        /// Flushes and commits the file to disk, AWAITING the underlying OPFS/disk <c>close()</c> Promise -
        /// which is what actually writes the buffered bytes. Prefer this (or <c>await using</c>) over a plain
        /// <c>using</c>: the synchronous <see cref="Dispose(bool)"/> cannot await the async commit, so a plain
        /// <c>using</c> can return before the file is written (browser-timing dependent - a subsequent read can
        /// see an empty/short file, notably on Firefox). Idempotent.
        /// </summary>
        public async Task CloseAsync()
        {
            if (_committed) return;
            _committed = true;
            await Exclusive(async () =>
            {
                if (Writer != null)
                {
                    // Writer.Close() flushes queued writes, closes the underlying stream (the commit), and
                    // releases the lock - all awaited.
                    await Writer.Close().ConfigureAwait(false);
                    Writer.Dispose();
                    Writer = null;
                }
                else if (FSStream != null)
                {
                    await FSStream.Close().ConfigureAwait(false);
                }
                FSStream?.Dispose();
                FSStream = null;
            });
        }
        /// <summary>
        /// Aborts the write WITHOUT committing: releases the underlying OPFS swap file and discards any buffered
        /// bytes. Call this instead of <see cref="CloseAsync"/> when the write FAILED (e.g. a
        /// <c>QuotaExceededError</c> mid-copy). Committing on failure would persist a partial/garbage file, and -
        /// worse - a stream left neither closed nor aborted leaks the temporary swap file <c>createWritable()</c>
        /// allocated until JS garbage collection; under a retry loop that compounds into a runaway that exhausts
        /// origin storage. Idempotent, and marks the stream committed so no later close/abort runs.
        /// </summary>
        public async Task AbortAsync()
        {
            if (_committed) return;
            _committed = true;
            await Exclusive(async () =>
            {
                if (Writer != null)
                {
                    try { Writer.ReleaseLock(); } catch { }
                    Writer.Dispose();
                    Writer = null;
                }
                if (FSStream != null)
                {
                    // Best-effort: abort() discards the swap file. Swallow any abort error - the write failure that
                    // sent us here is the exception the caller must see.
                    try { await FSStream.Abort().ConfigureAwait(false); } catch { }
                    FSStream.Dispose();
                    FSStream = null;
                }
            });
        }
        /// <summary>
        /// Commits the file (awaiting the OPFS <c>close()</c>) and then releases resources. Use
        /// <c>await using</c> or call <see cref="CloseAsync"/> explicitly when the written bytes must be
        /// readable afterward - the synchronous <see cref="Dispose(bool)"/> does NOT await the commit.
        /// </summary>
        public override async ValueTask DisposeAsync()
        {
            await CloseAsync().ConfigureAwait(false);
            Dispose();
        }
        ///<inheritdoc/>
        /// <remarks>
        /// WARNING: synchronous disposal CANNOT await the OPFS/disk <c>close()</c> commit - it fires it and
        /// returns, so the written bytes are not guaranteed on disk when this returns. Prefer
        /// <see cref="DisposeAsync"/> (<c>await using</c>) or <see cref="CloseAsync"/> whenever the file is
        /// read back afterward. This sync path exists only as a best-effort fallback.
        /// </remarks>
        protected override void Dispose(bool disposing)
        {
            if (_committed)
            {
                base.Dispose(disposing);
                return;
            }
            if (Writer != null)
            {
                Writer.ReleaseLock();
                Writer.Dispose();
                Writer = null!;
            }
            FSStream?.Close();   // fire-and-forget: commit NOT awaited - see the remarks / use DisposeAsync
            FSStream?.Dispose();
            FSStream = null!;
            base.Dispose(disposing);
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException("Only async access is supported");
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        public override void Flush()
        {
            throw new NotImplementedException("Only async access is supported");
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException("Only async access is supported");
        }
        public override void Close()
        {
            // does nothing
        }
        #region Async-Call-Queueing
        private SemaphoreSlim _handleLimiter = new SemaphoreSlim(1);
        private async Task Exclusive(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var hasHandle = false;
            try
            {
                await _handleLimiter.WaitAsync(cancellationToken);
                hasHandle = true;
            }
            finally
            {
                if (hasHandle) _handleLimiter.Release();
            }
        }
        private async Task Exclusive(Func<Task> withHandleFn, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var hasHandle = false;
            try
            {
                await _handleLimiter.WaitAsync(cancellationToken);
                hasHandle = true;
                await withHandleFn().WaitAsync(cancellationToken);
            }
            finally
            {
                if (hasHandle) _handleLimiter.Release();
            }
        }
        private async Task<TResult> Exclusive<TResult>(Func<Task<TResult>> withHandleFn, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var hasHandle = false;
            try
            {
                await _handleLimiter.WaitAsync(cancellationToken);
                hasHandle = true;
                return await withHandleFn().WaitAsync(cancellationToken);
            }
            finally
            {
                if (hasHandle) _handleLimiter.Release();
            }
        }
        #endregion
    }
}

