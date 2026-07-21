// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A GPUQuerySetDescriptor specifies the options to use in creating a GPUQuerySet.<br/>
    /// https://www.w3.org/TR/webgpu/#dictdef-gpuquerysetdescriptor
    /// </summary>
    public class GPUQuerySetDescriptor : GPUObjectDescriptorBase
    {
        /// <summary>
        /// The type of queries managed by GPUQuerySet.
        /// </summary>
        public required EnumString<GPUQueryType> Type { get; set; }
        /// <summary>
        /// The number of queries managed by GPUQuerySet.
        /// </summary>
        public GPUSize32 Count { get; set; }
    }
}
