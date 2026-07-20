// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    ///  Defines the layout of the vertex attributes within each structure. 
    ///  https://www.w3.org/TR/webgpu/#dictdef-gpuvertexattribute
    /// </summary>
    public class GPUVertexAttribute
    {
        /// <summary>
        /// The data format for this attribute. 
        /// </summary>
        public EnumString<GPUVertexFormat> Format { get; set; }

        /// <summary>
        /// The byte offset of this attribute within the vertex structure. 
        /// </summary>

        public GPUSize64 Offset { get; set; }

        /// <summary>
        /// The shader location that this attribute will be bound to. 
        /// </summary>
        public GPUIndex32 ShaderLocation { get; set; }
    }
}