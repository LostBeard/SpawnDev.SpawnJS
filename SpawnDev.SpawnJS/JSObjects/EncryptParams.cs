using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
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
        public virtual string Name { get; set; }
    }
}
