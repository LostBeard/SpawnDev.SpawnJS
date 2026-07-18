using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Interface for a wrapped JSObject that enables interop using SpawnJSRuntime Marshallers
    /// </summary>
    public interface ISpawnJSObjectReference
    {
        /// <summary>
        /// Returns true if JSObject is null or disposed
        /// </summary>
        bool IsDisposed { get; }
        /// <summary>
        /// The underlying JSObject
        /// </summary>
        JSObject JSObject { get; init; }
        /// <summary>
        /// Marshalls this to Javascript and then returns as type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T As<T>();
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19);
        /// <summary>
        /// Call the property
        /// </summary>
        T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20);
        /// <summary>
        /// Invoke a property and return as type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        T CallApply<T>(string identifier, object?[] args);
        /// <summary>
        /// Invoke a property
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        Task<T> CallApplyAsync<T>(string identifier, object?[] args);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19);
        /// <summary>
        /// Call the property
        /// </summary>
        Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20);
        /// <summary>
        /// Invoke a property constructor and return as type T
        /// </summary>
        T New<T>(string identifier);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20);
        /// <summary>
        /// Call the property constructor
        /// </summary>
        T NewApply<T>(string identifier, object?[] args);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19);
        /// <summary>
        /// Call the property
        /// </summary>
        void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20);
        /// <summary>
        /// Invoke a property
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        void CallVoidApply(string identifier, object?[] args);
        /// <summary>
        /// Invoke a property
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        Task CallVoidApplyAsync(string identifier, object?[] args);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19);
        /// <summary>
        /// Call the property
        /// </summary>
        Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20);
        /// <summary>
        /// Delete an object property
        /// </summary>
        /// <param name="identifier"></param>
        void Delete(string identifier);
        /// <summary>
        /// Dispose the object and its underlying JSObject
        /// </summary>
        void Dispose();
        /// <summary>
        /// Get object property as T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        T Get<T>(string identifier);
        /// <summary>
        /// Get object property as T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string identifier);
        /// <summary>
        /// Check if an object has a property with the specified identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="useIn"></param>
        /// <returns></returns>
        bool Has(string identifier, bool useIn = true);
        /// <summary>
        /// Set an object property to the specified value
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        void Set(string identifier, object? value);
    }
}
