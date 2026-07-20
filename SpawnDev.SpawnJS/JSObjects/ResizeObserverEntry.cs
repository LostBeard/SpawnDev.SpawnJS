// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ResizeObserverEntry interface represents the object passed to the ResizeObserver() constructor's callback function, which allows you to access the new dimensions of the Element or SVGElement being observed.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ResizeObserverEntry
    /// </summary>
    public class ResizeObserverEntry : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public ResizeObserverEntry(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// An array of objects containing the new border box size of the observed element when the callback is run.
        /// </summary>
        public List<ResizeObserverSize> BorderBoxSize => JSRef!.Get<List<ResizeObserverSize>>("borderBoxSize");
        /// <summary>
        /// An array of objects containing the new content box size of the observed element when the callback is run.
        /// </summary>
        public List<ResizeObserverSize> ContentBoxSize => JSRef!.Get<List<ResizeObserverSize>>("contentBoxSize");
        /// <summary>
        /// An array of objects containing the new content box size in device pixels of the observed element when the callback is run.
        /// </summary>
        public List<ResizeObserverSize> DevicePixelContentBoxSize => JSRef!.Get<List<ResizeObserverSize>>("devicePixelContentBoxSize");
        /// <summary>
        /// A DOMRectReadOnly object containing the new size of the observed element when the callback is run. Note that this is now a legacy property that is retained in the spec for backward-compatibility reasons only.
        /// </summary>
        public DOMRectReadOnly ContentRect => JSRef!.Get<DOMRectReadOnly>("contentRect");
        /// <summary>
        /// A reference to the Element or SVGElement being observed.
        /// </summary>
        public Element Target => JSRef!.Get<Element>("target");
    }
}
