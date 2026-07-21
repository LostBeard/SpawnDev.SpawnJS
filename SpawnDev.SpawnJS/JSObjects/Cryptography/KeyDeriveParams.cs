// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Base class for options used in SubtleCrypto.DeriveKey()
    /// </summary>
    public class KeyDeriveParams
    {
        /// <summary>
        /// A string indicating the derive key parameters the inheriting class holds
        /// </summary>
        public required virtual string Name { get; set; }
    }
}
