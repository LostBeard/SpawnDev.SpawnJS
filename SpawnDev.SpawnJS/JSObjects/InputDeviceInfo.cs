// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The InputDeviceInfo interface of the Media Capture and Streams API gives access to the capabilities of the input device that it represents.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/InputDeviceInfo
    /// </summary>
    public class InputDeviceInfo : MediaDeviceInfo
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public InputDeviceInfo(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns a MediaTrackCapabilities object describing the primary audio or video track of a device's MediaStream.
        /// </summary>
        public MediaTrackCapabilities GetCapabilities() => JSRef!.Call<MediaTrackCapabilities>("getCapabilities");
    }
}
