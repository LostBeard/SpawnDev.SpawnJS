namespace SpawnDev.SpawnJS.Toolbox
{
    /// <summary>
    /// OPFSStream Stream API mopde
    /// </summary>
    public enum OPFSSyncMode
    {
        /// <summary>
        /// If FileSystemSyncHandle is available (only in a DedicatedWorkerGlobalScope) it will be used and sync Stream access will be enabled. - Default
        /// </summary>
        Auto = 1,
        /// <summary>
        /// If FileSystemSyncHandle is available it will be used and sync Stream access will be enabled.<br/>
        /// If not running in a DedicatedWorkerGlobalScope and Read access is requested, the file will be read into JS memory to allow sync access.<br/>
        /// NOTE: Only DedicatedWorkerGlobalScope supports synchronous write access to FileSystemFileHandles via FileSystemSyncHandle.
        /// </summary>
        Required = 2,
        /// <summary>
        /// If FileSystemSyncHandle is available it will be used and sync Stream access will be enabled otherwise an exception is thrown.
        /// </summary>
        RequiredOnDisk = 3,
        /// <summary>
        /// Async Stream access will be enabled using BlobStream for read streams and FileSystemHandleWritableStream for write streams.
        /// </summary>
        Disabled = 16,
    }
}
