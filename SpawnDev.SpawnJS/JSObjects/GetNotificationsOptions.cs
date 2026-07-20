// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// An object containing options to filter the notifications returned.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/ServiceWorkerRegistration/getNotifications#options
    /// </summary>
    public class GetNotificationsOptions
    {
        /// <summary>
        /// A string representing a notification tag. If specified, only notifications that have this tag will be returned.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Tag { get; set; }

    }
}
