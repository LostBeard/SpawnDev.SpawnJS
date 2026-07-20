using SpawnDev.SpawnJS.Marshallers;
using SpawnDev.SpawnJS.Marshallers.SpawnDev.SpawnJS;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// SpawnJS runtime for .Net and Javascript interop
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform("browser")]
    public partial class SpawnJSRuntime : SpawnJSObjectReference
    {
        /// <summary>
        /// Set to true to see verbose debugging messages
        /// </summary>
        public bool Verbose
        {
            get => _verbose;
            set
            {
                _verbose = value;
                if (SpawnJSInterop?.IsDisposed == false) SpawnJSInterop.SetProperty("verbose", value);
            }
        }
        bool _verbose = false;
        /// <summary>
        /// SpawnJSRuntime instance
        /// </summary>
        public static SpawnJSRuntime? Instance { get; private set; }
        /// <summary>
        /// SpawnJSInterop Javascript instance
        /// </summary>
        private SpawnJSHandle SpawnJSInterop;
        /// <summary>
        /// SpawnJSInterop._netToJSCall function handle
        /// </summary>
        private SpawnJSHandle _netToJSCall;
        /// <summary>
        /// SpawnJSInterop._netToJSCallAsync function handle
        /// </summary>
        private SpawnJSHandle _netToJSCallAsync;
        /// <summary>
        /// JSObject marshallers used for marshalling data between .Net and Javasript
        /// </summary>
        public IList<JSMarshaller> Marshallers { get; private set; } = new List<JSMarshaller>();
        /// <summary>
        /// Create a new instance of SpawnJSRuntime
        /// </summary>
        public SpawnJSRuntime() : base(JSHost.GlobalThis)
        {
            if (Instance != null) throw new Exception("Already exists");
            // add built-in marshallers
            Marshallers.Add(new DefaultMarshaller());
            Marshallers.Add(new ObjectMarshaller());
            Marshallers.Add(new ArrayMarshaller());
            Marshallers.Add(new ListMarshaller());
            Marshallers.Add(new ByteArrayMarshaller());
            Marshallers.Add(new StringMarshaller());
            Marshallers.Add(new BooleanMarshaller());
            Marshallers.Add(new NumberMarshaller());
            Marshallers.Add(new SpawnJSObjectReferenceMarshaller());
            Marshallers.Add(new SpawnJSObjectMarshaller());
            Marshallers.Add(new JSObjectMarshaller());
            Marshallers.Add(new StructMarshaller());
            Marshallers.Add(new DelegateMarshaller());
            Marshallers.Add(new IMarshalOutByJSHandleMarshaller());
            Marshallers.Add(new SpawnJSHandleMarshaller());
            Marshallers.Add(new UnionMarshaller());
            // create a new instance of SpawnJSInterop Javascript class for interop with this isntance of .Net
            SpawnJSInterop = JSHandle.InvokePropertyConstructor("SpawnJSInterop", JSHost.DotnetInstance)!;
            // get _netToJSCall function in SpawnJSInterop
            _netToJSCall = SpawnJSInterop.GetPropertyAsJSHandle("_netToJSCall") ?? throw new Exception("SpawnJSInterop._netToJSCall not found");
            // get _netToJSCallAsync function in SpawnJSInterop
            _netToJSCallAsync = SpawnJSInterop.GetPropertyAsJSHandle("_netToJSCallAsync") ?? throw new Exception("SpawnJSInterop._netToJSCallAsync not found");
            // set _JSToNetCall to _JSToNetCall on SpawnJSInterop JS instance
            Reflect.Set(SpawnJSInterop.JSObject!, "_JSToNetCall", _JSToNetCall);
            Initializing = false;
            Instance = this;
        }
        /// <summary>
        /// Returns true while the runtime is initializing
        /// </summary>
        public bool Initializing { get; } = true;
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
        /// <summary>
        /// JS ➡️ .Net method
        /// </summary>
        private JSObject _JSToNetCall(string cmd, JSObject argsArray)
        {
            using var argsArrayHandle = argsArray == null ? null : new SpawnJSHandle(argsArray);
            var ret = Callback.JSToNetDispatch(cmd, argsArrayHandle!);
            return ret?.JSObject!;
        }
        /// <summary>
        /// Create a new Javascript Object as JSObject
        /// </summary>
        /// <returns></returns>
        public SpawnJSHandle NewJSObject() => JSHandle.InvokePropertyConstructor("Object")!;
        /// <summary>
        /// Create a new Javascript Array as JSObject
        /// </summary>
        /// <returns></returns>
        public SpawnJSHandle NewJSArray() => JSHandle.InvokePropertyConstructor("Array")!;
        /// <summary>
        /// Create a new Javascript Promise as JSObject with `resolve` and `reject` attached as properties
        /// </summary>
        /// <returns></returns>
        public SpawnJSHandle NewEasyPromise() => NetRun<SpawnJSHandle>("newEasyPromise");
    }
}
