// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The WebGLContextEvent interface is part of the WebGL API and is an interface for an event that is generated in response to a status change to the WebGL rendering context.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/WebGLContextEvent
    /// </summary>
    public class WebGLContextEvent : Event
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public WebGLContextEvent(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// A read-only property containing additional information about the event.
        /// </summary>
        public string StatusMessage => JSRef!.Get<string>("statusMessage");
    }
}
