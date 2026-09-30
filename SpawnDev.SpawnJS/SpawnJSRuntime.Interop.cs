using SpawnDev.SpawnJS.Marshaller;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

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
        /// This runtime's call tape. Every interop call is written here and crosses once - see <see cref="JSTape"/>.
        /// </summary>
        internal JSTape Tape { get; }
        /// <summary>
        /// Open call frames on this runtime's tape. 0 between calls; a count that never returns to 0 is a leaked frame.
        /// </summary>
        public int TapeDepth => Tape.Depth;

        #region Method index
        /// <summary>
        /// SpawnJSInterop's method names, by index. A call names its target by index, never by string.
        /// </summary>
        internal string[] InteropMethods
        {
            get => _interopMethods;
            set
            {
                _interopMethods = value;
                var indexes = new Dictionary<string, int>(value.Length);
                for (var i = 0; i < value.Length; i++) indexes.TryAdd(value[i], i);
                _interopMethodIndexes = indexes;
            }
        }
        string[] _interopMethods = System.Array.Empty<string>();
        // name -> index, rebuilt with the map. A linear search of the names used to run on every call.
        Dictionary<string, int> _interopMethodIndexes = new Dictionary<string, int>();
        int InteropMethodIndex(string methodName)
        {
            if (_interopMethodIndexes.TryGetValue(methodName, out var index)) return index;
            throw new Exception($"Unknown SpawnJSInterop method. Index not found: {_interopMethods.Length} {methodName}");
        }
        #endregion

        #region Scratch arrays - TRANSITIONAL
        // Marshallers not yet moved to the tape build their value into a pooled JS array (see
        // JSTape.WriteViaScratch). Javascript empties the array in place of reading it, so the same held array is
        // reused. Goes away with the last NetToJS.
        readonly Queue<SpawnJSObjectReference> _scratchArrays = new Queue<SpawnJSObjectReference>();
        internal SpawnJSObjectReference RentScratchArray() => _scratchArrays.TryDequeue(out var array) ? array : NewJSArray();
        internal void ReturnScratchArray(SpawnJSObjectReference array) => _scratchArrays.Enqueue(array);
        #endregion

        #region Sync calls
        /// <summary>
        /// Call any SpawnJSInterop static method that returns nothing (void).
        /// </summary>
        internal void InteropCallApplyVoid(string methodName, object?[]? args = null) => InteropCallApply<VoidType>(methodName, args);
        /// <summary>
        /// Calls a SpawnJSInterop static method synchronously with arguments whose types are only known at run
        /// time, reading the result back as <typeparamref name="T"/>.
        /// </summary>
        internal T InteropCallApply<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, object?[]? args = null)
        {
            var argCount = args?.Length ?? 0;
            Tape.BeginCall(InteropMethodIndex(methodName), argCount);
            try
            {
                for (var i = 0; i < argCount; i++) WriteRuntimeTyped(args![i]);
            }
            catch
            {
                Tape.EndCall();
                throw;
            }
            return EndCall<T>();
        }
        /// <summary>
        /// Writes a value typed by what it IS: the runtime Type is bridged back into a compile-time generic, so the
        /// value's own strongly typed marshaller writes it with no boxing on its side.
        /// </summary>
        void WriteRuntimeTyped(object? value)
        {
            if (value == null) { Tape.WriteNull(); return; }
            ((Delegate)writeTyped<object>).InvokeGeneric(value.GetType(), value);
            void writeTyped<T1>(T1 value) => GetMarshallerForWrite<T1>().Write(Tape, value);
        }
        /// <summary>
        /// Sends the innermost frame, reads the result off the tape, and closes the frame - one crossing.
        /// </summary>
        T EndCall<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            try
            {
                var marshaller = GetMarshaller<T>();
                var returnType = marshaller?.ReturnType ?? ReturnType.Void;
                var (address, length, capacity) = Tape.Send(returnType, out _);
                var written = _spawnJSInteropCall(DotnetInstance.Id, address, length, capacity);
                if (returnType == ReturnType.Void) return default!;
                if (written >= 0) return ReadResult(marshaller!, returnType, Tape.Result(written));
                // the result did not fit behind the frame: Javascript kept it and reports the bytes it needs
                var (overflowAddress, overflowCapacity, cells) = Tape.OverflowArea(-written);
                written = _spawnJSInteropCallResult(DotnetInstance.Id, overflowAddress, overflowCapacity);
                return ReadResult(marshaller!, returnType, cells.AsSpan(0, (written + 7) >> 3));
            }
            finally
            {
                Tape.EndCall();
            }
        }
        /// <summary>
        /// Reads a result Javascript wrote in <see cref="ReturnType"/> form. Scalars mirror what the runtime's own
        /// marshallers produced for the typed returns this replaces: null and undefined read as 0 / false.
        /// </summary>
        static T ReadResult<T>(JSMarshaller<T> marshaller, ReturnType returnType, ReadOnlySpan<double> cells)
        {
            switch (returnType)
            {
                case ReturnType.Double:
                    return marshaller.JSToNet(cells[0]);
                case ReturnType.Int32:
                    // written through an Int32Array, so it carries ToInt32's wrap, exactly as a typed int return did
                    return marshaller.JSToNet(MemoryMarshal.Cast<double, int>(cells)[0]);
                case ReturnType.Boolean:
                    return marshaller.JSToNet(cells[0] != 0);
                case ReturnType.DoubleNullable:
                    return marshaller.JSToNet(cells[0] == JSTape.TagNull ? (double?)null : cells[1]);
                case ReturnType.Int32Nullable:
                    return marshaller.JSToNet(cells[0] == JSTape.TagNull ? (int?)null : MemoryMarshal.Cast<double, int>(cells)[2]);
                case ReturnType.BooleanNullable:
                    return marshaller.JSToNet(cells[0] == JSTape.TagNull ? (bool?)null : cells[1] != 0);
                case ReturnType.String:
                case ReturnType.Json:
                    return marshaller.JSToNet(ReadString(cells)!);
                case ReturnType.SpawnJSObjectReference:
                    return marshaller.JSToNet(SpawnJSObjectReference.FromID(cells[0], false)!);
                case ReturnType.SpawnJSObjectReferenceNonNullable:
                    return marshaller.JSToNet(SpawnJSObjectReference.FromID(cells[0], true)!);
                default:
                    throw new Exception($"Invalid ReturnType for marshaller: {marshaller?.GetType().Name} {returnType}");
            }
        }
        static string? ReadString(ReadOnlySpan<double> cells)
        {
            if (cells[0] == JSTape.TagNull) return null;
            var length = (int)cells[1];
            return new string(MemoryMarshal.Cast<double, char>(cells.Slice(2)).Slice(0, length));
        }
        #endregion

        #region Async calls
        /// <summary>
        /// Calls a SpawnJSInterop static method asynchronously with arguments whose types are only known at run time.
        /// </summary>
        internal Task<T> InteropCallApplyAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, object?[]? args = null)
        {
            var argCount = args?.Length ?? 0;
            Tape.BeginCall(InteropMethodIndex(methodName), argCount);
            try
            {
                for (var i = 0; i < argCount; i++) WriteRuntimeTyped(args![i]);
            }
            catch
            {
                Tape.EndCall();
                throw;
            }
            return EndCallAsync<T>();
        }
        /// <summary>
        /// Sends the innermost frame as an async call. Javascript reads every argument before its first await, so
        /// the frame is closed as soon as the call returns; the result arrives later through a resolver, keyed by
        /// a per-call id matched to a TaskCompletionSource.
        /// </summary>
        Task<T> EndCallAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            TaskCompletionSource<T> tcs;
            double asyncCallbackId;
            try
            {
                var returnMarshaller = GetMarshaller<T>();
                var returnType = returnMarshaller.ReturnType;
                tcs = new TaskCompletionSource<T>();
                asyncCallbackId = ++_asyncCallbackId;
                RegisterAsyncCompletion(returnMarshaller, returnType, tcs, asyncCallbackId);
                var (address, length, _) = Tape.Send(returnType, out _);
                try
                {
                    _spawnJSInteropCallAsync(DotnetInstance.Id, asyncCallbackId, address, length);
                }
                catch
                {
                    // the call never started, so no resolver will ever run for this id
                    RemoveAsyncCompletion(asyncCallbackId);
                    throw;
                }
            }
            finally
            {
                Tape.EndCall();
            }
            return tcs.Task;
        }
        void RegisterAsyncCompletion<T>(JSMarshaller<T> returnMarshaller, ReturnType returnType, TaskCompletionSource<T> tcs, double asyncCallbackId)
        {
            switch (returnType)
            {
                case ReturnType.Void:
                    _voidCallbacks.TryAdd(asyncCallbackId, (error) =>
                    {
                        if (error == null) tcs.TrySetResult(default!);
                        else tcs.TrySetException(JSObjects.JSException.FromInteropError(error));
                    });
                    break;
                case ReturnType.Double:
                    _doubleCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(JSObjects.JSException.FromInteropError(error));
                        else tcs.TrySetResult(returnMarshaller.JSToNet(value!));
                    });
                    break;
                case ReturnType.Boolean:
                    _booleanCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(JSObjects.JSException.FromInteropError(error));
                        else tcs.TrySetResult(returnMarshaller.JSToNet(value));
                    });
                    break;
                case ReturnType.Int32:
                    _int32Callbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(JSObjects.JSException.FromInteropError(error));
                        else tcs.TrySetResult(returnMarshaller.JSToNet(value!));
                    });
                    break;
                case ReturnType.Int32Nullable:
                    _int32NullableCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(JSObjects.JSException.FromInteropError(error));
                        else tcs.TrySetResult(returnMarshaller.JSToNet(value!));
                    });
                    break;
                case ReturnType.DoubleNullable:
                    _doubleNullableCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(JSObjects.JSException.FromInteropError(error));
                        else tcs.TrySetResult(returnMarshaller.JSToNet(value!));
                    });
                    break;
                case ReturnType.BooleanNullable:
                    _booleanNullableCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(JSObjects.JSException.FromInteropError(error));
                        else tcs.TrySetResult(returnMarshaller.JSToNet(value));
                    });
                    break;
                case ReturnType.String:
                case ReturnType.Json:
                    _stringCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(JSObjects.JSException.FromInteropError(error));
                        else tcs.TrySetResult(returnMarshaller.JSToNet(value!));
                    });
                    break;
                case ReturnType.SpawnJSObjectReference:
                case ReturnType.SpawnJSObjectReferenceNonNullable:
                    var nonNullable = returnType == ReturnType.SpawnJSObjectReferenceNonNullable;
                    _doubleCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(JSObjects.JSException.FromInteropError(error));
                        else tcs.TrySetResult(returnMarshaller.JSToNet(SpawnJSObjectReference.FromID(value, nonNullable)!));
                    });
                    break;
                default:
                    throw new Exception($"Invalid ReturnType for marshaller: {returnMarshaller?.GetType().Name} {returnType}");
            }
        }
        void RemoveAsyncCompletion(double asyncCallbackId)
        {
            _voidCallbacks.TryRemove(asyncCallbackId, out _);
            _doubleCallbacks.TryRemove(asyncCallbackId, out _);
            _doubleNullableCallbacks.TryRemove(asyncCallbackId, out _);
            _booleanCallbacks.TryRemove(asyncCallbackId, out _);
            _booleanNullableCallbacks.TryRemove(asyncCallbackId, out _);
            _stringCallbacks.TryRemove(asyncCallbackId, out _);
            _int32Callbacks.TryRemove(asyncCallbackId, out _);
            _int32NullableCallbacks.TryRemove(asyncCallbackId, out _);
        }
        #endregion

        #region Typed InteropCall
        // InteropCall methods write each argument through its compile-time type's marshaller, so there is no
        // runtime Type bridge at all. The method is named by index, never by string.
        internal T InteropCall<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 0);
            return EndCall<T>();
        }
        internal T InteropCall<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 1);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
            }
            catch { Tape.EndCall(); throw; }
            return EndCall<T>();
        }
        internal T InteropCall<T1, T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 2);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
            }
            catch { Tape.EndCall(); throw; }
            return EndCall<T>();
        }
        internal T InteropCall<T1, T2, T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 3);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
            }
            catch { Tape.EndCall(); throw; }
            return EndCall<T>();
        }
        internal T InteropCall<T1, T2, T3, T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 4);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
            }
            catch { Tape.EndCall(); throw; }
            return EndCall<T>();
        }
        internal T InteropCall<T1, T2, T3, T4, T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 5);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
            }
            catch { Tape.EndCall(); throw; }
            return EndCall<T>();
        }
        internal T InteropCall<T1, T2, T3, T4, T5, T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 6);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
            }
            catch { Tape.EndCall(); throw; }
            return EndCall<T>();
        }
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 7);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
                GetMarshallerForWrite<T7>().Write(Tape, arg7);
            }
            catch { Tape.EndCall(); throw; }
            return EndCall<T>();
        }
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 8);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
                GetMarshallerForWrite<T7>().Write(Tape, arg7);
                GetMarshallerForWrite<T8>().Write(Tape, arg8);
            }
            catch { Tape.EndCall(); throw; }
            return EndCall<T>();
        }
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 9);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
                GetMarshallerForWrite<T7>().Write(Tape, arg7);
                GetMarshallerForWrite<T8>().Write(Tape, arg8);
                GetMarshallerForWrite<T9>().Write(Tape, arg9);
            }
            catch { Tape.EndCall(); throw; }
            return EndCall<T>();
        }
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 10);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
                GetMarshallerForWrite<T7>().Write(Tape, arg7);
                GetMarshallerForWrite<T8>().Write(Tape, arg8);
                GetMarshallerForWrite<T9>().Write(Tape, arg9);
                GetMarshallerForWrite<T10>().Write(Tape, arg10);
            }
            catch { Tape.EndCall(); throw; }
            return EndCall<T>();
        }
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 11);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
                GetMarshallerForWrite<T7>().Write(Tape, arg7);
                GetMarshallerForWrite<T8>().Write(Tape, arg8);
                GetMarshallerForWrite<T9>().Write(Tape, arg9);
                GetMarshallerForWrite<T10>().Write(Tape, arg10);
                GetMarshallerForWrite<T11>().Write(Tape, arg11);
            }
            catch { Tape.EndCall(); throw; }
            return EndCall<T>();
        }
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 12);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
                GetMarshallerForWrite<T7>().Write(Tape, arg7);
                GetMarshallerForWrite<T8>().Write(Tape, arg8);
                GetMarshallerForWrite<T9>().Write(Tape, arg9);
                GetMarshallerForWrite<T10>().Write(Tape, arg10);
                GetMarshallerForWrite<T11>().Write(Tape, arg11);
                GetMarshallerForWrite<T12>().Write(Tape, arg12);
            }
            catch { Tape.EndCall(); throw; }
            return EndCall<T>();
        }
        #endregion

        #region Typed InteropCallAsync
        internal Task<T> InteropCallAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 0);
            return EndCallAsync<T>();
        }
        internal Task<T> InteropCallAsync<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 1);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
            }
            catch { Tape.EndCall(); throw; }
            return EndCallAsync<T>();
        }
        internal Task<T> InteropCallAsync<T1, T2, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 2);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
            }
            catch { Tape.EndCall(); throw; }
            return EndCallAsync<T>();
        }
        internal Task<T> InteropCallAsync<T1, T2, T3, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 3);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
            }
            catch { Tape.EndCall(); throw; }
            return EndCallAsync<T>();
        }
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 4);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
            }
            catch { Tape.EndCall(); throw; }
            return EndCallAsync<T>();
        }
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 5);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
            }
            catch { Tape.EndCall(); throw; }
            return EndCallAsync<T>();
        }
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 6);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
            }
            catch { Tape.EndCall(); throw; }
            return EndCallAsync<T>();
        }
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 7);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
                GetMarshallerForWrite<T7>().Write(Tape, arg7);
            }
            catch { Tape.EndCall(); throw; }
            return EndCallAsync<T>();
        }
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 8);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
                GetMarshallerForWrite<T7>().Write(Tape, arg7);
                GetMarshallerForWrite<T8>().Write(Tape, arg8);
            }
            catch { Tape.EndCall(); throw; }
            return EndCallAsync<T>();
        }
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 9);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
                GetMarshallerForWrite<T7>().Write(Tape, arg7);
                GetMarshallerForWrite<T8>().Write(Tape, arg8);
                GetMarshallerForWrite<T9>().Write(Tape, arg9);
            }
            catch { Tape.EndCall(); throw; }
            return EndCallAsync<T>();
        }
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 10);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
                GetMarshallerForWrite<T7>().Write(Tape, arg7);
                GetMarshallerForWrite<T8>().Write(Tape, arg8);
                GetMarshallerForWrite<T9>().Write(Tape, arg9);
                GetMarshallerForWrite<T10>().Write(Tape, arg10);
            }
            catch { Tape.EndCall(); throw; }
            return EndCallAsync<T>();
        }
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 11);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
                GetMarshallerForWrite<T7>().Write(Tape, arg7);
                GetMarshallerForWrite<T8>().Write(Tape, arg8);
                GetMarshallerForWrite<T9>().Write(Tape, arg9);
                GetMarshallerForWrite<T10>().Write(Tape, arg10);
                GetMarshallerForWrite<T11>().Write(Tape, arg11);
            }
            catch { Tape.EndCall(); throw; }
            return EndCallAsync<T>();
        }
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            Tape.BeginCall(InteropMethodIndex(methodName), 12);
            try
            {
                GetMarshallerForWrite<T1>().Write(Tape, arg1);
                GetMarshallerForWrite<T2>().Write(Tape, arg2);
                GetMarshallerForWrite<T3>().Write(Tape, arg3);
                GetMarshallerForWrite<T4>().Write(Tape, arg4);
                GetMarshallerForWrite<T5>().Write(Tape, arg5);
                GetMarshallerForWrite<T6>().Write(Tape, arg6);
                GetMarshallerForWrite<T7>().Write(Tape, arg7);
                GetMarshallerForWrite<T8>().Write(Tape, arg8);
                GetMarshallerForWrite<T9>().Write(Tape, arg9);
                GetMarshallerForWrite<T10>().Write(Tape, arg10);
                GetMarshallerForWrite<T11>().Write(Tape, arg11);
                GetMarshallerForWrite<T12>().Write(Tape, arg12);
            }
            catch { Tape.EndCall(); throw; }
            return EndCallAsync<T>();
        }
        #endregion

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
