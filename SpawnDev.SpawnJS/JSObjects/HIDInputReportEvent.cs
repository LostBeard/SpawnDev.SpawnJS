// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Represents a HIDInputReportEvent fired by a HID device.
    /// This event contains the report data received from a HID device.
    /// Corresponds to the WebHID 'HIDInputReportEvent' interface.
    /// https://wicg.github.io/webhid/#hidinputreportevent-interface
    /// </summary>
    public class HIDInputReportEvent : Event
    {
        /// <summary>
        /// Creates a new instance of <see cref="HIDInputReportEvent"/>.
        /// </summary>
        /// <param name="_ref"></param>
        public HIDInputReportEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Event() constructor creates a new Event object. An event created in this way is called a synthetic event, as opposed to an event fired by the browser, and can be dispatched by a script.
        /// </summary>
        /// <param name="type">A string with the name of the event.</param>
        /// <param name="options"></param>
        public HIDInputReportEvent(string type, EventOptions? options = null) : base(options == null ? JS.New(nameof(HIDInputReportEvent), type) : JS.New(nameof(HIDInputReportEvent), type, options)) { }

        /// <summary>
        /// The HIDDevice object associated with this event, representing the device
        /// that sent the input report.
        /// </summary>
        public HIDDevice Device => new HIDDevice(JSRef!.Get<SpawnJSObjectReference>("device"));

        /// <summary>
        /// An 8-bit value indicating the report ID of the input report.
        /// A value of 0 means the device does not use report IDs.
        /// </summary>
        public byte ReportId => JSRef!.Get<byte>("reportId");

        /// <summary>
        /// A DataView object containing the raw input report data.
        /// </summary>
        public DataView Data => JSRef!.Get<DataView>("data");
    }
}