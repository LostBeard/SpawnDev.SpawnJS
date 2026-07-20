using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/Document/createElement#options
    /// </summary>
    public class ElementCreationOptions
    {
        /// <summary>
        /// The tag name of a custom element previously defined via customElements.define().
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Is { get; set; }
    }
}
