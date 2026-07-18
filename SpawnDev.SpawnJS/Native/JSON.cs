using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Native
{
    /// <summary>
    /// Typed bindings to the Javascript globalThis.JSON object
    /// </summary>
    public static partial class JSON
    {
        #region JSON
        /// <summary>
        /// JSON.stringify - converts the JSObject to a JSON string
        /// </summary>
        [JSImport("globalThis.JSON.stringify")]
        public static partial string Stringify(JSObject target);

        /// <summary>
        /// JSON.parse - parses the JSON string into a JSObject
        /// </summary>
        [JSImport("globalThis.JSON.parse")]
        public static partial JSObject? Parse(string target);

        /// <summary>
        /// JSON.stringify with a replacer - converts the JSObject to a JSON string
        /// </summary>
        [JSImport("globalThis.JSON.stringify")]
        public static partial string Stringify(JSObject target, JSObject replacer);

        /// <summary>
        /// JSON.parse with a reviver - parses the JSON string into a JSObject
        /// </summary>
        [JSImport("globalThis.JSON.parse")]
        public static partial JSObject? Parse(string target, JSObject reviver);
        #endregion
    }
}
