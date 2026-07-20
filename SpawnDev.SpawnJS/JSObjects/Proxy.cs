// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The Proxy object enables you to create a proxy for another object, which can intercept and redefine fundamental operations for that object.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Proxy
    /// </summary>
    public class Proxy : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public Proxy(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The Proxy() constructor creates Proxy objects.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="handler"></param>
        public Proxy(SpawnJSObject target, ProxyHandler handler) : base(JS.New(nameof(Proxy), target, handler)) { }
        /// <summary>
        /// The Proxy() constructor creates Proxy objects.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="handler"></param>
        public Proxy(SpawnJSObjectReference target, ProxyHandler handler) : base(JS.New(nameof(Proxy), target, handler)) { }

    }
}
