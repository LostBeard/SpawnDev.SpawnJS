namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSRuntime
    {
        #region NewArray

        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshallerForWrite<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshallerForWrite<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshallerForWrite<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshallerForWrite<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshallerForWrite<T5>().NetToJS(jsArgs!, 4, arg5!);
            GetMarshallerForWrite<T6>().NetToJS(jsArgs!, 5, arg6!);
            GetMarshallerForWrite<T7>().NetToJS(jsArgs!, 6, arg7!);
            GetMarshallerForWrite<T8>().NetToJS(jsArgs!, 7, arg8!);
            GetMarshallerForWrite<T9>().NetToJS(jsArgs!, 8, arg9!);
            GetMarshallerForWrite<T10>().NetToJS(jsArgs!, 9, arg10!);
            GetMarshallerForWrite<T11>().NetToJS(jsArgs!, 10, arg11!);
            GetMarshallerForWrite<T12>().NetToJS(jsArgs!, 11, arg12!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshallerForWrite<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshallerForWrite<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshallerForWrite<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshallerForWrite<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshallerForWrite<T5>().NetToJS(jsArgs!, 4, arg5!);
            GetMarshallerForWrite<T6>().NetToJS(jsArgs!, 5, arg6!);
            GetMarshallerForWrite<T7>().NetToJS(jsArgs!, 6, arg7!);
            GetMarshallerForWrite<T8>().NetToJS(jsArgs!, 7, arg8!);
            GetMarshallerForWrite<T9>().NetToJS(jsArgs!, 8, arg9!);
            GetMarshallerForWrite<T10>().NetToJS(jsArgs!, 9, arg10!);
            GetMarshallerForWrite<T11>().NetToJS(jsArgs!, 10, arg11!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshallerForWrite<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshallerForWrite<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshallerForWrite<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshallerForWrite<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshallerForWrite<T5>().NetToJS(jsArgs!, 4, arg5!);
            GetMarshallerForWrite<T6>().NetToJS(jsArgs!, 5, arg6!);
            GetMarshallerForWrite<T7>().NetToJS(jsArgs!, 6, arg7!);
            GetMarshallerForWrite<T8>().NetToJS(jsArgs!, 7, arg8!);
            GetMarshallerForWrite<T9>().NetToJS(jsArgs!, 8, arg9!);
            GetMarshallerForWrite<T10>().NetToJS(jsArgs!, 9, arg10!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshallerForWrite<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshallerForWrite<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshallerForWrite<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshallerForWrite<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshallerForWrite<T5>().NetToJS(jsArgs!, 4, arg5!);
            GetMarshallerForWrite<T6>().NetToJS(jsArgs!, 5, arg6!);
            GetMarshallerForWrite<T7>().NetToJS(jsArgs!, 6, arg7!);
            GetMarshallerForWrite<T8>().NetToJS(jsArgs!, 7, arg8!);
            GetMarshallerForWrite<T9>().NetToJS(jsArgs!, 8, arg9!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshallerForWrite<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshallerForWrite<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshallerForWrite<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshallerForWrite<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshallerForWrite<T5>().NetToJS(jsArgs!, 4, arg5!);
            GetMarshallerForWrite<T6>().NetToJS(jsArgs!, 5, arg6!);
            GetMarshallerForWrite<T7>().NetToJS(jsArgs!, 6, arg7!);
            GetMarshallerForWrite<T8>().NetToJS(jsArgs!, 7, arg8!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshallerForWrite<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshallerForWrite<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshallerForWrite<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshallerForWrite<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshallerForWrite<T5>().NetToJS(jsArgs!, 4, arg5!);
            GetMarshallerForWrite<T6>().NetToJS(jsArgs!, 5, arg6!);
            GetMarshallerForWrite<T7>().NetToJS(jsArgs!, 6, arg7!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshallerForWrite<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshallerForWrite<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshallerForWrite<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshallerForWrite<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshallerForWrite<T5>().NetToJS(jsArgs!, 4, arg5!);
            GetMarshallerForWrite<T6>().NetToJS(jsArgs!, 5, arg6!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshallerForWrite<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshallerForWrite<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshallerForWrite<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshallerForWrite<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshallerForWrite<T5>().NetToJS(jsArgs!, 4, arg5!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshallerForWrite<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshallerForWrite<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshallerForWrite<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshallerForWrite<T4>().NetToJS(jsArgs!, 3, arg4!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshallerForWrite<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshallerForWrite<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshallerForWrite<T3>().NetToJS(jsArgs!, 2, arg3!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2>(T1 arg1, T2 arg2)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshallerForWrite<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshallerForWrite<T2>().NetToJS(jsArgs!, 1, arg2!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1>(T1 arg1)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshallerForWrite<T1>().NetToJS(jsArgs!, 0, arg1!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray() 
            => new SpawnJSObjectReference(_spawnJSObjectNewArray());
        #endregion
    }
}
