using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.SpawnJSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/InputDeviceCapabilities<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/InputDeviceCapabilities
    /// </summary>
    public class InputDeviceCapabilities : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public InputDeviceCapabilities(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// A Boolean that indicates whether the device dispatches touch events.
        /// </summary>
        public bool FiresTouchEvents => JSRef!.Get<bool>("firesTouchEvents");
    }
}
