using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSObjectReference
    {
        internal string PropertyTypeInfo(string key) => SpawnJSRuntime._propertyTypeInfo(Id, key);
        internal bool PropertyIn(string key) => SpawnJSRuntime._propertyIn(Id, key);
        internal bool PropertyDelete(string key) => SpawnJSRuntime._propertyDelete(Id, key);
        #region PropertySet
        internal void PropertySetNull(string key) => SpawnJSRuntime._propertySetNull(Id, key);
        internal void PropertySetUndefined(string key) => SpawnJSRuntime._propertySetUndefined(Id, key);
        internal void PropertySet(string key, string value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(string key, bool value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(string key, double value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(string key, int value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(string key, int? value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(string key, bool? value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(string key, double? value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(string key, SpawnJSObjectReference? value) => SpawnJSRuntime._propertySetSpawnJSObject(Id, key, value?.Id ?? NullId);
        internal void PropertySet(string key, Callback? value)
        {
            value?.Sent = true;
            SpawnJSRuntime._propertySetCallback(Id, key, JS.DotnetInstance.Id, value?.Id ?? 0, value?.Once ?? false);
        }
        [RequiresUnreferencedCode("Uses reflection-based System.Text.Json; the (de)serialized types and their members must be preserved under trimming. Use a JsonTypeInfo/JsonSerializerContext source generator, or preserve the types yourself.")]
        internal void PropertySetJson(string key, object? value, JsonSerializerOptions? serializerOptions = null)
            => SpawnJSRuntime._propertySetJson(Id, key, JsonSerializer.Serialize(value, serializerOptions));
        /// <summary>
        /// Writes PRE-SERIALIZED JSON text straight to the property - the JS side JSON.parse's it.
        /// Unlike <see cref="PropertySetJson(String, Object, JsonSerializerOptions)"/> this does NOT run the serializer, so a caller
        /// that already holds JSON text (e.g. JsonElement.GetRawText) does not encode it a second time
        /// and the call carries no reflection-based System.Text.Json dependency.
        /// </summary>
        internal void PropertySetRawJson(string key, string json)
            => SpawnJSRuntime._propertySetJson(Id, key, json);
        internal void PropertySetWithReviver(string reviver, string key, string value)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex);
        }
        internal void PropertySetWithReviver(string reviver, string key, string value, string reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        internal void PropertySetWithReviver(string reviver, string key, string value, double reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        internal void PropertySetWithReviver(string reviver, string key, string value, bool reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        internal void PropertySetWithReviver(string reviver, string key, double value)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex);
        }
        internal void PropertySetWithReviver(string reviver, string key, double value, string reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        internal void PropertySetWithReviver(string reviver, string key, double value, double reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        internal void PropertySetWithReviver(string reviver, string key, double value, bool reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        internal void PropertySetHeapView(string key, long offset, long length, bool copy = true)
            => PropertySetHeapView(key, offset, length, JSArrayBufferView.Uint8Array, copy);
        internal void PropertySetHeapView(string key, long offset, long length, JSArrayBufferView viewType, bool copy = true)
            => SpawnJSRuntime._propertySetHeapView(Id, key, JS.DotnetInstance.Id, (double)viewType, offset, length, copy);
        internal void PropertySet(string key, HeapViewDescriptor value)
            => SpawnJSRuntime._propertySetHeapView(Id, key, JS.DotnetInstance.Id, (double)value.Type, value.Offset, value.Length, value.Copy);
        #endregion
        #region PropertyGet
        internal string? PropertyGetWithReplacerString(string key, string replacer, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerString(Id, key, methodIndex, replacerConfig);
        }
        internal double PropertyGetWithReplacerDouble(string key, string replacer, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDouble(Id, key, methodIndex, replacerConfig);
        }
        internal bool PropertyGetWithReplacerBoolean(string key, string replacer, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBoolean(Id, key, methodIndex, replacerConfig);
        }
        internal double? PropertyGetWithReplacerDoubleNullable(string key, string replacer, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDoubleNullable(Id, key, methodIndex, replacerConfig);
        }
        internal bool? PropertyGetWithReplacerBooleanNullable(string key, string replacer, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBooleanNullable(Id, key, methodIndex, replacerConfig);
        }
        internal string? PropertyGetWithReplacerString(string key, string replacer, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerString(Id, key, methodIndex, replacerConfig);
        }
        internal double PropertyGetWithReplacerDouble(string key, string replacer, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDouble(Id, key, methodIndex, replacerConfig);
        }
        internal bool PropertyGetWithReplacerBoolean(string key, string replacer, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBoolean(Id, key, methodIndex, replacerConfig);
        }
        internal double? PropertyGetWithReplacerDoubleNullable(string key, string replacer, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDoubleNullable(Id, key, methodIndex, replacerConfig);
        }
        internal bool? PropertyGetWithReplacerBooleanNullable(string key, string replacer, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBooleanNullable(Id, key, methodIndex, replacerConfig);
        }
        internal string? PropertyGetWithReplacerString(string key, string replacer)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerString(Id, key, methodIndex);
        }
        internal double PropertyGetWithReplacerDouble(string key, string replacer)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDouble(Id, key, methodIndex);
        }
        internal bool PropertyGetWithReplacerBoolean(string key, string replacer)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBoolean(Id, key, methodIndex);
        }
        internal double? PropertyGetWithReplacerDoubleNullable(string key, string replacer)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDoubleNullable(Id, key, methodIndex);
        }
        internal bool? PropertyGetWithReplacerBooleanNullable(string key, string replacer)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBooleanNullable(Id, key, methodIndex);
        }
        internal string? PropertyGetString(string key) => SpawnJSRuntime._propertyGetString(Id, key);
        internal double PropertyGetDouble(string key) => SpawnJSRuntime._propertyGetDouble(Id, key);
        internal bool PropertyGetBoolean(string key) => SpawnJSRuntime._propertyGetBoolean(Id, key);
        internal double? PropertyGetDoubleNullable(string key) => SpawnJSRuntime._propertyGetDoubleNullable(Id, key);
        internal bool? PropertyGetBooleanNullable(string key) => SpawnJSRuntime._propertyGetBooleanNullable(Id, key);
        internal SpawnJSObjectReference? PropertyGetSpawnJSObjectReference(string key, bool force = false)
        {
            var id = SpawnJSRuntime._propertyGetSpawnJSObjectReference(Id, key, force);
            return FromID(id);
        }
        [RequiresUnreferencedCode("Uses reflection-based System.Text.Json; the (de)serialized types and their members must be preserved under trimming. Use a JsonTypeInfo/JsonSerializerContext source generator, or preserve the types yourself.")]
        internal T PropertyGetJson<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(string key, JsonSerializerOptions? options = null)
        {
            var json = SpawnJSRuntime._propertyGetJson(Id, key);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, options)!;
        }
        #endregion
    }
}
