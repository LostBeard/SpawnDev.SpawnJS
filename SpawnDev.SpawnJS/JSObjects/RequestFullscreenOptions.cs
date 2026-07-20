using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Options for Element.RequestFullscreen()
    /// </summary>
    public class RequestFullscreenOptions
    {
        /// <summary>
        /// Options hide, show, auto (default)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? NavigationUI { get; set; }
    }
}