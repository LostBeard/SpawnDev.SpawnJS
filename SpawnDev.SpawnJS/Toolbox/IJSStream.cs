using SpawnDev.SpawnJS.JSObjects;

namespace SpawnDev.SpawnJS.Toolbox
{
    public abstract class JSStreamBase : Stream, IJSReadStream, IJSWriteStream
    {
        /// <inheritdoc/>
        public abstract bool CanReadSync { get; }
        /// <inheritdoc/>
        public abstract bool CanWriteSync { get; }
        /// <inheritdoc/>
        public abstract Task WriteUint8ArrayAsync(Uint8Array data, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract void WriteUint8Array(Uint8Array data);
        /// <inheritdoc/>
        public abstract Task<Uint8Array> ReadUint8ArrayAsync(int count, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Uint8Array ReadUint8Array(int count);

        /// <summary>
        /// Copies this stream into <paramref name="destination"/>. When the destination is an
        /// <see cref="IJSWriteStream"/>, the bytes are pumped as <see cref="Uint8Array"/> chunks (kept JS-side,
        /// never through the .NET heap) - the zero-copy fast path. Otherwise defers to the base managed copy.
        /// </summary>
        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            if (destination is IJSWriteStream jsWrite)
                return JSStreamCopy.CopyToAsync(this, jsWrite, bufferSize, cancellationToken);
            return base.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        /// <summary>
        /// Copies this stream into <paramref name="destination"/>. When the destination is a synchronously
        /// writable <see cref="IJSWriteStream"/> and this stream can read synchronously, the bytes are copied
        /// as <see cref="Uint8Array"/> chunks (kept JS-side) - the zero-copy fast path. Otherwise defers to the
        /// base managed copy (which, for an async-only stream, throws - the correct fail-loud behavior).
        /// </summary>
        public override void CopyTo(Stream destination, int bufferSize)
        {
            if (destination is IJSWriteStream jsWrite && CanReadSync && jsWrite.CanWriteSync)
            {
                JSStreamCopy.CopyTo(this, jsWrite, bufferSize);
                return;
            }
            base.CopyTo(destination, bufferSize);
        }
    }
}
