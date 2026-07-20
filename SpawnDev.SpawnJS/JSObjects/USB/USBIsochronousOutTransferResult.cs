// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The USBIsochronousOutTransferResult interface of the WebUSB API provides the result from a call to the isochronousTransferOut() method of the USBDevice interface. It represents the result from requesting a transfer of data from the USB host to the USB device.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/USBIsochronousOutTransferResult
    /// </summary>
    public class USBIsochronousOutTransferResult : SpawnJSObject
    {
        /// <inheritdoc/>
        public USBIsochronousOutTransferResult(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns an array of USBIsochronousOutTransferPacket objects containing the result of each request to send a packet to the device.
        /// </summary>
        public Array<USBIsochronousOutTransferPacket> Packets => JSRef!.Get<Array<USBIsochronousOutTransferPacket>>("packets");
    }
}
