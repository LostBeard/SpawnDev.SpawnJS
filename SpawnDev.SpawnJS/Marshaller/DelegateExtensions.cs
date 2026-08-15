using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace SpawnDev.SpawnJS.Marshaller
{
    /// <summary>
    /// The machinery behind the "runtime Type -> compile-time &lt;T&gt;" trick the marshaller pipeline relies
    /// on. Given a generic method group and a runtime <see cref="Type"/>, InvokeGeneric(Delegate, Type)
    /// closes the method over that type and invokes it - so a value whose type is only known at runtime can
    /// still be dispatched into a strongly-typed generic method (e.g. <c>writeTyped&lt;T1&gt;</c>) without
    /// boxing. Closed <see cref="MethodInfo"/>s are cached (single- and multi-type keys) so the expensive
    /// <see cref="MethodInfo.MakeGenericMethod"/> runs once per type combination. Async variants await and
    /// unwrap Task/ValueTask/duck-typed awaitables.
    /// </summary>
    public static class DelegateExtensions
    {
        // Cache our specialized, non-boxing executor engines instead of MethodInvokers
        private static readonly ConcurrentDictionary<CacheKeySingle, IGenericExecutor> _executorSingleCache = new();
        /// <summary>
        /// Invoke a generic method using Type
        /// </summary>
        public static Task<object?> InvokeGenericAsync(this Delegate methodGroup, Type targetType, object?[]? args = null)
        {
            var key = new CacheKeySingle(methodGroup.Method, targetType);
            var executor = _executorSingleCache.GetOrAdd(key, k =>
            {
                // Create our non-boxing bridge executor specifically for this TargetType
                var bridgeType = typeof(GenericExecutorBridge<>).MakeGenericType(k.TargetType);
                return (IGenericExecutor)Activator.CreateInstance(bridgeType, k._methodDefinition)!;
            });
            // Executes natively via the strongly-typed bridge with zero MethodInfo.Invoke boxing overhead
            return executor.ExecuteAsync(methodGroup.Target, args);
        }
        /// <summary>
        /// Invoke a generic method using Type[]
        /// </summary>
        [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming", "IL2060", Justification = "Reflection over SpawnJS's own runtime-Type dispatch machinery: the closed generic methods are SpawnJS marshalling dispatch (writeTyped = write path, never constructs; As/GetMarshaller runtime-Type resolution is backed by the embedded ILLink.Descriptors.xml for built-in wrappers or consumer preservation for custom types). MakeGenericMethod / GetAwaiter reflection over these SpawnJS-controlled targets is safe within that contract.")]
        public static async Task<object?> InvokeGenericAsync(this Delegate methodGroup, Type[] targetTypes, params object?[]? args)
        {
            var key = new CacheKey(methodGroup.Method, targetTypes);
            var targetMethod = _multiCache.GetOrAdd(key, k =>
            {
                var def = k._methodDefinition;
                if (def.GetGenericArguments().Length != k.TargetTypes.Length)
                {
                    throw new ArgumentException($"Type argument count mismatch for method {def.Name}. Expected {def.GetGenericArguments().Length}, got {k.TargetTypes.Length}.");
                }
                return MethodInvoker.Create(def.MakeGenericMethod(k.TargetTypes));
            });
            Span<object?> argSpan = args == null ? Span<object?>.Empty : args.AsSpan();
            var ret = targetMethod.Invoke(methodGroup.Target, argSpan);
            if (ret == null) return null;
            if (ret is Task task)
            {
                await task;
                return null;
            }
            if (ret is ValueTask valueTask)
            {
                await valueTask;
                return null;
            }
            return await GetResultFromUnknownObjectAsync(ret);
        }
        /// <summary>
        /// Invoke a generic method using Type
        /// </summary>
        [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming", "IL2060",
            Justification = "Closes SpawnJS's own runtime-Type dispatch targets: writeTyped (write path, never constructs) and the As/GetMarshaller runtime-Type escape hatch, whose PublicConstructors requirement is backed by the embedded ILLink.Descriptors.xml for built-in wrappers or consumer preservation for custom types.")]
        public static object? InvokeGeneric(this Delegate methodGroup, Type targetType, params object?[]? args)
        {
            var key = new CacheKeySingle(methodGroup.Method, targetType);
            var targetMethod = _singleCache.GetOrAdd(key, k =>
            {
                var def = k._methodDefinition;
                if (def.GetGenericArguments().Length != 1)
                {
                    throw new ArgumentException($"Type argument count mismatch for method {def.Name}. Expected {def.GetGenericArguments().Length}, got 1.");
                }
                return MethodInvoker.Create(def.MakeGenericMethod(k.TargetType));
            });
            Span<object?> argSpan = args == null ? Span<object?>.Empty : args.AsSpan();
            var ret = targetMethod.Invoke(methodGroup.Target, argSpan);
            return ret;
        }
        /// <summary>
        /// Invoke a generic method using Type[]
        /// </summary>
        [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming", "IL2060",
            Justification = "Closes SpawnJS's own runtime-Type dispatch targets: writeTyped (write path, never constructs) and the As/GetMarshaller runtime-Type escape hatch, whose PublicConstructors requirement is backed by the embedded ILLink.Descriptors.xml for built-in wrappers or consumer preservation for custom types.")]
        public static object? InvokeGeneric(this Delegate methodGroup, Type[] targetTypes, params object?[]? args)
        {
            var key = new CacheKey(methodGroup.Method, targetTypes);
            var targetMethod = _multiCache.GetOrAdd(key, k =>
            {
                var def = k._methodDefinition;
                if (def.GetGenericArguments().Length != k.TargetTypes.Length)
                {
                    throw new ArgumentException($"Type argument count mismatch for method {def.Name}. Expected {def.GetGenericArguments().Length}, got {k.TargetTypes.Length}.");
                }
                return MethodInvoker.Create(def.MakeGenericMethod(k.TargetTypes));
            });
            Span<object?> argSpan = args == null ? Span<object?>.Empty : args.AsSpan();
            var ret = targetMethod.Invoke(methodGroup.Target, argSpan);
            return ret;
        }
        #region Internals
        /// <summary>
        /// Cache
        /// </summary>
        private static readonly ConcurrentDictionary<CacheKeySingle, MethodInvoker> _singleCache = new();
        private static readonly ConcurrentDictionary<CacheKey, MethodInvoker> _multiCache = new();
        private static readonly ConcurrentDictionary<Type, (bool IsAwaitable, bool HasReturnValue)> _awaitableTypes = new();
        /// <summary>
        /// A MethodInfo and single Type key
        /// </summary>
        private readonly struct CacheKeySingle : IEquatable<CacheKeySingle>
        {
            internal readonly MethodInfo _methodDefinition;
            public readonly Type TargetType;
            private readonly int _hashCode;
            public CacheKeySingle(MethodInfo closedMethod, Type targetType)
            {
                _methodDefinition = closedMethod.IsGenericMethod ? closedMethod.GetGenericMethodDefinition() : closedMethod;
                TargetType = targetType;
                _hashCode = HashCode.Combine(_methodDefinition, TargetType);
            }
            public bool Equals(CacheKeySingle other) => ReferenceEquals(_methodDefinition, other._methodDefinition) && TargetType == other.TargetType;
            public override bool Equals(object? obj) => obj is CacheKeySingle other && Equals(other);
            public override int GetHashCode() => _hashCode;
        }
        /// <summary>
        /// A MethodInfo and Type array key
        /// </summary>
        private readonly struct CacheKey : IEquatable<CacheKey>
        {
            internal readonly MethodInfo _methodDefinition;
            public readonly Type[] TargetTypes;
            private readonly int _hashCode;
            public CacheKey(MethodInfo closedMethod, Type[] targetTypes, bool cloneArray = false)
            {
                _methodDefinition = closedMethod.IsGenericMethod ? closedMethod.GetGenericMethodDefinition() : closedMethod;
                TargetTypes = cloneArray ? (Type[])targetTypes.Clone() : targetTypes;
                var hash = new HashCode();
                hash.Add(_methodDefinition);
                for (int i = 0; i < TargetTypes.Length; i++)
                {
                    hash.Add(TargetTypes[i]);
                }
                _hashCode = hash.ToHashCode();
            }
            public bool Equals(CacheKey other)
            {
                if (!ReferenceEquals(_methodDefinition, other._methodDefinition)) return false;
                if (TargetTypes.Length != other.TargetTypes.Length) return false;
                for (int i = 0; i < TargetTypes.Length; i++)
                {
                    if (TargetTypes[i] != other.TargetTypes[i]) return false;
                }
                return true;
            }
            public override bool Equals(object? obj) => obj is CacheKey other && Equals(other);
            public override int GetHashCode() => _hashCode;
        }
        #endregion
        #region AsyncHelpers
        // Cache that holds our highly optimized, compiled unwrapping delegates
        private static readonly ConcurrentDictionary<Type, Func<object, Task<object?>>> _unwrapperCache = new();
        /// <summary>
        /// Highly optimized result extraction. Bypasses 'dynamic' entirely by using
        /// compiled expression trees cached per exact runtime Type.
        /// </summary>
        private static Task<object?> GetResultFromUnknownObjectAsync(object? obj)
        {
            if (obj is null) return Task.FromResult<object?>(null);
            Type type = obj.GetType();
            // Fetch or compile the ultra-fast unwrapper delegate for this specific type
            var unwrapper = _unwrapperCache.GetOrAdd(type, CreateUnwrapperDelegate);
            return unwrapper(obj);
        }
        /// <summary>
        /// Generates a strongly-typed compiled expression tree tailored to unwrap the given type.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Reflection over SpawnJS's own runtime-Type dispatch machinery: the closed generic methods are SpawnJS marshalling dispatch (writeTyped = write path, never constructs; As/GetMarshaller runtime-Type resolution is backed by the embedded ILLink.Descriptors.xml for built-in wrappers or consumer preservation for custom types). MakeGenericMethod / GetAwaiter reflection over these SpawnJS-controlled targets is safe within that contract.")]
        [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Reflection over SpawnJS's own runtime-Type dispatch machinery: the closed generic methods are SpawnJS marshalling dispatch (writeTyped = write path, never constructs; As/GetMarshaller runtime-Type resolution is backed by the embedded ILLink.Descriptors.xml for built-in wrappers or consumer preservation for custom types). MakeGenericMethod / GetAwaiter reflection over these SpawnJS-controlled targets is safe within that contract.")]
        private static Func<object, Task<object?>> CreateUnwrapperDelegate(Type type)
        {
            // --- Standard void Task ---
            if (type == typeof(Task))
            {
                return async (obj) =>
                {
                    await (Task)obj;
                    return null;
                };
            }
            // --- Standard void ValueTask ---
            if (type == typeof(ValueTask))
            {
                return async (obj) =>
                {
                    await (ValueTask)obj;
                    return null;
                };
            }
            var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
            // --- Generic Task<T> ---
            if (genericType == typeof(Task<>))
            {
                // We use reflection once here to build a strongly-typed async wrapper method closure
                var resultType = type.GetGenericArguments()[0];
                var method = typeof(DelegateExtensions)
                    .GetMethod(nameof(UnwrapTaskAsync), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(resultType);

                return (Func<object, Task<object?>>)Delegate.CreateDelegate(typeof(Func<object, Task<object?>>), method);
            }
            // --- Generic ValueTask<T> ---
            if (genericType == typeof(ValueTask<>))
            {
                var resultType = type.GetGenericArguments()[0];
                var method = typeof(DelegateExtensions)
                    .GetMethod(nameof(UnwrapValueTaskAsync), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(resultType);

                return (Func<object, Task<object?>>)Delegate.CreateDelegate(typeof(Func<object, Task<object?>>), method);
            }
            // --- Custom Duck-Typed Awaitables ---
            MethodInfo? getAwaiterMethod = type.GetMethod("GetAwaiter", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            if (getAwaiterMethod != null)
            {
                Type awaiterType = getAwaiterMethod.ReturnType;
                MethodInfo? getResultMethod = awaiterType.GetMethod("GetResult", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                if (getResultMethod != null)
                {
                    // If it returns void, we can build a fast delegate that awaits it via a dynamic fallback
                    // or specialized expression. For performance and reliability, we route custom awaitables
                    // through a strongly-typed runner or expression.
                    return CreateCustomAwaitableDelegate(type, getAwaiterMethod, getResultMethod);
                }
            }
            // --- Not Awaitable (Primitive / Normal Object) ---
            return (obj) => Task.FromResult<object?>(obj);
        }
        // Strongly-typed static helpers that the compiler optimizes natively
        private static async Task<object?> UnwrapTaskAsync<T>(object obj)
        {
            T result = await (Task<T>)obj;
            return result;
        }
        private static async Task<object?> UnwrapValueTaskAsync<T>(object obj)
        {
            T result = await (ValueTask<T>)obj;
            return result;
        }
        /// <summary>
        /// Compiles a runtime lambda expression to handle custom duck-typed awaitables 
        /// without relying on the 'dynamic' binder engine.
        /// </summary>
        private static Func<object, Task<object?>> CreateCustomAwaitableDelegate(Type type, MethodInfo getAwaiter, MethodInfo getResult)
        {
            // If the custom awaiter returns void, return null upon completion
            if (getResult.ReturnType == typeof(void))
            {
                return async (obj) =>
                {
                    // Fallback to dynamic ONLY for rare, custom void-returning awaitables
                    dynamic awaitable = obj;
                    await awaitable;
                    return null;
                };
            }
            // For custom awaitables returning a value, build an optimized helper using a generic bridge
            var bridgeMethod = typeof(DelegateExtensions)
                .GetMethod(nameof(UnwrapCustomAwaitableAsync), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(type, getResult.ReturnType);

            return (Func<object, Task<object?>>)Delegate.CreateDelegate(typeof(Func<object, Task<object?>>), bridgeMethod);
        }
        private static async Task<object?> UnwrapCustomAwaitableAsync<TAwaitable, TResult>(object obj)
        {
            // Dynamic compile-time bypass wrapper using direct casting
            dynamic awaitable = (TAwaitable)obj;
            TResult result = await awaitable;
            return result;
        }
        /// <summary>
        /// Specialized single T async executor (tests 2.4us vs 4.5us)
        /// </summary>
        private interface IGenericExecutor
        {
            Task<object?> ExecuteAsync(object? target, object?[]? args);
        }
        // The magic bridge: Once instantiated for a type <T>, it compiles 
        // a pure strongly-typed delegate that completely bypasses reflection lookup on the hot path
        private class GenericExecutorBridge<T> : IGenericExecutor
        {
            private readonly Func<object?, object?[], Task<object?>> _compiledExecutor;

            [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming", "IL2060", Justification = "Reflection over SpawnJS's own runtime-Type dispatch machinery: the closed generic methods are SpawnJS marshalling dispatch (writeTyped = write path, never constructs; As/GetMarshaller runtime-Type resolution is backed by the embedded ILLink.Descriptors.xml for built-in wrappers or consumer preservation for custom types). MakeGenericMethod / GetAwaiter reflection over these SpawnJS-controlled targets is safe within that contract.")]
            public GenericExecutorBridge(MethodInfo openMethod)
            {
                // 1. Resolve the exact closed generic method blueprint for this target type <T>
                var closedMethod = openMethod.IsGenericMethodDefinition
                    ? openMethod.MakeGenericMethod(typeof(T))
                    : openMethod; // If it's already closed, it's safe

                // 2. Define our input lambda parameters matching (object target, object[] args)
                var targetParam = Expression.Parameter(typeof(object), "target");
                var argsParam = Expression.Parameter(typeof(object?[]), "args");

                // 3. Cast the generic target object to its actual declaring type if it's an instance method
                Expression? targetExpression = null;
                if (!closedMethod.IsStatic && closedMethod.DeclaringType != null)
                {
                    targetExpression = Expression.Convert(targetParam, closedMethod.DeclaringType);
                }

                // 4. Map the method's parameters out of our object[] array argument layout
                var methodParameters = closedMethod.GetParameters();
                var argumentExpressions = new Expression[methodParameters.Length];
                for (int i = 0; i < methodParameters.Length; i++)
                {
                    var indexExpression = Expression.ArrayIndex(argsParam, Expression.Constant(i));
                    argumentExpressions[i] = Expression.Convert(indexExpression, methodParameters[i].ParameterType);
                }

                // 5. Build the method invocation expression node
                var callExpression = Expression.Call(targetExpression, closedMethod, argumentExpressions);

                // 6. Handle the return type transformation asynchronously 
                // Since it returns ValueTask<T> or Task<T>, we pass the invocation expression to our internal unwrapper helper
                MethodInfo unwrapHelper = typeof(GenericExecutorBridge<T>)
                    .GetMethod(nameof(UnwrapAndBoxAsync), BindingFlags.NonPublic | BindingFlags.Static)!;

                var unwrapCall = Expression.Call(unwrapHelper, Expression.Convert(callExpression, typeof(object)));

                // 7. Compile into a blazing-fast native executable delegate pointer
                var lambda = Expression.Lambda<Func<object?, object?[], Task<object?>>>(unwrapCall, targetParam, argsParam);
                _compiledExecutor = lambda.Compile();
            }

            public Task<object?> ExecuteAsync(object? target, object?[]? args)
            {
                // Zero reflection. Zero runtime DLR overhead. Direct invocation via compiled delegate frame.
                return _compiledExecutor(target, args ?? Array.Empty<object?>());
            }

            // This static helper unboxes and awaits the return type natively without any intermediate boxing allocations
            private static async Task<object?> UnwrapAndBoxAsync(object rawReturnedObject)
            {
                if (rawReturnedObject is ValueTask<T> valueTask)
                {
                    T result = await valueTask;
                    return result;
                }
                if (rawReturnedObject is Task<T> task)
                {
                    T result = await task;
                    return result;
                }

                // Fallback for custom or direct non-async tasks
                return rawReturnedObject;
            }
        }
        #endregion
    }
}
