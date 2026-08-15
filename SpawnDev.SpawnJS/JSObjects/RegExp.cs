
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RegExp
    /// </summary>
    public class RegExp : SpawnJSObject
    {
        /// <inheritdoc/>
        public RegExp(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The RegExp() constructor creates RegExp objects.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="flags"></param>
        public RegExp(string pattern, string? flags = null) : base(flags == null ? JS.New(nameof(RegExp), pattern) : JS.New(nameof(RegExp), pattern, flags)) { }
        /// <summary>The text of the pattern.</summary>
        public string Source => JSRef!.Get<string>("source");
        /// <summary>The flags of the regular expression (e.g. "gi").</summary>
        public string Flags => JSRef!.Get<string>("flags");
        /// <summary>Whether the "g" (global) flag is set.</summary>
        public bool Global => JSRef!.Get<bool>("global");
        /// <summary>Whether the "i" (ignoreCase) flag is set.</summary>
        public bool IgnoreCase => JSRef!.Get<bool>("ignoreCase");
        /// <summary>The index at which to start the next match (for global/sticky regexes).</summary>
        public int LastIndex { get => JSRef!.Get<int>("lastIndex"); set => JSRef!.Set("lastIndex", value); }
        /// <summary>Tests for a match against a .NET string. NOTE: brings the string into JS - prefer the
        /// <see cref="String"/> overload when the text is already held JS-side.</summary>
        public bool Test(string str) => JSRef!.Call<string, bool>("test", str);
        /// <summary>Tests for a match against a JS String held JS-side (no marshaling of the text).</summary>
        public bool Test(String str) => JSRef!.Call<global::SpawnDev.SpawnJS.JSObjects.String, bool>("test", str);
    }
}
