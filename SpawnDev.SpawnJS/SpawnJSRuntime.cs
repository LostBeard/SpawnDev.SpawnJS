using SpawnDev.SpawnJS.Extensions;
using SpawnDev.SpawnJS.Marshallers;
using SpawnDev.SpawnJS.Marshallers.SpawnDev.SpawnJS;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// SpawnJS runtime for .Net and Javascript interop
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform("browser")]
    public partial class SpawnJSRuntime : ISpawnJSObjectReference, IDisposable
    {
        JSObject SpawnJSInterop;
        JSObject _netToJSCall;
        JSObject _netToJSCallAsync;
        /// <summary>
        /// Set to true to see verbose debugging messages
        /// </summary>
        public static bool Verbose = false;
        /// <summary>
        /// GlobalThis (do not dispose this instance)
        /// </summary>
        public JSObject JSObject { get; init; }
        /// <summary>
        /// SpawnJSRuntime instance
        /// </summary>
        public static SpawnJSRuntime? Instance { get; private set; }
        /// <summary>
        /// JSObject marshallers used for marshalling data between .Net and Javasript
        /// </summary>
        public IList<JSMarshaller> Marshallers { get; private set; } = new List<JSMarshaller>();
        /// <summary>
        /// Returns true if disposed
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// Create a new instance of SpawnJSRuntime
        /// </summary>
        public SpawnJSRuntime()
        {
            if (Instance != null) throw new Exception("Already exists");
            Instance = this;
            Marshallers.Add(new DefaultMarshaller());
            Marshallers.Add(new ObjectMarshaller());
            Marshallers.Add(new IEnumerableMarshaller());
            Marshallers.Add(new ByteArrayMarshaller());
            Marshallers.Add(new StringMarshaller());
            Marshallers.Add(new BooleanMarshaller());
            Marshallers.Add(new NumberMarshaller());
            Marshallers.Add(new SpawnJSObjectReferenceMarshaller());
            Marshallers.Add(new SpawnJSObjectMarshaller());
            Marshallers.Add(new JSObjectMarshaller());
            Marshallers.Add(new StructMarshaller());
            Marshallers.Add(new JSToNetInvokerMarshaller());
            JSObject = JSHost.GlobalThis;
            SpawnJSInterop = JSObject.InvokePropertyConstructor("SpawnJSInterop", JSHost.DotnetInstance)!;
            _netToJSCall = SpawnJSInterop.GetPropertyAsJSObject("_netToJSCall") ?? throw new Exception("SpawnJSInterop._netToJSCall not found");
            _netToJSCallAsync = SpawnJSInterop.GetPropertyAsJSObject("_netToJSCallAsync") ?? throw new Exception("SpawnJSInterop._netToJSCallAsync not found");
            var gthis = new SpawnJSObjectReference(SpawnJSInterop);
            gthis.Set("_JSToNetCall", (Func<string, JSObject, JSObject>)_JSToNetCall);
        }
        /// <summary>
        /// Log to the Javascript console (console.log)
        /// </summary>
        /// <param name="args"></param>
        public void Log(params object?[] args)
        {
            CallVoidApply("console.log", args);
        }
        /// <summary>
        /// Log an error to the Javascript console (console.error)
        /// </summary>
        /// <param name="args"></param>
        public void LogError(params object?[] args)
        {
            CallVoidApply("console.error", args);
        }
        JSObject _JSToNetCall(string cmd, JSObject argsArray)
        {
            Console.WriteLine("_JSToNetCall called!!!!!!!!!!");
            return null!;
        }
        async Task _JSToNetCallVoidAsync(string cmd, JSObject argsArray)
        {
            throw new NotImplementedException();
        }
        async Task<JSObject?> _JSToNetCallAsync(string cmd, JSObject argsArray)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Marshalls this to Javascript and then returns as type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T As<T>() => NetRun<T>("returnMe", new object[] { JSObject });
        /// <summary>
        /// Get object property as T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public T Get<T>(string identifier) => NetRun<T>("getProperty", new object[] { JSObject, identifier });
        /// <summary>
        /// Get object property as T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string identifier) => NetRunAsync<T>("getProperty", new object[] { JSObject, identifier });
        /// <summary>
        /// Check if an object has a property with the specified identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="useIn"></param>
        /// <returns></returns>
        public bool Has(string identifier, bool useIn = true)
            => useIn ? NetRun<bool>("_in", new object[] { identifier, JSObject }) : NetRun<bool>("hasOwnPropertySafe", new object[] { JSObject, identifier });

        /// <summary>
        /// Set an object property to the specified value
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public void Set(string identifier, object? value) => NetRunVoid("setProperty", new object[] { JSObject, identifier, value! });

        /// <summary>
        /// Delete an object property
        /// </summary>
        /// <param name="identifier"></param>
        public void Delete(string identifier) => NetRunVoid("deleteProperty", new object[] { JSObject, identifier });

        #region New
        /// <summary>
        /// Invoke a property constructor and return as type T
        /// </summary>
        public SpawnJSObjectReference New(string identifier)
            => NewApply(identifier, new object?[] { });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1)
            => NewApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2)
            => NewApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference New(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => NewApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public SpawnJSObjectReference NewApply(string identifier, object?[] args) => NetRun<SpawnJSObjectReference>("invokePropertyConstructor", new object[] { JSObject, identifier, args });
        #endregion

        #region New T
        /// <summary>
        /// Invoke a property constructor and return as type T
        /// </summary>
        public T New<T>(string identifier)
            => NewApply<T>(identifier, new object?[] { });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1)
            => NewApply<T>(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2)
            => NewApply<T>(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T New<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => NewApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Call the property constructor
        /// </summary>
        public T NewApply<T>(string identifier, object?[] args) => NetRun<T>("invokePropertyConstructor", new object[] { JSObject, identifier, args });
        #endregion

        #region Call
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier)
            => CallApply<T>(identifier, new object?[] { });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1)
            => CallApply<T>(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2)
            => CallApply<T>(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property
        /// </summary>
        public T Call<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => CallApply<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Invoke a property and return as type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T CallApply<T>(string identifier, object?[] args) => NetRun<T>("invokeProperty", new object[] { JSObject, identifier, args });
        #endregion

        #region CallVoid
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier)
            => CallVoidApply(identifier, new object?[] { });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1)
            => CallVoidApply(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2)
            => CallVoidApply(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property
        /// </summary>
        public void CallVoid(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => CallVoidApply(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Invoke a property
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        public void CallVoidApply(string identifier, object?[] args) => NetRunVoid("invokeProperty", new object[] { JSObject, identifier, args });
        #endregion

        #region CallVoidAsync
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier)
            => CallVoidApplyAsync(identifier, new object?[] { });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1)
            => CallVoidApplyAsync(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task CallVoidAsync(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => CallVoidApplyAsync(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Invoke a property
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        public Task CallVoidApplyAsync(string identifier, object?[] args) => NetRunVoidAsync("invokeProperty", new object[] { JSObject, identifier, args });
        #endregion

        #region CallAsync
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier)
            => CallApplyAsync<T>(identifier, new object?[] { });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1)
            => CallApplyAsync<T>(identifier, new object?[] { arg1 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 });
        /// <summary>
        /// Call the property
        /// </summary>
        public Task<T> CallAsync<T>(string identifier, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, object? arg11, object? arg12, object? arg13, object? arg14, object? arg15, object? arg16, object? arg17, object? arg18, object? arg19, object? arg20)
            => CallApplyAsync<T>(identifier, new object?[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20 });
        /// <summary>
        /// Invoke a property
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        public Task<T> CallApplyAsync<T>(string identifier, object?[] args) => NetRunAsync<T>("invokeProperty", new object[] { JSObject, identifier, args });
        #endregion
        /// <summary>
        /// Create a new Javascript Object as JSObject
        /// </summary>
        /// <returns></returns>
        public JSObject NewJSObject() => JSObject.InvokePropertyConstructor("Object")!;
        /// <summary>
        /// Create a new Javascript Array as JSObject
        /// </summary>
        /// <returns></returns>
        public JSObject NewJSArray() => JSObject.InvokePropertyConstructor("Array")!;
        /// <summary>
        /// Create a new Javascript Promise as JSObject with `resolve` and `reject` attached as properties
        /// </summary>
        /// <returns></returns>
        public JSObject NewEasyPromise() => NetRun<JSObject>("newEasyPromise");
        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
        }
    }
}
