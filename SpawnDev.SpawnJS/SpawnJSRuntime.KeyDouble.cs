using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSRuntime
    {
        [JSImport("globalThis.SpawnJSInterop.propertyTypeInfo")]
        internal static partial string _propertyTypeInfo(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyDelete")]
        internal static partial bool _propertyDelete(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyIn")]
        internal static partial bool _propertyIn(double sjsId, double key);



        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial string _propertyGetString(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial double _propertyGetDouble(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial double? _propertyGetDoubleNullable(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial bool _propertyGetBoolean(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial bool? _propertyGetBooleanNullable(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial int _propertyGetInt32(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial int? _propertyGetInt32Nullable(double sjsId, double key);


        [JSImport("globalThis.SpawnJSInterop.propertyGetSpawnJSObjectReference")]
        internal static partial double _propertyGetSpawnJSObjectReference(double sjsId, double key, bool force);

        [JSImport("globalThis.SpawnJSInterop.propertyGetJson")]
        internal static partial string? _propertyGetJson(double sjsId, double key);


        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial string _propertyGetWithReplacerString(double sjsId, double key, double methodIndex);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double _propertyGetWithReplacerDouble(double sjsId, double key, double methodIndex);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double? _propertyGetWithReplacerDoubleNullable(double sjsId, double key, double methodIndex);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool _propertyGetWithReplacerBoolean(double sjsId, double key, double methodIndex);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool? _propertyGetWithReplacerBooleanNullable(double sjsId, double key, double methodIndex);


        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial string _propertyGetWithReplacerString(double sjsId, double key, double methodIndex, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double _propertyGetWithReplacerDouble(double sjsId, double key, double methodIndex, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double? _propertyGetWithReplacerDoubleNullable(double sjsId, double key, double methodIndex, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool _propertyGetWithReplacerBoolean(double sjsId, double key, double methodIndex, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool? _propertyGetWithReplacerBooleanNullable(double sjsId, double key, double methodIndex, double replacerConfig);


        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial string _propertyGetWithReplacerString(double sjsId, double key, double methodIndex, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double _propertyGetWithReplacerDouble(double sjsId, double key, double methodIndex, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double? _propertyGetWithReplacerDoubleNullable(double sjsId, double key, double methodIndex, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool _propertyGetWithReplacerBoolean(double sjsId, double key, double methodIndex, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool? _propertyGetWithReplacerBooleanNullable(double sjsId, double key, double methodIndex, string replacerConfig);



        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, double key, string value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, double key, bool value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, double key, double value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, double key, int value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, double key, bool? value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, double key, double? value);


        [JSImport("globalThis.SpawnJSInterop.propertySetSpawnJSObject")]
        internal static partial void _propertySetSpawnJSObject(double sjsId, double key, double value);

        [JSImport("globalThis.SpawnJSInterop.propertySetJson")]
        internal static partial void _propertySetJson(double sjsId, double key, string value);

        [JSImport("globalThis.SpawnJSInterop.propertySetHeapView")]
        internal static partial void _propertySetHeapView(double sjsId, double key, double dotnetId, double viewType, double offset, double length, bool copy);

        [JSImport("globalThis.SpawnJSInterop.propertySetNull")]
        internal static partial void _propertySetNull(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertySetUndefined")]
        internal static partial void _propertySetUndefined(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertySetCallback")]
        internal static partial void _propertySetCallback(double sjsId, double key, double dotnetId, double callbackId, bool once);


        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, string value, double reviverIndex);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, double value, double reviverIndex);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, string value, double reviverIndex, string reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, double value, double reviverIndex, string reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, string value, double reviverIndex, double reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, double value, double reviverIndex, double reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, string value, double reviverIndex, bool reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, double value, double reviverIndex, bool reviverConfig);

    }
}
