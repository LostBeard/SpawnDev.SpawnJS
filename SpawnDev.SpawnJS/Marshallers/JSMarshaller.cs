using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals data between .Net and Javascript using SpawnJSHandle
    /// </summary>
    public abstract class JSMarshaller
    {
        /// <summary>
        /// SpawnJSRuntime
        /// </summary>
        public SpawnJSRuntime JS => SpawnJSRuntime.Instance ?? throw new InvalidOperationException("SpawnJSRuntime has not been created.");
        /// <summary>
        /// Returns true if the data type can be marshalled.<br/>
        /// <paramref name="typeToConvert"/> may be null when the .Net value being marshalled is null.
        /// </summary>
        public abstract bool CanMarshal(Type? typeToConvert);
        /// <summary>
        /// If this class reported true to CanMarshal, GetMarshaller may be called to get the marshaller to do the marshalling<br/>
        /// </summary>
        /// <returns></returns>
        public virtual JSMarshaller GetMarshaller(Type? typeToConvert) => this;
        /// <summary>
        /// Given a JS parent object, and the JS property key: read the value.<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public abstract object? JSToNet(Type typeToConvert, SpawnJSHandle jsHandle);
        /// <summary>
        /// Given a JS parent object, the JS property key, and the .Net value: write the value.<br/>
        /// <paramref name="typeToConvert"/> and <paramref name="value"/> may be null when the .Net value being marshalled is null.
        /// </summary>
        public abstract void NetToJS(Type? typeToConvert, SpawnJSHandle jsParent, object jsKey, object? value);
        /// <summary>
        /// Offers this value as a single ARGUMENT-FRAME slot: a tag and eight bytes, written into .Net's
        /// own memory with no boundary crossing at all.<br/>
        /// <br/>
        /// Returning false is always safe and is the default. The caller then builds the value Javascript
        /// side the usual way - <see cref="NetToJS"/> into the scratch array - and passes its index, which
        /// costs exactly what it costs today. So this is an opt-in fast lane, not a second contract every
        /// marshaller has to satisfy.<br/>
        /// <br/>
        /// Worth implementing wherever the value is ALREADY expressible as one number: a number, a
        /// boolean, a numeric enum, or anything that already holds a slot id - a wrapper, a handle, an
        /// interned string. That covers essentially everything a GPU dispatch passes, which is why the
        /// argument transport gets to be free for the calls that matter.<br/>
        /// <br/>
        /// Do NOT implement it for a value that has to be CONSTRUCTED in Javascript - a descriptor object,
        /// an array. Those have to be built there whatever the transport does.
        /// </summary>
        /// <param name="typeToConvert">The value's runtime type</param>
        /// <param name="value">The value, never null - null is handled by the caller</param>
        /// <param name="tag">One of the <see cref="ArgTag"/> values</param>
        /// <param name="payload">The number that carries it: the value itself, or a slot id</param>
        public virtual bool TryWriteArg(Type? typeToConvert, object value, out byte tag, out double payload)
        {
            tag = 0;
            payload = 0;
            return false;
        }
        /// <summary>
        /// Writes one member of an object or struct being cloned into Javascript.<br/>
        /// A member whose declared type cannot be derived from resolves its marshaller once and keeps it -
        /// the answer cannot change, because the value's runtime type is pinned by the declaration. Every
        /// other member, and every null, goes the general route and is typed by the value itself.
        /// </summary>
        protected void WriteMember(ClassMemberJsonInfo member, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value != null && member.RuntimeTypeIsKnown)
            {
                var marshaller = member.CachedMarshaller ??= JS.GetMarshaller(member.MemberType);
                marshaller.NetToJS(member.MemberType, jsParent, jsKey, value);
                return;
            }
            JS.MarshallNetToJS(jsParent, jsKey, value);
        }
    }

    /// <summary>
    /// Marshals data between .Net and Javascript using SpawnJSHandle
    /// </summary>
    public abstract class JSMarshaller<TType> : JSMarshaller
    {
        /// <summary>
        /// Returns true if the data type can be marshalled
        /// </summary>
        public override bool CanMarshal(Type? typeToConvert) => typeof(TType) == typeToConvert;
    }
}
