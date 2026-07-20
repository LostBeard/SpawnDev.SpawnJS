using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Represents the object that should be passed as the algorithm parameter into SubtleCrypto.sign() or SubtleCrypto.verify()
    /// </summary>
    public class CryptoSignParams
    {
        /// <summary>
        /// A string that specifies the signature algorithm to use
        /// </summary>
        public virtual string Name { get; set; }
    }
}
