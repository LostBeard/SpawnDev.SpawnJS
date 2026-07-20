// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The String object is used to represent and manipulate a sequence of characters.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String
    /// </summary>
    public class String : SpawnJSObject
    {
        /// <summary>
        /// Implicit conversion to .Net string
        /// </summary>
        /// <param name="strObj"></param>
        public static implicit operator string(String strObj) => strObj.ValueOf();
        /// <summary>
        /// Explicit cast from .Net string to StringPrimitive
        /// </summary>
        /// <param name="source">.Net string</param>
        public static explicit operator String(string source) => new String(source);
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public String(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The String() constructor creates String objects.
        /// </summary>
        /// <param name="thing">Anything to be converted to a string.</param>
        public String(object thing) : base(JS.New(nameof(String), thing is string thingStr ? (StringPrimitive)thingStr : thing)) { }
        /// <summary>
        /// Returns the primitive value of the specified object. Overrides the Object.prototype.valueOf() method.
        /// </summary>
        /// <returns></returns>
        public string ValueOf() => JSRef!.Call<string>("valueOf");
        /// <summary>
        /// Returns the primitive string as a .Net string
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ValueOf();
    }
}
