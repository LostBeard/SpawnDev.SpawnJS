// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The StorageManagerEstimate dictionary is used by the StorageManager.estimate() method to return an estimate of the total amount of storage available to the current origin and how much of it is already in use.
    /// </summary>
    public class StorageManagerEstimate
    {
        /// <summary>
        /// Quota
        /// </summary>
        public long Quota { get; set; }
        /// <summary>
        /// Usage
        /// </summary>
        public long Usage { get; set; }
        /// <summary>
        /// UsageDetails
        /// </summary>
        public StorageManagerEstimateUsageDetails? UsageDetails { get; set; }
    }
}
