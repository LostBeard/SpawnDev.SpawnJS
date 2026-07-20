// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The USBIsochronousInTransferPacket interface of the WebUSB API is part of the response from a call to the isochronousTransferIn() method of the USBDevice interface. It represents the status of an individual packet from a request to transfer data from the USB device to the USB host over an isochronous endpoint.
    /// https://developer.mozilla.org/en-US/docs/Web/API/USBIsochronousInTransferPacket
    /// </summary>
    public class USBIsochronousInTransferPacket : SpawnJSObject
    {
        /// <inheritdoc/>
        public USBIsochronousInTransferPacket(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns a DataView object containing the data received from the USB device in this packet, if any.
        /// </summary>
        public DataView Data => JSRef!.Get<DataView>("data");
        /// <summary>
        /// Returns the status of the transfer request, one of:<br/>
        /// "ok" - The transfer was successful.<br/>
        /// "stall" - The device indicated an error by generating a stall condition on the endpoint.A stall on an isochronous endpoint does not need to be cleared.<br/>
        /// "babble" - The device responded with more data than was expected.
        /// </summary>
        public string Status => JSRef!.Get<string>("status");
    }
}
