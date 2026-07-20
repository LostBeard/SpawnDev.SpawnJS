// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The XRHitTestSource interface of the WebXR Device API handles hit test subscriptions. You can get an XRHitTestSource object by using the XRSession.requestHitTestSource() method.<br/>
    /// This object doesn't itself contain hit test results, but it is used to compute hit tests for each XRFrame by calling XRFrame.getHitTestResults(), which returns XRHitTestResult objects.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRHitTestSource
    /// </summary>
    public class XRHitTestSource : EventTarget
    {
        /// <inheritdoc/>
        public XRHitTestSource(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The cancel() method of the XRHitTestSource interface unsubscribes a hit test.
        /// </summary>
        public void Cancel() => JSRef!.CallVoid("cancel");
    }
}
