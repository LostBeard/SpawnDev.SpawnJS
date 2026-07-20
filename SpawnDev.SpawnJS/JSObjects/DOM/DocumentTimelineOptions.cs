// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;
/// <summary>
/// An object specifying options for the new timeline.
/// </summary>
public class DocumentTimelineOptions
{
    /// <summary>
    /// A number that specifies the zero time for the DocumentTimeline as a number of milliseconds relative to Performance.timeOrigin. Defaults to 0.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? OriginTime { get; set; }
}
