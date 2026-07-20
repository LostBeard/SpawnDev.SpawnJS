// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// FileSystemDirectory.RemoveEntry options
    /// </summary>
    public class RemoveEntryOptions
    {
        /// <summary>
        /// A boolean value, which defaults to false. When set to true entries will be removed recursively.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Recursive { get; set; }
    }
}
