using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// PeriodicSync options
    /// </summary>
    public class PeriodicSyncOptions
    {
        /// <summary>
        /// The minimum interval time, in milliseconds, at which the periodic sync should occur.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MinInterval { get; set; }
    }
}
