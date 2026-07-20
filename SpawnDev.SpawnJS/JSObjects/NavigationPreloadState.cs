// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/service-workers/#dictdef-navigationpreloadstate
    /// </summary>
    public class NavigationPreloadState
    {
        /// <summary>
        /// true if preloading is enabled, and false otherwise.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// A string containing the value that will be sent in the Service-Worker-Navigation-Preload HTTP header following a preloading fetch(). This defaults to true unless the value was changed using NavigationPreloadManager.setHeaderValue().
        /// </summary>
        public string? HeaderValue { get; set; }
    }
}
