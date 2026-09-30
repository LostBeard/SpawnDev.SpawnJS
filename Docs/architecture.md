# Architecture

SpawnJS 3.x is a `JSImport` / `JSExport` interop layer, a managed marshaller graph, and a **call tape**: every call is written into .NET memory and crosses the boundary once. It does not use Microsoft's `JSObject` as a handle (creation/lookup cost, Symbol tagging, dispose aliasing). The only `JSObject` in the library is `JSHost.DotnetInstance`, passed once into `SpawnJSInterop._registerInstance`.

2.x crossed once per value: each argument, each POCO member, each array element was its own `propertySet` import, and reading a result back was one `Get` per member. 3.x keeps 2.x's structure - the object table, the dispatcher, the marshaller registry, instance-based state - and replaces that transport. See [v3-design.md](v3-design.md) for the measurements.

## The constraint

`[JSImport]` / `[JSExport]` marshalling is decided at compile time. You cannot write one import that accepts "any type," and you cannot pick a marshaller at runtime on the JS side. Blazor works around this with JSON; SpawnJS 2.x by crossing only primitives the generator knows, one value at a time. 3.x writes the whole call - any shape - into .NET memory, and crosses with a single import whose signature never changes: `(dotnetId, address, length, capacity)`.

## Pieces

### 1. JS object table

`SpawnJSInterop.spawnJSObjects` (in `wwwroot/SpawnDev.SpawnJS.lib.module.js`) holds every JS value .NET currently references. .NET addresses it with a `double` id (`SpawnJSObjectReference.Id`).

- Ids are monotonic (`_sjsObjectIdNext`) and **never reused**. A disposed handle cannot name a later value.
- Negative ids are sentinels with no table entry: `-1` `globalThis`, `-2` `undefined`, `-3` `null`, `-4` the table, `-5` `SpawnJSInterop`.
- `spawnJSObjectHold` / `spawnJSObjectRelease` add and drop entries. The table is a strong ref; nothing in the JS GC will free a held value. Dispose is mandatory.

`SpawnJSRuntime` is itself a `SpawnJSObjectReference` whose id is `GlobalThisId`, so `JS.Get` / `JS.Call` operate on `globalThis`.

### 2. The call tape (.NET to JS)

`JSTape` belongs to one `SpawnJSRuntime`. It is a stack of pinned `double[]` segments; everything is an 8 byte cell, read by JS through `Float64Array` / `Int32Array` / `Uint16Array` views over the WASM heap.

A call (`InteropCall<...>`):

