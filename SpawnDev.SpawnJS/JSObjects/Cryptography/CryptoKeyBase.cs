using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Base class for CryptoKeyPair and CryptoKey
    /// </summary>
    public class CryptoKeyBase : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public CryptoKeyBase(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
