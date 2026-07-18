using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Blazor
{
    /// <summary>
    /// IJSInProcessObjectReference implemented via SpawnJSObject
    /// </summary>
    public class SpawnJSObjectBlazor : SpawnJSObjectReference, IJSInProcessObjectReference
    {
        /// <summary>
        /// Create a new instance that wraps the specified JSObject
        /// </summary>
        /// <param name="jsObject"></param>
        public SpawnJSObjectBlazor(JSObject jsObject) : base(jsObject)
        {

        }
        /// <summary>
        /// Dispose the object and its underlying JSObject
        /// </summary>
        /// <returns></returns>
        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }
        /// <summary>
        /// Invoke the specified JS function synchronously and return the result as TValue
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public TValue Invoke<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, params object?[]? args)
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
        public IJSInProcessObjectReference InvokeConstructor(string identifier, object?[]? args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the specified property value as TValue
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public TValue GetValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set the specified property to the specified value
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public void SetValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, TValue value)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
