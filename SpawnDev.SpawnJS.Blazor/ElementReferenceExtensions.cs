using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Versioning;

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
            /// <exception cref="InvalidOperationException">
            /// The reference could not be resolved - it is default, or it was captured by a renderer other
            /// than Blazor WebAssembly.
            /// </exception>
            /// <remarks>
            /// 🔴 THIS USED TO RETURN <c>null!</c> AND SAY NOTHING. A method whose signature promises
            /// <typeparamref name="T"/> handing back a null-forgiving null means every caller dereferences
            /// it and gets a bare <c>NullReferenceException</c> - "Arg_NullReferenceException", no frame,
            /// no cause, nothing naming this method at all.
            ///
            /// MEASURED 2026-09-16: an app hosted by <c>SpawnDomRenderer</c> called this, got null every
            /// time, and the symptom - a chat transcript that would not scroll - was blamed in turn on
            /// flex layout, on scroll-anchoring logic and on image load order, through three rounds of
            /// fixes to a method that had never once run.
            ///
            /// ⚠️ Nothing that WORKED changes: a caller that got a usable element still gets one. Only the
            /// already-broken case differs, and now it names the cause and the fix.
            /// </remarks>
            [SupportedOSPlatform("browser")]
            public T As<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>() where T : SpawnJSObject
            {
                var ipJS = elementReference.AsSpawnJSObjectReference();
                if (ipJS == null)
                    throw new InvalidOperationException(
                        $"ElementReference '{elementReference.Id}' could not be resolved to an element. Its "
                        + "Context is not a Blazor WebElementReferenceContext, so a different renderer "
                        + "captured it - SpawnDomRenderer, for instance, in which case reference "
                        + "SpawnDev.SpawnJS.RazorRenderer and use its As<T>()/ElementRef<T> instead of this "
                        + "package. (A @ref is also unset until after the first render.)");
                return (T)Activator.CreateInstance(typeof(T), ipJS)!;
            }

            /// <summary>
            /// Return the ElementReference as a SpawnJSObjectReference
            /// </summary>
            /// <returns>The ElementReference as a SpawnJSObjectReference</returns>
            [SupportedOSPlatform("browser")]
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
