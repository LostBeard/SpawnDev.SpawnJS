# Architecture

## The constraint that shapes everything

.NET WebAssembly interop is built on `[JSImport]` / `[JSExport]`, whose marshalling is decided **at compile time** by a source generator. You cannot choose a marshaller at runtime, and you cannot write one `JSImport` that accepts "any type." Blazor works around this by serializing everything to JSON. SpawnJS takes a different route.

## The core bend

Instead of trying to make one import handle every type, SpawnJS **reduces every interop operation to a handful of fixed-signature primitives the generator already understands**, carries all variety as opaque `JSObject` handles, and composes the richness back in managed code.

1. **A few generic JS primitives.** A small JS class, `SpawnJSInterop` (in `wwwroot/SpawnDev.SpawnJS.lib.module.js`), exposes generic operations - `getProperty`, `setProperty`, `deleteProperty`, `invokeProperty`, `invokePropertyConstructor`, `hasOwnPropertySafe`, and so on. Each takes a target, an identifier, and an argument array. Identifiers support dotted paths (`"a.b.c"`) and null-conditional segments (`"a?.b"`) via a helper called `pathObjectInfo`.

2. **One dispatcher, two entry points.** Two `JSImport`-bound functions do all outbound calls:
   - `_netToJSCall(cmd, argsArray)` - synchronous
   - `_netToJSCallAsync(cmd, argsArray)` - asynchronous (JS `async`)

   `cmd` selects which primitive to run; `argsArray` holds the marshalled arguments.

3. **Everything is a live `JSObject` array.** The transport is never a serialized string - it is an actual JS array holding actual JS values.

4. **The `[ret]` wrap.** Both dispatchers wrap their result in a one-element array: `return [ret]`. This turns an unknown-typed JS return into a known operation: "return a `JSObject`, then read index 0 as the caller's target type." One fixed JS signature can now return any type.

5. **Managed marshaller graph.** On the .NET side, a registry of `JSMarshaller`s reads and writes typed values into that `JSObject` array. See [writing-marshallers.md](writing-marshallers.md).

The whole library is one pattern - **typed read/write of a `JSObject` array** - applied across get / set / call / construct, in both directions, sync and async.

## Outbound call path (.NET -> JS)

Managed code (`SpawnJSRuntime.Marshal.cs`) does the same four steps for every call:

```
NetRun<T>(cmd, args):
  jsArgs = NetArrayToJSArray(args)          // marshal each arg into a JS array via the graph
  ret    = NetToJSCall(cmd, jsArgs)         // JSImport -> _netToJSCall -> primitive -> [ret]
  return GetMarshaller(typeof(T))
           .JSToNet(typeof(T), ret, 0, ...)  // read index 0 of [ret] back as T
```

`NetRunVoid` skips the return read. `NetRunAsync` / `NetRunVoidAsync` are the exact same shape with an `await` on `_netToJSCallAsync` - adding async required no new mechanism, which is the sign the core bend is right. The `using` on both the argument array and the `[ret]` wrapper keeps the transport `JSObject`s from leaking.

## Why sync and async are separate JS functions

`_netToJSCall` and `_netToJSCallAsync` cannot be merged. A synchronous `JSImport` pointing at an `async` JS function would marshal the returned Promise as a `JSObject` instead of awaiting it. The value path (returns a `JSObject`) and the void path (ignores the `[ret]`) *can* share one JS function, so there are exactly two JS entry points, not four.

## Inbound call path (JS -> .NET)

The mirror direction uses the same bend in reverse. A single `JSExport`-style channel - `_JSToNetCall(intent, argsArray)` - carries every inbound call regardless of type or arity: `intent` selects what to run, `argsArray` holds the arguments, which are marshalled **in** through the same graph, and the result is marshalled back **out**. The `intent` is a `string` today and can become an `int` (backed by a message-type `enum`) for a more efficient dispatch key without changing the pattern. This path is wired and fires; inbound argument marshalling and dispatch are still being built out (see [roadmap.md](roadmap.md)).

## Where the win is

The biggest practical win is **correctness**: non-primitive return values that Blazor cannot marshal (it must `JSON.stringify` them on a JS side that has no knowledge of .NET types) cross intact here, because nothing is serialized - the .NET side owns all marshalling. The performance win is on **orchestration traffic** - the many small property/method/metadata calls that each otherwise allocate and parse a JSON string on both ends. Bulk GPU/OPFS data already avoids JSON in SpawnDev.BlazorJS (via typed-array / memory-view paths); SpawnJS does not rescue those, but it makes the zero-copy seam the natural default.
