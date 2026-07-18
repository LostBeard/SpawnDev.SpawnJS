using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.SpawnJSObjects
{
    /// <summary>
    /// Represents a Javascript Promise
    /// </summary>
    public class Promise : SpawnJSObject
    {
        /// <summary>
        /// Create a new instance that wraps the specified Promise reference
        /// </summary>
        /// <param name="jsObject"></param>
        public Promise(SpawnJSObjectReference jsObject) : base(jsObject) { }
    }
}
