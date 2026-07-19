using System.Collections.Concurrent;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSRuntime
    {
        #region JS -> .Net handlers
        // Named .Net handlers that Javascript can invoke by name. The inbound path is the exact mirror
        // of the outbound one: JS calls SpawnJSInterop.jsToNetCall(name, ...args) (or jsToNetCallApply),
        // which forwards (name, argsArray) to the _JSToNetCall JSExport binding; here we marshal each
        // argument IN through the marshaller graph (typed by the handler's parameters), invoke the handler,
        // and marshal the result back OUT as a one-element [result] array so JS reads index 0 - symmetric
        // with the outbound [ret] wrapper.
        readonly ConcurrentDictionary<string, Delegate> _jsToNetHandlers = new ConcurrentDictionary<string, Delegate>();

        /// <summary>
        /// Register a .Net handler that Javascript can invoke by name via
        /// <c>SpawnJSInterop.jsToNetCall(name, ...args)</c> or <c>jsToNetCallApply(name, args)</c>.
        /// Arguments are marshalled in (typed by the handler's parameters) and the result is marshalled
        /// back out using the runtime Marshallers.
        /// </summary>
        /// <param name="name">The intent name Javascript uses to select this handler.</param>
        /// <param name="handler">The .Net delegate to invoke.</param>
        public void SetHandler(string name, Delegate handler) => _jsToNetHandlers[name] = handler;

        /// <summary>
        /// Remove a previously registered JS-callable .Net handler.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if a handler was removed.</returns>
        public bool RemoveHandler(string name) => _jsToNetHandlers.TryRemove(name, out _);

        /// <summary>
        /// Returns true if a handler with the specified name is registered.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasHandler(string name) => _jsToNetHandlers.ContainsKey(name);

        /// <summary>
        /// Dispatch a JS -> .Net call. Invoked from Javascript through the _JSToNetCall JSExport binding.
        /// Marshals the JS argument array into the handler's parameters, invokes it, and returns the
        /// result wrapped in a one-element JS array so JS reads index 0.
        /// </summary>
        SpawnJSHandle JSToNetDispatch(string cmd, SpawnJSHandle argsArray)
        {
            if (Verbose) Console.WriteLine($">> JSToNetDispatch: {cmd}");
            if (!_jsToNetHandlers.TryGetValue(cmd, out var handler))
                throw new Exception($"SpawnJSRuntime: no JS->.Net handler registered for '{cmd}'");
            var method = handler.Method;
            var parameters = method.GetParameters();
            var netArgs = new object?[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
                netArgs[i] = JSToNet(parameters[i].ParameterType, argsArray, i);
            var result = handler.DynamicInvoke(netArgs);
            // Package the result as [result] so JS reads index 0 (mirror of the outbound [ret] wrapper).
            var retArray = NewJSArray();
            if (method.ReturnType != typeof(void))
                NetToJS(retArray, 0, result);
            if (Verbose) Console.WriteLine($"<< JSToNetDispatch: {cmd}");
            return retArray;
        }
        #endregion
    }
}
