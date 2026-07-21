// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
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
        public required virtual string Name { get; set; }
    }
}
