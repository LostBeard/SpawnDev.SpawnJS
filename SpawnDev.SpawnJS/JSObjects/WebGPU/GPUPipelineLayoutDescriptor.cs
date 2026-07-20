// <auto-ported> from SpawnDev.BlazorJS by Tools/PortJSObjects.cs - do not hand edit
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Toolbox;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// Describes the layout of a pipeline, which is used to create a GPUPipelineLayout object.<br/>
    /// https://www.w3.org/TR/webgpu/#dictdef-gpupipelinelayoutdescriptor
    /// </summary>
    public class GPUPipelineLayoutDescriptor : GPUObjectDescriptorBase
    {
        /// <summary>
        /// An array of GPUBindGroupLayout objects (which are in turn created via calls to GPUDevice.createBindGroupLayout()). 
        /// Each one corresponds to a @group attribute in the shader code contained in the GPUShaderModule used in a related pipeline.
        /// </summary>
        public IEnumerable<GPUBindGroupLayout>? BindGroupLayouts { get; set; }
    }
}
