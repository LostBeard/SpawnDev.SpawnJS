using SpawnDev.SpawnJS.Native;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using SpawnDev.SpawnJS.Extensions;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Adds extenion methods to JSObject
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform("browser")]
    public static partial class SpawnJSHandleExtensions
    {
        #region As
        /// <summary>
        /// Get as object
        /// </summary>
        public static object? AsObject(this SpawnJSHandle _this) => _this.ReadSelfAny();
        /// <summary>
        /// Get as nullable int
        /// </summary>
        public static int? AsInt32Nullable(this SpawnJSHandle _this) => _this.ReadSelfInt32Nullable();
        /// <summary>
        /// Get as nullable double
        /// </summary>
        public static double? AsDoubleNullable(this SpawnJSHandle _this) => _this.ReadSelfDoubleNullable();
        /// <summary>
        /// Get as nullable bool
        /// </summary>
        public static bool? AsBooleanNullable(this SpawnJSHandle _this) => _this.ReadSelfBooleanNullable();
        /// <summary>
        /// Get as bool
        /// </summary>
        public static bool AsBoolean(this SpawnJSHandle _this) => _this.ReadSelfBoolean();
        /// <summary>
        /// Get as int
        /// </summary>
        public static int AsInt32(this SpawnJSHandle _this) => _this.ReadSelfInt32();
        /// <summary>
        /// Get as double
        /// </summary>
        public static double AsDouble(this SpawnJSHandle _this) => _this.ReadSelfDouble();
        /// <summary>
        /// Get as string
        /// </summary>
        public static string? AsString(this SpawnJSHandle _this) => _this.ReadSelfString();
        /// <summary>
        /// Get as JSObject
        /// </summary>
        public static JSObject? AsJSObject(this SpawnJSHandle _this) => Reflect.GetJSObject(_this.JSParent, _this.JSKey);
        /// <summary>
        /// Get as JSObject
        /// </summary>
        /// <remarks>
        /// Takes the value into a handle of its own through the slot table when it can, so nothing becomes
        /// a JSObject proxy. This one is on the hot path for arrays and lists, which take a handle to the
        /// Javascript array before reading a single element out of it.
        /// </remarks>
        public static SpawnJSHandle? AsJSHandle(this SpawnJSHandle _this)
        {
            if (_this.TryTakeOwnedValue(out var owned)) return owned;
            return (SpawnJSHandle?)Reflect.GetJSObject(_this.JSParent, _this.JSKey)!;
        }
        /// <summary>
        /// Marshal the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? AsByteArray(this SpawnJSHandle _this) => _this.ReadSelfByteArray();
        /// <summary>
        /// Marshal the SpawnJSHandle and return as T using JSON seriliazation
        /// </summary>
        public static T AsJson<T>(this SpawnJSHandle _this)
        {
            return _this.JSParent.GetPropertyAsJson<T>(_this.JSKey);
        }
        /// <summary>
        /// Marshal the SpawnJSHandle and return as T using JSON seriliazation
        /// </summary>
        public static T AsJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions)
        {
            return _this.JSParent.GetPropertyAsJson<T>(_this.JSKey, jsonSerializerOptions);
        }
        #endregion
        static JSObject GlobalThis => JSHost.GlobalThis;
        /// <summary>
        /// Create a new empty JavaScript Object
        /// </summary>
        /// <returns></returns>
        static JSObject NewJSObject() => GlobalThis.InvokePropertyConstructor("Object")!;
        /// <summary>
        /// Create a new empty JavaScript Array
        /// </summary>
        /// <returns></returns>
        static JSObject NewJSArray() => GlobalThis.InvokePropertyConstructor("Array")!;
        /// <summary>
        /// Returns true if the proeprty exists
        /// </summary>
        public static bool HasProperty(this SpawnJSHandle _this, object identifier) => _this.HasPropertyValue(identifier);
        #region GetProperty
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static bool GetPropertyAsBoolean(this SpawnJSHandle _this, object identifier) => _this.GetPropertyBoolean(identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static int GetPropertyAsInt32(this SpawnJSHandle _this, object identifier) => _this.GetPropertyInt32(identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static double GetPropertyAsDouble(this SpawnJSHandle _this, object identifier) => _this.GetPropertyDouble(identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static string? GetPropertyAsString(this SpawnJSHandle _this, object identifier) => _this.GetPropertyString(identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static JSObject? GetPropertyAsJSObject(this SpawnJSHandle _this, object identifier) => Reflect.GetJSObject(_this.JSObjectRequired, identifier)!;
        /// <summary>
        /// Get the property value as object
        /// </summary>
        public static object? GetPropertyAsObject(this SpawnJSHandle _this, object identifier) => Reflect.GetObject(_this.JSObjectRequired, identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static SpawnJSHandle? GetPropertyAsJSHandle(this SpawnJSHandle _this, object identifier)
        {
            // slot first: reading a property into a handle needs no proxy for either object
            if (_this.CanReadBySlot) return _this.TryGetPropertyHandle(identifier);
            return (SpawnJSHandle)Reflect.GetJSObject(_this.JSObjectRequired, identifier)!;
        }
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static byte[]? GetPropertyAsByteArray(this SpawnJSHandle _this, object identifier) => _this.GetPropertyByteArray(identifier);
        /// <summary>
        /// Returns true if the proeprty exists
        /// </summary>
        public static bool HasProperty(this SpawnJSHandle _this,string identifier) => _this.HasPropertyValue(identifier);
        /// <summary>
        /// Returns strings like: '[object String]'<br/>
        /// USes: Object.prototype.toString.call
        /// </summary>
        public static string GetTypeOfProperty(this SpawnJSHandle _this, string identifier)
        {
            using var jsObject = _this.GetPropertyAsJSObject(identifier);
            return Reflect.TypeOf(jsObject!);
        }
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static bool GetPropertyAsBoolean(this SpawnJSHandle _this, string identifier) => _this.GetPropertyBoolean(identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static int GetPropertyAsInt32(this SpawnJSHandle _this, string identifier) => _this.GetPropertyInt32(identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static double GetPropertyAsDouble(this SpawnJSHandle _this, string identifier) => _this.GetPropertyDouble(identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static string? GetPropertyAsString(this SpawnJSHandle _this, string identifier) => _this.GetPropertyString(identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static JSObject? GetPropertyAsJSObject(this SpawnJSHandle _this, string identifier) => Reflect.GetJSObject(_this.JSObjectRequired, identifier)!;
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static SpawnJSHandle? GetPropertyAsJSHandle(this SpawnJSHandle _this, string identifier)
        {
            // slot first: reading a property into a handle needs no proxy for either object
            if (_this.CanReadBySlot) return _this.TryGetPropertyHandle(identifier);
            return (SpawnJSHandle)Reflect.GetJSObject(_this.JSObjectRequired, identifier)!;
        }
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static byte[]? GetPropertyAsByteArray(this SpawnJSHandle _this, string identifier) => _this.GetPropertyByteArray(identifier);
        /// <summary>
        /// Get the property value as T using JSON seriliazation
        /// </summary>
        public static T GetPropertyAsJson<T>(this SpawnJSHandle _this, string identifier)
        {
            using var jsObject = _this.GetPropertyAsJSObject(identifier);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json)!;
        }
        /// <summary>
        /// Get the property value as T using JSON seriliazation
        /// </summary>
        public static T GetPropertyAsJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, string identifier)
        {
            using var jsObject = _this.GetPropertyAsJSObject(identifier);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
        }
        /// <summary>
        /// Get the property value as object
        /// </summary>
        public static object? GetPropertyAsObject(this SpawnJSHandle _this, string identifier) => Reflect.GetObject(_this.JSObjectRequired, identifier);
        /// <summary>
        /// Get the property value as nullable int
        /// </summary>
        public static int? GetPropertyAsInt32Nullable(this SpawnJSHandle _this, string identifier) => Reflect.GetInt32Nullable(_this.JSObjectRequired, identifier);
        /// <summary>
        /// Get the property value as nullable double
        /// </summary>
        public static double? GetPropertyAsDoubleNullable(this SpawnJSHandle _this, string identifier) => Reflect.GetDoubleNullable(_this.JSObjectRequired, identifier);
        /// <summary>
        /// Get the property value as nullable bool
        /// </summary>
        public static bool? GetPropertyAsBooleanNullable(this SpawnJSHandle _this, string identifier) => Reflect.GetBooleanNullable(_this.JSObjectRequired, identifier);
        /// <summary>
        /// Get a nested property using a dot separated path (a trailing '?' on a segment short circuits to null when missing)
        /// </summary>
        public static SpawnJSHandle? Deep(this SpawnJSHandle _this, string identifier)
        {
            SpawnJSHandle? ret = _this;
            var parts = identifier.Split(".");
            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var canShortCircuit = false;
                if (part[part.Length - 1] == '?')
                {
                    part = part.Substring(0, part.Length - 1);
                    canShortCircuit = true;
                }
                var next = ret!.GetPropertyAsJSHandle(part);
                if (i > 0) ret.Dispose();
                ret = next;
                if (i == parts.Length - 1) return ret;
                if (ret == null)
                {
                    if (canShortCircuit) return null;
                    throw new Exception($"Property not found: {part}");
                }
            }
            return null;
        }
        /// <summary>
        /// Get the property value as T using JSON seriliazation
        /// </summary>
        public static T GetPropertyAsJson<T>(this SpawnJSHandle _this, int identifier)
        {
            using var jsObject = _this.GetPropertyAsJSObject(identifier);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json)!;
        }
        /// <summary>
        /// Get the property value as T using JSON seriliazation
        /// </summary>
        public static T GetPropertyAsJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions, int identifier)
        {
            using var jsObject = _this.GetPropertyAsJSObject(identifier);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
        }
        /// <summary>
        /// Get the property value as object
        /// </summary>
        public static object? GetPropertyAsObject(this SpawnJSHandle _this, int identifier) => Reflect.GetObject(_this.JSObjectRequired, identifier);
        /// <summary>
        /// Get the property value as nullable int
        /// </summary>
        public static int? GetPropertyAsInt32Nullable(this SpawnJSHandle _this, int identifier) => Reflect.GetInt32Nullable(_this.JSObjectRequired, identifier);
        /// <summary>
        /// Get the property value as nullable double
        /// </summary>
        public static double? GetPropertyAsDoubleNullable(this SpawnJSHandle _this, int identifier) => Reflect.GetDoubleNullable(_this.JSObjectRequired, identifier);
        /// <summary>
        /// Get the property value as nullable bool
        /// </summary>
        public static bool? GetPropertyAsBooleanNullable(this SpawnJSHandle _this, int identifier) => Reflect.GetBooleanNullable(_this.JSObjectRequired, identifier);
        /// <summary>
        /// Get the property value as bool
        /// </summary>
        public static bool GetPropertyAsBoolean(this SpawnJSHandle _this, int identifier) => _this.GetPropertyBoolean(identifier);
        /// <summary>
        /// Get the property value as int
        /// </summary>
        public static int GetPropertyAsInt32(this SpawnJSHandle _this, int identifier) => _this.GetPropertyInt32(identifier);
        /// <summary>
        /// Get the property value as double
        /// </summary>
        public static double GetPropertyAsDouble(this SpawnJSHandle _this, int identifier) => _this.GetPropertyDouble(identifier);
        /// <summary>
        /// Get the property value as string
        /// </summary>
        public static string? GetPropertyAsString(this SpawnJSHandle _this, int identifier) => _this.GetPropertyString(identifier);
        /// <summary>
        /// Get the property value as JSObject
        /// </summary>
        public static JSObject? GetPropertyAsJSObject(this SpawnJSHandle _this, int identifier) => Reflect.GetJSObject(_this.JSObjectRequired, identifier);
        /// <summary>
        /// Get the property value as JSObject
        /// </summary>
        public static SpawnJSHandle? GetPropertyAsJSHandle(this SpawnJSHandle _this, int identifier)
        {
            // slot first: reading a property into a handle needs no proxy for either object
            if (_this.CanReadBySlot) return _this.TryGetPropertyHandle(identifier);
            return (SpawnJSHandle)Reflect.GetJSObject(_this.JSObjectRequired, identifier)!;
        }
        /// <summary>
        /// Get the property value as byte[]
        /// </summary>
        public static byte[]? GetPropertyAsByteArray(this SpawnJSHandle _this, int identifier) => _this.GetPropertyByteArray(identifier);
        #endregion
        #region DeleteProperty
        /// <summary>
        /// Delete the property
        /// </summary>
        public static bool DeleteProperty(this SpawnJSHandle _this, string identifier) => Reflect.DeletePropertyVoid(_this.JSObjectRequired, identifier);
        /// <summary>
        /// Delete the property
        /// </summary>
        public static bool DeleteProperty(this SpawnJSHandle _this, long identifier) => Reflect.DeletePropertyVoid(_this.JSObjectRequired, identifier);
        #endregion
        #region SetProperty
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, bool value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, int value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, double value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, string? value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, JSObject? value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, SpawnJSHandle? value) => Reflect.Set(_this.JSObjectRequired, identifier, value?.JSObjectRequired);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, byte[]? value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value using JSON seriliazation
        /// </summary>
        public static void SetPropertyJson(this SpawnJSHandle _this, string identifier, object? value, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var json = jsonSerializerOptions == null ? JsonSerializer.Serialize(value) : JsonSerializer.Serialize(value, jsonSerializerOptions);
            using var jsObject = JSON.Parse(json);
            _this.SetProperty(identifier, jsObject);
        }
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertyObject(this SpawnJSHandle _this, string identifier, object? value) => Reflect.SetObject(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertyArraySegmentByte(this SpawnJSHandle _this, string identifier, ArraySegment<byte> value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertySpanByte(this SpawnJSHandle _this, string identifier, Span<byte> value) => Reflect.Set(_this.JSObjectRequired, identifier, value);

        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, bool value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, int value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, double value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, string? value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, JSObject? value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, SpawnJSHandle? value) => Reflect.Set(_this.JSObjectRequired, identifier, value?.JSObjectRequired);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, byte[]? value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value using JSON seriliazation
        /// </summary>
        public static void SetPropertyJson(this SpawnJSHandle _this, int identifier, object? value, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var json = jsonSerializerOptions == null ? JsonSerializer.Serialize(value) : JsonSerializer.Serialize(value, jsonSerializerOptions);
            using var jsObject = JSON.Parse(json);
            _this.SetProperty(identifier, jsObject);
        }
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertyObject(this SpawnJSHandle _this, int identifier, object? value) => Reflect.SetObject(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertyArraySegmentByte(this SpawnJSHandle _this, int identifier, ArraySegment<byte> value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertySpanByte(this SpawnJSHandle _this, int identifier, Span<byte> value) => Reflect.Set(_this.JSObjectRequired, identifier, value);

        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, bool value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, int value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, double value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, string? value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, JSObject? value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, SpawnJSHandle? value) => Reflect.Set(_this.JSObjectRequired, identifier, value?.JSObjectRequired);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, byte[]? value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value using JSON seriliazation
        /// </summary>
        public static void SetPropertyJson(this SpawnJSHandle _this, object identifier, object? value, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var json = jsonSerializerOptions == null ? JsonSerializer.Serialize(value) : JsonSerializer.Serialize(value, jsonSerializerOptions);
            using var jsObject = JSON.Parse(json);
            _this.SetProperty(identifier, jsObject);
        }
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertyObject(this SpawnJSHandle _this, object identifier, object? value) => Reflect.SetObject(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertyArraySegmentByte(this SpawnJSHandle _this, object identifier, ArraySegment<byte> value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertySpanByte(this SpawnJSHandle _this, object identifier, Span<byte> value) => Reflect.Set(_this.JSObjectRequired, identifier, value);
        #endregion
    }
}
