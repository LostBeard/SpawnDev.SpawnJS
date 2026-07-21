namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSObjectReference
    {
        /// <summary>
        /// Calls a method with its arguments already in the ARGUMENT FRAME - ONE crossing for the whole
        /// call. Returns false when this target or key is not eligible and the caller should take the
        /// normal path.<br/>
        /// <br/>
        /// This is where the remaining cost was, and it survived the first round of frame work because
        /// only the generic dispatcher was moved over. A call on a held wrapper still built a Javascript
        /// argument array the expensive way: one crossing to create it, one PER ARGUMENT to fill it, one
        /// to invoke, one to free it - N+3 crossings for arguments that were already sitting in .Net
        /// memory. That is exactly the shape a GPU dispatch is made of: setPipeline, setBindGroup,
        /// dispatchWorkgroups, end, submit.<br/>
        /// <br/>
        /// Now the target is a slot number, the arguments are frame slots, and the boundary carries a slot
        /// id, a method name, an offset and a length.<br/>
        /// <br/>
        /// A dotted identifier is not eligible: the Javascript side indexes the target by the literal
        /// name, so "console.log" would look for a property called exactly that.
        /// </summary>
        private bool TryInvokeVoidBySlot(string identifier, object?[] args)
        {
            if (identifier.Contains('.')) return false;
            if (!JSHandle.TryGetSlot(out var thisSlot)) return false;

            var offset = JS.WriteArgsToFrame(args);
            try
            {
                SlotInterop.InvokeFrameVoid(thisSlot, identifier, offset, args.Length);
                return true;
            }
            finally
            {
                JS.ReleaseFrameArgs(offset);
            }
        }

        /// <summary>
        /// Calls a method through the frame and converts its result.<br/>
        /// The result is written back into this call's own frame slot, so a number or a boolean moves no
        /// data across the boundary in either direction, and anything else arrives as a slot id - which
        /// means an object returned from a call becomes a handle with no crossing and no proxy.<br/>
        /// <br/>
        /// The separate typed invoke bindings this replaces (InvokeDouble, InvokeString, InvokeBoolean,
        /// InvokeSlot) each cost their own crossing to deliver the arguments. One framed call covers every
        /// return type, so the typed variants bought nothing once the arguments stopped crossing.
        /// </summary>
        private bool TryInvokeBySlot<T>(string identifier, object?[] args, out T value)
        {
            value = default!;
            if (identifier.Contains('.')) return false;
            if (!JSHandle.TryGetSlot(out var thisSlot)) return false;

            var offset = JS.WriteArgsToFrame(args);
            try
            {
                SlotInterop.InvokeFrameResult(thisSlot, identifier, offset, args.Length);
                value = (T)JS.ReadFrameResult(typeof(T), offset)!;
                return true;
            }
            finally
            {
                JS.ReleaseFrameArgs(offset);
            }
        }
    }
}
