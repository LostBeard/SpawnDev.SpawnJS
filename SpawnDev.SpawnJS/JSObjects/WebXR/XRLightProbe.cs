// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The XRLightProbe interface of the WebXR Device API contains lighting information at a given point in the user's environment. You can get an XRLighting object using the XRSession.requestLightProbe() method.<br/>
    /// This object doesn't itself contain lighting values, but it is used to collect lighting states for each XRFrame. See XRLightEstimate for the estimated lighting values for an XRLightProbe.
    /// https://developer.mozilla.org/en-US/docs/Web/API/XRLightProbe
    /// </summary>
    public class XRLightProbe : EventTarget
    {
        /// <inheritdoc/>
        public XRLightProbe(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// An XRSpace tracking the position and orientation the lighting estimations are relative to.
        /// </summary>
        public XRSpace ProbeSpace => JSRef!.Get<XRSpace>("probeSpace");
        /// <summary>
        /// Whenever the reflectionchange event fires on a light probe, you can retrieve an updated cube map by calling XRWebGLBinding.getReflectionCubeMap(). This is less expensive than retrieving lighting information with every XRFrame.
        /// </summary>
        public ActionEvent<Event> OnReflectionChange { get => new ActionEvent<Event>("reflectionchange", AddEventListener, RemoveEventListener); set { } }
    }
}
