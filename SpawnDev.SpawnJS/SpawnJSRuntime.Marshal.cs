using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Marshallers;
using System.Collections.Concurrent;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSRuntime
    {
        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string CopyProperty(JSObject srcParent, object? srcKey, JSObject destParent, object? destKey)
        {
            if (srcParent == null || srcParent.IsDisposed || srcParent == null) throw new Exception("CopyProperty invalid JSObject");
            return NetRun<string>("copyProperty", new object[] { srcParent, srcKey!, destParent!, destKey! });
        }
        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string MoveProperty(JSObject srcParent, object? srcKey, JSObject destParent, object? destKey)
        {
            if (srcParent == null || srcParent.IsDisposed || srcParent == null) throw new Exception("MoveProperty invalid JSObject");
            return NetRun<string>("moveProperty", new object[] { srcParent, srcKey!, destParent!, destKey! });
        }
        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string CopyProperty(SpawnJSHandle srcParent, object? srcKey, SpawnJSHandle destParent, object? destKey)
        {
            if (srcParent == null || srcParent.IsDisposed || srcParent.JSObject == null) throw new Exception("CopyProperty invalid SpawnJSHandle");
            return NetRun<string>("copyProperty", new object[] { srcParent.JSObject, srcKey!, destParent.JSObject!, destKey! });
        }
        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string MoveProperty(SpawnJSHandle srcParent, object? srcKey, SpawnJSHandle destParent, object? destKey)
        {
            if (srcParent == null || srcParent.IsDisposed || srcParent.JSObject == null) throw new Exception("MoveProperty invalid SpawnJSHandle");
            return NetRun<string>("moveProperty", new object[] { srcParent.JSObject, srcKey!, destParent.JSObject!, destKey! });
        }

        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string[] GetPropertyConstructorNames(SpawnJSHandle jsParent, object? jsKey)
        {
            if (jsParent == null || jsParent.IsDisposed || jsParent.JSObject == null) throw new Exception("ObjectPrototypeToStringCall invalid SpawnJSHandle");
            return NetRun<string[]>("getPropertyConstructorNames", new object[] { jsParent.JSObject, jsKey! });
        }

        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string[] GetConstructorNames(SpawnJSHandle jsHandle)
        {
            if (jsHandle == null || jsHandle.IsDisposed || jsHandle.JSParent == null) throw new Exception("ObjectPrototypeToStringCall invalid SpawnJSHandle");
            return NetRun<string[]>("getConstructorNames", new object[] { jsHandle.JSParent });
        }

        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string[] GetPropertyConstructorNames(JSObject jsParent, object? jsKey)
        {
            if (jsParent == null || jsParent.IsDisposed) throw new Exception("ObjectPrototypeToStringCall invalid SpawnJSHandle");
            return NetRun<string[]>("getPropertyConstructorNames", new object[] { jsParent, jsKey! });
        }

        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string[] GetConstructorNames(JSObject jsHandle)
        {
            if (jsHandle == null || jsHandle.IsDisposed || jsHandle == null) throw new Exception("ObjectPrototypeToStringCall invalid SpawnJSHandle");
            return NetRun<string[]>("getConstructorNames", new object[] { jsHandle,  });
        }

        /// <summary>
        /// The WebAssembly linear memory ArrayBuffer the .Net heap lives in.<br/>
        /// This is what a zero copy view is built over: pin a .Net array, take its address, and hand
        /// Javascript a TypedArray onto these bytes so the data is never copied across the boundary.
        /// </summary>
        public SpawnJSHandle WasmMemoryBuffer() => NetRun<SpawnJSHandle>("wasmMemoryBuffer");
        /// <summary>
        /// Which runtime shape the WebAssembly memory buffer was reached through, or an empty string if it
        /// could not be reached. Diagnostics: the runtime exposes the heap differently across versions,
        /// and reaching it the wrong way yields a view onto the wrong bytes rather than an error.
        /// </summary>
        public string WasmMemoryBufferSource() => NetRun<string>("wasmMemoryBufferSource");
        /// <summary>
        /// Returns the Javascript typeof followed by the value's prototype chain constructor names, most
        /// derived first: ["object", "TypeError", "Error", "Object"].<br/>
        /// One call, because identifying a Javascript value needs both and a marshaller should not pay two
        /// round trips to find out what it is reading.
        /// </summary>
        public string[] GetPropertyTypeAndConstructorNames(JSObject jsParent, object? jsKey)
        {
            if (jsParent == null || jsParent.IsDisposed) throw new Exception("GetPropertyTypeAndConstructorNames invalid JSObject");
            return NetRun<string[]>("getPropertyTypeAndConstructorNames", new object[] { jsParent, jsKey! });
        }
        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string GetPropertyTypeInfo(SpawnJSHandle jsParent, object? jsKey)
        {
            if (jsParent == null || jsParent.IsDisposed || jsParent.JSObject == null) throw new Exception("ObjectPrototypeToStringCall invalid SpawnJSHandle");
            return NetRun<string>("getPropertyTypeInfo", new object[] { jsParent.JSObject, jsKey! });
        }
        /// <summary>
        /// Returns a string with typeof and className: "object Array"
        /// </summary>
        public string GetPropertyTypeInfo(JSObject jsParent, object? jsKey)
        {
            if (jsParent == null || jsParent.IsDisposed) throw new Exception("ObjectPrototypeToStringCall invalid SpawnJSHandle");
            return NetRun<string>("getPropertyTypeInfo", new object[] { jsParent, jsKey! });
        }
        /// <summary>
        /// Compares two values using Javascript equality.<br/>
        /// full == true uses strict equality (===), otherwise loose equality (==)
        /// </summary>
        public bool ObjectEquals(object? obj1, object? obj2, bool full = false)
            => NetRun<bool>("objectEquals", new object?[] { obj1, obj2, full });
        #region Marshal
        ConcurrentDictionary <Type, JSMarshaller> _typeMarshallerCache = new ConcurrentDictionary<Type, JSMarshaller>();
        JSMarshaller? _nullTypeMarshaller = null;
        /// <summary>
        /// Returns the marshaller that will be used for the given .Net type.<br/>
        /// Marshallers are matched in reverse registration order, so a later registration wins, and the
        /// match is cached per type. A marshaller may hand back an instance specialised to the type, in
        /// which case that specialisation is what gets cached and returned.<br/>
        /// Public because the marshaller registry is part of the API - anyone adding a marshaller needs to
        /// be able to see which one a type actually resolves to.
        /// </summary>
        public JSMarshaller GetMarshaller(Type? type)
        {
            JSMarshaller? marshaller = null;
            if (type == null && _nullTypeMarshaller != null)
            {
                marshaller = _nullTypeMarshaller;
            }
            else if (type == null || !_typeMarshallerCache.TryGetValue(type, out marshaller))
            {
                // reset, because TryGetValue writes null on a miss and the loop below may decline every
                // candidate. Leaving a declined candidate in `marshaller` would defeat the null check.
                marshaller = null;
                // A Nullable<T> is selected for as T.
                // On the write path a value arrives boxed, and a boxed Nullable<int> reports int as its
                // type, so the write path has always selected for T. The read path passes the DECLARED
                // type, so without this unwrap the two disagree: Nullable<int> is a value type that is
                // neither an enum nor primitive, so StructMarshaller claims it and walks HasValue/Value
                // instead of reading the number - every nullable property wrote correctly and read back
                // as null. Selecting for T here is what makes a read agree with a write.
                var selectionType = type == null ? null : Nullable.GetUnderlyingType(type) ?? type;
                var length = Marshallers.Count;
                for (var i = length - 1; i >= 0; i--)
                {
                    var candidate = Marshallers[i];
                    if (!candidate.CanMarshal(selectionType)) continue;
                    // GetMarshaller lets a marshaller hand back a per-type specialization (UnionMarshaller
                    // returns one bound to the concrete Union<...> arms). Cache and use THAT, not the
                    // generic candidate - otherwise the specialization hook does nothing.
                    var typeMarshaller = candidate.GetMarshaller(selectionType);
                    if (typeMarshaller == null) continue;
                    marshaller = typeMarshaller;
                    if (type == null)
                    {
                        _nullTypeMarshaller = typeMarshaller;
                    }
                    else
                    {
                        _typeMarshallerCache.TryAdd(type, typeMarshaller);
                    }
                    break;
                }
            }
            if (marshaller == null) throw new Exception($"GetMarshaller failed: {type?.Name}");
            if (Verbose) Console.WriteLine($"<< GetMarshaller: {type?.Name} {marshaller.GetType().Name}");
            return marshaller;
        }
        /// <summary>
        /// Top of the shared call buffer. Arguments are appended here and the top unwinds when the call
        /// completes, so it behaves as a stack: a nested call - a marshaller reading a property while an
        /// argument is being marshalled - writes above the outer call's region and cannot disturb it.
        /// <br/>
        /// Sync only. An async call has not finished when it returns, so its region cannot be released yet
        /// and the top would unwind out of order.
        /// </summary>
        int _bufferTop;

        /// <summary>
        /// Whether outbound calls carry their arguments in the ARGUMENT FRAME - .Net's own memory, which
        /// Javascript views directly - rather than in the Javascript side array.<br/>
        /// The old path is kept so the two can be measured against each other on demand; it is not a
        /// fallback for correctness, and both are covered by the same tests.
        /// </summary>
        public static bool UseArgFrame { get; set; } = true;

        /// <summary>
        /// A call whose arguments never cross. Only the command name, an offset and a length do.
        /// </summary>
        object? NetRunViaFrame(Type type, string cmd, object?[] args)
        {
            var offset = WriteArgsToFrame(args);
            try
            {
                SlotInterop.FrameCall(CtxId, cmd, offset, args.Length);
                var netRet = ReadFrameResult(type, offset);
                var expected = Nullable.GetUnderlyingType(type) ?? type;
                if (netRet != null && !expected.IsInstanceOfType(netRet))
                    throw new Exception($"{nameof(SpawnJSRuntime)}.NetRun expected {expected.Name} got {netRet.GetType().Name}");
                return netRet;
            }
            finally
            {
                ReleaseFrameArgs(offset);
            }
        }

        /// <summary>
        /// Top of the argument frame. Unwinds like the buffer top, so a nested call - a marshaller
        /// building a value while an argument is being written - takes slots above the outer call's
        /// region and cannot disturb it.
        /// </summary>
        int _frameTop;

        /// <summary>
        /// Writes the arguments into the ARGUMENT FRAME, which lives in .Net's own memory, and returns the
        /// slot index they start at.<br/>
        /// <br/>
        /// Every argument that is already expressible as one number - a number, a boolean, a numeric enum,
        /// a wrapper or handle (which holds a slot), an interned string, null - is a plain store and costs
        /// NOTHING to deliver. Only a value that has to be BUILT in Javascript falls back: it is marshalled
        /// into the scratch array the old way, and the frame carries its index. That fallback costs exactly
        /// what the whole transport used to cost, so nothing is ever slower than before.
        /// </summary>
        internal int WriteArgsToFrame(object?[] args)
        {
            var offset = _frameTop;
            // reserve at least one slot even with no arguments - the result lands in the first slot of
            // this call's region, the same convention the array buffer used
            _frameTop += Math.Max(args.Length, 1);
            if (_frameTop > _argFrame.Capacity)
                throw new InvalidOperationException($"the argument frame holds {_argFrame.Capacity} slots and a call needed {_frameTop}");
            for (var i = 0; i < args.Length; i++)
            {
                var item = args[i];
                if (item == null)
                {
                    _argFrame.WriteTagged(offset + i, ArgTag.Null, 0);
                    continue;
                }
                var itemType = item.GetType();
                var marshaller = GetMarshaller(itemType);
                if (marshaller.TryWriteArg(itemType, item, out var tag, out var payload))
                {
                    _argFrame.WriteTagged(offset + i, tag, payload);
                    continue;
                }
                // has to be constructed Javascript side - build it in the scratch array and point at it
                marshaller.NetToJS(itemType, _netToJSBuffer, (double)(offset + i), item);
                _argFrame.WriteTagged(offset + i, ArgTag.Scratch, offset + i);
            }
            return offset;
        }

        /// <summary>Releases the current call's region of the frame.</summary>
        internal void ReleaseFrameArgs(int offset) => _frameTop = offset;

        /// <summary>
        /// Writes one member of an object being built into the frame, as a name/value PAIR at
        /// <paramref name="index"/>.<br/>
        /// The name is an interned slot id - a property name is a fixed literal per type, so it crosses
        /// once per process and is a number every time after.
        /// </summary>
        internal void WriteMemberToFrame(int offset, int index, Marshallers.ClassMemberJsonInfo member, object? value)
        {
            var at = offset + index * 2;
            _argFrame.WriteTagged(at, ArgTag.Slot, member.NameSlot);
            if (value == null)
            {
                _argFrame.WriteTagged(at + 1, ArgTag.Null, 0);
                return;
            }
            // Resolve the marshaller ONCE per member, exactly as the per-member path did - and ONLY when
            // the declaration pins the runtime type. A member declared as a base class may hold any
            // subclass, and the two do not marshal the same way, so that case has to keep asking the value
            // for its own type. Caching it unconditionally is a real bug that ObjectMemberMarshalTests
            // exists to catch.
            Type valueType;
            JSMarshaller marshaller;
            if (member.RuntimeTypeIsKnown)
            {
                valueType = member.MemberType;
                marshaller = member.CachedMarshaller ??= GetMarshaller(valueType);
            }
            else
            {
                valueType = value.GetType();
                marshaller = GetMarshaller(valueType);
            }
            if (marshaller.TryWriteArg(valueType, value, out var tag, out var payload))
            {
                _argFrame.WriteTagged(at + 1, tag, payload);
                return;
            }
            // has to be built Javascript side - the scratch path, exactly as before
            marshaller.NetToJS(valueType, _netToJSBuffer, (double)(at + 1), value);
            _argFrame.WriteTagged(at + 1, ArgTag.Scratch, at + 1);
        }

        /// <summary>
        /// Reserves room for <paramref name="memberCount"/> name/value pairs and returns the frame offset.
        /// Pair with <see cref="ReleaseFrameArgs"/> in a finally.
        /// </summary>
        internal int ReserveMemberPairs(int memberCount)
        {
            var offset = _frameTop;
            _frameTop += Math.Max(memberCount * 2, 1);
            if (_frameTop > _argFrame.Capacity)
                throw new InvalidOperationException($"the argument frame holds {_argFrame.Capacity} slots and an object needed {_frameTop}");
            return offset;
        }

        /// <summary>
        /// Reads the result the Javascript side wrote into this call's own frame slot.<br/>
        /// A primitive is read straight out of .Net memory with no crossing. Anything else arrives as a
        /// slot id, so an object returned from a call becomes a handle with no crossing and no proxy
        /// either - only a string still needs a typed read, because a Javascript string cannot live in
        /// .Net memory.
        /// </summary>
        internal object? ReadFrameResult(Type type, int offset)
        {
            var tag = _argFrame.ReadTag(offset);
            var payload = _argFrame.Read(offset);
            switch (tag)
            {
                case ArgTag.Null:
                case ArgTag.Undefined:
                    return type.GetDefaultValue();
                case ArgTag.Number:
                case ArgTag.Boolean:
                {
                    // reading a primitive out of the frame means the whole call moved no data across the
                    // boundary at all. Anything the frame cannot represent goes through the marshaller.
                    if (type == typeof(double)) return payload;
                    if (type == typeof(int)) return (int)payload;
                    if (type == typeof(float)) return (float)payload;
                    if (type == typeof(long)) return (long)payload;
                    if (type == typeof(bool)) return payload != 0;
                    break;
                }
            }
            // everything else - a slot id, a scratch value, or a primitive wanted as some other type -
            // is handed to the marshaller registry through a handle on the value.
            using var handle = tag == ArgTag.Slot
                ? new SpawnJSHandle(payload)
                : new SpawnJSHandle(_netToJSBuffer, (double)offset, true);
            return MarshallJSToNet(type, handle);
        }

        /// <summary>
        /// Writes the arguments into the shared buffer and returns the offset they start at. Pair every
        /// call with <see cref="ReleaseArgs"/> in a finally.
        /// </summary>
        int WriteArgs(object?[] args)
        {
            var offset = _bufferTop;
            // Reserve at least one slot even with no arguments: the result comes back in the FIRST slot of
            // this call's region, so with zero arguments there would be no region and the top would still
            // point at the result slot. A nested call made while marshalling that result - and marshallers
            // do make them - would then write its own arguments straight over it.
            _bufferTop += Math.Max(args.Length, 1);
            for (var i = 0; i < args.Length; i++)
            {
                var item = args[i];
                var itemType = item?.GetType();
                GetMarshaller(itemType).NetToJS(itemType, _netToJSBuffer, (double)(offset + i), item);
            }
            return offset;
        }

        /// <summary>
        /// Releases the current call's region of the buffer
        /// </summary>
        void ReleaseArgs(int offset) => _bufferTop = offset;

        /// <summary>
        /// Marshall object?[]? args to a Javascript Array
        /// </summary>
        internal SpawnJSHandle? MarshallNetArrayToJSArray(object?[]? args)
        {
            if (args == null) return null;
            var ret = NewJSArray();
            for (var i = 0; i < args.Length; i++)
            {
                var item = args[i];
                var itemType = item?.GetType();
                var marshaller = GetMarshaller(itemType);
                marshaller.NetToJS(itemType, ret, i, item);
            }
            return ret;
        }
        /// <summary>
        /// Writes obj to jsParent[jsKey]
        /// </summary>
        internal void MarshallNetToJS(SpawnJSHandle jsParent, object jsKey, object? obj)
        {
            if (CountCalls) CountCall("marshal:netToJS");
            var type = obj?.GetType();
            var marshaller = GetMarshaller(type);
            marshaller.NetToJS(type, jsParent, jsKey, obj);
        }
        /// <summary>
        /// Reads type from jsParent[jsKey]
        /// </summary>
        internal object? MarshallJSToNet(Type type, SpawnJSHandle jsParent, object jsKey)
        {
            if (CountCalls) CountCall("marshal:jsToNet");
            var marshaller = GetMarshaller(type);
            using var jsHandle = new SpawnJSHandle(jsParent, jsKey, true);
            return marshaller.JSToNet(type, jsHandle);
        }
        /// <summary>
        /// Reads type from jsParent[jsKey]
        /// </summary>
        internal object? MarshallJSToNet(Type type, SpawnJSHandle jsHandle)
        {
            var marshaller = GetMarshaller(type);
            return marshaller.JSToNet(type, jsHandle);
        }
        #endregion
        #region Call counting
        /// <summary>
        /// Set true to count generic-dispatcher calls by command name. Off by default; the check is a
        /// single bool read on the call path.<br/>
        /// This exists because "which calls does a workload actually make" is otherwise guesswork, and
        /// guessing sent an optimisation at the wrong shape once already.
        /// </summary>
        public static bool CountCalls { get; set; }
        /// <summary>
        /// Generic-dispatcher calls by command name since the last <see cref="ResetCallCounts"/>.
        /// </summary>
        public static Dictionary<string, long> CallCounts { get; } = new();
        /// <summary>
        /// Clears <see cref="CallCounts"/>.
        /// </summary>
        public static void ResetCallCounts() { lock (CallCounts) CallCounts.Clear(); }
        internal static void CountCall(string cmd)
        {
            lock (CallCounts) CallCounts[cmd] = CallCounts.TryGetValue(cmd, out var n) ? n + 1 : 1;
        }
        #endregion
        #region Sync NetRun
        internal T NetRun<T>(string cmd, object?[]? args = null)
        {
            var ret = NetRun(typeof(T), cmd, args)!;
            return (T)ret;
        }
        internal object? NetRun(Type type, string cmd, object?[]? args = null)
        {
            if (CountCalls) CountCall(cmd);
            args ??= new object?[0];
            if (UseArgFrame) return NetRunViaFrame(type, cmd, args);
            var offset = WriteArgs(args);
            try
            {
                NetToJSCall(cmd, offset, args.Length);
                // the result comes back in the first slot of this call's own region
                var netRet = MarshallJSToNet(type, _netToJSBuffer, (double)offset);
                // A Nullable<T> target is satisfied by a T. There is no boxed Nullable<T> at runtime -
                // boxing one with a value produces a boxed T - so comparing against the declared type
                // rejects every non-null nullable value type, Get<int?> included.
                var expected = Nullable.GetUnderlyingType(type) ?? type;
                // assignability, not exact equality: a marshaller may legitimately hand back a SUBCLASS of
                // what was asked for. An async method returning Task<T> actually returns an
                // AsyncStateMachineBox<T>, and a wrapper marshaller may return a derived wrapper. Exact
                // equality rejected both while still being no better at catching a genuinely wrong type.
                if (netRet != null && !expected.IsInstanceOfType(netRet))
                    throw new Exception($"{nameof(SpawnJSRuntime)}.NetRun expected {expected.Name} got {netRet.GetType().Name}");
                return netRet;
            }
            finally
            {
                ReleaseArgs(offset);
            }
        }
        internal void NetRunVoid(string cmd, object?[]? args = null)
        {
            if (CountCalls) CountCall(cmd);
            args ??= new object?[0];
            var offset = WriteArgs(args);
            try
            {
                NetToJSCall(cmd, offset, args.Length);
            }
            finally
            {
                ReleaseArgs(offset);
            }
        }
        #endregion
        #region Async NetRun
        // There is no async dispatcher. An async command is just a SYNCHRONOUS call that happens to return
        // a Promise, so it goes over the same flat buffer stack as everything else and gets the same "only
        // primitives cross" property. The Promise is then turned into a Task the way SpawnDev.BlazorJS has
        // always done it - then(resolve, reject) with .Net callbacks - rather than by asking the runtime to
        // marshal a Task across the boundary.
        //
        // That is what lets the buffer be a stack at all. A real async binding could not use one: the call
        // has not finished when it returns, so its region could not be released in order. Here the
        // synchronous part finishes immediately and the region unwinds; the eventual value arrives later
        // through the callback channel, which has its own storage.
        internal async Task<T> NetRunAsync<T>(string cmd, object?[]? args = null)
        {
            using var promise = NetRun<Promise>(cmd, args);
            return await promise.ThenAsync<T>();
        }

        internal async Task<object?> NetRunAsync(Type type, string cmd, object?[]? args = null)
        {
            using var promise = NetRun<Promise>(cmd, args);
            return await promise.ThenAsync(type);
        }

        internal async Task NetRunVoidAsync(string cmd, object?[]? args = null)
        {
            using var promise = NetRun<Promise>(cmd, args);
            await promise.ThenAsync();
        }
        #endregion
        #region NetToJS calls
        /// <summary>
        /// Runs a synchronous command. Only primitives cross: the command name, the offset its arguments
        /// start at in the shared buffer, and how many there are. The result is left in the first slot of
        /// that region.
        /// </summary>
        private void NetToJSCall(string cmd, int offset, int length)
            => Reflect.ApplyVoid(_netToJSCall.JSObjectRequired, SpawnJSInterop.JSObjectRequired, new object?[] { cmd, (double)offset, (double)length });

        #endregion
    }
}
