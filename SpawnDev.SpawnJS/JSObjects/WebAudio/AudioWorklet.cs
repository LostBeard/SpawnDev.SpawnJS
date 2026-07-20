// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The AudioWorklet interface of the Web Audio API is used to supply custom audio processing scripts that execute in a separate thread to provide very low latency audio processing.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/AudioWorklet
    /// </summary>
    public class AudioWorklet : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public AudioWorklet(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Adds the module at the specified URL to the AudioWorklet scope.
        /// </summary>
        /// <param name="moduleURL">A string containing the URL of a JavaScript module to add.</param>
        /// <returns></returns>
        public Task AddModule(string moduleURL) => JSRef!.CallVoidAsync("addModule", moduleURL);
        /// <summary>
        /// Adds the module at the specified URL to the AudioWorklet scope.
        /// </summary>
        /// <param name="moduleURL">A string containing the URL of a JavaScript module to add.</param>
        /// <param name="options">An object that can contain the credentials property with a value of "omit", "same-origin", or "include".</param>
        /// <returns></returns>
        public Task AddModule(string moduleURL, object options) => JSRef!.CallVoidAsync("addModule", moduleURL, options);
    }
}
