// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Represents a fragment of data or a computational unit intended for processing on a GPU.
    /// https://www.w3.org/TR/webgpu/#dictdef-gpufragmentstate
    /// </summary>
    public class GPUFragmentState : GPUProgrammableStage
    {
        /// <summary>
        /// An array of objects representing color states that represent configuration details for the 
        /// colors output by the fragment shader stage.
        /// </summary>
        [JsonPropertyName("targets")]
        public IEnumerable<GPUColorTargetState>? Targets { get; set; }
    }
}
