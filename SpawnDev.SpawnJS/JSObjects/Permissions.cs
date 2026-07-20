// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The Permissions interface of the Permissions API provides the core Permission API functionality, such as methods for querying and revoking permissions<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/Permissions
    /// </summary>
    public class Permissions : SpawnJSObject
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public Permissions(SpawnJSObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Returns the user permission status for a given API.
        /// </summary>
        /// <param name="permissionDescriptor"></param>
        /// <returns></returns>
        public Task<PermissionStatus> Query(PermissionDescriptor permissionDescriptor) => JSRef!.CallAsync<PermissionStatus>("query", permissionDescriptor);
    }
}
