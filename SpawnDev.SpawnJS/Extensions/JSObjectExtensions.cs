using SpawnDev.SpawnJS.Native;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;

namespace SpawnDev.SpawnJS.Extensions
{
    /// <summary>
    /// Adds extenion methods to JSObject
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform("browser")]
    public static partial class JSObjectExtensions
    {
        static JSObject GlobalThis => JSHost.GlobalThis;
        #region GetProperty
        /// <summary>
        /// Get a nested property using a dot separated path (a trailing '?' on a segment short circuits to null when missing)
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static JSObject? Deep(this JSObject _this, string identifier)
        {
            JSObject? ret = _this;
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
                var next = ret!.GetPropertyAsJSObject(part);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static T GetPropertyAsJson<T>(this JSObject _this, int identifier)
        {
            using var jsObject = _this.GetPropertyAsJSObject(identifier);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json)!;
        }
        /// <summary>
        /// Get the property value as T using JSON seriliazation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static T GetPropertyAsJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, int identifier)
        {
            using var jsObject = _this.GetPropertyAsJSObject(identifier);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
        }
        /// <summary>
        /// Get the property value as T using JSON seriliazation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static T GetPropertyAsJson<T>(this JSObject _this, string identifier)
        {
            using var jsObject = _this.GetPropertyAsJSObject(identifier);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json)!;
        }
        /// <summary>
        /// Get the property value as T using JSON seriliazation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static T GetPropertyAsJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions, string identifier)
        {
            using var jsObject = _this.GetPropertyAsJSObject(identifier);
            var json = jsObject == null ? null : JSON.Stringify(jsObject);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
        }
        /// <summary>
        /// Get the property value as object
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static object? GetPropertyAsObject(this JSObject _this, string identifier) => Reflect.GetObject(_this, identifier);
        /// <summary>
        /// Get the property value as nullable int
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static int? GetPropertyAsInt32Nullable(this JSObject _this, string identifier) => Reflect.GetInt32Nullable(_this, identifier);
        /// <summary>
        /// Get the property value as nullable double
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static double? GetPropertyAsDoubleNullable(this JSObject _this, string identifier) => Reflect.GetDoubleNullable(_this, identifier);
        /// <summary>
        /// Get the property value as nullable bool
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static bool? GetPropertyAsBooleanNullable(this JSObject _this, string identifier) => Reflect.GetBooleanNullable(_this, identifier);

        /// <summary>
        /// Get the property value as object
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static object? GetPropertyAsObject(this JSObject _this, int identifier) => Reflect.GetObject(_this, identifier);
        /// <summary>
        /// Get the property value as nullable int
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static int? GetPropertyAsInt32Nullable(this JSObject _this, int identifier) => Reflect.GetInt32Nullable(_this, identifier);
        /// <summary>
        /// Get the property value as nullable double
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static double? GetPropertyAsDoubleNullable(this JSObject _this, int identifier) => Reflect.GetDoubleNullable(_this, identifier);
        /// <summary>
        /// Get the property value as nullable bool
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static bool? GetPropertyAsBooleanNullable(this JSObject _this, int identifier) => Reflect.GetBooleanNullable(_this, identifier);

        /// <summary>
        /// Get the property value as bool
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static bool GetPropertyAsBoolean(this JSObject _this, int identifier) => Reflect.GetBoolean(_this, identifier);
        /// <summary>
        /// Get the property value as int
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static int GetPropertyAsInt32(this JSObject _this, int identifier) => Reflect.GetInt32(_this, identifier);
        /// <summary>
        /// Get the property value as double
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static double GetPropertyAsDouble(this JSObject _this, int identifier) => Reflect.GetDouble(_this, identifier);
        /// <summary>
        /// Get the property value as string
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static string? GetPropertyAsString(this JSObject _this, int identifier) => Reflect.GetString(_this, identifier);
        /// <summary>
        /// Get the property value as JSObject
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static JSObject? GetPropertyAsJSObject(this JSObject _this, int identifier) => Reflect.GetJSObject(_this, identifier);
        /// <summary>
        /// Get the property value as byte[]
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static byte[]? GetPropertyAsByteArray(this JSObject _this, int identifier) => Reflect.GetByteArray(_this, identifier);
        #endregion
        #region DeleteProperty
        /// <summary>
        /// Delete the property
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static bool DeleteProperty(this JSObject _this, string identifier) => Reflect.DeletePropertyVoid(_this, identifier);
        /// <summary>
        /// Delete the property
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static bool DeleteProperty(this JSObject _this, long identifier) => Reflect.DeletePropertyVoid(_this, identifier);
        #endregion
        #region As
        
        /// <summary>
        /// Create a new empty JavaScript Object
        /// </summary>
        /// <returns></returns>
        public static JSObject NewJSObject() => GlobalThis.InvokePropertyConstructor("Object")!;
        /// <summary>
        /// Create a new empty JavaScript Array
        /// </summary>
        /// <returns></returns>
        public static JSObject NewJSArray() => GlobalThis.InvokePropertyConstructor("Array")!;
        /// <summary>
        /// Marshal the JSObject and return as byte[]
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static byte[]? AsByteArray(this JSObject _this)
        {
            using var array = NewJSArray();
            array.SetProperty(0, _this);
            var ret = array.GetPropertyAsByteArray(0);
            return ret;
        }
        /// <summary>
        /// Marshal the JSObject and return as string
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string? AsString(this JSObject _this)
        {
            using var array = NewJSArray();
            array.SetProperty(0, _this);
            var ret = array.GetPropertyAsString(0);
            return ret;
        }
        /// <summary>
        /// Marshal the JSObject and return as int
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static int AsInt32(this JSObject _this)
        {
            using var array = NewJSArray();
            array.SetProperty(0, _this);
            var ret = GlobalThis.GetPropertyAsInt32(0);
            return ret;
        }
        /// <summary>
        /// Marshal the JSObject and return as double
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static double AsDouble(this JSObject _this)
        {
            using var array = NewJSArray();
            array.SetProperty(0, _this);
            var ret = GlobalThis.GetPropertyAsDouble(0);
            return ret;
        }
        /// <summary>
        /// Marshal the JSObject and return as bool
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool AsBoolean(this JSObject _this)
        {
            using var array = NewJSArray();
            array.SetProperty(0, _this);
            var ret = GlobalThis.GetPropertyAsBoolean(0);
            return ret;
        }
        /// <summary>
        /// Marshal the JSObject and return as nullable int
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static int? AsInt32Nullable(this JSObject _this)
        {
            using var array = NewJSArray();
            array.SetProperty(0, _this);
            var ret = GlobalThis.GetPropertyAsInt32Nullable(0);
            return ret;
        }
        /// <summary>
        /// Marshal the JSObject and return as nullable double
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static double? AsDoubleNullable(this JSObject _this)
        {
            using var array = NewJSArray();
            array.SetProperty(0, _this);
            var ret = GlobalThis.GetPropertyAsDoubleNullable(0);
            return ret;
        }
        /// <summary>
        /// Marshal the JSObject and return as nullable bool
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool? AsBooleanNullable(this JSObject _this)
        {
            using var array = NewJSArray();
            array.SetProperty(0, _this);
            var ret = GlobalThis.GetPropertyAsBooleanNullable(0);
            return ret;
        }
        /// <summary>
        /// Marshal the JSObject and return as T using JSON seriliazation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static T AsJson<T>(this JSObject _this)
        {
            var json = JSON.Stringify(_this);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json)!;
        }
        /// <summary>
        /// Marshal the JSObject and return as T using JSON seriliazation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        public static T AsJson<T>(this JSObject _this, JsonSerializerOptions jsonSerializerOptions)
        {
            var json = JSON.Stringify(_this);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
        }
        #endregion
        #region SetProperty
        /// <summary>
        /// Set the property to the specified value using JSON seriliazation
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        /// <param name="jsonSerializerOptions"></param>
        public static void SetPropertyJson(this JSObject _this, string identifier, object? value, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var json = jsonSerializerOptions == null ? JsonSerializer.Serialize(value) : JsonSerializer.Serialize(value, jsonSerializerOptions);
            using var jsObject = JSON.Parse(json);
            _this.SetProperty(identifier, jsObject);
        }
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public static void SetPropertyObject(this JSObject _this, string identifier, object? value)
        {
            Reflect.SetObject(_this, identifier, value);
        }
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public static void SetPropertyArraySegmentByte(this JSObject _this, string identifier, ArraySegment<byte> value)
        {
            Reflect.SetArraySegmentByte(_this, identifier, value);
        }
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public static void SetPropertySpanByte(this JSObject _this, string identifier, Span<byte> value)
        {
            Reflect.SetSpanByte(_this, identifier, value);
        }

        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public static void SetProperty(this JSObject _this, int identifier, bool value) => Reflect.Set(_this, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public static void SetProperty(this JSObject _this, int identifier, int value) => Reflect.Set(_this, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public static void SetProperty(this JSObject _this, int identifier, double value) => Reflect.Set(_this, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public static void SetProperty(this JSObject _this, int identifier, string? value) => Reflect.Set(_this, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public static void SetProperty(this JSObject _this, int identifier, JSObject? value) => Reflect.SetJSObject(_this, identifier, value);
        /// <summary>
        /// Set the property to the specified value
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public static void SetProperty(this JSObject _this, int identifier, byte[]? value) => Reflect.Set(_this, identifier, value);
        #endregion
    }
}
