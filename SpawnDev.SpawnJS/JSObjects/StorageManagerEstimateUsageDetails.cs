// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The StorageManagerEstimateUsageDetails dictionary is used by the StorageManager.estimate() method to return a detailed estimate of the amount of storage in use by various features.
    /// </summary>
    public class StorageManagerEstimateUsageDetails
    {
        /// <summary>
        /// Caches
        /// </summary>
        public long Caches { get; set; }
        /// <summary>
        /// serviceWorkerRegistrations
        /// </summary>
        public long serviceWorkerRegistrations { get; set; }
    }
}
