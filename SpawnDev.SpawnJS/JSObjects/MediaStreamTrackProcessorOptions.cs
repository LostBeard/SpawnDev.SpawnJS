// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// MediaStreamTrackProcessor constructor options.<br/>
    /// https://developer.mozilla.org/en-US/docs/Web/API/MediaStreamTrackProcessor/MediaStreamTrackProcessor#options
    /// </summary>
    public class MediaStreamTrackProcessorOptions
    {
        /// <summary>
        /// The MediaStreamTrack to be processed.
        /// </summary>
        public MediaStreamTrack? Track { get; set; }
        /// <summary>
        /// An integer specifying the maximum number of media frames to be buffered.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MaxBufferSize { get; set; }
    }
}
