
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
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

        // --- String methods that keep the text JS-side (results returned as JS refs unless a small,
        //     bounded value like a count or match list is the whole point) --------------------------------
        /// <summary>The number of UTF-16 code units, read JS-side (no marshaling of the text).</summary>
        public int Length => JSRef!.Get<int>("length");
        /// <summary>Whether the string contains <paramref name="search"/>.</summary>
        public bool Includes(string search) => JSRef!.Call<string, bool>("includes", search);
        /// <summary>Extracts a section as a new JS String (held JS-side).</summary>
        public String Slice(int start) => JSRef!.Call<int, String>("slice", start);
        /// <summary>Extracts a section as a new JS String (held JS-side).</summary>
        public String Slice(int start, int end) => JSRef!.Call<int, int, String>("slice", start, end);
        /// <summary>Splits the string by a separator into a JS Array of substrings (held JS-side).</summary>
        public Array Split(string separator) => JSRef!.Call<string, Array>("split", separator);
        /// <summary>Splits the string by a RegExp into a JS Array of substrings (held JS-side).</summary>
        public Array Split(RegExp separator) => JSRef!.Call<global::SpawnDev.SpawnJS.JSObjects.RegExp, Array>("split", separator);
        /// <summary>Replaces matches of a RegExp, returning a NEW JS String held JS-side (the content
        /// never enters the .NET heap). Use a global ("g") RegExp to replace all. Replacement supports
        /// JS patterns like $1, $&amp;.</summary>
        public String Replace(RegExp pattern, string replacement) => JSRef!.Call<global::SpawnDev.SpawnJS.JSObjects.RegExp, string, String>("replace", pattern, replacement);
        /// <summary>Replaces the first literal occurrence, returning a new JS String held JS-side.</summary>
        public String Replace(string search, string replacement) => JSRef!.Call<string, string, String>("replace", search, replacement);
        /// <summary>Runs match() against a RegExp and returns the JS result Array held JS-side (null if no
        /// match). With a global RegExp this is the array of matched substrings; read Length for a count
        /// without marshaling, or ToList&lt;string&gt;() to bring the (bounded) matches into .NET.</summary>
        public Array? Match(RegExp pattern) => JSRef!.Call<global::SpawnDev.SpawnJS.JSObjects.RegExp, Array?>("match", pattern);
    }
}
