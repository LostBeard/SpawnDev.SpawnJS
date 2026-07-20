// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The XRTransientInputHitTestSource interface of the WebXR Device API handles transient input hit test subscriptions. You can get an XRTransientInputHitTestSource object by calling the XRSession.requestHitTestSourceForTransientInput().<br/>
    /// This object doesn't itself contain transient input hit test results, but it is used to compute hit tests for each XRFrame by calling XRFrame.getHitTestResultsForTransientInput(), which returns XRTransientInputHitTestResult objects.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRTransientInputHitTestSource
    /// </summary>
    public class XRTransientInputHitTestSource : SpawnJSObject
    {
        /// <inheritdoc/>
        public XRTransientInputHitTestSource(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The cancel() method of the XRTransientInputHitTestSource interface unsubscribes a transient input hit test.
        /// </summary>
        public void Cancel() => JSRef!.CallVoid("cancel");
    }
}
