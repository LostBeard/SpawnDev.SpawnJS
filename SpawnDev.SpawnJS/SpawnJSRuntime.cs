using SpawnDev.SpawnJS.JSObjects;
using SpawnDev.SpawnJS.Marshaller;
using SpawnDev.SpawnJS.Marshallers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// The entry point for all .Net to Javascript interop in SpawnJS.<br/>
    /// <br/>
    /// Design rule that defines this library: <see cref="JSObject"/> (Microsoft's WASM interop handle) is
    /// NOT used anywhere. Its disposal quirk was the multi-year blocker for the previous interop attempts,
    /// and it leaked into Gemineachy. The ONLY place a <see cref="JSObject"/> is permitted is the single
    /// _registerInstance call that hands this app's DotnetInstance to the JS side -
    /// and never again. Everything else references JS values by a numeric id (see
    /// <see cref="SpawnJSObjectReference"/>), so nothing crosses the boundary that needs disposing on the
    /// Microsoft interop table.<br/>
    /// <br/>
    /// The runtime itself is a <see cref="SpawnJSObjectReference"/> whose id is GlobalThisId,
    /// so calling <c>JS.PropertyGet*/PropertySet</c> operates directly on the JS <c>globalThis</c>.
    /// </summary>
    public partial class SpawnJSRuntime : SpawnJSObjectReference, IGlobalScopeSource
    {
        /// <summary>
        /// True when running on the browser-wasm runtime. It asks the .Net runtime rather than Javascript,
        /// so it is valid before any interop has happened.<br/>
        /// ⚠️ This does NOT mean "running in a browser". A WebAssembly console app on Node also reports
        /// true, because it targets the same runtime - measured: the console host reports IsBrowser=True
        /// with a global scope of "Object". To ask whether there is a page, use <see cref="IsWindow"/>.
        /// SpawnDev.BlazorJS has the same semantics.
        /// </summary>
        public bool IsBrowser => OperatingSystem.IsBrowser();
        internal SpawnJSObjectReference SpawnJSInterop { get; } = new SpawnJSObjectReference(SpawnJSInteropId);
        /// <summary>
        /// The constructor name of globalThis, which is what identifies the scope: "Window",
        /// "DedicatedWorkerGlobalScope", "SharedWorkerGlobalScope", "ServiceWorkerGlobalScope" - or on a
        /// non browser host, something else entirely.
        /// </summary>
        public string GlobalScopeName => _globalScopeName ??= ConstructorName() ?? "";
        string? _globalScopeName;

        /// <inheritdoc/>
        Task<GlobalScope> IGlobalScopeSource.GetGlobalScope() => Task.FromResult(GlobalScope);
        /// <summary>
        /// GlobalScope enum
        /// </summary>
        public GlobalScope GlobalScope { get; private set; }
        /// <summary>
        /// GlobalThis
        /// </summary>
        public SpawnJSObject? GlobalThis { get; private set; }
        /// <summary>
        /// If the globalThis is a Window, WindowThis will refer to globalThis, otherwise null.
        /// </summary>
        public Window? WindowThis { get; private set; }
        /// <summary>
        /// If the globalThis is a DedicatedWorkerGlobalScope, DedicateWorkerThis will refer to globalThis, otherwise null.
        /// </summary>
        public DedicatedWorkerGlobalScope? DedicateWorkerThis { get; private set; }
        /// <summary>
        /// If the globalThis is a SharedWorkerGlobalScope, SharedWorkerThis will refer to globalThis, otherwise null.
        /// </summary>
        public SharedWorkerGlobalScope? SharedWorkerThis { get; private set; }
        /// <summary>
        /// If the globalThis is a ServiceWorkerGlobalScope, ServiceWorkerThis will refer to globalThis, otherwise null.
        /// </summary>
        public ServiceWorkerGlobalScope? ServiceWorkerThis { get; private set; }
        /// <summary>
        /// This app instance's id
        /// </summary>
        public string InstanceId { get; }
        /// <summary>
        /// The URL this app was LOADED from - the origin of its own <c>main.*</c> / <c>_framework</c>, with
        /// a trailing slash. Unlike the host page's <c>document.baseURI</c>, this stays correct when the app
        /// is served from a CDN at a different path than the host page, which is what worker entry scripts
        /// (main.classic.js / main.module.js / _framework/*) must resolve against.<br/>
        /// Determined per-runtime from THIS app's own dotnet runtime, so two SpawnJS apps loaded from
        /// different origins on one page each report their own base.<br/>
        /// Empty string when it could not be determined (e.g. a non-browser host).
        /// </summary>
        public string AppBaseUri { get; private set; } = "";

        /// <summary>
        /// True when running in a page rather than a worker
        /// </summary>
        public bool IsWindow => GlobalScopeName == "Window";

        /// <summary>
        /// True in a dedicated worker
        /// </summary>
        public bool IsDedicatedWorkerGlobalScope => GlobalScopeName == "DedicatedWorkerGlobalScope";

        /// <summary>
        /// True in a shared worker
        /// </summary>
        public bool IsSharedWorkerGlobalScope => GlobalScopeName == "SharedWorkerGlobalScope";

        /// <summary>
        /// True in a service worker
        /// </summary>
        public bool IsServiceWorkerGlobalScope => GlobalScopeName == "ServiceWorkerGlobalScope";

        /// <summary>
        /// True in any kind of worker
        /// </summary>
        public bool IsWorker => IsDedicatedWorkerGlobalScope || IsSharedWorkerGlobalScope || IsServiceWorkerGlobalScope;
        /// <summary>
        /// The process-wide singleton. Created on first access if it does not already exist.
        /// </summary>
        public static SpawnJSRuntime Instance => _instance ??= new SpawnJSRuntime();
        private static SpawnJSRuntime? _instance;
        /// <summary>
        /// True once the singleton has been constructed.
        /// </summary>
        public static bool IsCreated => _instance != null;
        /// <summary>
        /// The ordered set of marshallers that convert values between .Net and Javascript. A marshaller is
        /// selected by asking each (last registered wins) whether it can handle a given .Net type - see
        /// <see cref="GetMarshaller{TType}"/>.
        /// </summary>
        public IList<JSMarshaller> Marshallers { get; private set; } = new List<JSMarshaller>();
        /// <summary>
        /// A reference to this .Net WASM app's DotnetInstance held on the JS side. Used to reach the
        /// assembly's [JSExport] methods (e.g. to resolve async calls) - see <see cref="InteropCallApplyAsync{T}"/>.
        /// </summary>
        public SpawnJSObjectReference DotnetInstance { get; private set; }
        /// <summary>
        /// When true, marshaller selection is logged to the console. Off by default so libraries stay quiet.
        /// </summary>
        public bool Verbose
        {
            get => _verbose;
            set
            {
                if (_verbose == value) return;
                _verbose = value;
                Set("SpawnJSInterop.verbose", _verbose);
            }
        }
        bool _verbose = false;
        internal string[] InteropMethods;
        /// <summary>
        /// Creates the runtime. The base id is GlobalThisId so the instance addresses JS
        /// <c>globalThis</c> directly. Registers the built-in marshallers in priority order (last wins).
        /// </summary>
        private SpawnJSRuntime() : base(GlobalThisId)
        {
            _instance = this;
            var id = Convert.ToHexString(RandomNumberGenerator.GetBytes(8));
            var chunkSize = 4;
            InstanceId = string.Join("-", Enumerable.Range(0, id.Length / chunkSize).Select(i => id.Substring(i * chunkSize, chunkSize)));

            // SpawnJSRuntime requires IsBrowser so if not, exit here
            if (!IsBrowser)
            {
                DotnetInstance = default!;
                InteropMethods = new string[0];
                GlobalScope = GlobalScope.NonBrowser;
                return;
            }

            // Registration order matters: GetMarshaller scans this list in REVERSE, so a marshaller added
            // later takes precedence when more than one reports it can marshal a type. The more specific /
            // higher-priority handlers (arrays, object references) are therefore added last.
            // .Net POCO <-> plain JS object (property-walk clone, honours Json attributes). Handles class
            // AND struct POCOs (and Nullable<TStruct>) - a struct differs only in needing a boxed target on
            // the read, so it does not warrant a second marshaller that would duplicate the whole walk.
            // Most generic, so registered FIRST = lowest priority; any more specific marshaller below wins
            // the reverse scan - which is also what keeps this from ever seeing a number, enum or tuple.
            Marshallers.Add(new PocoMarshaller<object>());

            // .Net IEnumerable<> -> JS: Array
            Marshallers.Add(new IEnumerableMarshaller<string>());           
            // VoidType - nothing is marshalled
            Marshallers.Add(new VoidTypeMarshaller());
            // .Net: object <-> JS: Object
            Marshallers.Add(new ObjectMarshaller());
            // .Net: string <-> JS: string
            Marshallers.Add(new StringMarshaller());
            // .Net: INumber<> <-> JS: Number
            Marshallers.Add(new INumberMarshaller<float>());
            // .Net: Nullable<INumber<>> <-> JS: Number? (catch-all for long?/ulong?/short?/... - added before the
            // specific int?/double?/BigInteger? nullable marshallers so, with the reverse scan, those still win)
            Marshallers.Add(new INumberNullableMarshaller<float>());
            // .Net: double <-> JS: Number
            Marshallers.Add(new DoubleMarshaller());
            // .Net: double? <-> JS: Number?
            Marshallers.Add(new DoubleNullableMarshaller());
            // .Net: double <-> JS: Number
            Marshallers.Add(new Int32Marshaller());
            // .Net: double? <-> JS: Number?
            Marshallers.Add(new Int32NullableMarshaller());
            // .Net: bool <-> JS: bool
            Marshallers.Add(new BooleanMarshaller());
            // .Net: bool? <-> JS: bool?
            Marshallers.Add(new BooleanNullableMarshaller());
            // .Net: Tuple, ValueTuple <-> JS: Array
            Marshallers.Add(new ITupleMarshallerFactory());
            // .Net: SpawnJSObjectReference <-> JS: Any
            Marshallers.Add(new SpawnJSObjectReferenceMarshaller());
            // .Net: T[] <-> JS: Array<>
            Marshallers.Add(new ArrayMarshaller<object>());
            // .Net: List<> <-> JS: Array<>
            Marshallers.Add(new ListMarshaller<object>());
            // .Net: Dictionary<string,>/IDictionary<string,>/IReadOnlyDictionary<string,> <-> JS: plain object (record)
            Marshallers.Add(new DictionaryMarshaller<Dictionary<string, object>>());
            // .Net: HeapViewDescriptor -> JS: ArrayBufferView (TypedArray, DataView; copy or persistent)
            Marshallers.Add(new HeapViewDescriptorMarshaller());
            // .Net: Action, Action<>, Func<> -> JS: Function
            Marshallers.Add(new CallbackMarshaller<Callback>());
            // .Net: byte[] <-> JS: Uint8Array
            Marshallers.Add(new ByteArrayMarshaller());
            // .Net: Task, Task<> <-> JS: Promise, Promise<T>
            Marshallers.Add(new TaskMarshaller());
            // .Net: BingInteger <-> JS: BigInt
            Marshallers.Add(new BigIntegerMarshaller());
            // .Net: Union <-> JS: Any
            Marshallers.Add(new UnionMarshallerFactory());
            // .Net: Action, Action<>, Func<> -> JS: Function
            Marshallers.Add(new DelegateMarshallerFactory());
            // .Net: BingInteger? <-> JS: BigInt?
            Marshallers.Add(new BigIntegerNullableMarshaller());
            // .Net: SpawnJSObject <-> JS: Any
            Marshallers.Add(new SpawnJSObjectMarshaller<SpawnJSObject>());
            // .Net: Enum <-> JS: Number
            Marshallers.Add(new EnumMarshallerFactory());
            // .Net: EnumString<> <-> JS: String?
            Marshallers.Add(new EnumStringMarshallerFactory());
            // .Net: EpochDateTime <-> JS: Number?
            Marshallers.Add(new EpochDateTimeMarshaller());
            // .Net: DateTime <-> JS: String
            Marshallers.Add(new DateTimeMarshaller());
            // .Net: DateTime? <> <-> JS: String?
            Marshallers.Add(new DateTimeNullableMarshaller());
            // .Net: HeapView? <> -> JS: ArrayBufferView (TypedArray, DataView; copy or persistent)
            Marshallers.Add(new HeapViewMarshaller<HeapView>());
            // The one and only permitted JSObject use: hand this app's DotnetInstance to the JS side and
            // immediately reduce it to a numeric SpawnJSObjectReference id. Never touched as a JSObject again.
            DotnetInstance = new SpawnJSObjectReference(
                _registerInstance(JSHost.DotnetInstance,
                MappedMethodsChanged,
                AsyncCallResolvedVoid,
                ResolveDouble,
                ResolveBoolean,
                ResolveString,
                ResolveDoubleNullable,
                ResolveBooleanNullable,
                ResolveInt32,
                ResolveInt32Nullable,
                OnDetachedHeap,
                Callback.HandleCallback));
            // load method names to enable indexed based interop calling (vs string)
            InteropMethods = _refreshMethodMap();
            HeapSize = GetHeapSize();
            switch (GlobalScopeName)
            {
                case nameof(Window):
                    // in firefox browser extension running in content mode, a window and globalThis are not the same so they are loaded separately here to normalize usage
                    WindowThis = Get<Window>("window");
                    GlobalThis = Get<Window>("globalThis");
                    GlobalScope = GlobalScope.Window;
                    break;
                case nameof(DedicatedWorkerGlobalScope):
                    DedicateWorkerThis = Get<DedicatedWorkerGlobalScope>("globalThis");
                    GlobalThis = DedicateWorkerThis;
                    GlobalScope = GlobalScope.DedicatedWorker;
                    break;
                case nameof(SharedWorkerGlobalScope):
                    SharedWorkerThis = Get<SharedWorkerGlobalScope>("globalThis");
                    GlobalThis = SharedWorkerThis;
                    GlobalScope = GlobalScope.SharedWorker;
                    break;
                case nameof(ServiceWorkerGlobalScope):
                    ServiceWorkerThis = Get<ServiceWorkerGlobalScope>("globalThis");
                    GlobalThis = ServiceWorkerThis;
                    GlobalScope = GlobalScope.ServiceWorker;
                    break;
                default:
                    GlobalThis = Get<SpawnJSObject>("globalThis");
                    GlobalScope = GlobalScope.BrowserOther;
                    break;
            }
            AppBaseUri = SpawnJSInterop.Call<SpawnJSObjectReference, string>("appBaseUri", DotnetInstance) ?? "";
        }
        /// <summary>
        /// The last reported size of the .Net heap
        /// </summary>
        public long HeapSize { get; private set; } = 0;
        void OnDetachedHeap(long oldSize, long newSize)
        {
            HeapSize = newSize;
            OnHeapGrow?.Invoke(oldSize, newSize);
        }
        /// <summary>
        /// Fired on a heap detach event
        /// </summary>
        public event Action<long, long>? OnHeapGrow;
        /// <summary>
        /// Get the current heap size
        /// </summary>
        /// <returns></returns>
        public long GetHeapSize() => InteropCall<double, long>("getHeapSize", DotnetInstance.Id);
        /// <summary>
        /// Force the heap to grow. Useful for debugging heap growth issues.
        /// </summary>
        /// <returns></returns>
        public long GrowHeap()
        {
            var tmp = new List<byte[]>();
            var heapSize = GetHeapSize();
            long diff = 0;
            while (true)
            {
                try
                {
                    var sizeNow = GetHeapSize();
                    diff = sizeNow - heapSize;
                    if (diff > 0) break;
                    var data = new byte[5000000];
                    tmp.Add(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"GrowHeap eror: {ex.ToString()}");
                    break;
                }
            }
            tmp.Clear();
            GC.Collect(2, GCCollectionMode.Forced, blocking: true, compacting: true);
            return diff;
        }
        /// <summary>
        /// Get document
        /// </summary>
        /// <returns></returns>
        public Document? GetDocument() => Get<Document>("document");
        /// <summary>
        /// Returns value as type T
        /// </summary>
        /// <typeparam name="T">The type to return value as</typeparam>
        /// <typeparam name="T1">The value</typeparam>
        /// <returns>value as type T</returns>
        public T ReturnAs<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(T1 value) => InteropCall<T1, T>("returnMe", value);
        /// <summary>
        /// Returns value as type
        /// </summary>
        /// <param name="type">The type to return value as</param>
        /// <param name="value">The value</param>
        /// <returns>value as type T</returns>
        public object? ReturnAs(Type type, object? value) => ((Delegate)ReturnAs<object, object>).InvokeGeneric([value?.GetType() ?? typeof(object), type], value);
        /// <summary>
        /// Compares two values using Javascript equality.<br/>
        /// full == true uses strict equality (===), otherwise loose equality (==)
        /// </summary>
        public bool ObjectEquals<T1, T2>(T1 obj1, T2 obj2, bool full = false) => InteropCall<T1, T2, bool, bool>("objectEquals", obj1, obj2, full);
        /// <summary>
        /// Log to the Javascript console (console.log)
        /// </summary>
        /// <param name="args"></param>
        public void Log(params object?[] args)
        {
            if (IsBrowser) CallApplyVoid("console.log", args);
            else Console.WriteLine(string.Join(Environment.NewLine, args));
        }
        /// <summary>
        /// Log an error to the Javascript console (console.error)
        /// </summary>
        /// <param name="args"></param>
        public void LogError(params object?[] args)
        {
            if (IsBrowser) CallApplyVoid("console.error", args);
            else Console.Error.WriteLine(string.Join(Environment.NewLine, args));
        }
        /// <summary>
        /// Load the given script by adding a document head script element if the specified global var is not defined.
        /// </summary>
        public async Task LoadScript(string src, string? ifThisGlobalVarIsUndefined = null)
        {
            if (!string.IsNullOrEmpty(ifThisGlobalVarIsUndefined) && Has(ifThisGlobalVarIsUndefined)) return;
            await LoadScript(src);
        }
        /// <summary>
        /// Load the given script by adding a document head script element, if in a window scope.<br/>
        /// If in a WorkerGlobalScope, load the given script using importScripts.
        /// </summary>
        public async Task LoadScript(string src)
        {
            if (GlobalThis is WorkerGlobalScope workerGlobalScope)
            {
                workerGlobalScope.ImportScripts(src);
            }
            else if (WindowThis != null)
            {
                using var document = GetDocument();
                using var head = document!.Head;
                using var script = new HTMLScriptElement();
                script.Src = src;
                head!.Append(script);
                await script.OnLoadAsync();
            }
            else
            {
                throw new NotImplementedException("Unsupported global scope");
            }
        }
        /// <summary>
        /// Loads the specified scripts
        /// </summary>
        public async Task LoadScripts(string[] sources)
        {
            var tasks = new List<Task>();
            foreach (var src in sources) tasks.Add(LoadScript(src));
            await Task.WhenAll(tasks);
        }
        /// <summary>
        /// The import() syntax, commonly called dynamic import, is a function-like expression that allows loading an ECMAScript module asynchronously and dynamically into a potentially non-module environment.
        /// </summary>
        /// <param name="moduleName">The module to import from. The evaluation of the specifier is host-specified, but always follows the same algorithm as static import declarations.</param>
        /// <returns>Returns a promise which fulfills to a module namespace object: an object containing all exports from moduleName.</returns>
        public Task<ModuleNamespaceObject> Import(string moduleName) => SpawnJSInterop.CallAsync<string, ModuleNamespaceObject>("import", moduleName);
        /// <summary>
        /// Import a module and assign it to the specified global variable name<br/>
        /// Ex.:<br/>
        /// await JS.Import("name", "moduleName")<br/>
        /// Is roughly equivalent to:<br/>
        /// import * as name from "moduleName"<br/>
        /// </summary>
        /// <param name="name">Global variable name to assign the import to. Must be a valid JavaScript identifier.</param>
        /// <param name="moduleName">The module to import from. The evaluation of the specifier is host-specified, but always follows the same algorithm as static import declarations.</param>
        /// <returns></returns>
        public async Task<ModuleNamespaceObject> Import(string name, string moduleName)
        {
            ModuleNamespaceObject ret;
            if (!Has(name))
            {
                ret = (await Import(moduleName))!;
                Set(name, ret);
            }
            else
            {
                ret = Get<ModuleNamespaceObject>(name);
            }
            return ret;
        }
        /// <summary>
        /// The import() syntax, commonly called dynamic import, is a function-like expression that allows loading an ECMAScript module asynchronously and dynamically into a potentially non-module environment.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="moduleName">The module to import from. The evaluation of the specifier is host-specified, but always follows the same algorithm as static import declarations.</param>
        /// <returns></returns>
        public Task<T> Import<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string moduleName) => SpawnJSInterop.CallAsync<string, T>("import", moduleName);
        /// <summary>
        /// Import a module and assign it to the specified global variable name<br/>
        /// Ex.:<br/>
        /// await JS.Import("acorn", "https://www.acornlib.com/acorn.js")<br/>
        /// Is roughly equivalent to:<br/>
        /// import * as acorn from "https://www.acornlib.com/acorn.js"<br/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">Global variable name to assign the import to. Must be a valid JavaScript identifier.</param>
        /// <param name="moduleName">The module to import from. The evaluation of the specifier is host-specified, but always follows the same algorithm as static import declarations.</param>
        /// <returns></returns>
        public async Task<T> Import<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string name, string moduleName)
        {
            T ret;
            if (!Has(name))
            {
                ret = await Import<T>(moduleName);
                Set(name, ret);
            }
            else
            {
                ret = Get<T>(name);
            }
            return ret;
        }
        /// <summary>
        /// Calls fetch
        /// </summary>
        public Task<Response> Fetch(Request resource) => CallAsync<Request, Response>("fetch", resource);
        /// <summary>
        /// Calls fetch
        /// </summary>
        public Task<Response> Fetch(string resource) => CallAsync<string, Response>("fetch", resource);
        /// <summary>
        /// Calls fetch
        /// </summary>
        public Task<Response> Fetch(string resource, FetchOptions options) => CallAsync<string, FetchOptions, Response>("fetch", resource, options);

        public static bool EnableIDisposableWatcher { get; set; }
        public static void IDisposableFinalizerAlert<T>(T obj, string creationStackTrace) where T : IDisposable
        {
            // Log clearly to the browser console/standard error
            Console.Error.WriteLine(
                $"WARNING: {obj.GetType().FullName} was garbage collected but was never Disposed.\n" +
                $"Allocation stack trace:\n{creationStackTrace}\n"
            );
        }
    }
}
