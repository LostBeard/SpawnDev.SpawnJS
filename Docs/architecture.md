# Architecture

## The constraint that shapes everything

.NET WebAssembly interop is built on `[JSImport]` / `[JSExport]`, whose marshalling is decided **at compile time** by a source generator. You cannot choose a marshaller at runtime, and you cannot write one `JSImport` that accepts "any type." Blazor works around this by serializing everything to JSON. SpawnJS takes a different route.

## The core bend

Instead of trying to make one import handle every type, SpawnJS **reduces every interop operation to a handful of fixed-signature primitives the generator already understands**, carries the arguments in a flat buffer that needs no per-value crossing, and composes the richness back in managed code.

1. **A few generic JS primitives.** A small JS class, `SpawnJSInterop` (in `wwwroot/SpawnDev.SpawnJS.lib.module.js`), exposes generic operations - `getProperty`, `setProperty`, `deleteProperty`, `invokeProperty`, `invokePropertyConstructor`, `hasOwnPropertySafe`, and so on. Each takes a target, an identifier, and its arguments. Identifiers support dotted paths (`"a.b.c"`) and null-conditional segments (`"a?.b"`).

2. **One dispatcher.** A single `JSImport`-bound entry point carries every outbound call, and **only three primitives cross the boundary: a command name, an offset and a length.** On the JavaScript side, `_netToJSCall(cmd, offset, length)` reads that call's arguments from a shared buffer over the `[offset, offset+length)` slice, invokes `this[cmd](...)`, and leaves the result in the first slot of the caller's own region. The .NET side binds this through `SlotInterop.FrameCall(ctx, cmd, offset, length)`.

3. **Arguments live in .NET memory.** They are written into an **argument frame in .NET's own linear memory**, which JavaScript views directly through the runtime's `HEAPF64` / `HEAPU8` views. A .NET write is a plain array store that costs nothing to "deliver" - so the call carries only `(cmd, offset, length)`, and no per-argument value has to cross. Object references are carried as **integer slots**, not `JSObject` proxies.

4. **The result returns in place.** The dispatcher writes the return value back into the first slot of the caller's argument region, so one fixed JS signature can return any type - the .NET side reads that slot back as the caller's target type through the marshaller graph.

5. **Managed marshaller graph.** On the .NET side, a registry of `JSMarshaller`s reads and writes typed values. See [writing-marshallers.md](writing-marshallers.md).

The whole library is one pattern - **typed read/write over a shared buffer** - applied across get / set / call / construct, in both directions, sync and async.

## Outbound call path (.NET -> JS)

Managed code (`SpawnJSRuntime.Marshal.cs`) does the same shape for every call:

```
NetRun<T>(cmd, args):
  offset = <marshal args into the argument frame in .NET memory>
  FrameCall(ctx, cmd, offset, args.Length)   // only cmd, offset, length cross the boundary
  return GetMarshaller(typeof(T))
           .JSToNet(...)                       // read the result back from the frame as T
```

`NetRunVoid` skips the return read. The frame region unwinds like a stack when the call completes, so nothing is allocated per call. Held-reference operations (a simple key on an object you already hold) take a typed `Reflect` fast path instead of the general dispatcher - one binding, nothing allocated - which is why they are several times faster again than the dotted-path form.

## Async is the sync path returning a Promise

There is **no async dispatcher.** An async command is just a **synchronous call that happens to return a Promise**, so it rides the same flat buffer as everything else and keeps the same "only primitives cross" property:

```
NetRunAsync<T>(cmd, args):
  using var promise = NetRun<Promise>(cmd, args)   // the ordinary synchronous call
  return await promise.ThenAsync<T>()              // then(resolve, reject) with .NET callbacks
```

That is what lets the buffer be a stack at all. A real async binding could not use one: the call has not finished when it returns, so its region could not be released in order. Here the synchronous part finishes immediately and the region unwinds; the eventual value arrives later through the callback channel, which has its own storage. No `Task` is ever marshalled across the boundary.

## Inbound call path (JS -> .NET)

The mirror direction uses the same bend in reverse. A single `JSExport` channel - `_JSToNetCall(intent, argsArray)` - carries every inbound call regardless of type or arity: `intent` selects what to run, `argsArray` holds the arguments, which are marshalled **in** through the same graph, and the result is marshalled back **out**. This is the path every DOM event, callback and settled promise takes. The `intent` is a `string` today and can become an `int` (backed by a message-type `enum`) for a more efficient dispatch key without changing the pattern.

## Where the win is

The biggest practical win is **correctness**: non-primitive return values that Blazor cannot marshal (it must `JSON.stringify` them on a JS side that has no knowledge of .NET types) cross intact here, because nothing is serialized - the .NET side owns all marshalling. The performance win is on **orchestration traffic** - the many small property/method/metadata calls that each otherwise allocate and parse a JSON string on both ends. Bulk GPU/OPFS data already avoids JSON in SpawnDev.BlazorJS (via typed-array / memory-view paths); SpawnJS does not rescue those, but it makes the zero-copy seam the natural default.
