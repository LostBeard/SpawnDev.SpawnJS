// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The Presentation interface of the Presentation API provides a way to access the presentation API features.
    /// </summary>
    public class Presentation : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        public Presentation(SpawnJSObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Returns the default presentation request.
        /// </summary>
        public PresentationRequest DefaultRequest
        {
            get => JSRef!.Get<PresentationRequest>("defaultRequest");
            set => JSRef!.Set("defaultRequest", value);
        }

        /// <summary>
        /// Returns the presentation receiver.
        /// </summary>
        public PresentationReceiver Receiver => JSRef!.Get<PresentationReceiver>("receiver");
    }
}
