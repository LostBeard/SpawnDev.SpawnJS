using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
namespace SpawnDev.SpawnJS.JSObjects
{
    /// <summary>
    /// A GPUBindGroup defines a set of resources to be bound together in a group and how the resources are used in shader stages.
    /// https://www.w3.org/TR/webgpu/#gpubindgroup
    /// </summary>
    public class GPUBindGroup : GPUObjectBase
    {
        /// <inheritdoc/>
        public GPUBindGroup(SpawnJSObjectReference _ref) : base(_ref) { }
    }
}
