using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The CrashReportBody interface of the Reporting API represents the body of a crash report. A crash report is generated when a document (or a worker) crashes.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/CrashReportBody
    /// </summary>
    public class CrashReportBody : ReportBody
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public CrashReportBody(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// A string representing the reason for the crash. The possible values are: "oom" (Out of Memory) or "unresponsive".
        /// </summary>
        public string Reason => JSRef!.Get<string>("reason");
    }
}
