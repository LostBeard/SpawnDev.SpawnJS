using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshalls Func`string, JSObject, JSObject` to JS
    /// </summary>
    public class JSToNetInvokerMarshaller : JSMarshaller<Func<string, JSObject, JSObject>>
    {
        /// <inheritdoc/>
        public override object? JSToNet(Type type, JSObject jsParent, object jsKey, SpawnJSRuntime runtime)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc/>
        public override void NetToJS(Type? type, JSObject jsParent, object jsKey, object? value, SpawnJSRuntime runtime)
        {
            Reflect.Set(jsParent, jsKey, (Func<string, JSObject, JSObject>)value!);
        }
    }
    ///// <summary>
    ///// Marshalls Func`string, JSObject, JSObject` to JS
    ///// </summary>
    //public class JSToNetAsyncInvokerMarshaller : JSMarshaller<Func<string, JSObject, Task<JSObject>>>
    //{
    //    /// <inheritdoc/>
    //    public override object JSToNet(Type type, JSObject jsParent, object jsKey, SpawnJSRuntime runtime)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    /// <inheritdoc/>
    //    public override void NetToJS(Type type, JSObject jsParent, object jsKey, object value, SpawnJSRuntime runtime)
    //    {
    //        Reflect.Set(jsParent, jsKey, (Func<string, JSObject, Task<JSObject>>)value);
    //    }
    //}
}
