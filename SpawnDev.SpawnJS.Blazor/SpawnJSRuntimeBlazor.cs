using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using SpawnDev.SpawnJS;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Blazor
{
    /// <summary>
    /// IJSInProcessRuntime implemented via SpawnJSRuntime
    /// </summary>
    public partial class SpawnJSRuntimeBlazor : SpawnJSRuntime, IJSInProcessRuntime
    {
        /// <summary>
        /// Invoke the specified JS function synchronously and return the result as TResult
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public TResult Invoke<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TResult>(string identifier, params object?[]? args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invoke the specified JS function asynchronously and return the result as TValue
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, object?[]? args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invoke the specified JS function asynchronously, with cancellation, and return the result as TValue
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            throw new NotImplementedException();
        }

#if NET10_0_OR_GREATER
        /// <summary>
        /// Invoke the specified JS constructor (new) and return the created object reference
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IJSInProcessObjectReference InvokeConstructor(string identifier, params object?[]? args)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Set the specified global property to the specified value
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public void SetValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, TValue value)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get the specified global property value as TValue
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public TValue GetValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
