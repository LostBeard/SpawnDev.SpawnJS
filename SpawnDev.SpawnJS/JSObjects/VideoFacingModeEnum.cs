// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// The VideoFacingModeEnum enum of the Media Statistics API is used to describe the facing mode of a video source.
    /// </summary>
    public enum VideoFacingModeEnum
    {
        /// <summary>
        /// The source is facing toward the user (a self-view camera).
        /// </summary>
        [JsonPropertyName("user")]
        User,
        /// <summary>
        /// The source is facing away from the user (viewing the environment).
        /// </summary>
        [JsonPropertyName("environment")]
        Environment,
        /// <summary>
        /// The source is facing to the left of the user.
        /// </summary>
        [JsonPropertyName("left")]
        Left,
        /// <summary>
        /// The source is facing to the right of the user.
        /// </summary>
        [JsonPropertyName("right")]
        Right,
    }
}
