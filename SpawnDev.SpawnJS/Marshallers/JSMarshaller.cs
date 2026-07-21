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
