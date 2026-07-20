// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The WEBGL_lose_context extension is part of the WebGL API and exposes functions to simulate losing and restoring the WebGL context.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/WEBGL_lose_context
    /// </summary>
    public class WEBGL_lose_context : SpawnJSObject
    {
        /// <inheritdoc/>
        public WEBGL_lose_context(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Simulates the losing of the context.
        /// </summary>
        public void LoseContext() => JSRef!.CallVoid("loseContext");
        /// <summary>
        /// Simulates the restoring of the context.
        /// </summary>
        public void RestoreContext() => JSRef!.CallVoid("restoreContext");
    }
}
