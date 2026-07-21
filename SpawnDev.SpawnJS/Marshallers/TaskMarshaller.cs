using SpawnDev.SpawnJS.JSObjects;
using System.Collections.Concurrent;
using System.Reflection;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls .Net <see cref="Task"/> and <see cref="Task{TResult}"/> as a Javascript Promise, and a
    /// Javascript Promise back as a Task.<br/>
    /// <br/>
    /// Task and Promise are the same idea on either side of the boundary, so they are marshalled as each
    /// other. This matters most where a .Net method is called BY Javascript: a callback returning a Task
    /// hands Javascript a Promise it can await, which is what any Javascript caller expects. Before this
    /// existed a Task was a plain class to the graph, so ObjectMarshaller claimed it and property-walked
    /// it into <c>{result, id, status, ...}</c> - well formed, silently wrong, and no exception.<br/>
    /// <br/>
    /// Reading is lazy in the right way: the Promise's continuation is attached immediately and a Task is
    /// returned straight away, so nothing blocks. The Task completes when the Promise settles, and a
    /// rejected Promise faults the Task.
    /// </summary>
    public class TaskMarshaller : JSMarshaller
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type? type) => type != null && typeof(Task).IsAssignableFrom(type);

        /// <inheritdoc/>
        public override object? JSToNet(Type type, SpawnJSHandle jsHandle)
        {
            // read the value as a Promise first - it is a JSObject like any other, so the graph does it
            using var promise = (Promise?)JS.MarshallJSToNet(typeof(Promise), jsHandle);
            if (promise == null) return null;

            // Task<T> carries a result to marshal; a plain Task carries only completion
            var resultType = type.AsyncReturnType();
            if (resultType == null || resultType.IsVoid()) return promise.ThenAsync();

            // the GENERIC ThenAsync<T> is used rather than ThenAsync(Type): it types the resolved value
            // as T, and it hands back a Task<T> directly, so nothing has to adapt a Task<object?>
            return ThenAsyncFor(resultType).Invoke(promise, new object[] { 0 });
        }

        /// <inheritdoc/>
        public override void NetToJS(Type? type, SpawnJSHandle jsParent, object jsKey, object? value)
        {
            if (value is Task task)
            {
                // Promise(Task) already resolves with the Task's result when there is one, and rejects on
                // fault or cancellation. Disposing the wrapper afterwards is safe: the property assignment
                // means Javascript holds the promise, and the wrapper is only this side's handle to it.
                using var promise = new Promise(task);
                JS.MarshallNetToJS(jsParent, jsKey, promise);
            }
            else
            {
                Reflect.Set(jsParent.JSObjectRequired, jsKey, (string?)null);
            }
        }

        static readonly ConcurrentDictionary<Type, MethodInfo> _thenAsyncByResultType = new();

        /// <summary>
        /// <c>Promise.ThenAsync&lt;TResult&gt;(int)</c> closed over the given result type, resolved once
        /// per type. Selected by shape rather than by a single GetMethod call, because ThenAsync has
        /// several overloads and only one of them is generic with a single int parameter.
        /// </summary>
        static MethodInfo ThenAsyncFor(Type resultType) => _thenAsyncByResultType.GetOrAdd(resultType, t =>
            typeof(Promise).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Single(m => m.Name == nameof(Promise.ThenAsync)
                          && m.IsGenericMethodDefinition
                          && m.GetParameters() is { Length: 1 } p
                          && p[0].ParameterType == typeof(int))
                .MakeGenericMethod(t));
    }
}
