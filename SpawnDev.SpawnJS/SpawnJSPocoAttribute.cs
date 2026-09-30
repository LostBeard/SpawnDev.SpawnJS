namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Asks SpawnDev.SpawnJS.Generators for a compile-time codec for this type (see <see cref="JSPocoCodec{T}"/>).
    /// <para>
    /// A POCO this assembly passes to or reads from SpawnJS - as a type argument, an argument, or a member of one - is
    /// found without it. Mark the types only the running code knows about: one held in an <c>object</c>, an interface
    /// or a base class member, or passed through an <c>object[]</c>. A type the generator cannot mirror exactly keeps
    /// the reflection plan either way.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class SpawnJSPocoAttribute : Attribute
    {
    }
}
