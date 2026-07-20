// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// GPUFrontFace defines which polygons are considered front-facing by a GPURenderPipeline. See § 23.2.5.4 Polygon Rasterization for additional details:<br/>
    /// https://www.w3.org/TR/webgpu/#enumdef-gpufrontface
    /// </summary>
    public enum GPUFrontFace
    {
        /// <summary>
        /// Polygons with vertices whose framebuffer coordinates are given in counter-clockwise order are considered front-facing.
        /// </summary>
        [JsonPropertyName("ccw")]
        CCW,
        /// <summary>
        /// Polygons with vertices whose framebuffer coordinates are given in clockwise order are considered front-facing.
        /// </summary>
        [JsonPropertyName("cw")]
        CW,
    }
}
