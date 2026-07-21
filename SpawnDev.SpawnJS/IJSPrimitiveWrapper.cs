namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Marks a wrapper type that represents a Javascript PRIMITIVE rather than an object - a string, a
    /// number, a boolean, a symbol.<br/>
    /// <br/>
    /// Reading a wrapper normally refuses a non-reference value, so <c>JS.Get&lt;Window&gt;("someNumber")</c>
    /// fails loudly rather than handing back a wrapper whose every property reads undefined. A handful of
    /// types exist precisely to carry a primitive though - <see cref="JSObjects.StringPrimitive"/> is the
    /// one in the library today - and for those the refusal is wrong.<br/>
    /// <br/>
    /// This is possible at all because a SLOT holds any Javascript value. The restriction was never the
    /// library's: it is the runtime's JSObject PROXY that cannot represent a primitive, which is what
    /// raises "JSObject proxy of string is not supported". Once a wrapper is addressed by slot rather than
    /// by proxy, a primitive is an ordinary value.<br/>
    /// <br/>
    /// A marker interface rather than a type check in the marshaller, because a hardcoded list of type
    /// names is exactly the kind of thing that silently drifts as types are added.
    /// </summary>
    public interface IJSPrimitiveWrapper
    {
    }
}
