// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The TouchList interface represents a list of contact points on a touch surface. For example, if the user has three fingers on the touch surface (such as a screen or trackpad), the corresponding TouchList object would have one Touch object for each finger, for a total of three entries.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/TouchList
    /// </summary>
    public class TouchList : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public TouchList(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The number of Touch objects in the TouchList.
        /// </summary>
        public int Length => JSRef!.Get<int>("length");
        /// <summary>
        /// Returns the Touch object at the specified index in the list.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Touch Items(int i) => JSRef!.Call<Touch>("item", i);
    }
}
