// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// GPUErrorFilter defines the type of errors that should be caught when calling pushErrorScope():<br/>
    /// https://www.w3.org/TR/webgpu/#enumdef-gpuerrorfilter
    /// </summary>
    public enum GPUErrorFilter
    {
        /// <summary>
        /// Indicates that the error scope will catch a GPUValidationError.
        /// </summary>
        [JsonPropertyName("validation")]
        Validation,
        /// <summary>
        /// Indicates that the error scope will catch a GPUOutOfMemoryError.
        /// </summary>
        [JsonPropertyName("out-of-memory")]
        OutOfMemory,
        /// <summary>
        /// Indicates that the error scope will catch a GPUInternalError.
        /// </summary>
        [JsonPropertyName("internal")]
        Internal,
    }
}
