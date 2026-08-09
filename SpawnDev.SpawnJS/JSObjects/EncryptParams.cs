
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Base class for parameter types used when calling SubtleCrypto.encrypt
    /// </summary>
    public class EncryptParams
    {
        /// <summary>
        /// A string.
        /// </summary>
        public required virtual string Name { get; set; }
    }
}
