// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// HID Device request options
    /// https://wicg.github.io/webhid/#dom-hid-requestdevice
    /// </summary>
    public class HIDDeviceRequestOptions
    {
        /// <summary>
        /// An array of HIDDeviceFilter objects used to filter the requested devices.
        /// </summary>
        public IEnumerable<HIDDeviceFilter> Filters { get; set; }

        /// <summary>
        /// An optional array of HIDDeviceFilter objects used to exclude devices from the requested devices.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<HIDDeviceFilter>? ExclusionFilters { get; set; }

    }
}
