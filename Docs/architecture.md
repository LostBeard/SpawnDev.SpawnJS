# Architecture

SpawnJS 2.x is a `JSImport` / `JSExport` interop layer plus a managed marshaller graph. It does not use Microsoft's `JSObject` as a handle (creation/lookup cost, Symbol tagging, dispose aliasing). The only `JSObject` in the library is `JSHost.DotnetInstance`, passed once into `SpawnJSInterop._registerInstance`.

Version 1.x used a shared heap argument frame (`HEAPF64` / command + offset + length). That transport is gone. Do not treat 1.x frame docs, `SlotInterop.FrameCall`, or `_netToJSCall` as current.

## The constraint

`[JSImport]` / `[JSExport]` marshalling is decided at compile time. You cannot write one import that accepts "any type," and you cannot pick a marshaller at runtime on the JS side. Blazor works around this with JSON. SpawnJS works around it by crossing only primitives the generator already knows (`bool`, `int`, `double`, `string`, void) and integer object-table ids.

## Pieces

### 1. JS object table

`SpawnJSInterop.spawnJSObjects` (in `wwwroot/SpawnDev.SpawnJS.lib.module.js`) holds every JS value .NET currently references. .NET addresses it with a `double` id (`SpawnJSObjectReference.Id`).

- Ids are monotonic (`_sjsObjectIdNext`) and **never reused**. A disposed handle cannot name a later value.
- Negative ids are sentinels with no table entry: `-1` `globalThis`, `-2` `undefined`, `-3` `null`, `-4` the table, `-5` `SpawnJSInterop`.
- `spawnJSObjectHold` / `spawnJSObjectRelease` add and drop entries. The table is a strong ref; nothing in the JS GC will free a held value. Dispose is mandatory.

`SpawnJSRuntime` is itself a `SpawnJSObjectReference` whose id is `GlobalThisId`, so `JS.Get` / `JS.Call` operate on `globalThis`.

### 2. Outbound calls (.NET to JS)

`SpawnJSRuntime.InteropCallApply<T>`:

1. Resolve `GetMarshaller<T>()` so the JS side knows the `ReturnType`.
2. If there are arguments, take a pooled JS array (`spawnJSObjectNewArray` / `_callArrays`). For each arg, resolve a marshaller from the **boxed value's runtime type** and `NetToJS` it onto the array by index.
3. Call `_spawnJSInteropCall*` with `(returnType, methodIndex, argsId)`.
4. JS looks up `SpawnJSInterop._methodMap[methodIndex]`, replaces the args slot with a fresh `[]` (so the pool can reuse the same id), runs the primitive, then `_serializeToNet` shapes the result (`spawnJSObjectHold` for object returns, `JSON.stringify` only for `ReturnType.Json`).
5. .NET reads the primitive through the typed import and `JSToNet`s it as `T`.
6. The emptied args array goes back on the pool.

Async is `_spawnJSInteropCallAsync`: JS awaits the primitive (typically a Promise), then invokes the matching `resolve*` callback registered at `_registerInstance`, which completes a `TaskCompletionSource`. No `Task` object crosses the boundary.

Public `Get` / `Set` / `Call` / `New` are thin wrappers that name primitives such as `propertyGet`, `propertySet`, `propertyCall`, `propertyNew`, `propertyCallApply`. Identifiers go through `pathObjectInfo`, which walks dotted paths and `?.` short-circuits.

Typed `PropertyGet*` / `PropertySet*` `JSImport`s exist for primitive keys and are what marshallers use when writing an `int`/`string`/`bool` onto a parent. The public `Get<T>` / `Set<T>` path still goes through `InteropCall` so the marshaller graph runs.

### 3. Inbound calls (JS to .NET)

At startup, `_registerInstance` stores this app's `DotnetInstance` and a `handleCallback` function. `propertySetCallback` installs a JS function that, when invoked:

1. Holds the `arguments` array (`spawnJSObjectHold`).
2. Calls `handleCallback(callbackId, argsId, argsCount)`.
3. Releases the args slot in a `finally` (so a throwing handler cannot leak the array or leave a `once` callback registered).

`Callback.HandleCallback` looks up the id, builds a `SpawnJSObjectReference` with `preventDispose: true` (JS owns that slot), and the `ActionCallback` / `FuncCallback` reads args by index through the marshaller graph. Func results are written back onto the same args array for JS to pick up.

`Action` / `Func` values are wrapped by `DelegateMarshaller`: one `Callback` per delegate instance (cached), so the same .NET method does not allocate a new JS function every crossing.

### 4. Multi-instance

`SpawnJSInterop._instances` is keyed by the held `dotnetId`. Two .NET WASM apps on one page each register; none of this state lives as a single page global that a second runtime would clobber. `AppBaseUri` is resolved per instance from that app's own load URL.

### 5. Marshallers

See [writing-marshallers.md](writing-marshallers.md). The registry is scanned in reverse registration order and cached per `Type`. Writes type from the value; reads type from the declared `T`.

### 6. Heap views

`HeapView` / `HeapViewDescriptor` pin .NET memory and build a JS `TypedArray` or `DataView` over WASM linear memory (copy or persistent). Heap growth detaches `ArrayBuffer`s; `onDetachedHeap` fires when JS notices. Persistent views are invalid after a detach. Prefer copies (`copy: true`, `To<T>()`) unless the view is short-lived inside one synchronous call.

## What 2.x is not

- Not Blazor's `IJSInProcessRuntime` and not JSON on the default path.
- Not Microsoft `JSObject` / `JSHost` except the one `DotnetInstance` handoff.
- Not the 1.x heap argument frame. A call does not carry `(cmd, offset, length)` into `HEAPF64`.
