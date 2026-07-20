// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The PasswordCredentialData interface of the Credential Management API is the dictionary containing the data used to create a PasswordCredential.
    /// </summary>
    public class PasswordCredentialData
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; } = "";
        /// <summary>
        /// IconURL
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconURL { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; } = "";
    }
}
