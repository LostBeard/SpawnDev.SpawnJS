// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The SerialPortRequestOptions dictionary represents the options for a Serial.requestPort() call.
    /// </summary>
    public class SerialPortOptions
    {
        /// <summary>
        /// A list of objects containing vendor and product IDs used to search for attached devices. The USB Implementors Forum assigns IDs to specific companies. Each company assigns IDs to its products. 
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("filters")]
        public IEnumerable<SerialPortFilter>? Filters { get; set; }
    }
}
