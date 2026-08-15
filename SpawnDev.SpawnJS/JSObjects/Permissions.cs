
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
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
        public Task<PermissionStatus> Query(PermissionDescriptor permissionDescriptor) => JSRef!.CallAsync<global::SpawnDev.SpawnJS.JSObjects.PermissionDescriptor, PermissionStatus>("query", permissionDescriptor);
    }
}
