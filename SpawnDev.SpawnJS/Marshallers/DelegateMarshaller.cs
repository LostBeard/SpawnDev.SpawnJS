using SpawnDev.SpawnJS.Marshaller;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Shared, non-generic <see cref="Delegate"/> -&gt; <see cref="Callback"/> machinery. One cache is shared
    /// across every closed <see cref="DelegateMarshaller{TDelegate}"/> so the same delegate reuses one JS
    /// function (matching the delegate-keyed reuse the event system uses). An <c>Action&lt;...&gt;</c> maps to
    /// the matching <c>ActionCallback&lt;...&gt;</c> and a <c>Func&lt;...&gt;</c> to <c>FuncCallback&lt;...&gt;</c>
    /// - their generic argument lists are identical, so the delegate's own type arguments close the callback.
    /// </summary>
    static class DelegateCallbacks
    {
        static readonly ConcurrentDictionary<Delegate, Callback> _cache = new ConcurrentDictionary<Delegate, Callback>();
        public static Callback GetOrCreate(Delegate value) => _cache.GetOrAdd(value, Build);
        // Open callback types by arg count. ActionCallback: 0..10 args (index = arg count).
        // FuncCallback: 1..11 generic args (T1..Tn + TResult); index = generic-arg count.
        static readonly Type?[] ActionOpen = {
            typeof(ActionCallback),
            typeof(ActionCallback<>), typeof(ActionCallback<,>), typeof(ActionCallback<,,>),
            typeof(ActionCallback<,,,>), typeof(ActionCallback<,,,,>), typeof(ActionCallback<,,,,,>),
            typeof(ActionCallback<,,,,,,>), typeof(ActionCallback<,,,,,,,>), typeof(ActionCallback<,,,,,,,,>),
            typeof(ActionCallback<,,,,,,,,,>),
        };
        static readonly Type?[] FuncOpen = {
            null,
            typeof(FuncCallback<>), typeof(FuncCallback<,>), typeof(FuncCallback<,,>),
            typeof(FuncCallback<,,,>), typeof(FuncCallback<,,,,>), typeof(FuncCallback<,,,,,>),
            typeof(FuncCallback<,,,,,,>), typeof(FuncCallback<,,,,,,,>), typeof(FuncCallback<,,,,,,,,>),
            typeof(FuncCallback<,,,,,,,,,>), typeof(FuncCallback<,,,,,,,,,,>),
        };
        [UnconditionalSuppressMessage("Trimming", "IL2055",
            Justification = "MakeGenericType over SpawnJS's own ActionCallback<>/FuncCallback<> family, closed with the delegate's own generic arguments.")]
        [UnconditionalSuppressMessage("Trimming", "IL2071",
            Justification = "The type arguments are the delegate's own generic arguments; the callback's PublicConstructors requirement (used only by the JS->.Net invoke path) is satisfied at runtime by the actual arg types.")]
        [UnconditionalSuppressMessage("Trimming", "IL2062",
            Justification = "The runtime Type is one of SpawnJS's own ActionCallback/FuncCallback family (selected from the typeof table above); its public (delegate, bool) ctor is what Build invokes. Verified to survive a trimmed WASM publish (the Func path is exercised only through this reflection).")]
        [UnconditionalSuppressMessage("Trimming", "IL2072",
            Justification = "Activator constructs SpawnJS's own ActionCallback/FuncCallback via the public (delegate, bool) ctor; the closed types come from the typeof table above. Verified to survive a trimmed WASM publish.")]
        static Callback Build(Delegate value)
        {
            var delType = value.GetType();
            var isFunc = (delType.IsGenericType ? delType.GetGenericTypeDefinition().FullName : delType.FullName)?
                .StartsWith("System.Func`", StringComparison.Ordinal) == true;
            var gargs = delType.IsGenericType ? delType.GetGenericArguments() : Type.EmptyTypes;
            var table = isFunc ? FuncOpen : ActionOpen;
            var open = gargs.Length < table.Length ? table[gargs.Length] : null;
            if (open == null)
                throw new NotSupportedException($"No callback type for delegate {delType.Name} (generic arity {gargs.Length}).");
            var callbackType = open.IsGenericTypeDefinition ? open.MakeGenericType(gargs) : open;
            return (Callback)Activator.CreateInstance(callbackType, new object?[] { value, false })!;
        }
    }
    /// <summary>
    /// Matches the <see cref="Action"/> / <see cref="Func{TResult}"/> delegate families and hands back a
    /// <see cref="DelegateMarshaller{TDelegate}"/> bound to the concrete delegate type.
    /// </summary>
    public class DelegateMarshallerFactory : JSMarshaller
    {
        public override bool CanMarshal(Type type) => IsActionOrFunc(type);
        internal static bool IsActionOrFunc(Type type)
        {
            if (type == null) return false;
            if (type == typeof(Action)) return true;
            if (!type.IsGenericType) return false;
            var name = type.GetGenericTypeDefinition().FullName;
            return name != null
                && (name.StartsWith("System.Action`", StringComparison.Ordinal)
                    || name.StartsWith("System.Func`", StringComparison.Ordinal));
        }
        [UnconditionalSuppressMessage("Trimming", "IL2055",
            Justification = "MakeGenericType over SpawnJS's own DelegateMarshaller<>, closed with the requested delegate type.")]
        [UnconditionalSuppressMessage("Trimming", "IL2072",
            Justification = "Activator over SpawnJS's own DelegateMarshaller<> (parameterless ctor), referenced via typeof here. Verified to survive a trimmed WASM publish.")]
        public override JSMarshaller<T> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            var marshallerTyped = typeof(DelegateMarshaller<>).MakeGenericType(typeof(T));
            return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
        }
    }
    /// <summary>
    /// Write path: converts an <see cref="Action"/>/<see cref="Func{TResult}"/> delegate to a JS function via
    /// a cached <see cref="Callback"/> and assigns it. Read path (JS function -&gt; typed delegate) mirrors
    /// <see cref="CallbackMarshaller{TCallback}"/> and is not implemented here - it needs a Function wrapper,
    /// which lives outside SpawnJS.Core.
    /// </summary>
    public class DelegateMarshaller<TDelegate> : JSMarshallerFromSpawnJSObjectReference<TDelegate?> where TDelegate : Delegate
    {
        public override TDelegate? JSToNet(SpawnJSObjectReference value)
            => throw new NotImplementedException($"Reading a JS function back into a {typeof(TDelegate).Name} is not supported by SpawnJS.Core.");
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TDelegate? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            jsParent.PropertySet(jsKey, DelegateCallbacks.GetOrCreate(value));
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TDelegate? value)
        {
            if (value == null) { jsParent.PropertySetNull(jsKey); return; }
            jsParent.PropertySet(jsKey, DelegateCallbacks.GetOrCreate(value));
        }
    }
}
