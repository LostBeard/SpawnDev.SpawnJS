# SpawnDev.SpawnJS

JSON-free JavaScript interop for .NET WebAssembly.

> *Blazor pushes the marshalling decision to the side that can't make it. SpawnJS moves it to the side that can.*

SpawnJS is a direct .NET <-> JavaScript interop layer built on `JSImport`/`JSExport`/`JSHost`/`JSObject` only. It has **no Blazor dependency**, so it runs in any .NET WASM host - Blazor, Avalonia, headless WASM console apps, and Web Workers.

Its API is modeled to be compatible with the Blazor WASM `IJSInProcessRuntime` / `IJSInProcessObjectReference` surface, so existing SpawnDev.BlazorJS-style interop code can run on SpawnJS with little or no change. Where Blazor's interop marshals values by serializing them to a JSON string and parsing them on the other side, SpawnJS marshals them directly as live JS values, removing the JSON serializer from the boundary.

## Why it exists

- **Kill the JSON boundary.** Blazor's `IJSInProcessRuntime` routes every value through a JSON serialize/parse on both ends. That cost is invisible on bulk data (which already goes zero-copy) but real on orchestration traffic - the thousands of small property reads, property writes, and method calls that make up normal interop. SpawnJS removes the serializer entirely.
- **Drop the Blazor dependency.** SpawnJS uses only the runtime interop primitives, so libraries built on it can reach .NET WASM apps that are not Blazor - for example Avalonia, satisfying [LostBeard/SpawnDev.BlazorJS.WebWorkers#28](https://github.com/LostBeard/SpawnDev.BlazorJS.WebWorkers/issues/28).
- **Make browser GPU compute faster.** [SpawnDev.ILGPU](https://github.com/LostBeard/SpawnDev.ILGPU) and
  SpawnDev.ILGPU.ML run real GPU compute in the browser across WebGPU, WebGL and Wasm, and they are
  interop-heavy by nature: every kernel launch plumbs buffers, bind groups and parameters across the
  boundary before any work reaches the GPU. A measured WebGPU kernel launch costs **15 interop calls** -
  nothing for a single kernel, but a model forward pass runs thousands of launches, and that is where the
  cost concentrates. Those libraries were a large part of the reason this one was written.
- **Simplify Web Worker startup.** SpawnDev.BlazorJS.WebWorkers currently has to build a fake `window` environment to boot a Blazor app inside a worker. Without the Blazor interop layer, that shim is no longer needed.

## Measured against SpawnDev.BlazorJS

Both runtimes in one Blazor app, hitting the same Javascript object with the same operation, so the only
variable is the interop path. 20,000 iterations per case.

| operation | BlazorJS | SpawnJS | |
|---|---:|---:|---:|
| read a string from a held object | 1661 ms | **38 ms** | **43.4x** |
| read an int from a held object | 1618 ms | **42 ms** | **39.0x** |
| write an int to a held object | 1244 ms | **42 ms** | **29.6x** |
| call a method on a held object | 1818 ms | **115 ms** | **15.9x** |
| read a string by path from globalThis | 1500 ms | **273 ms** | **5.5x** |
| read an int by path from globalThis | 1427 ms | **289 ms** | **4.9x** |
| write an int by path on globalThis | 1065 ms | **251 ms** | **4.2x** |
| call a method by path | 1675 ms | **523 ms** | **3.2x** |
| take a handle to an object | 2510 ms | **909 ms** | **2.8x** |

The two shapes differ because they take different routes. A **held object reference with a simple key**
resolves to a single typed `Reflect` binding - one call across the boundary, nothing allocated. A **dotted
path** has to be resolved at each call, so it goes through the general dispatcher. Most interop in a real
library is the first shape: hold a `GPUDevice` or an `HTMLCanvasElement`, then read and write its
properties.

Where the difference comes from:

- **Nothing is serialized.** There is no JSON encode or parse on either side.
- **Nothing is allocated per call.** Arguments and results share one flat buffer, and a call carries only
  a command name, an offset and a length - no Javascript object reference crosses at all.
- **One Javascript function per delegate.** A `Callback` shares its function with every other Callback over
  the same .NET method rather than registering a new one.
- **No proxy per reference.** Javascript holds the value in a slot table and .NET holds the integer that
  addresses it, so a reference costs a number rather than a GC handle plus a proxy-table entry.

## Measured on a real workload: GPU dispatch

Microbenchmarks prove a layer is fast in isolation; they do not prove it moves real work. This is
[SpawnDev.ILGPU](https://github.com/LostBeard/SpawnDev.ILGPU) dispatching an actual WebGPU compute kernel,
same ILGPU source on both interop layers, same Blazor host, same browser, same GPU (NVIDIA Lovelace,
software fallback adapters rejected), kernel output verified correct on every run.

| | BlazorJS | SpawnJS | |
|---|---:|---:|---:|
| kernel launch | 1284 us | **~490 us** | **2.6x** |
| first dispatch (compile + setup) | ~125 ms | **~34 ms** | **3.7x** |

Neither side using bind-group caching, so the comparison is like for like.

This number was earned rather than assumed. SpawnJS started out **1.7x slower** than BlazorJS on this
workload, and each of the three wins was somewhere the shape of the code did not predict:

| fix | saved |
|---|---:|
| Cache per-type serializable members (an attribute scan ran on every marshal) | 1075 us |
| Slot-backed references (creating a `JSObject` proxy cost 21 us; a slot costs 1.3 us) | 326 us |
| Slot-native invocation (target, method and arguments all stay in Javascript) | 245 us |

Along the way SpawnJS's own per-dispatch cost went from 49 `JSObject` proxies to 4, and from 4 generic
dispatcher calls to none. (Those two are SpawnJS before against SpawnJS after - the equivalent internals
of the BlazorJS path were not instrumented.)

## Object marshalling

| shape | cost |
|---|---:|
| write a primitive to a held object | 2.0 us |
| marshal an empty object | 5.5 us |
| marshal an object with 3 properties | 12.4 us |
| marshal an object with a nested object | 20.3 us |
| create a Javascript object | 2.0 us |
| take a slot reference | 1.3 us |
- **Async is a synchronous call that returns a Promise**, converted to a Task with `then`, so no Task is
  marshalled across the boundary and async uses the same buffer as everything else.

Reproduce it:

```
dotnet run --project BlazorBrowserDemo -c Release --urls http://localhost:5199
dotnet run --project SpawnJS.TestRunner -- --url "http://localhost:5199/?bench" --verbose
```

For an interop-heavy consumer the per-call figures are the ones that matter. A SpawnDev.ILGPU WebGPU kernel
launch was measured at 15 interop calls, so the saving scales with launch count rather than with data size -
irrelevant for one kernel, potentially dominant across a graph with thousands of nodes.

That end-to-end number has now been measured, on a real GPU rather than a fallback adapter (NVIDIA
Lovelace, `fallback=False` - the harness fails rather than report a software adapter's timings as a
GPU's):

| SpawnDev.ILGPU WebGPU dispatch | before | after |
|---|---:|---:|
| kernel launch (queue only) | 207.2 us | **154.4 us** |
| dispatch + `SynchronizeAsync` | 695.4 us | 782.8 us |

The launch path is 25% cheaper. The synchronising round trip is **slower**, and is not yet understood.
The callback id was the stated suspect and has been ruled out by measurement - making anonymous
callbacks numeric end to end left it unchanged - so the cause is still open. The honest number is
printed here rather than the flattering half of it.

Both figures are one kernel on one machine, published (not interpreted) - read them against each other,
not as absolutes.

Read the ratios rather than the absolute times. The run above is an interpreted WASM build with no AOT, so
the absolute numbers are much higher than a published app would see, and each case ran once. Run-to-run
variance on the dispatcher cases was a few tens of percent; the held-reference figures were stable.

## Complex return values just work

This is the biggest practical win over Blazor interop, and it is about correctness, not only speed.

When a JS method returns anything that is not a primitive - even a plain object with a single property, or a `Uint8Array` - Blazor's **JS side** must serialize that return value with `JSON.stringify` before handing it back to .NET. It has no choice: JavaScript has no access to the .NET type system, so it cannot know how .NET wants the value marshalled. The result is that many "complex" objects (and they do not have to be complex - a one-property object counts) fail to cross, or lose their real shape, because they do not survive `JSON.stringify`. A `Uint8Array` serializes to nothing useful.

In SpawnDev.BlazorJS this had to be worked around with a `HybridObjectJsonConverter` that walks the .NET object on the JS -> .NET crossing and pulls each value across one at a time through `JsonConverter`s.

**SpawnJS never calls `JSON.stringify` at all.** The return value stays a live `JSObject` handle, and the **.NET side** does every bit of the marshalling through its marshaller graph, reading back exactly the type the caller asked for. A `Uint8Array`, a one-property object, a nested object - all cross intact, because nothing is serialized on the way.

## The core design

`JSImport`/`JSExport` marshalling is frozen at compile time by the source generator - you cannot pick a marshaller at runtime. SpawnJS turns that constraint into its architecture:

1. **Reduce every operation to a few fixed-signature JS primitives.** A small JS class (`SpawnJSInterop`, in `wwwroot/SpawnDev.SpawnJS.lib.module.js`) exposes generic operations - `getProperty`, `setProperty`, `deleteProperty`, `invokeProperty`, `invokePropertyConstructor`, `hasOwnPropertySafe`, and so on - each taking a target object, an identifier, and an argument array. Identifiers support dotted paths (`"a.b.c"`) and null-conditional segments (`"a?.b"`).
2. **Route everything through one dispatcher.** One `JSImport`-bound entry point, `_netToJSCall`, takes a command name plus an offset and length into a shared flat buffer, and invokes the matching primitive. There is no separate async dispatcher: an async command is a **synchronous call that returns a Promise**, which is converted to a `Task` with `then`, so the same path and the same buffer serve both.
3. **Carry references as slots, not proxies.** Javascript holds values in a slot table and .NET holds the integer that addresses them. A `JSObject` is a runtime proxy - creating one allocates a GC handle, a proxy-table entry and an enumerable `Symbol` tag on the object - so proxies are materialised only when something genuinely needs one.
4. **Share one flat buffer.** Arguments are appended and the top unwinds when the call completes, so it behaves as a stack: nothing is allocated per call and only a command name, an offset and a length cross the boundary.
5. **Compose the richness in managed code** via a marshaller graph (see below).

The whole library is one pattern - typed read/write of a `JSObject` array - applied across get / set / call / construct, in both directions, sync and async. Adding async required no new mechanism; it reuses the sync path with an `await`.

The **JS -> .NET direction is the exact mirror**. A single `JSExport` channel - `_JSToNetCall(intent, argsArray)` - carries every inbound call regardless of type or argument count. The `intent` selects what to run and the `argsArray` holds the arguments; the arguments are marshalled **in** through the same marshaller graph and the result is marshalled back **out**, exactly as the outbound path does in reverse. One channel handles all inbound traffic - the variety lives in the array, never in the signature. The intent is a `string` today; it can become an `int` (backed by a message-type `enum`) for a more efficient dispatch key without changing the pattern.

## The marshaller registry

The marshaller registry is the product. Any .NET type is marshalled by the first registered `JSMarshaller` whose `CanMarshal` returns true, scanned in **reverse registration order** so later registrations override earlier ones (SpawnDev's built-ins register first; user overrides register last and win). Resolved marshallers are cached per type.

Two design laws govern every marshaller:

- **Parity by default, performance by opt-in.** The default marshal graph mirrors what `JSON.stringify` would produce, so existing code behaves identically. `List<long>` becomes an ordinary JS number array, **not** a `BigInt64Array`. TypedArrays and `BigInt` are opt-in via explicit .NET wrapper types - never auto-selected because an element "happens to be" a `long` or a `float`. The caller chooses the fast lane by choosing the type.
- **Any type, users bring their own.** Because the transport is a live `JSObject`, a custom marshaller can drop to its own optimal `JSImport`/`JSExport`/`MemoryView`/`HeapView` route with no JSON middleman foreclosing an optimization.

Built-in marshallers include `Default`, `Object`, `IEnumerable`, `ByteArray`, `String`, `Boolean`, `Number`, `Struct`, `JSObject`, `SpawnJSObject`, `SpawnJSObjectReference`, and `JSToNetInvoker`.

## Usage

```csharp
using SpawnDev.SpawnJS;

var JS = new SpawnJSRuntime();

// Construct a JS object: new Uint8Array(100)
using var uint8Array = JS.New("Uint8Array", 100);

// Set a global property: globalThis._uint8 = uint8Array
JS.Set("_uint8", uint8Array);

// Call a method: uint8Array.set([1, 3, 5, 7, 9])
uint8Array.CallVoid("set", new byte[] { 1, 3, 5, 7, 9 });

// Read a property with a target type: uint8Array.length
int length = uint8Array.Get<int>("length");

// Async call: await someObj.someAsyncMethod(arg)
var result = await JS.CallAsync<string>("fetchText", url);
```

The runtime exposes the familiar interop surface: `Get`/`Set`/`Has`/`Delete`, `Call`/`CallVoid`/`CallAsync`/`CallVoidAsync`, `New`/`NewApply`, and their `*Apply` overloads, with strongly-typed generic returns. Set `SpawnJSRuntime.Verbose = true` for step-by-step interop logging to the browser console.

## Argument passing

The call/construct methods (`Call`, `CallVoid`, `CallAsync`, `New`, and so on) are provided as explicit fixed-arity overloads - `arg1`, `arg1, arg2`, up to `arg1 ... arg20` - rather than a single `params object?[] args` method. This is deliberate, and it avoids a silent `params` footgun.

With a `params object?[] args` method, the compiler checks whether the single argument you passed is *itself* assignable to `object?[]`. If it is, the compiler binds it **as** the argument array instead of wrapping it:

```csharp
// Imagine Call had a single `params object?[] args` overload:
obj.Call<T>("fn", "hello");        // args => [ "hello" ]      (wrapped, as expected)
obj.Call<T>("fn", someObjectArray); // args => someObjectArray  (spread! NOT [ someObjectArray ])
obj.Call<T>("fn", someStringArray); // args => spread too - string?[] is covariant to object?[]
```

So passing a single array argument would silently spread into N arguments instead of being one array argument, and the JS side would receive the wrong argument shape - with no compiler warning and no exception. It is even inconsistent by element type: a `string[]` spreads, but an `int[]` (value-type arrays are not covariant to `object?[]`) wraps.

The fixed-arity overloads sidestep this entirely: each one builds `new object?[] { arg1, ... }` internally, so "one array argument" always means exactly one argument.

When you genuinely have a pre-built argument array to pass, use the non-`params` `*Apply` methods, which take the array as a single explicit parameter (no collapse):

```csharp
object?[] args = new object?[] { someObjectArray }; // one argument that is an array
obj.CallApply<T>("fn", args);
obj.CallVoidApply("fn", args);
obj.NewApply("Thing", args);
```

## Documentation

Deeper docs live in [`Docs/`](Docs/README.md):

- [Architecture](Docs/architecture.md) - the core design, the dispatch primitives, and the shared call buffer.
- [Argument passing](Docs/argument-passing.md) - the arity-overload / `Apply` design and the `params` collapse it avoids.
- [Writing marshallers](Docs/writing-marshallers.md) - the `JSMarshaller` contract, resolution order, and registration.
- [Roadmap](Docs/roadmap.md) - current state and what is next.

## Projects

| Project | Purpose |
| --- | --- |
| `SpawnDev.SpawnJS` | The core interop runtime. No Blazor dependency. |
| `SpawnDev.SpawnJS.Blazor` | Blazor-host adapters (`SpawnJSRuntimeBlazor`, `SpawnJSObjectBlazor`). Shells until the core proves out. |
| `WasmBrowserDemo` | Live .NET WASM (non-Blazor) test harness. The current working demo. |
| `BlazorBrowserDemo` | Blazor host demo. |
| `TestsShared` | Shared interop test cases. |

## Status

Early development (version 0.0.1). Working and verified via `WasmBrowserDemo`:

- **.NET -> JS**, sync and async: get, set, delete, call, void call, and construct, with typed returns.
- **Marshaller graph** with per-type caching, reverse-order priority, and null-argument handling.
- **Transport** - a shared flat buffer that unwinds as a stack, so no per-call allocation.
- **Slot-backed references** - `SlotInterop` plus a Javascript slot table; proxies are created lazily.
  Slot keys are allocated monotonically and never reused, so a disposed handle can never resurrect
  another value (locked down by `ReleasedSlotKeyIsNotReusedTest`).
- **Opt-in call counting** (`SpawnJSRuntime.CountCalls`, default off) for attributing interop cost.
- **Verified end to end** by SpawnDev.ILGPU running real WebGPU compute kernels.

In progress / not yet complete:

- **JS -> .NET dispatch** (`_JSToNetCall`): the single inbound channel now fires - a managed method registered as a JS-callable function is reached from JavaScript, carrying an `intent` plus an args array (the mirror of the outbound path). Inbound argument marshalling and intent dispatch are still being built out: reading arguments back through the same marshaller graph in reverse and marshalling the result out (the symmetric form of the SpawnDev.BlazorJS `Callback` pattern). A later refinement can swap the `string` intent for an `int`/`enum` dispatch key.
- **Opt-in marshallers** for TypedArrays and `BigInt`.
- **Blazor adapters** are placeholder shells pending core validation.

## The SpawnDev Crew

- **LostBeard** (Todd Tanner) - Captain, library author, keeper of the vision
- **Riker** - First Officer, implementation lead on consuming projects
- **Data** - Operations Officer, deep-library work, test rigor, root-cause analysis
- **Tuvok** - Security/Research Officer, design planning, documentation, code review
- **Geordi** - Chief Engineer, library internals, GPU kernels, backend work
- **Seven** - Wasm backend, GPU kernels, fail-loud verification
