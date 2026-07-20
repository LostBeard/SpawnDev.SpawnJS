// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Options for FileSystemFileHandle.createWritable
    /// </summary>
    public class FileSystemCreateWritableOptions
    {
        /// <summary>
        /// A Boolean. Default false. When set to true if the file exists, the existing file is first copied to the temporary file. Otherwise the temporary file starts out empty.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? KeepExistingData { get; set; }
    }
}
