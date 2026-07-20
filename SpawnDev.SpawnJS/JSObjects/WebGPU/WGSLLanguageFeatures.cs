// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The WGSLLanguageFeatures interface of the WebGPU API is a setlike object that reports the WGSL language extensions supported by the WebGPU implementation.
    /// </summary>
    public class WGSLLanguageFeatures : Set<string>
    {
        #region Constructors
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public WGSLLanguageFeatures(SpawnJSObjectReference _ref) : base(_ref) { }
        #endregion
    }
}
