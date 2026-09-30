using SpawnDev.SpawnJS.Marshaller;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// How a POCO type is written to the tape: its <see cref="JSShape"/> - the Javascript member names, which cross once
    /// per runtime instead of with every object - and one typed writer per member. Built once per type per runtime.
    /// <para>
    /// The members, their names, their order and their [JsonIgnore] rules are exactly what v2's property walk used
    /// (<c>TypeExtensions.GetTypeJsonProperties</c>). What changes is the reading: a property getter becomes a
    /// typed delegate, so a member is read without a reflection invoke and without boxing, and its value goes through
    /// a <see cref="ValueWriter{T}"/> - the declared type's marshaller when that type fixes the runtime type, what the
    /// value IS otherwise.
    /// </para>
    /// </summary>
    internal abstract class PocoWritePlan
    {
        public abstract void WriteBoxed(JSTape tape, object value);

        // statics are per .Net runtime, so this is per SpawnJSRuntime - and so are the shapes the plans hold
        static readonly ConcurrentDictionary<Type, PocoWritePlan> _plans = new ConcurrentDictionary<Type, PocoWritePlan>();

        [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Closes SpawnJS's own plan over a POCO type; a consumer marshalling a POCO in a trimmed app preserves its accessors, the same contract v2's reflection walk had.")]
        [UnconditionalSuppressMessage("Trimming", "IL2055", Justification = "See IL2070.")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "The same runtime-Type instantiation v2's walk made through InvokeGeneric.")]
        public static PocoWritePlan For(Type type) => _plans.GetOrAdd(type,
            t => (PocoWritePlan)Activator.CreateInstance(typeof(PocoWritePlan<>).MakeGenericType(t))!);
    }

    internal sealed class PocoWritePlan<TObj> : PocoWritePlan
    {
        readonly JSShape _shape;
        readonly MemberWriter<TObj>[] _members;

        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "See PocoWritePlan.For.")]
        [UnconditionalSuppressMessage("Trimming", "IL2055", Justification = "See PocoWritePlan.For.")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "See PocoWritePlan.For.")]
        public PocoWritePlan()
        {
            var members = typeof(TObj).GetTypeJsonProperties();
            var names = new string[members.Count];
            _members = new MemberWriter<TObj>[members.Count];
            for (var i = 0; i < members.Count; i++)
            {
                var member = members[i];
                names[i] = member.GetJsonName();
                var getter = member.PropertyInfo?.GetGetMethod(true);
                // a getter a delegate can bind: an instance getter with no index parameters
                _members[i] = getter != null && !getter.IsStatic && getter.GetParameters().Length == 0
                    ? (MemberWriter<TObj>)Activator.CreateInstance(typeof(PropertyWriter<,>).MakeGenericType(typeof(TObj), member.PropertyInfo!.PropertyType), member, getter)!
                    : new ReflectionWriter<TObj>(member);
            }
            _shape = new JSShape(names);
        }

        public void Write(JSTape tape, TObj value)
        {
            tape.WriteObject(_shape);
            var members = _members;
            for (var i = 0; i < members.Length; i++) members[i].Write(tape, value);
        }

        public override void WriteBoxed(JSTape tape, object value) => Write(tape, (TObj)value);
    }

    internal abstract class MemberWriter<TObj>
    {
        public abstract void Write(JSTape tape, TObj obj);

        /// <summary>
        /// [JsonIgnore] Always members are already left out of the walk; WhenWritingNull / WhenWritingDefault are
        /// decided per value, exactly as ClassMemberJsonInfo.GetShouldWrite does.
        /// </summary>
        protected static JsonIgnoreCondition Condition(ClassMemberJsonInfo member)
            => member.JsonIgnoreAttribute?.Condition ?? JsonIgnoreCondition.Never;
    }

    internal sealed class PropertyWriter<TObj, TMember> : MemberWriter<TObj>
    {
        delegate TMember StructGetter(ref TObj obj);

        readonly Func<TObj, TMember>? _getter;
        readonly StructGetter? _structGetter;
        readonly JsonIgnoreCondition _condition;
        readonly ValueWriter<TMember> _writer = new ValueWriter<TMember>();

        public PropertyWriter(ClassMemberJsonInfo member, MethodInfo getter)
        {
            _condition = Condition(member);
            // a struct's instance getter takes its target by reference
            if (typeof(TObj).IsValueType) _structGetter = (StructGetter)Delegate.CreateDelegate(typeof(StructGetter), getter);
            else _getter = (Func<TObj, TMember>)Delegate.CreateDelegate(typeof(Func<TObj, TMember>), getter);
        }

        public override void Write(JSTape tape, TObj obj)
        {
            var value = _getter != null ? _getter(obj) : _structGetter!(ref obj);
            switch (_condition)
            {
                case JsonIgnoreCondition.WhenWritingNull when value is null:
                case JsonIgnoreCondition.WhenWritingDefault when EqualityComparer<TMember>.Default.Equals(value, default!):
                    tape.WriteAbsent();
                    return;
            }
            // v2's walk wrote a null member as JS null before it chose any marshaller
            if (value is null) { tape.WriteNull(); return; }
            _writer.Write(tape, value);
        }
    }

    /// <summary>A field, or a property a delegate cannot bind: read by reflection, as v2 read every member.</summary>
    internal sealed class ReflectionWriter<TObj> : MemberWriter<TObj>
    {
        readonly ClassMemberJsonInfo _member;
        public ReflectionWriter(ClassMemberJsonInfo member) => _member = member;
        public override void Write(JSTape tape, TObj obj)
        {
            var value = _member.PropertyInfo != null ? _member.PropertyInfo.GetValue(obj) : _member.FieldInfo!.GetValue(obj);
            if (!_member.GetShouldWrite(value)) { tape.WriteAbsent(); return; }
            tape.WriteValue(value);
        }
    }
}
