// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// ClipboardItem options
    /// </summary>
    public class ClipboardItemOptions
    {
        /// <summary>
        /// One of the three strings: unspecified, inline or attachment. The default is unspecified.<br/>
        /// 'inline' signifies to apps that receive the paste that the ClipboardItem should be inserted inline at the point of paste.<br/>
        /// 'attachment' signifies to apps that receive the paste that the ClipboardItem should be added as an attachment.<br/>
        /// 'unspecified' doesn't signify any information to apps that receive the paste.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PresentationStyle { get; set; }
    }
}