1. `BeginCall` opens a frame: `methodIndex | resultSchemaId | argCount`. The method is an `InteropMethod` handle whose index is resolved once.
2. Each argument's marshaller `Write`s it: a tag and its payload (numbers, strings as UTF-16, object refs as table ids, objects as a shape id plus member values, arrays, bulk number arrays, heap views, revived values, callbacks). Nothing crosses.
3. Definitions the call needs (a result schema, a callback's argument schemas) follow the arguments.
4. **One** `_spawnJSInteropCall(dotnetId, address, length, capacity)`. JS reads the frame, runs `SpawnJSInterop._methodMap[methodIndex]`, and writes the result back over the frame by the result schema.
5. The result marshaller `Read`s it with a `JSTapeReader` (a ref struct). A result too big for the frame's segment is held by JS and fetched into its own array (`_spawnJSInteropCallResult`).

Frames nest - a marshaller can make a call while writing an argument, a callback can call back in - so a new frame never moves an open one, and once a result arrives the write position moves past it (reading can re-enter interop).

Async is `_spawnJSInteropCallAsync`: JS reads the frame before it returns (the frame is then released), awaits the primitive, and completes a `TaskCompletionSource` through the `resolve*` callbacks registered at `_registerInstance` - or, for a composite result, holds the encoded bytes and calls `resolveTape`.

Public `Get` / `Set` / `Call` / `New` are thin wrappers over the dispatcher primitives (`propertyGet`, `propertySet`, `propertyCall`, `propertyNew`, `propertyCallApply`...). Identifiers go through `pathObjectInfo`, which walks dotted paths and `?.` short-circuits.

### 3. Shapes and schemas

- **Write:** a POCO crosses as a `JSShape` id plus its member values. The member names cross once per runtime, inline, ahead of the first object that uses them. A shape is only *confirmed* after a call carrying its definition completes; until then every frame re-sends it, so an abandoned call can never leave .NET believing JS knows a shape it does not.
- **Read:** a marshaller's `JSSchema` tells JS what to write back. Kinds 0-10 are 2.x's `ReturnType` values (same numbers, same conversions); composite kinds (`Object`, `Array`, `Numbers`, `Record`, `Tuple`, `Bytes`) describe a whole value, so a POCO or an array comes back in the same crossing. Schema definitions follow a call's arguments and are confirmed like shapes.

### 4. Inbound calls (JS to .NET)

A `Callback` written to the tape carries the schemas of its argument types. The JS function it becomes:

1. Holds the `arguments` array (`spawnJSObjectHold`) - a `Func` result is written back onto it.
2. Writes the arguments by their schemas into the runtime's **inbound buffer** - pinned .NET memory registered once per instance (`_registerInbound`), used as a stack because callbacks nest - and puts their offset in the buffer's cell 0.
3. Calls `handleCallback(callbackId, argsId, argsCount)`.
4. Releases the args slot and restores the stack in a `finally`.

`Callback.HandleCallback` reads the offset, then every argument from .NET memory (`ReadArg<T>`). No argument is read back across the boundary.

`Action` / `Func` values are wrapped by `DelegateMarshaller`: one `Callback` per delegate instance (cached).

### 5. Multi-instance

Two .NET WASM apps can share a page. Everything per runtime lives on the `instanceInfo` that `_registerInstance` creates, keyed by the held `dotnetId`: the tape's heap views, shapes, schemas, the inbound buffer, result scratch buffers. Every import carries its `dotnetId`. The only page global is the `SpawnJSInterop` class. `twin.html` + `TwinTests` boot two real runtimes in one page and prove it (`SpawnJS.TestRunner --twin`).

### 6. Marshallers

See [writing-marshallers.md](writing-marshallers.md). The registry is scanned in reverse registration order; the winner is cached per type in a static slot. Writes type from the value; reads type from the declared `T`.

### 6a. Generated POCO codecs

`SpawnDev.SpawnJS.Generators` (a Roslyn incremental generator shipped in the package under `analyzers/dotnet/cs`) writes a `JSPocoCodec<T>` for each POCO an assembly sends or reads, registered by a module initializer. A codec reads and sets members directly (an `[UnsafeAccessor]` for a private or protected one), with `[JsonIgnore]` / `[JsonPropertyName]` / `[JsonInclude]` resolved at compile time. Each value still goes through the member type's marshaller, so Javascript receives exactly what the reflection plan sends.

- **Found by use:** the type arguments of every SpawnJS method called and SpawnJS type named, the type of every argument passed to one, and everything reachable through their members, elements and type arguments. Only types declared in the compiling assembly.
- **Found by attribute:** `[SpawnJSPoco]`, for a type only the running code knows (held in an `object`, an interface or a base class member).
- **Left to reflection:** anything the generator cannot mirror exactly: generic, abstract, enumerable or dedicated-marshaller types, an `override` or a `new` redeclaration, an indexer, a write-only property. Reads are left to reflection when a member is init-only, `required` or a readonly field, or there is no public parameterless constructor.
- **The gate:** `Codec.Parity.Names` compares every codec's names and order with `GetTypeJsonProperties`, and `Codec.Parity.Write` compares what Javascript receives from each type, empty and filled, codec against reflection. The whole suite also runs with `?nocodecs` (`SpawnJS.TestRunner --nocodecs`, `JSPocoCodecs.UseGenerated = false`), and must pass both ways.

### 7. Heap views

`HeapView` / `HeapViewDescriptor` build a JS `TypedArray` or `DataView` over WASM linear memory (copy or persistent). A `byte[]` argument is pinned until its call ends and copied once, by JS. Heap growth detaches `ArrayBuffer`s; the tape re-reads the buffer when it has changed. Persistent views are invalid after a detach.

## What 3.x is not

- Not Blazor's `IJSInProcessRuntime` and not JSON on the default path.
- Not Microsoft `JSObject` / `JSHost` except the one `DotnetInstance` handoff.
- Not 1.x's frame: 1.x hung its frame on page globals and broke a second runtime. The tape is per instance.
- Not batched: one call is one crossing, with normal error timing. Calls are not deferred.
