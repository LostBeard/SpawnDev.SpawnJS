// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The Ink interface of the Ink API provides access to InkPresenter objects for the application to use to render ink strokes.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/Ink
    /// </summary>
    public class Ink : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public Ink(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The requestPresenter() method of the Ink interface returns a Promise that fulfills with an InkPresenter object.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<InkPresenter> RequestPresenter(InkPresenterParam param) => JSRef!.CallAsync<InkPresenter>("requestPresenter", param);
    }
}
