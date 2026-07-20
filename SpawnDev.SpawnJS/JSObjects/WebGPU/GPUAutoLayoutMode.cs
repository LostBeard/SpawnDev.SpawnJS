// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// https://www.w3.org/TR/webgpu/#enumdef-gpuautolayoutmode
    /// </summary>
    public enum GPUAutoLayoutMode
    {
        /// <summary>
        /// Note: If "auto" is used the pipeline cannot share GPUBindGroups with any other pipelines.
        /// </summary>
        [JsonPropertyName("auto")]
        Auto,
    }
}
