using SpawnDev.SpawnJS.JSObjects;
using System.Reflection;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Extension methods for Task
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Result property per Task type, resolved once
        /// </summary>
        private static readonly Dictionary<Type, PropertyInfo?> ResultProperties = new Dictionary<Type, PropertyInfo?>();
        /// <summary>
        /// Returns the result of an ALREADY COMPLETED Task, or null if the Task is not generic and so has
        /// no result.<br/>
        /// This is the synchronous counterpart to <see cref="GetResult"/>, which awaits and therefore
        /// returns a Task rather than a value. Use this one from inside a continuation, where the Task is
        /// known to be finished - calling GetResult there hands back the Task object itself, which then
        /// gets marshalled to Javascript in place of the actual result.
        /// </summary>
        public static object? GetCompletedResult(this Task _this)
        {
            var typeofTask = _this.GetType();
            if (!typeofTask.IsGenericType) return null;
            if (!ResultProperties.TryGetValue(typeofTask, out var resultProperty))
            {
                resultProperty = typeofTask.GetProperty("Result");
                ResultProperties[typeofTask] = resultProperty;
            }
            return resultProperty?.GetValue(_this, null);
        }

        /// <summary>
        /// Awaits the Task and returns its result, or null if the Task is not generic.<br/>
        /// Note this returns a Task: it has to await before it can read a result. From inside a
        /// continuation, where the Task has already finished, use <see cref="GetCompletedResult"/>.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static async Task<object?> GetResult(this Task _this)
        {
            await _this;
            var typeofTask = _this.GetType();
            if (!typeofTask.IsGenericType) return null;
            var resultProperty = typeofTask.GetProperty("Result");
            if (resultProperty == null) return null;
            var retValue = resultProperty.GetValue(_this, null);
            return retValue;
        }

        private static async Task<TResult> ConvertTaskObjectTyped<TResult>(Task<object?> task) => (TResult)(await task)!;
        private static async Task ConvertTaskObjectVoid(Task<object?> task) => await task;
        private static Dictionary<Type, MethodInfo?> ConvertTaskObjectTypedCache = new Dictionary<Type, MethodInfo?>();
        private static MethodInfo? ConvertTaskObjectTypedInfo { get; set; }
        /// <summary>
        /// Recasts a Task&lt;object?&gt; to a Task&lt;T&gt; where T is the specified type
        /// </summary>
        /// <param name="task"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object RecastTask(this Task<object?> task, Type type)
        {
            if (type == typeof(void)) return ConvertTaskObjectVoid(task);
            if (!ConvertTaskObjectTypedCache.TryGetValue(type, out MethodInfo? convertTaskObjectTyped))
            {
                ConvertTaskObjectTypedInfo ??= typeof(TaskExtensions).GetMethod(nameof(ConvertTaskObjectTyped), BindingFlags.NonPublic | BindingFlags.Static) ?? throw new Exception($"WorkerServiceProxy static constructor error");
                convertTaskObjectTyped = ConvertTaskObjectTypedInfo!.MakeGenericMethod(type);
                ConvertTaskObjectTypedCache[type] = convertTaskObjectTyped;
            }
            return convertTaskObjectTyped!.Invoke(null, new object?[] { task })!;
        }

        private static async ValueTask<TResult> ConvertTaskObjectTypedValueTask<TResult>(Task<object?> task) => (TResult)(await task)!;
        private static async ValueTask ConvertTaskObjectTypedValueTaskVoid(Task<object?> task) => await task;
        private static Dictionary<Type, MethodInfo?> ConvertTaskObjectTypedValueTaskCache = new Dictionary<Type, MethodInfo?>();
        private static MethodInfo? ConvertTaskObjectTypedValueTaskInfo { get; set; }
        /// <summary>
        /// Recasts a Task&lt;object?&gt; to a ValueTask&lt;T&gt; where T is the specified type
        /// </summary>
        /// <param name="task"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object? RecastValueTask(this Task<object?> task, Type type)
        {
            if (type == typeof(void)) return ConvertTaskObjectTypedValueTaskVoid(task);
            if (!ConvertTaskObjectTypedValueTaskCache.TryGetValue(type, out MethodInfo? convertTaskObjectTyped))
            {
                ConvertTaskObjectTypedValueTaskInfo ??= typeof(TaskExtensions).GetMethod(nameof(ConvertTaskObjectTypedValueTask), BindingFlags.NonPublic | BindingFlags.Static) ?? throw new Exception($"WorkerServiceProxy static constructor error");
                convertTaskObjectTyped = ConvertTaskObjectTypedValueTaskInfo!.MakeGenericMethod(type);
                ConvertTaskObjectTypedValueTaskCache[type] = convertTaskObjectTyped;
            }
            return convertTaskObjectTyped!.Invoke(null, new object?[] { task });
        }
    }
}
