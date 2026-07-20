// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// FileSystemDirectory.GetHandle options
    /// </summary>
    public class GetHandleOptions
    {
        /// <summary>
        /// A boolean value, which defaults to false. When set to true if the directory is not found, one with the specified name will be created and returned.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Create { get; set; }
    }
}
