namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSObjectReference
    {
        /// <summary>
        /// Calls a method entirely through the slot table, so nothing on either side becomes a .Net
        /// proxy. Returns false when this target or key is not eligible and the caller should take the
        /// normal path.<br/>
        /// <br/>
        /// This is where the remaining cost was. Creating objects in slots made building a descriptor
        /// cheap, but handing it to a method still went through JSObject for the target AND the argument
        /// array, so a proxy was created at call time anyway - measured at 7.6us per handle, ~40 of them
        /// per GPU dispatch. With the call itself slotted, the boundary carries a slot number, a method
        /// name and a slot number, and nothing else.<br/>
        /// <br/>
        /// A dotted identifier is not eligible: the Javascript side indexes the target by the literal
        /// name, so "console.log" would look for a property called exactly that.
        /// </summary>
        private bool TryInvokeVoidBySlot(string identifier, object?[] args)
        {
            if (identifier.Contains('.')) return false;
            if (!JSHandle.TryGetSlot(out var thisSlot)) return false;

            using var argsHandle = JS.MarshallNetArrayToJSArray(args);
            if (argsHandle == null || !argsHandle.TryGetSlot(out var argsSlot)) return false;

            SlotInterop.InvokeVoid(thisSlot, identifier, argsSlot);
            return true;
        }

        /// <summary>
        /// Calls a method through the slot table and converts its result.<br/>
        /// A primitive comes straight back. Anything else stays in Javascript, in a slot, and is handed
        /// to the marshaller as a handle - so an object-returning call only materialises a proxy if the
        /// destination type actually needs one.
        /// </summary>
        private bool TryInvokeBySlot<T>(string identifier, object?[] args, out T value)
        {
            value = default!;
            if (identifier.Contains('.')) return false;
            if (!JSHandle.TryGetSlot(out var thisSlot)) return false;

            using var argsHandle = JS.MarshallNetArrayToJSArray(args);
            if (argsHandle == null || !argsHandle.TryGetSlot(out var argsSlot)) return false;

            if (typeof(T) == typeof(double)) { value = (T)(object)SlotInterop.InvokeDouble(thisSlot, identifier, argsSlot); return true; }
            if (typeof(T) == typeof(int)) { value = (T)(object)(int)SlotInterop.InvokeDouble(thisSlot, identifier, argsSlot); return true; }
            if (typeof(T) == typeof(float)) { value = (T)(object)(float)SlotInterop.InvokeDouble(thisSlot, identifier, argsSlot); return true; }
            if (typeof(T) == typeof(long)) { value = (T)(object)(long)SlotInterop.InvokeDouble(thisSlot, identifier, argsSlot); return true; }
            if (typeof(T) == typeof(bool)) { value = (T)(object)SlotInterop.InvokeBoolean(thisSlot, identifier, argsSlot); return true; }
            if (typeof(T) == typeof(string)) { value = (T)(object)SlotInterop.InvokeString(thisSlot, identifier, argsSlot)!; return true; }

            // Non primitive: keep the result in Javascript and let the registry decide what it becomes.
            // The handle owns the returned slot, so the slot is released when it is disposed - a wrapper
            // type takes its own reference during marshalling.
            var resultSlot = SlotInterop.InvokeSlot(thisSlot, identifier, argsSlot);
            using var resultHandle = new SpawnJSHandle(resultSlot);
            value = (T)JS.MarshallJSToNet(typeof(T), resultHandle)!;
            return true;
        }
    }
}
