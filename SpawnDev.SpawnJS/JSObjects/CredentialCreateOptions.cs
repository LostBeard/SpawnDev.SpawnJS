// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Options used for CredentialsContainer.Create()
    /// https://developer.mozilla.org/en-US/docs/Web/API/CredentialsContainer/create#options
    /// </summary>
    public abstract class CredentialCreateOptions
    {
        /// <summary>
        /// An AbortSignal object instance that allows an ongoing create() operation to be aborted. An aborted operation may complete normally (generally if the abort was received after the operation finished) or reject with an "AbortError" DOMException.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AbortSignal? Signal { get; set; }
    }
}
