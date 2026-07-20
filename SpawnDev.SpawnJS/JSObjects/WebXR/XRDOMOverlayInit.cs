// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The XRDOMOverlayInit dictionary is used by the XRSystem.requestSession() method to specify options for a DOM overlay.
    /// </summary>
    public class XRDOMOverlayInit
    {
        /// <summary>
        /// An HTMLElement which will be the root of the DOM overlay.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public HTMLElement? Root { get; set; }
    }
}
