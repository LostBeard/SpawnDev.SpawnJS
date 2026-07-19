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
        public static bool HasProperty(this SpawnJSHandle _this, object identifier) => Reflect.Has(_this.JSObject, identifier);
        /// <summary>
        /// Returns strings like: '[object String]'<br/>
        /// USes: Object.prototype.toString.call
        /// </summary>
        public static string GetTypeOfProperty(this SpawnJSHandle _this, object identifier)
        {
            using var jsObject = _this.GetPropertyAsJSObject(identifier);
            return Reflect.TypeOf(jsObject!);
        }
        #region GetProperty
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static bool GetPropertyAsBoolean(this SpawnJSHandle _this, object identifier) => Reflect.GetBoolean(_this.JSObject, identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static int GetPropertyAsInt32(this SpawnJSHandle _this, object identifier) => Reflect.GetInt32(_this.JSObject, identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static double GetPropertyAsDouble(this SpawnJSHandle _this, object identifier) => Reflect.GetDouble(_this.JSObject, identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static string? GetPropertyAsString(this SpawnJSHandle _this, object identifier) => Reflect.GetString(_this.JSObject, identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static JSObject? GetPropertyAsJSObject(this SpawnJSHandle _this, object identifier) => Reflect.GetJSObject(_this.JSObject, identifier)!;
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static SpawnJSHandle? GetPropertyAsJSHandle(this SpawnJSHandle _this, object identifier) => (SpawnJSHandle)Reflect.GetJSObject(_this.JSObject, identifier)!;
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static byte[]? GetPropertyAsByteArray(this SpawnJSHandle _this, object identifier) => Reflect.GetByteArray(_this.JSObject, identifier);
        /// <summary>
        /// Returns true if the proeprty exists
        /// </summary>
        public static bool HasProperty(this SpawnJSHandle _this,string identifier) => Reflect.Has(_this.JSObject, identifier);
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
        public static bool GetPropertyAsBoolean(this SpawnJSHandle _this, string identifier) => Reflect.GetBoolean(_this.JSObject, identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static int GetPropertyAsInt32(this SpawnJSHandle _this, string identifier) => Reflect.GetInt32(_this.JSObject, identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static double GetPropertyAsDouble(this SpawnJSHandle _this, string identifier) => Reflect.GetDouble(_this.JSObject, identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static string? GetPropertyAsString(this SpawnJSHandle _this, string identifier) => Reflect.GetString(_this.JSObject, identifier);
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static JSObject? GetPropertyAsJSObject(this SpawnJSHandle _this, string identifier) => Reflect.GetJSObject(_this.JSObject, identifier)!;
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static SpawnJSHandle? GetPropertyAsJSHandle(this SpawnJSHandle _this, string identifier) => (SpawnJSHandle)Reflect.GetJSObject(_this.JSObject, identifier)!;
        /// <summary>
        /// Returns the property value
        /// </summary>
        public static byte[]? GetPropertyAsByteArray(this SpawnJSHandle _this, string identifier) => Reflect.GetByteArray(_this.JSObject, identifier);
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
        public static object? GetPropertyAsObject(this SpawnJSHandle _this, string identifier) => Reflect.GetObject(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as nullable int
        /// </summary>
        public static int? GetPropertyAsInt32Nullable(this SpawnJSHandle _this, string identifier) => Reflect.GetInt32Nullable(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as nullable double
        /// </summary>
        public static double? GetPropertyAsDoubleNullable(this SpawnJSHandle _this, string identifier) => Reflect.GetDoubleNullable(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as nullable bool
        /// </summary>
        public static bool? GetPropertyAsBooleanNullable(this SpawnJSHandle _this, string identifier) => Reflect.GetBooleanNullable(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as object
        /// </summary>
        public static object? GetPropertyAsObject(this SpawnJSHandle _this, int identifier) => Reflect.GetObject(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as nullable int
        /// </summary>
        public static int? GetPropertyAsInt32Nullable(this SpawnJSHandle _this, int identifier) => Reflect.GetInt32Nullable(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as nullable double
        /// </summary>
        public static double? GetPropertyAsDoubleNullable(this SpawnJSHandle _this, int identifier) => Reflect.GetDoubleNullable(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as nullable bool
        /// </summary>
        public static bool? GetPropertyAsBooleanNullable(this SpawnJSHandle _this, int identifier) => Reflect.GetBooleanNullable(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as bool
        /// </summary>
        public static bool GetPropertyAsBoolean(this SpawnJSHandle _this, int identifier) => Reflect.GetBoolean(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as int
        /// </summary>
        public static int GetPropertyAsInt32(this SpawnJSHandle _this, int identifier) => Reflect.GetInt32(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as double
        /// </summary>
        public static double GetPropertyAsDouble(this SpawnJSHandle _this, int identifier) => Reflect.GetDouble(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as string
        /// </summary>
        public static string? GetPropertyAsString(this SpawnJSHandle _this, int identifier) => Reflect.GetString(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as JSObject
        /// </summary>
        public static JSObject? GetPropertyAsJSObject(this SpawnJSHandle _this, int identifier) => Reflect.GetJSObject(_this.JSObject, identifier);
        /// <summary>
        /// Get the property value as JSObject
        /// </summary>
        public static SpawnJSHandle? GetPropertyAsJSHandle(this SpawnJSHandle _this, int identifier) => (SpawnJSHandle)Reflect.GetJSObject(_this.JSObject, identifier)!;
        /// <summary>
        /// Get the property value as byte[]
        /// </summary>
        public static byte[]? GetPropertyAsByteArray(this SpawnJSHandle _this, int identifier) => Reflect.GetByteArray(_this.JSObject, identifier);
        #endregion
        #region DeleteProperty
        /// <summary>
        /// Delete the property
        /// </summary>
        public static bool DeleteProperty(this SpawnJSHandle _this, string identifier) => Reflect.DeletePropertyVoid(_this.JSObject, identifier);
        /// <summary>
        /// Delete the property
        /// </summary>
        public static bool DeleteProperty(this SpawnJSHandle _this, long identifier) => Reflect.DeletePropertyVoid(_this.JSObject, identifier);
        #endregion
        #region As
        /// <summary>
        /// Marshal the SpawnJSHandle and return as byte[]
        /// </summary>
        public static byte[]? AsByteArray(this SpawnJSHandle _this)
        {
            using var array = NewJSArray();
            array.SetProperty(0, _this.JSObject);
            var ret = array.GetPropertyAsByteArray(0);
            return ret;
        }
        /// <summary>
        /// Marshal the SpawnJSHandle and return as T using JSON seriliazation
        /// </summary>
        public static T AsJson<T>(this SpawnJSHandle _this)
        {
            var json = JSON.Stringify(_this.JSObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json)!;
        }
        /// <summary>
        /// Marshal the SpawnJSHandle and return as T using JSON seriliazation
        /// </summary>
        public static T AsJson<T>(this SpawnJSHandle _this, JsonSerializerOptions jsonSerializerOptions)
        {
            var json = JSON.Stringify(_this.JSObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
        }
        #endregion
        #region SetProperty
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, bool value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, int value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, double value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, string? value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, JSObject? value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, SpawnJSHandle? value) => Reflect.Set(_this.JSObject, identifier, value?.JSObject);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, string identifier, byte[]? value) => Reflect.Set(_this.JSObject, identifier, value);
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
        public static void SetPropertyObject(this SpawnJSHandle _this, string identifier, object? value) => Reflect.SetObject(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertyArraySegmentByte(this SpawnJSHandle _this, string identifier, ArraySegment<byte> value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertySpanByte(this SpawnJSHandle _this, string identifier, Span<byte> value) => Reflect.Set(_this.JSObject, identifier, value);

        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, bool value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, int value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, double value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, string? value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, JSObject? value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, SpawnJSHandle? value) => Reflect.Set(_this.JSObject, identifier, value?.JSObject);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, int identifier, byte[]? value) => Reflect.Set(_this.JSObject, identifier, value);
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
        public static void SetPropertyObject(this SpawnJSHandle _this, int identifier, object? value) => Reflect.SetObject(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertyArraySegmentByte(this SpawnJSHandle _this, int identifier, ArraySegment<byte> value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertySpanByte(this SpawnJSHandle _this, int identifier, Span<byte> value) => Reflect.Set(_this.JSObject, identifier, value);

        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, bool value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, int value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, double value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, string? value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, JSObject? value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, SpawnJSHandle? value) => Reflect.Set(_this.JSObject, identifier, value?.JSObject);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetProperty(this SpawnJSHandle _this, object identifier, byte[]? value) => Reflect.Set(_this.JSObject, identifier, value);
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
        public static void SetPropertyObject(this SpawnJSHandle _this, object identifier, object? value) => Reflect.SetObject(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertyArraySegmentByte(this SpawnJSHandle _this, object identifier, ArraySegment<byte> value) => Reflect.Set(_this.JSObject, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        public static void SetPropertySpanByte(this SpawnJSHandle _this, object identifier, Span<byte> value) => Reflect.Set(_this.JSObject, identifier, value);
        #endregion
    }
}
