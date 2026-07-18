
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Text;

namespace SpawnDev.SpawnJS.Native
{
    /// <summary>
    /// Typed bindings to the Javascript globalThis.console object
    /// </summary>
    public static partial class JSConsole
    {
        #region JSON
        /// <summary>
        /// console.log - logs the JSObject to the Javascript console
        /// </summary>
        [JSImport("globalThis.console.log")]
        public static partial void Stringify(JSObject target);

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
