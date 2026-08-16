using SpawnDev.SpawnJS.Marshaller;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// A handle to a Javascript value, addressed by a numeric id rather than a Microsoft <c>JSObject</c>.
    /// The JS side keeps the actual value in its object table (see SpawnJSInterop.spawnJSObjects) keyed by
    /// this <see cref="Id"/>; disposing this handle releases that entry so JS can garbage-collect it.<br/>
    /// <br/>
    /// Negative ids are reserved sentinels (globalThis / undefined / null / the object table itself) that
    /// have no table entry and are never released. Positive ids are real held values. This id-only model is
    /// what lets the whole library avoid <c>JSObject</c> and its disposal quirk.
    /// </summary>
    public partial class SpawnJSObjectReference : IDisposable
    {
        /// <summary>
        /// Creates a SpawnJSObjectReference from its Id.
        /// </summary>
        /// <param name="fromJS"></param>
        /// <param name="nonNullable"></param>
        /// <param name="preventDispose"></param>
        /// <returns></returns>
        public static SpawnJSObjectReference? FromID(double fromJS, bool nonNullable = false, bool preventDispose = false)
        {
            var isValid = fromJS != NullId && fromJS != UndefinedId && fromJS != double.NaN && fromJS != 0;
            return !nonNullable && !isValid ? null : new SpawnJSObjectReference(fromJS) { PreventDispose = preventDispose };
        }
        /// <summary>
        /// If true, this item will not dispose when dispsoe is called
        /// </summary>
        public bool PreventDispose { get; set; }
        /// <summary>Sentinel id for JS <c>globalThis</c>.</summary>
        public const double GlobalThisId = -1;
        /// <summary>Sentinel id for JS <c>undefined</c> (also the id a handle is set to once released).</summary>
        public const double UndefinedId = -2;
        /// <summary>Sentinel id for JS <c>null</c>.</summary>
        public const double NullId = -3;
        /// <summary>Sentinel id for the JS object table itself.</summary>
        public const double SpawnJSObjectsId = -4;
        /// <summary>Sentinel id for SpawnJSInterop.</summary>
        public const double SpawnJSInteropId = -5;
        /// <summary>True once this handle has been disposed (its JS table entry released).</summary>
        public bool IsDisposed { get; private set; }
        /// <summary>Shortcut to the runtime singleton.</summary>
        static SpawnJSRuntime JS => SpawnJSRuntime.Instance;
        /// <summary>The JS object table id this handle references. Set to <see cref="UndefinedId"/> once released.</summary>
        public double Id { get; private set; }
        /// <summary>True if this handle references JS <c>undefined</c> (or has been released).</summary>
        public bool IsUndefined => Id == UndefinedId;
        /// <summary>True if this handle references JS <c>null</c>.</summary>
        public bool IsNull => Id == NullId;
        /// <summary>True if this handle references JS <c>globalThis</c>.</summary>
        public bool IsGlobalThis => Id == GlobalThisId;
        /// <summary>
        /// Constructor.name
        /// </summary>
        public string ConstructorName()
        {
            if (_typeOf == null) GetTypeInfo();
            return _constructorName!;
        }
        /// <summary>
        /// Constructor.name
        /// </summary>
        public string TypeOf()
        {
            if (_typeOf == null) GetTypeInfo();
            return _typeOf!;
        }
        private void GetTypeInfo()
        {
            try
            {
                var tmp = SpawnJSRuntime._getTypeInfo(Id) ?? "";
                var parts = tmp.Split(" ");
                _typeOf = parts[0];
                _constructorName = parts.Length > 1 ? parts[1] : "";
            }
            catch { }
            if (string.IsNullOrEmpty(_typeOf)) _typeOf = "undefined";
            if (string.IsNullOrEmpty(_constructorName)) _constructorName = "";
        }
        string? _constructorName = null;
        string? _typeOf = null;
        /// <summary>
        /// Get the (typeof this, this?.consturctor?.name)
        /// </summary>
        /// <returns></returns>
        public (string TypeOf, string ConstructorName) TypeInfo() => (TypeOf(), ConstructorName());

        /// <summary>The object's own enumerable property names (Object.keys).</summary>
        public List<string> Keys(bool hasOwnProperty = false)
            => JS.SpawnJSInterop.Call<SpawnJSObjectReference, bool, List<string>>("objectKeys", this, hasOwnProperty);

        /// <summary>Awaits a promise-valued property, discarding the result.</summary>
        public Task GetVoidAsync(string key) => GetAsync<VoidType>(key);

        /// <summary>The constructor names down the prototype chain (most-derived first).</summary>
        public List<string> ConstructorNames()
        {
            if (_constructorNames != null) return _constructorNames;
            try
            {
                _constructorNames ??= JS.SpawnJSInterop.Call<SpawnJSObjectReference, List<string>>("getConstructorNames", this);
            }
            catch { }
            _constructorNames ??= new List<string>();
            return _constructorNames;
        }
        List<string>? _constructorNames = null;
        /// <summary>Wraps an existing JS object table id.</summary>
        public SpawnJSObjectReference(double sjsId)
        {
            Id = sjsId;
        }
        /// <summary>
        /// Returns the referenced Javascript value as type T
        /// </summary>
        public T As<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            var ret = JS.ReturnAs<SpawnJSObjectReference, T>(this);
            return ret;
        }
        /// <summary>
        /// Returns the referenced Javascript value as type T
        /// </summary>
        public object? As(Type type) => ((Delegate)As<object>).InvokeGeneric(type);
        #region ReleaseAs
        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public double? ReleaseAsDoubleNullable()
        {
            var ret = SpawnJSRuntime.SpawnJSObjectReleaseDoubleNullable(Id);
            Id = UndefinedId;
            Dispose();
            return ret;
        }
        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public bool? ReleaseAsBooleanNullable()
        {
            var ret = SpawnJSRuntime.SpawnJSObjectReleaseBooleanNullable(Id);
            Id = UndefinedId;
            Dispose();
            return ret;
        }
        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public int ReleaseAsInt32()
        {
            var ret = SpawnJSRuntime.SpawnJSObjectReleaseInt32(Id);
            Id = UndefinedId;
            Dispose();
            return ret;
        }
        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public int? ReleaseAsInt32Nullable()
        {
            var ret = SpawnJSRuntime.SpawnJSObjectReleaseInt32Nullable(Id);
            Id = UndefinedId;
            Dispose();
            return ret;
        }
        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public double ReleaseAsDouble()
        {
            var ret = SpawnJSRuntime.SpawnJSObjectReleaseDouble(Id);
            Id = UndefinedId;
            Dispose();
            return ret;
        }
        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public bool ReleaseAsBoolean()
        {
            var ret = SpawnJSRuntime.SpawnJSObjectReleaseBoolean(Id);
            Id = UndefinedId;
            Dispose();
            return ret;
        }
        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public string ReleaseAsString()
        {
            var ret = SpawnJSRuntime.SpawnJSObjectReleaseString(Id);
            Id = UndefinedId;
            Dispose();
            return ret;
        }
        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        [RequiresUnreferencedCode("Uses reflection-based System.Text.Json; the (de)serialized types and their members must be preserved under trimming. Use a JsonTypeInfo/JsonSerializerContext source generator, or preserve the types yourself.")]
        public T ReleaseAsJson<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(JsonSerializerOptions? serializerOptions = null)
        {
            var json = SpawnJSRuntime.SpawnJSObjectReleaseJson(Id);
            var ret = json == null ? default : JsonSerializer.Deserialize<T>(json, serializerOptions);
            Id = UndefinedId;
            Dispose();
            return ret!;
        }
        #endregion
        /// <summary>
        /// Releases the JS object table entry so the underlying value can be garbage-collected on the JS
        /// side. Only positive ids reference a real table entry; the negative sentinels (globalThis, null,
        /// etc.) have nothing to release.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed || PreventDispose) return;
            IsDisposed = true;
            var id = Id;
            Id = UndefinedId;
            if (id > 0)
            {
                SpawnJSRuntime.SpawnJSObjectRelease(id);
            }
        }
        /// <summary>
        /// Move the SpawnJSObjectReference Id out of this SpawnJSObjectReference and take responsibility for disposing it.
        /// </summary>
        /// <returns></returns>
        public double MoveId()
        {
            var id = Id;
            Id = UndefinedId;
            return id;
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsDisposed) return;
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose resources if not already disposed to prevent memory leaks
        /// </summary>
        ~SpawnJSObjectReference()
        {
            Dispose(false);
        }
    }
}
