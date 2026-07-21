namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSObjectReference
    {
        /// <summary>
        /// Reads a property of a primitive type without going through the generic dispatcher.
        /// Returns false when this type or key is not eligible, in which case the caller takes the normal
        /// path.<br/>
        /// <br/>
        /// The generic path costs a lot for what a scalar read is: NetRun marshals the arguments into a
        /// NEW Javascript array, calls the dispatcher, receives the result wrapped in a SECOND array, wraps
        /// that in a handle and reads index 0. Two Javascript allocations and several boundary crossings to
        /// move one number. The typed Reflect bindings do it in a single JSImport call with no allocation,
        /// and they already exist - the generic path simply never used them.<br/>
        /// <br/>
        /// A dotted identifier is NOT eligible. Reflect.get takes a literal key, while the getProperty
        /// primitive resolves "a.b.c" through pathObjectInfo, so the fast path would silently read a
        /// property literally named "a.b.c" and find nothing.
        /// </summary>
        private bool TryGetFast<T>(string identifier, out T value)
        {
            value = default!;
            if (identifier.Contains('.')) return false;
            if (SpawnJSRuntime.CountCalls) SpawnJSRuntime.CountCall("fast:get");

            // Slot first. The Reflect path below needs a JSObject for the object being read FROM, so it
            // materialised a proxy on EVERY property access - and a proxy costs a GC handle, a proxy-table
            // entry and a permanent enumerable Symbol tag on the Javascript object. That tag is what makes
            // a record-typed web API throw, so this was not only the hottest cost in the library but a
            // correctness hazard reintroduced by every read of a wrapper's property.
            if (JSHandle.TryGetSlot(out var slot))
            {
                if (typeof(T) == typeof(int)) { value = (T)(object)SlotInterop.GetInt32(slot, identifier); return true; }
                if (typeof(T) == typeof(double)) { value = (T)(object)SlotInterop.GetDouble(slot, identifier); return true; }
                if (typeof(T) == typeof(bool)) { value = (T)(object)SlotInterop.GetBoolean(slot, identifier); return true; }
                if (typeof(T) == typeof(string)) { value = (T)(object)SlotInterop.GetString(slot, identifier)!; return true; }
                if (typeof(T) == typeof(int?)) { value = (T)(object)SlotInterop.GetInt32Nullable(slot, identifier)!; return true; }
                if (typeof(T) == typeof(double?)) { value = (T)(object)SlotInterop.GetDoubleNullable(slot, identifier)!; return true; }
                if (typeof(T) == typeof(bool?)) { value = (T)(object)SlotInterop.GetBooleanNullable(slot, identifier)!; return true; }
                return false;
            }

            var target = JSObject;

            if (typeof(T) == typeof(int)) { value = (T)(object)Reflect.GetInt32(target, identifier); return true; }
            if (typeof(T) == typeof(double)) { value = (T)(object)Reflect.GetDouble(target, identifier); return true; }
            if (typeof(T) == typeof(bool)) { value = (T)(object)Reflect.GetBoolean(target, identifier); return true; }
            if (typeof(T) == typeof(string)) { value = (T)(object)Reflect.GetString(target, identifier)!; return true; }
            if (typeof(T) == typeof(int?)) { value = (T)(object)Reflect.GetInt32Nullable(target, identifier)!; return true; }
            if (typeof(T) == typeof(double?)) { value = (T)(object)Reflect.GetDoubleNullable(target, identifier)!; return true; }
            if (typeof(T) == typeof(bool?)) { value = (T)(object)Reflect.GetBooleanNullable(target, identifier)!; return true; }
            return false;
        }

        /// <summary>
        /// Writes a property of a primitive type without going through the generic dispatcher.
        /// Returns false when this value or key is not eligible. See <see cref="TryGetFast{T}"/>.
        /// </summary>
        private bool TrySetFast(string identifier, object? value)
        {
            if (identifier.Contains('.')) return false;
            if (SpawnJSRuntime.CountCalls) SpawnJSRuntime.CountCall("fast:set");

            // Slot first, for the same reason as TryGetFast - SetProperty is already slot native and falls
            // back to the proxy path itself, so this only has to recognise the eligible value types.
            if (JSHandle.TryGetSlot(out _))
            {
                switch (value)
                {
                    case int i: JSHandle.SetProperty(identifier, (double)i); return true;
                    case double d: JSHandle.SetProperty(identifier, d); return true;
                    case bool b: JSHandle.SetProperty(identifier, b); return true;
                    case string s: JSHandle.SetProperty(identifier, s); return true;
                    case float f: JSHandle.SetProperty(identifier, (double)f); return true;
                    case long l: JSHandle.SetProperty(identifier, (double)l); return true;
                    default: return false;
                }
            }

            var target = JSObject;

            switch (value)
            {
                case int i: Reflect.Set(target, identifier, i); return true;
                case double d: Reflect.Set(target, identifier, d); return true;
                case bool b: Reflect.Set(target, identifier, b); return true;
                case string s: Reflect.Set(target, identifier, s); return true;
                case float f: Reflect.Set(target, identifier, (double)f); return true;
                case long l: Reflect.Set(target, identifier, (double)l); return true;
                default: return false;
            }
        }

        /// <summary>
        /// Calls a method with a primitive return type without going through the generic dispatcher.
        /// Returns false when this return type or key is not eligible.<br/>
        /// <br/>
        /// CallApply costs three Javascript array allocations: NetRun marshals its own arguments into one,
        /// the caller's argument array becomes a second inside it, and the result comes back wrapped in a
        /// third. Applying the function directly with a typed Reflect binding needs only the argument
        /// array - the typed binding returns the value itself, so there is no wrapper to allocate or
        /// unwrap.<br/>
        /// <br/>
        /// The function is reached through a VOLATILE handle, which shares its parent's storage rather
        /// than taking a slot of its own, and which reference counts the underlying proxy. Taking a raw
        /// JSObject and disposing it would be wrong: the runtime interns proxies, so disposing ours would
        /// kill any sibling holding the same Javascript function.
        /// </summary>
        private bool TryCallFast<T>(string identifier, object?[] args, out T value)
        {
            value = default!;
            if (identifier.Contains('.')) return false;
            if (typeof(T) != typeof(int) && typeof(T) != typeof(double)
                && typeof(T) != typeof(bool) && typeof(T) != typeof(string)) return false;

            if (SpawnJSRuntime.CountCalls) SpawnJSRuntime.CountCall("fast:call");
            using var function = new SpawnJSHandle(JSHandle, identifier, true);
            var fn = function.JSObject;
            if (fn == null) return false;

            using var jsArgs = JS.MarshallNetArrayToJSArray(args);
            var argsObject = jsArgs!.JSObjectRequired;
            var target = JSObject;

            if (typeof(T) == typeof(int)) { value = (T)(object)Reflect.ApplyInt32(fn, target, argsObject); return true; }
            // (the remaining primitive returns follow)
            if (typeof(T) == typeof(double)) { value = (T)(object)Reflect.ApplyDouble(fn, target, argsObject); return true; }
            if (typeof(T) == typeof(bool)) { value = (T)(object)Reflect.ApplyBoolean(fn, target, argsObject); return true; }
            value = (T)(object)Reflect.ApplyString(fn, target, argsObject)!;
            return true;
        }

        /// <summary>
        /// Calls a method that returns nothing, without going through the generic dispatcher.
        /// Returns false when the key is not eligible or names no function, in which case the caller takes
        /// the normal path.<br/>
        /// <br/>
        /// This is the shape that dominates real work. A GPU dispatch is a burst of void calls -
        /// setPipeline, setBindGroup, dispatchWorkgroups, end, submit, writeBuffer - and none of them
        /// could use the fast path while it only accepted primitive RETURN types, so the layer built to
        /// make interop cheap was sitting out the calls that matter. Measured against SpawnDev.BlazorJS,
        /// SpawnDev.ILGPU was SLOWER per kernel launch for exactly this reason.<br/>
        /// <br/>
        /// The generic path costs an object[] for the dispatcher arguments plus the caller's argument
        /// array marshalled inside it, then a command-name dispatch on the Javascript side. Applying the
        /// function directly needs only the argument array.
        /// </summary>
        private bool TryCallVoidFast(string identifier, object?[] args)
        {
            if (identifier.Contains('.')) return false;

            // Volatile handle: it shares its parent's storage instead of taking a slot, and reference
            // counts the proxy. Taking a raw JSObject and disposing it would kill any sibling holding the
            // same Javascript function, because the runtime interns proxies. Same reasoning as TryCallFast.
            if (SpawnJSRuntime.CountCalls) SpawnJSRuntime.CountCall("fast:callVoid");
            using var function = new SpawnJSHandle(JSHandle, identifier, true);
            var fn = function.JSObject;
            if (fn == null) return false;

            using var jsArgs = JS.MarshallNetArrayToJSArray(args);
            Reflect.ApplyVoid(fn, JSObject, jsArgs!.JSObjectRequired);
            return true;
        }
    }
}
