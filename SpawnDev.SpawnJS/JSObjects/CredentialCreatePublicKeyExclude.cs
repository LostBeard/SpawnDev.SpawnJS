// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Options for CredentialCreatePublicKey.ExcludeCredentials<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/CredentialsContainer/create#excludecredentials
    /// </summary>
    public class CredentialCreatePublicKeyExclude
    {
        /// <summary>
        /// An ArrayBuffer, TypedArray, or DataView representing the existing credential ID.
        /// </summary>
        public required BufferSource Id { get; set; }
        /// <summary>
        /// An Array of strings representing allowed transports. Possible transports are: "ble", "hybrid", "internal", "nfc", and "usb" (see getTransports() for more details).
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Transports { get; set; }
        /// <summary>
        /// A string defining the type of public key credential to create. This can currently take a single value, "public-key", but more values may be added in the future.
        /// </summary>
        public string Type { get; set; } = "public-key";
    }
}
