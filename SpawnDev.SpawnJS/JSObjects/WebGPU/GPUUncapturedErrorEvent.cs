// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The GPUUncapturedErrorEvent interface of the WebGPU API is the event object type for the GPUDevice uncapturederror event, used for telemetry and to report unexpected errors.
    /// </summary>
    public class GPUUncapturedErrorEvent : Event
    {
        /// <inheritdoc/>
        public GPUUncapturedErrorEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// A GPUError object instance providing access to the details of the error.
        /// </summary>
        public GPUError Error => JSRef!.Get<GPUError>("error");
    }
}
