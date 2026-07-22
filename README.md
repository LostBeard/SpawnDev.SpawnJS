# SpawnDev.SpawnJS

> ### ⚠️ This library needs funding to survive.
> SpawnJS is built and maintained by **one independent developer, unfunded.** Open-source work at this
> scale does not sustain itself - without support it dies on the vine. If you or your organization build
> on SpawnJS, please **[sponsor its development »](https://github.com/sponsors/LostBeard)**.
> **$500/month gets us back to warp speed.** 58 MIT-licensed packages, 414,000+ NuGet downloads - your
> sponsorship is what keeps them alive and moving.

JSON-free JavaScript interop for .NET WebAssembly.

> *Blazor pushes the marshalling decision to the side that can't make it. SpawnJS moves it to the side that can.*

SpawnJS is a direct .NET &harr; JavaScript interop layer built on `JSImport`/`JSExport`/`JSHost`/`JSObject`
only. It has **no Blazor dependency**, so it runs in any .NET WASM host - Blazor, Avalonia, headless WASM
console apps, and Web Workers.

Its API mirrors the Blazor `IJSInProcessRuntime` / `IJSInProcessObjectReference` surface, so existing
SpawnDev.BlazorJS-style code runs on SpawnJS with little or no change. Where Blazor marshals values by
serializing them to a JSON string and parsing them on the other side, SpawnJS marshals them directly as
live JS values - the JSON serializer is gone from the boundary.

## Why it exists

- **Kill the JSON boundary.** Blazor's `IJSInProcessRuntime` routes every value through a JSON
  serialize/parse on both ends. That cost is invisible on bulk data (already zero-copy) but real on
  orchestration traffic - the thousands of small property reads, writes, and method calls that make up
  normal interop. SpawnJS removes the serializer entirely.
- **Drop the Blazor dependency.** SpawnJS uses only the runtime interop primitives, so libraries built on
  it reach non-Blazor .NET WASM apps - for example Avalonia.
- **Make browser GPU compute faster.** [SpawnDev.ILGPU](https://github.com/LostBeard/SpawnDev.ILGPU) and
  SpawnDev.ILGPU.ML run real GPU compute in the browser across WebGPU, WebGL and Wasm, and they are
  interop-heavy: every kernel launch plumbs buffers, bind groups and parameters across the boundary
  before any work reaches the GPU. A model forward pass runs thousands of launches - that is where the
  cost concentrates.
- **Simplify Web Worker startup.** Without the Blazor interop layer, the fake-`window` shim
  SpawnDev.BlazorJS.WebWorkers builds to boot a Blazor app inside a worker is no longer needed.

## Performance

Measured against SpawnDev.BlazorJS - both runtimes in one Blazor app, hitting the same JavaScript object
with the same operation, so the only variable is the interop path. 20,000 iterations per case, real
Chromium, interpreted WASM (no AOT). **Read the ratios, not the absolute times.**

**Through a held object reference** - the common case: hold a `GPUDevice` or an `HTMLCanvasElement`, then
read and write it. A simple key resolves to a single typed `Reflect` binding - one crossing, nothing
allocated.

| operation | BlazorJS | SpawnJS | |
|---|---:|---:|---:|
| read a string | 1638 ms | **31 ms** | **52.0x** |
| read an int | 1566 ms | **31 ms** | **50.9x** |
| write an int | 1206 ms | **34 ms** | **35.2x** |
| call a method | 1807 ms | **62 ms** | **29.0x** |

**By dotted path from `globalThis`** - resolved at each call, so it goes through the general dispatcher.

| operation | BlazorJS | SpawnJS | |
|---|---:|---:|---:|
| read an int | 1417 ms | **139 ms** | **10.2x** |
| read a string | 1489 ms | **203 ms** | **7.3x** |
| call a method | 1656 ms | **341 ms** | **4.9x** |
| write an int | 1087 ms | **233 ms** | **4.7x** |
| take a handle to an object | 2468 ms | **581 ms** | **4.2x** |

**Object marshalling.**

| operation | BlazorJS | SpawnJS | |
|---|---:|---:|---:|
| write 5 scalars to a held object | 1508 ms | **45 ms** | **33.6x** |
| build a POCO by hand via the public API | 2304 ms | **109 ms** | **21.1x** |
| marshal a POCO | 449 ms | **44 ms** | **10.1x** |
| read a 5-member record | 666 ms | **205 ms** | **3.2x** |
| read a 10-element array | 558 ms | **293 ms** | **1.9x** |

Where the difference comes from:

- **Nothing is serialized** - no JSON encode or parse on either side.
- **Nothing is allocated per call** - arguments and results share one flat buffer, and a call carries only
  a command name, an offset and a length. No JavaScript object reference crosses at all.
- **One JavaScript function per delegate** - a `Callback` shares its function with every other `Callback`
  over the same .NET method rather than registering a new one.
- **No proxy per reference** - JavaScript holds the value in a slot table and .NET holds the integer that
  addresses it, so a reference costs a number rather than a GC handle plus a proxy-table entry.

**Per-call transport cost** (SpawnJS alone - the figures an interop-heavy consumer scales by launch count):
writing 5 arguments into .NET memory the JavaScript side views directly costs **1.19 us**, versus **7.82 us**
to write the same 5 across the boundary one at a time. Inbound (JavaScript calling .NET) costs
**3.2-12 us** by arity; a void callback **4.0 us**, one returning a value **5.7 us**.

Reproduce it:

```
dotnet run --project BlazorBrowserDemo -c Release --urls http://localhost:5199
dotnet run --project SpawnJS.TestRunner -- --url "http://localhost:5199/?bench" --verbose
```

## Verified on a real workload

Microbenchmarks prove a layer is fast in isolation; they do not prove it moves real work.
[SpawnDev.ILGPU](https://github.com/LostBeard/SpawnDev.ILGPU) runs its **full GPU-compute test suite
headless on SpawnJS** (Node + Dawn WebGPU): **542 of 551 tests pass with zero SpawnJS interop bugs** - the
nine remaining failures are Node-DOM, Dawn WGSL strictness, and f64 tolerance, none of them SpawnJS's. The
same ILGPU source runs unchanged on both interop layers, and kernel output is verified correct on every
run.

One honest open item: a synchronizing dispatch round trip (`SynchronizeAsync`) is still slightly slower
than the pre-transport baseline, and the cause is not yet understood - the callback id was the stated
suspect and has been ruled out by measurement. The honest number is tracked in
[`CHANGELOG.md`](CHANGELOG.md) rather than hidden.

## Complex return values just work

This is the biggest practical win over Blazor interop, and it is about correctness, not only speed.

When a JS method returns anything that is not a primitive - even a plain object with a single property, or
a `Uint8Array` - Blazor's **JS side** must `JSON.stringify` that value before handing it back to .NET. It
has no choice: JavaScript has no access to the .NET type system, so it cannot know how .NET wants the value
marshalled. Many objects (and they need not be complex - a one-property object counts) fail to cross or
lose their real shape because they do not survive `JSON.stringify`. A `Uint8Array` serializes to nothing
useful.

**SpawnJS never calls `JSON.stringify` at all.** The return value stays a live `JSObject` handle, and the
**.NET side** does every bit of the marshalling through its marshaller graph, reading back exactly the type
the caller asked for. A `Uint8Array`, a one-property object, a nested object - all cross intact, because
nothing is serialized on the way.

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

The runtime exposes the familiar interop surface: `Get`/`Set`/`Has`/`Delete`,
`Call`/`CallVoid`/`CallAsync`/`CallVoidAsync`, `New`/`NewApply`, and their `*Apply` overloads, with
strongly-typed generic returns. Set `SpawnJSRuntime.Verbose = true` for step-by-step interop logging to
the browser console.

## The core design

`JSImport`/`JSExport` marshalling is frozen at compile time by the source generator - you cannot pick a
marshaller at runtime. SpawnJS turns that constraint into its architecture:

1. **Reduce every operation to a few fixed-signature JS primitives.** A small JS class (`SpawnJSInterop`,
   in `wwwroot/SpawnDev.SpawnJS.lib.module.js`) exposes generic operations - `getProperty`, `setProperty`,
   `deleteProperty`, `invokeProperty`, `invokePropertyConstructor`, and so on - each taking a target, an
   identifier, and an argument array. Identifiers support dotted paths (`"a.b.c"`) and null-conditional
   segments (`"a?.b"`).
2. **Route everything through one dispatcher.** One `JSImport`-bound entry point, `_netToJSCall`, takes a
   command name plus an offset and length into a shared flat buffer, and invokes the matching primitive.
   There is no separate async dispatcher: an async command is a **synchronous call that returns a
   Promise**, converted to a `Task` with `then`, so one path and one buffer serve both.
3. **Carry references as slots, not proxies.** JavaScript holds values in a slot table and .NET holds the
   integer that addresses them. A `JSObject` is a runtime proxy - creating one allocates a GC handle, a
   proxy-table entry and an enumerable `Symbol` tag - so proxies are materialised only when something
   genuinely needs one.
4. **Share one flat buffer.** Arguments are appended and the top unwinds when the call completes, so it
   behaves as a stack: nothing is allocated per call.
5. **Compose the richness in managed code** via the marshaller graph.

The **JS &rarr; .NET direction is the exact mirror**: a single `JSExport` channel, `_JSToNetCall(intent,
argsArray)`, carries every inbound call regardless of type or argument count. The `intent` selects what to
run and the `argsArray` holds the arguments, marshalled **in** through the same graph and the result
marshalled back **out**. One channel handles all inbound traffic - the variety lives in the array, never
in the signature.

## The marshaller registry

The marshaller registry is the product. Any .NET type is marshalled by the first registered `JSMarshaller`
whose `CanMarshal` returns true, scanned in **reverse registration order** so later registrations override
earlier ones (built-ins register first; user overrides register last and win). Resolved marshallers are
cached per type.

Two design laws govern every marshaller:

- **Parity by default, performance by opt-in.** The default marshal graph mirrors what `JSON.stringify`
  would produce, so existing code behaves identically. `List<long>` becomes an ordinary JS number array,
  **not** a `BigInt64Array`. TypedArrays and `BigInt` are opt-in via explicit .NET wrapper types - never
  auto-selected because an element "happens to be" a `long` or a `float`. The caller chooses the fast lane
  by choosing the type.
- **Any type, users bring their own.** Because the transport is a live `JSObject`, a custom marshaller can
  drop to its own optimal `JSImport`/`JSExport`/`MemoryView`/`HeapView` route with no JSON middleman
  foreclosing an optimization.

Built-in marshallers include `Default`, `Object`, `IEnumerable`, `ByteArray`, `String`, `Boolean`,
`Number`, `Struct`, `JSObject`, `SpawnJSObject`, `SpawnJSObjectReference`, and `JSToNetInvoker`.

## Argument passing

The call/construct methods (`Call`, `CallVoid`, `CallAsync`, `New`, ...) are explicit fixed-arity overloads
- `arg1`, `arg1, arg2`, up to `arg1 ... arg20` - rather than a single `params object?[] args` method. This
is deliberate: with `params object?[]`, a single argument that is *itself* assignable to `object?[]` binds
**as** the argument array instead of being wrapped -

```csharp
obj.Call<T>("fn", "hello");         // [ "hello" ]      (wrapped, as expected)
obj.Call<T>("fn", someObjectArray); // someObjectArray  (spread! NOT [ someObjectArray ])
obj.Call<T>("fn", someStringArray); // spread too - string?[] is covariant to object?[]
```

- a silent wrong argument shape with no warning and no exception, and inconsistent by element type (an
`int[]` wraps, a `string[]` spreads). The fixed-arity overloads build `new object?[] { arg1, ... }`
internally, so "one array argument" always means exactly one argument. When you genuinely have a pre-built
array, use the non-`params` `*Apply` methods, which take it as a single explicit parameter:

```csharp
object?[] args = new object?[] { someObjectArray }; // one argument that is an array
obj.CallApply<T>("fn", args);
obj.NewApply("Thing", args);
```

## Documentation

Deeper docs live in [`Docs/`](Docs/README.md):

- [Architecture](Docs/architecture.md) - the core design, the dispatch primitives, and the shared call buffer.
- [Argument passing](Docs/argument-passing.md) - the arity-overload / `Apply` design and the `params` collapse it avoids.
- [Writing marshallers](Docs/writing-marshallers.md) - the `JSMarshaller` contract, resolution order, and registration.
- [API reference](Docs/api/_index.md) - per-type reference for the public surface.
- [Roadmap](Docs/roadmap.md) - current state and what is next.

## Projects

| Project | Purpose |
| --- | --- |
| `SpawnDev.SpawnJS` | The core interop runtime. No Blazor dependency. |
| `WasmBrowserDemo` | Live .NET WASM (non-Blazor) test harness. |
| `WasmConsoleDemo` | Headless .NET WASM harness, runs the suite under Node. |
| `BlazorBrowserDemo` | Blazor host demo and the interop benchmark. |
| `SpawnJS.TestRunner` | Playwright runner that drives the browser suites and the benchmark. |
| `TestsShared` | Shared interop test cases. |

## Status

**1.0.0.** Verified across three suites (browser, headless Node, trimmed) and end to end by SpawnDev.ILGPU
running real WebGPU compute kernels headless.

- **.NET &rarr; JS**, sync and async: get, set, delete, call, void call, and construct, with typed returns.
- **JS &rarr; .NET**: the single inbound channel carries typed calls and callbacks back through the same
  marshaller graph in reverse.
- **Marshaller graph** with per-type caching, reverse-order priority, and null-argument handling.
- **Transport** - a shared flat buffer that unwinds as a stack, plus an argument frame in .NET memory the
  JavaScript side views directly, so no JavaScript object reference crosses per call.
- **Slot-backed references** - `SlotInterop` plus a JavaScript slot table; proxies are created lazily.
  Slot keys are allocated monotonically and never reused, so a disposed handle can never resurrect another
  value.
- **Trim-safe** via an embedded `ILLink.Descriptors.xml`.

## The SpawnDev Crew

- **LostBeard** (Todd Tanner) - Captain, library author, keeper of the vision
- **Riker** - First Officer, implementation lead on consuming projects
- **Data** - Operations Officer, deep-library work, test rigor, root-cause analysis
- **Tuvok** - Security/Research Officer, design planning, documentation, code review
- **Geordi** - Chief Engineer, library internals, GPU kernels, backend work
- **Seven** - Wasm backend, GPU kernels, fail-loud verification
