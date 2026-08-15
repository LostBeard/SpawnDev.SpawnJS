using SpawnDev.SpawnJS.Marshaller;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSRuntime
    {
        // Per-type marshaller cache. Populated by GetMarshaller so a resolved marshaller can be reused.
        ConcurrentDictionary<Type, JSMarshaller> _typeMarshallerCache = new ConcurrentDictionary<Type, JSMarshaller>();
        public JSMarshaller GetMarshaller(Type type)
        {
            return (JSMarshaller)((Delegate)GetMarshaller<object>).InvokeGeneric(type)!;
        }
        /// <summary>
        /// Selects the marshaller for <typeparamref name="TType"/>. Marshallers are scanned in REVERSE
        /// registration order so later (more specific) registrations win. A marshaller may hand back a
        /// per-type specialization via <see cref="JSMarshaller.GetMarshaller{T}"/> (e.g. ArrayMarshaller
        /// returns one bound to the concrete element type); that specialization is what gets used and cached.
        /// </summary>
        public JSMarshaller<TType> GetMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TType>()
        {
            var type = typeof(TType);
            //var selectionType = Nullable.GetUnderlyingType(type) ?? type;
            if (_typeMarshallerCache.TryGetValue(type, out var cachedMarshaller))
            {
                return (JSMarshaller<TType>)cachedMarshaller;
            }
            JSMarshaller<TType>? marshaller = null;
            var length = Marshallers.Count;
            for (var i = length - 1; i >= 0; i--)
            {
                var candidate = Marshallers[i];
                if (!candidate.CanMarshal(type)) continue;
                // GetMarshaller lets a marshaller hand back a per-type specialization (UnionMarshaller
                // returns one bound to the concrete Union<...> arms). Cache and use THAT, not the
                // generic candidate - otherwise the specialization hook does nothing.
                var typeMarshaller = candidate.GetMarshaller<TType>();
                if (typeMarshaller == null) continue;
                marshaller = typeMarshaller;
                _typeMarshallerCache.TryAdd(type, typeMarshaller);
                break;
            }
            if (marshaller == null) throw new Exception($"GetMarshaller failed: {type?.GetCSharpName()}");
            if (Verbose) Console.WriteLine($"<< GetMarshaller: {type?.Name} {marshaller.GetType().GetCSharpName()}");
            return marshaller;
        }
        /// <summary>
        /// Marshaller resolution for the WRITE path (.Net -> JS). Identical to <see cref="GetMarshaller{TType}"/>
        /// but carries NO DynamicallyAccessedMembers requirement: NetToJS only reads a wrapper's JSRef and
        /// never invokes its constructor, so the PublicConstructors requirement that the read/JSToNet path
        /// needs does not apply here. Using this on the write path keeps that requirement from cascading onto
        /// every interop INPUT type parameter - it stays scoped to return types, where wrappers are built.
        /// </summary>
        [UnconditionalSuppressMessage("Trimming", "IL2091",
            Justification = "The resolved marshaller is used only for NetToJS (write), which reads value.JSRef and never constructs the wrapper. The PublicConstructors requirement of GetMarshaller<T> is exercised solely by the read/JSToNet path.")]
        internal JSMarshaller<T> GetMarshallerForWrite<T>() => GetMarshaller<T>();
        /// <summary>
        /// Call any SpawnJSInterop static method that returns nothing (void).
        /// </summary>
        internal void InteropCallApplyVoid(string methodName, object?[]? args = null) => InteropCallApply<VoidType>(methodName, args);
        /// <summary>
        /// Pool of reusable JS argument arrays so each call does not have to allocate a new one. The JS side
        /// empties the array's slot after consuming it (spawnJSObjectGetAndReplace), so the same held
        /// reference can be handed back out on the next call.
        /// </summary>
        Queue<SpawnJSObjectReference> _callArrays = new Queue<SpawnJSObjectReference>();
        /// <summary>
        /// Calls a SpawnJSInterop static method synchronously, marshalling <paramref name="args"/> into a JS
        /// array and reading the result back as <typeparamref name="T"/>.
        /// </summary>
        internal T InteropCallApply<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, object?[]? args = null)
        {
            var returnType = typeof(T);
            var inMarshaller = GetMarshaller<T>();
            // The JS side empties this array's slot after the call, so it can be returned to the pool.
            SpawnJSObjectReference? jsArgs = null;
            if (args != null && args.Length > 0)
            {
                if (!_callArrays.TryDequeue(out jsArgs))
                {
                    jsArgs = NewJSArray();
                }
                for (var i = 0; i < args.Length; i++)
                {
                    var item = args[i];
                    var itemType = item?.GetType()!;
                    if (itemType == null)
                    {
                        jsArgs.PropertySetNull(i);
                        continue;
                    }
                    // The Type -> <T> trick: each arg's runtime Type is bridged back into a compile-time
                    // generic via InvokeGeneric, so writeTyped<T1> runs the strongly-typed marshaller path
                    // (JSMarshaller<T1>.NetToJS) with NO boxing of the value. GetMarshaller<T1> matches on
                    // T1's exact type, and the value is written straight into the JS array by index.
                    ((Delegate)writeTyped<object>).InvokeGeneric(itemType, item);
                    void writeTyped<T1>(T1 value)
                    {
                        var marshaller = GetMarshallerForWrite<T1>();
                        if (marshaller == null) jsArgs.PropertySetNull(i);
                        else marshaller.NetToJS(jsArgs!, i, value!);
                    }
                }
            }
            return _InteropCallApply<T>(methodName, jsArgs);
        }
        internal T _InteropCallApply<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(int methodIndex, SpawnJSObjectReference? jsArgs = null)
        {
            if (methodIndex < 0) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {InteropMethods.Length} {methodIndex}");
            var returnType = typeof(T);
            var inMarshaller = GetMarshaller<T>();
            var returnTypeIndex = inMarshaller?.ReturnType ?? ReturnType.Void;
            T ret = default!;
            var argsId = jsArgs?.Id ?? UndefinedId;
            try
            {
                switch (returnTypeIndex)
                {
                    case ReturnType.Void:
                        {
                            _spawnJSInteropCallVoid((int)returnTypeIndex, methodIndex, argsId);
                        }
                        break;
                    case ReturnType.Double:
                        {
                            var fromJS = _spawnJSInteropCallDouble((int)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    case ReturnType.Boolean:
                        {
                            var fromJS = _spawnJSInteropCallBoolean((int)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    case ReturnType.Int32:
                        {
                            var fromJS = _spawnJSInteropCallInt32((int)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    case ReturnType.Int32Nullable:
                        {
                            var fromJS = _spawnJSInteropCallInt32Nullable((int)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    case ReturnType.DoubleNullable:
                        {
                            var fromJS = _spawnJSInteropCallDoubleNullable((int)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    case ReturnType.BooleanNullable:
                        {
                            var fromJS = _spawnJSInteropCallBooleanNullable((int)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    case ReturnType.String:
                        {
                            var fromJS = _spawnJSInteropCallString((int)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    case ReturnType.SpawnJSObjectReference:
                        {
                            var fromJS = _spawnJSInteropCallDouble((int)returnTypeIndex, methodIndex, argsId);
                            var spawnJSObjectReference = SpawnJSObjectReference.FromID(fromJS, false);
                            ret = inMarshaller!.JSToNet(spawnJSObjectReference!);
                        }
                        break;
                    case ReturnType.SpawnJSObjectReferenceNonNullable:
                        {
                            var fromJS = _spawnJSInteropCallDouble((int)returnTypeIndex, methodIndex, argsId);
                            var spawnJSObjectReference = SpawnJSObjectReference.FromID(fromJS, true);
                            ret = inMarshaller!.JSToNet(spawnJSObjectReference!);
                        }
                        break;
                    case ReturnType.Json:
                        {
                            var fromJS = _spawnJSInteropCallString((int)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    default:
                        throw new Exception($"Invalid ReturnType for marshaller: {inMarshaller?.GetType().Name} {returnTypeIndex}");
                }
            }
            finally
            {
                if (jsArgs != null)
                {
                    // returen the array to the usable call queue. it has already been reset by js
                    _callArrays.Enqueue(jsArgs);
                }
            }
            return ret;
        }
        async Task<T> _InteropCallApplyAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(double methodIndex, SpawnJSObjectReference? jsArgs = null)
        {
            var typeOfT = typeof(T);
            var returnMarshaller = GetMarshaller<T>();
            var returnTypeIndex = returnMarshaller.ReturnType;
            var tcs = new TaskCompletionSource<T>();
            var asyncCallbackId = ++_asyncCallbackId;
            switch (returnTypeIndex)
            {
                case ReturnType.Void:
                    {
                        _voidCallbacks.TryAdd(asyncCallbackId, (error) =>
                        {
                            if (error == null) tcs.TrySetResult(default!);
                            else tcs.TrySetException(new Exception(error));
                        });
                    }
                    break;
                case ReturnType.Double:
                    _doubleCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(new Exception(error));
                        else
                        {
                            var ret = returnMarshaller.JSToNet(value!);
                            tcs.TrySetResult(ret);
                        }
                    });
                    break;
                case ReturnType.Boolean:
                    {
                        _booleanCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                        {
                            if (error != null) tcs.TrySetException(new Exception(error));
                            else
                            {
                                var ret = returnMarshaller.JSToNet(value);
                                tcs.TrySetResult(ret);
                            }
                        });
                    }
                    break;
                case ReturnType.Int32:
                    _int32Callbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(new Exception(error));
                        else
                        {
                            var ret = returnMarshaller.JSToNet(value!);
                            tcs.TrySetResult(ret);
                        }
                    });
                    break;
                case ReturnType.Int32Nullable:
                    _int32NullableCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(new Exception(error));
                        else
                        {
                            var ret = returnMarshaller.JSToNet(value!);
                            tcs.TrySetResult(ret);
                        }
                    });
                    break;
                case ReturnType.DoubleNullable:
                    _doubleNullableCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(new Exception(error));
                        else
                        {
                            var ret = returnMarshaller.JSToNet(value!);
                            tcs.TrySetResult(ret);
                        }
                    });
                    break;
                case ReturnType.BooleanNullable:
                    {
                        _booleanNullableCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                        {
                            if (error != null) tcs.TrySetException(new Exception(error));
                            else
                            {
                                var ret = returnMarshaller.JSToNet(value);
                                tcs.TrySetResult(ret);
                            }
                        });
                    }
                    break;
                case ReturnType.String:
                    {
                        _stringCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                        {
                            if (error != null) tcs.TrySetException(new Exception(error));
                            else
                            {
                                var ret = returnMarshaller.JSToNet(value!);
                                tcs.TrySetResult(ret);
                            }
                        });
                    }
                    break;
                case ReturnType.SpawnJSObjectReference:
                    {
                        _doubleCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                        {
                            if (error != null) tcs.TrySetException(new Exception(error));
                            else
                            {
                                var spawnJSObjectReference = SpawnJSObjectReference.FromID(value, false);
                                var ret = returnMarshaller.JSToNet(spawnJSObjectReference!);
                                tcs.TrySetResult(ret);
                            }
                        });
                    }
                    break;
                case ReturnType.SpawnJSObjectReferenceNonNullable:
                    {
                        _doubleCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                        {
                            if (error != null) tcs.TrySetException(new Exception(error));
                            else
                            {
                                var spawnJSObjectReference = SpawnJSObjectReference.FromID(value, true);
                                var ret = returnMarshaller.JSToNet(spawnJSObjectReference!);
                                tcs.TrySetResult(ret);
                            }
                        });
                    }
                    break;
                case ReturnType.Json:
                    {
                        _stringCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                        {
                            if (error != null) tcs.TrySetException(new Exception(error));
                            else
                            {
                                var ret = returnMarshaller.JSToNet(value!);
                                tcs.TrySetResult(ret);
                            }
                        });
                    }
                    break;
                default:
                    return default!;
            }
            try
            {
                var argsId = jsArgs?.Id ?? UndefinedId;
                _spawnJSInteropCallAsync((int)returnMarshaller.ReturnType, DotnetInstance.Id, asyncCallbackId, methodIndex, argsId);
                // wait for the tcs to complete or throw
                return await tcs.Task.ConfigureAwait(false);
            }
            finally
            {
                if (jsArgs != null)
                {
                    // returen the array to the usable call queue. it has already been reset by js
                    _callArrays.Enqueue(jsArgs);
                }
            }
        }
        /// <summary>
        /// Calls a SpawnJSInterop static method asynchronously. A per-call id is registered against a
        /// TaskCompletionSource; the JS side runs the underlying promise then invokes an AsyncCallResolved*
        /// [JSExport] with that id, which completes the task. The assembly export table is loaded once on
        /// the first async call.
        /// </summary>
        internal async Task<T> InteropCallApplyAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, object?[]? args = null)
        {
            // The JS side empties this array's slot after the call, so it can be returned to the pool.
            SpawnJSObjectReference? jsArgs = null;
            if (args != null && args.Length > 0)
            {
                if (!_callArrays.TryDequeue(out jsArgs))
                {
                    jsArgs = NewJSArray();
                }
                for (var i = 0; i < args.Length; i++)
                {
                    var item = args[i];
                    // Type -> <T> trick (see InteropCallApply): marshal each arg with no boxing.
                    var itemType = item?.GetType()!;
                    if (itemType == null)
                    {
                        jsArgs.PropertySetNull(i);
                        continue;
                    }
                    ((Delegate)writeTyped<object>).InvokeGeneric(itemType, item);
                    void writeTyped<T1>(T1 value)
                    {
                        var marshaller = GetMarshallerForWrite<T1>();
                        if (marshaller == null) jsArgs.PropertySetNull(i);
                        else marshaller.NetToJS(jsArgs!, i, value!);
                    }
                }
            }
            return await _InteropCallApplyAsync<T>(methodName, jsArgs);
        }
        /// <summary>
        /// InteropCall methods are efficient callers into SpawnJSInterop because instead of passing the methodName to JS for teh call, they pass the method index
        /// </summary>
        internal T InteropCall<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName)
            => _InteropCallApply<T>(methodName);
        internal T InteropCall<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1));
        internal T InteropCall<T1, T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2));
        internal T InteropCall<T1, T2, T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3));
        internal T InteropCall<T1, T2, T3, T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4));
        internal T InteropCall<T1, T2, T3, T4, T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5));
        internal T InteropCall<T1, T2, T3, T4, T5, T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6));
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
        internal Task<T> InteropCallAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName)
            => _InteropCallApplyAsync<T>(methodName);
        internal Task<T> InteropCallAsync<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1));
        internal Task<T> InteropCallAsync<T1, T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2));
        internal Task<T> InteropCallAsync<T1, T2, T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
        Task<T> _InteropCallApplyAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, SpawnJSObjectReference? jsArgs = null)
        {
            var methodIndex = InteropMethods.IndexOf(methodName);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {InteropMethods.Length} {methodName}");
            return _InteropCallApplyAsync<T>(methodIndex, jsArgs);
        }
        internal T _InteropCallApply<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, SpawnJSObjectReference? jsArgs = null)
        {
            var methodIndex = InteropMethods.IndexOf(methodName);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {InteropMethods.Length} {methodName}");
            return _InteropCallApply<T>(methodIndex, jsArgs);
        }
        // Monotonic id handed to JS with each async call and echoed back to match the completion to its task.
        double _asyncCallbackId = 0;
        // Pending async completions, keyed by asyncCallbackId, one dictionary per JS result shape. The
        // matching resolver [JSExport] below removes and invokes the entry when JS reports the result.
        static ConcurrentDictionary<double, Action<string?>> _voidCallbacks = new ConcurrentDictionary<double, Action<string?>>();
        static ConcurrentDictionary<double, Action<double, string?>> _doubleCallbacks = new ConcurrentDictionary<double, Action<double, string?>>();
        static ConcurrentDictionary<double, Action<double?, string?>> _doubleNullableCallbacks = new ConcurrentDictionary<double, Action<double?, string?>>();
        static ConcurrentDictionary<double, Action<bool, string?>> _booleanCallbacks = new ConcurrentDictionary<double, Action<bool, string?>>();
        static ConcurrentDictionary<double, Action<bool?, string?>> _booleanNullableCallbacks = new ConcurrentDictionary<double, Action<bool?, string?>>();
        static ConcurrentDictionary<double, Action<string?, string?>> _stringCallbacks = new ConcurrentDictionary<double, Action<string?, string?>>();
        static ConcurrentDictionary<double, Action<int, string?>> _int32Callbacks = new ConcurrentDictionary<double, Action<int, string?>>();
        static ConcurrentDictionary<double, Action<int?, string?>> _int32NullableCallbacks = new ConcurrentDictionary<double, Action<int?, string?>>();

        private void MappedMethodsChanged()
        {
            Console.WriteLine($"_JSToNetMappedMethodsChanged");
            InteropMethods = _refreshMethodMap();
        }
        // Resolvers invoked by JS (_spawnJSInteropCallAsync) to complete a pending async call. error is
        // non-null when the JS promise rejected.
        void AsyncCallResolvedVoid(double asyncCallId, string? error)
        {
            if (_voidCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask(error);
        }
        void ResolveInt32(double asyncCallId, int value, string? error)
        {
            if (_int32Callbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask(value, error);
        }
        void ResolveInt32Nullable(double asyncCallId, object? value, string? error)
        {
            if (_int32NullableCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask((int?)value, error);
        }
        void ResolveDouble(double asyncCallId, double value, string? error)
        {
            if (_doubleCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask(value, error);
        }
        void ResolveBoolean(double asyncCallId, bool value, string? error)
        {
            if (_booleanCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask(value, error);
        }
        void ResolveString(double asyncCallId, string? value, string? error)
        {
            if (_stringCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask(value, error);
        }
        void ResolveDoubleNullable(double asyncCallId, object? value, string? error)
        {
            if (_doubleNullableCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask((double?)value, error);
        }
        void ResolveBooleanNullable(double asyncCallId, object? value, string? error)
        {
            if (_booleanNullableCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask((bool?)value, error);
        }
    }
}
