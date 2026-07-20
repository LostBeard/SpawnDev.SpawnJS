// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Options utilized when calling PerformanceObserver.observe().<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/PerformanceObserver/observe
    /// </summary>
    public class PerformanceObserverInit
    {
        /// <summary>
        /// A single string specifying the entry type to observe. usage of this property is not compatible with entryTypes.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; set; }
        /// <summary>
        /// An array of strings, each specifying one entry type to observe. Unrecognized types are ignored, though the browser may output a warning message to the console. usage of this property is not compatible with type or buffered.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? EntryTypes { get; set; }
        /// <summary>
        /// A boolean flag indicating whether or not to observe entries that occurred before the observer was created. The default is false. usage of this property is not compatible with entryTypes.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Buffered { get; set; }
    }
}
