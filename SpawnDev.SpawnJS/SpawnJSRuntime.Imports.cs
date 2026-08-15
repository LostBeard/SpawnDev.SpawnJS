using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSRuntime
    {
        /// <summary>
        /// This is the ONLY JSImport/JSExport that is allowed to use JSObject and it is ONLY used to get a reference to this .Net Wasm app's DotNet Instance
        /// </summary>
        /// <param name="dotnetInstance">ONLY allowed JSObject in entire library and will ONLY be called once</param>
        /// <returns></returns>
        [JSImport("globalThis.SpawnJSInterop._registerInstance")]
        internal static partial double _registerInstance(
            JSObject dotnetInstance,
            [JSMarshalAs<JSType.Function>] Action onMethodAdded,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.String>>] Action<double, string> onAsyncResolvedVoid,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.Number, JSType.String>>] Action<double, double, string> onAsyncResolvedDouble,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.Boolean, JSType.String>>] Action<double, bool, string> onAsyncResolvedBool,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.String, JSType.String>>] Action<double, string, string> onAsyncResolvedString,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.Any, JSType.String>>] Action<double, object, string> onAsyncResolvedDoubleNullable,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.Any, JSType.String>>] Action<double, object, string> onAsyncResolvedBooleanNullable,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.Number, JSType.String>>] Action<double, int, string> onAsyncResolvedInt32,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.Any, JSType.String>>] Action<double, object, string> onAsyncResolvedInt32Nullable,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.Number>>] Action<long, long> onDetachedHeap,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.Number, JSType.Number>>] Action<double, double, double> onCallback);

        #region _spawnJSInteropCall
        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial bool? _spawnJSInteropCallBooleanNullable(int returnType, int methodIndex, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial double? _spawnJSInteropCallDoubleNullable(int returnType, int methodIndex, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial bool _spawnJSInteropCallBoolean(int returnType, int methodIndex, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial int _spawnJSInteropCallInt32(int returnType, int methodIndex, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial int? _spawnJSInteropCallInt32Nullable(int returnType, int methodIndex, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial double _spawnJSInteropCallDouble(int returnType, int methodIndex, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial string _spawnJSInteropCallString(int returnType, int methodIndex, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial void _spawnJSInteropCallVoid(int returnType, int methodIndex, double argsId);
        #endregion

        #region _spawnJSInteropCallAsync
        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCallAsync")]
        internal static partial void _spawnJSInteropCallAsync(int returnType, double dotnetId, double asyncCallId, double methodIndex, double argsId);
        #endregion

        /// <summary>
        /// Creates a new JS Array, holds it, and returns the sjsId
        /// </summary>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectNewArray")]
        internal static partial double _spawnJSObjectNewArray();

        /// <summary>
        /// Gets an up to date method map so calls can use method indexes instead of names
        /// </summary>
        /// <returns></returns>
        [JSImport("globalThis.SpawnJSInterop.refreshMethodMap")]
        internal static partial string[] _refreshMethodMap();

        /// <summary>
        /// Releases a Callback function
        /// </summary>
        /// <param name="dotnetId">The owning dotnet instance id</param>
        /// <param name="callbackId">The  Callback's Id</param>
        [JSImport("globalThis.SpawnJSInterop.releaseCallback")]
        internal static partial void _releaseCallback(double dotnetId, double callbackId);

        #region SpawnJSObject self
        #region spawnJSObjectRelease
        /// <summary>
        /// Release a SPawnJSObject reference in Javascript
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial void SpawnJSObjectRelease(double sjsId);

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a bool
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial bool SpawnJSObjectReleaseBoolean(double sjsId);

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a double
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial double SpawnJSObjectReleaseDouble(double sjsId);

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a int
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial int SpawnJSObjectReleaseInt32(double sjsId);

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a int?
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial int? SpawnJSObjectReleaseInt32Nullable(double sjsId);

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a bool
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial bool? SpawnJSObjectReleaseBooleanNullable(double sjsId);

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a double
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial double? SpawnJSObjectReleaseDoubleNullable(double sjsId);

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a string
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial string SpawnJSObjectReleaseString(double sjsId);
        #endregion

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a string
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectReleaseAsJson")]
        internal static partial string SpawnJSObjectReleaseJson(double sjsId);

        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectHoldExists")]
        internal static partial bool SpawnJSObjectHoldExists(double sjsId);

        // propertyTypeInfo: returns "<typeof> <toStringTag>" for a property, or null if absent.
        [JSImport("globalThis.SpawnJSInterop.getTypeInfo")]
        internal static partial string _getTypeInfo(double sjsId);
        #endregion
    }
}
