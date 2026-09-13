namespace SpawnDev.SpawnJS.Toolbox
{
    /// <summary>
    /// OPFSStream Stream API mopde
    /// </summary>
    [Flags]
    public enum OPFSFileOptions
    {
        /// <summary>
        /// Async access will be enabled.<br/>
        /// If FileSystemSyncHandle is available (only in a DedicatedWorkerGlobalScope) it will be used and sync Stream access will be enabled. - Default
        /// </summary>
        Auto,
        /// <summary>
        /// If FileSystemSyncHandle is available it will be used and sync Stream access will be enabled.<br/>
        /// If not running in a DedicatedWorkerGlobalScope and Read access is requested and Direct flag is not set, the file will be read into JS memory to allow sync access.<br/>
        /// NOTE: Only DedicatedWorkerGlobalScope supports synchronous write access to FileSystemFileHandles via FileSystemSyncHandle.
        /// </summary>
        SyncRequired = 2,
        /// <summary>
        /// Only the async access wil be enabled and used
        /// </summary>
        SyncDisabled = 4,
        /// <summary>
        /// Writes directly to file, reads directly from file.
        /// </summary>
        Direct = 8,
    }
}
