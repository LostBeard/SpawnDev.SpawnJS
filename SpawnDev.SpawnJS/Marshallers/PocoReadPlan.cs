using SpawnDev.SpawnJS.Marshaller;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// How a POCO type is read from the tape: an Object <see cref="JSSchema"/> - Javascript writes every member in the
    /// call's one crossing - and one typed setter per member. Built once per type per runtime.
    /// <para>
    /// Same members, names and types as v2's read (<c>TypeExtensions.GetTypeJsonProperties</c>, each member read
    /// by its DECLARED type's marshaller), and the same rule that a member read as null is not set, so it keeps its
    /// initializer. One deliberate difference: a property with no setter is left out. v2 tried to set it and threw.
    /// </para>
    /// </summary>
    internal abstract class PocoReadPlan
    {
        public JSSchema Schema { get; protected set; } = null!;
        public abstract object? ReadBoxed(ref JSTapeReader reader);
        protected abstract void ResolveMembers();

        // statics are per .Net runtime, so this is per SpawnJSRuntime
        static readonly ConcurrentDictionary<Type, PocoReadPlan> _plans = new ConcurrentDictionary<Type, PocoReadPlan>();

        /// <summary>
        /// The plan for <paramref name="type"/>. Cached BEFORE its members are resolved: a type that contains itself
        /// (directly, or through an array or another POCO) resolves to this same plan and schema instead of recursing.
        /// </summary>
        [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Closes SpawnJS's own plan over a POCO type; a consumer marshalling a POCO in a trimmed app preserves its members, the same contract v2's reflection walk had.")]
        [UnconditionalSuppressMessage("Trimming", "IL2055", Justification = "See IL2070.")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "The same runtime-Type instantiation v2's walk made through InvokeGeneric.")]
        public static PocoReadPlan For(Type type)
        {
            if (_plans.TryGetValue(type, out var plan)) return plan;
            var codec = JSPocoCodecs.Get(type);
            // a generated codec that reads; otherwise (none, or it left reading to reflection) the reflection plan
            plan = codec is { ReadNames: not null }
                ? (PocoReadPlan)Activator.CreateInstance(typeof(GeneratedReadPlan<>).MakeGenericType(type), codec)!
                : (PocoReadPlan)Activator.CreateInstance(typeof(PocoReadPlan<>).MakeGenericType(type))!;
            if (!_plans.TryAdd(type, plan)) return _plans[type];
            try
            {
                plan.ResolveMembers();
            }
            catch
            {
                // a half-resolved plan must not stay behind: every later read would use it
                _plans.TryRemove(type, out _);
                throw;
            }
            return plan;
        }
    }

    internal sealed class PocoReadPlan<TObj> : PocoReadPlan
    {
        readonly List<ClassMemberJsonInfo> _memberInfos;
        MemberReader<TObj>[] _members = System.Array.Empty<MemberReader<TObj>>();

        public PocoReadPlan()
        {
            // members that can be set: fields, and properties with a setter and no index parameters
            _memberInfos = typeof(TObj).GetTypeJsonProperties()
                .Where(m => m.FieldInfo != null || (m.PropertyInfo!.GetSetMethod(true) != null && m.PropertyInfo.GetIndexParameters().Length == 0))
                .ToList();
            Schema = JSSchema.Object(_memberInfos.Select(m => m.GetJsonName()).ToArray());
        }

        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "See PocoReadPlan.For.")]
        [UnconditionalSuppressMessage("Trimming", "IL2055", Justification = "See PocoReadPlan.For.")]
        [UnconditionalSuppressMessage("Trimming", "IL2076",
            Justification = "A member's type comes from reflection (PropertyType), which cannot carry DynamicallyAccessedMembers - the same boundary as v2's per-member Get<TMember>. Built-in wrapper constructors are preserved by the embedded ILLink.Descriptors.xml; a consumer POCO member of a custom wrapper type in a trimmed app must preserve that type's constructor itself.")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "See PocoReadPlan.For.")]
        protected override void ResolveMembers()
        {
            var members = new MemberReader<TObj>[_memberInfos.Count];
            for (var i = 0; i < members.Length; i++)
            {
                var info = _memberInfos[i];
                members[i] = info.PropertyInfo != null
                    ? (MemberReader<TObj>)Activator.CreateInstance(typeof(PropertyReader<,>).MakeGenericType(typeof(TObj), info.PropertyInfo.PropertyType), info.PropertyInfo.GetSetMethod(true)!)!
                    : new FieldReader<TObj>(info.FieldInfo!);
                Schema.Members![i] = members[i].Schema;
            }
            _members = members;
        }

        [UnconditionalSuppressMessage("Trimming", "IL2087", Justification = "TObj is PocoMarshaller<T>'s T (or its Nullable underlying struct), which carries PublicConstructors; the plan is closed over it by reflection, so the annotation cannot flow here.")]
        public bool Read(ref JSTapeReader reader, out TObj value)
        {
            if (!reader.ReadObjectStart())
            {
                value = default!;
                return false;
            }
            // v2 built the same way: a parameterless constructor, or a zeroed struct
            var obj = typeof(TObj).IsValueType ? default! : (TObj)Activator.CreateInstance(typeof(TObj))!;
            var members = _members;
            for (var i = 0; i < members.Length; i++) members[i].Read(ref reader, ref obj);
            value = obj;
            return true;
        }

        public override object? ReadBoxed(ref JSTapeReader reader) => Read(ref reader, out var value) ? value : null;
    }

    /// <summary>A type read by its generated <see cref="JSPocoCodec{T}"/>.</summary>
    internal sealed class GeneratedReadPlan<TObj> : PocoReadPlan
    {
        readonly JSPocoCodec<TObj> _codec;
        public GeneratedReadPlan(JSPocoCodec<TObj> codec)
        {
            _codec = codec;
            Schema = codec.ReadSchema;
        }
        protected override void ResolveMembers() => _codec.ResolveReadMembers();
        public bool Read(ref JSTapeReader reader, out TObj value)
        {
            if (!reader.ReadObjectStart())
            {
                value = default!;
                return false;
            }
            value = _codec.ReadMembers(ref reader);
            return true;
        }
        public override object? ReadBoxed(ref JSTapeReader reader) => Read(ref reader, out var value) ? value : null;
    }

    internal abstract class MemberReader<TObj>
    {
        public abstract JSSchema Schema { get; }
        public abstract void Read(ref JSTapeReader reader, ref TObj obj);
    }

    internal sealed class PropertyReader<TObj, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMember> : MemberReader<TObj>
    {
        delegate void StructSetter(ref TObj obj, TMember value);

        readonly JSMarshaller<TMember> _marshaller = SpawnJSRuntime.Instance.GetMarshaller<TMember>();
        readonly Action<TObj, TMember>? _setter;
        readonly StructSetter? _structSetter;

        public PropertyReader(MethodInfo setter)
        {
            // a struct's instance setter takes its target by reference
            if (typeof(TObj).IsValueType) _structSetter = (StructSetter)Delegate.CreateDelegate(typeof(StructSetter), setter);
            else _setter = (Action<TObj, TMember>)Delegate.CreateDelegate(typeof(Action<TObj, TMember>), setter);
        }

        public override JSSchema Schema => _marshaller.Schema;

        public override void Read(ref JSTapeReader reader, ref TObj obj)
        {
            var value = _marshaller.Read(ref reader);
            // v2: a member read as null is not set - it keeps its initializer
            if (value is null) return;
            if (_setter != null) _setter(obj, value);
            else _structSetter!(ref obj, value);
        }
    }

    /// <summary>A [JsonInclude] field: read by its type's marshaller, set by reflection as v2 set it.</summary>
    internal sealed class FieldReader<TObj> : MemberReader<TObj>
    {
        readonly FieldInfo _field;
        readonly JSMarshaller _marshaller;
        public FieldReader(FieldInfo field)
        {
            _field = field;
            _marshaller = SpawnJSRuntime.Instance.GetMarshaller(field.FieldType);
        }
        public override JSSchema Schema => _marshaller.Schema;
        public override void Read(ref JSTapeReader reader, ref TObj obj)
        {
            var value = _marshaller.ReadBoxed(ref reader);
            if (value is null) return;
            _field.SetValueDirect(__makeref(obj), value);
        }
    }
}
