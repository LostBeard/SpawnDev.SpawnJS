using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection;

namespace SpawnDev.SpawnJS.JSObjects
{
    public static class ElementReferenceExtensions
    {
        static PropertyInfo? _webElementReferenceContext_JSRuntimePropertyInfo;
        extension (ElementReference elementReference)
        {
            /// <summary>
            /// Return the ElementReference as a SpawnJSObject type
            /// </summary>
            /// <typeparam name="T">SpawnJSObject type</typeparam>
            /// <returns>The ElementReference as a SpawnJSObject of type T</returns>
            public T As<T>() where T : SpawnJSObject
            {
                var ipJS = elementReference.AsSpawnJSObjectReference();
                return ipJS == null ? null! : (T)Activator.CreateInstance(typeof(T), ipJS)!;
            }
            /// <summary>
            /// Return the ElementReference as a SpawnJSObjectReference
            /// </summary>
            /// <returns>The ElementReference as a SpawnJSObjectReference</returns>
            public SpawnJSObjectReference? AsSpawnJSObjectReference()
            {
                SpawnJSObjectReference? ret = default!;
                var ipJS = elementReference.GetRuntime();
                if (ipJS != null)
                {
                    var sjsId = ipJS.Invoke<double>($"SpawnJSInterop.spawnJSObjectHold", elementReference);
                    ret = SpawnJSObjectReference.FromID(sjsId);
                }
                return ret;
            }
            internal IJSInProcessRuntime? GetRuntime()
            {
                if (elementReference.Context is WebElementReferenceContext ctx)
                {
                    _webElementReferenceContext_JSRuntimePropertyInfo ??= ctx.GetType().GetProperty("JSRuntime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
                    var jsRuntime = _webElementReferenceContext_JSRuntimePropertyInfo.GetValue(ctx);
                    if (jsRuntime is IJSInProcessRuntime js) return js;
                }
                return default;
            }
        }
    }
}
