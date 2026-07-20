// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The ReportBody interface of the Reporting API represents the body of a report. Individual report types inherit from this interface, adding specific properties for the report type.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ReportBody
    /// </summary>
    public class ReportBody : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public ReportBody(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The toJSON() method of the ReportBody interface is a serializer; it returns a JSON representation of the ReportBody object.
        /// </summary>
        /// <returns></returns>
        public T ToJSON<T>() => JSRef!.Call<T>("toJSON");
    }
}
