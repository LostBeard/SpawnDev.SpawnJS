> ### ⚠️ This library needs funding to survive.
> SpawnJS is built and maintained by **one independent developer.** If you or your organization build
> on SpawnJS, please **[sponsor its development](https://github.com/sponsors/LostBeard)**.
> **$500/month gets us back to warp speed.** 68 MIT-licensed packages, 500,000+ NuGet downloads - your
> sponsorship is what keeps them alive and moving.

# SpawnDev.SpawnJS

[![NuGet](https://img.shields.io/nuget/dt/SpawnDev.SpawnJS.svg?label=SpawnDev.SpawnJS)](https://www.nuget.org/packages/SpawnDev.SpawnJS)
[![NuGet](https://img.shields.io/nuget/dt/SpawnDev.SpawnJS.Blazor.svg?label=SpawnDev.SpawnJS.Blazor)](https://www.nuget.org/packages/SpawnDev.SpawnJS.Blazor)

JSON-free JavaScript interop for **.NET WebAssembly**. No Blazor dependency. Targets **.NET 10**.

SpawnJS is the next-generation successor to [SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS) (first published December 2022, 198,000+ NuGet downloads). SpawnJS 1.0.0 shipped July 2026 and already has 9,000+ downloads. Version **2.x** rewrote the core: marshalling lives in a managed `JSMarshaller` graph, and the transport is `JSImport` / `JSExport` plus a numeric object table. Microsoft's `JSObject` and `JSHost` are not used as the interop handle. Version **3.x** makes every call **one crossing**: arguments, POCOs, arrays and the result travel in a call tape in .NET memory instead of one boundary crossing per value, and POCO codecs are generated at compile time.

It runs in any .NET WASM host: Blazor, Avalonia, Web Workers, and a headless .NET WASM console app under Node with no browser and no DOM. Two or more SpawnJS apps can share one page without conflicts.

```csharp
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;

// JS comes from DI in a real app (see Setup). Instance is the same singleton.
var JS = SpawnJSRuntime.Instance;

using var bytes = JS.New<int, Uint8Array>("Uint8Array", 100);
JS.Set("_uint8", bytes);
bytes.CallVoid<byte[]>("set", new byte[] { 1, 3, 5, 7, 9 });
int length = bytes.Get<int>("length");

using var canvas = JS.Call<string, HTMLCanvasElement>("document.getElementById", "myCanvas");
```

`Get` reads a property. `Call` invokes a method. Do not embed a call in a property path: `JS.Get<T>("document.getElementById('id')")` looks up a property of that literal name.

## Why SpawnJS

**Blazor's `IJSInProcessRuntime` JSON-serializes every crossing.** JavaScript has no access to the .NET type system, so a non-primitive return - a one-property object, a `Uint8Array` - must survive `JSON.stringify` on the JS side. Many values do not. SpawnJS never puts the default path through JSON. The value stays live in a JS table; **.NET** chooses the marshaller from the declared (or boxed) type.

**SpawnDev.BlazorJS is not going away**, but it rides that JSON boundary by design. SpawnJS is the library you want when the workload is interop-dense: GPU bind groups, DOM, workers, typed arrays, callbacks.

**Microsoft `JSObject` is not used as a handle.** Three reasons, all measured or observed in production:

1. **Creation and lookup are slow** - surprising, and enough on its own for a hot interop path.
2. **Symbol tagging** makes some objects incompatible with browser APIs that reject tagged values.
3. **Shared handles and dispose** - two wrappers can name the same runtime handle, so disposing one poisons the other.

The one exception: `JSHost.DotnetInstance` is handed to `SpawnJSInterop._registerInstance` once so JS can call back into this .NET instance and reach WASM memory. After that, every JS value is an integer id in `SpawnJSInterop.spawnJSObjects`. Ids are monotonic and never reused, so a disposed handle cannot resurrect another value.

**Trim-friendly.** The package embeds `ILLink.Descriptors.xml`, and its source generator writes a codec for each POCO you send or read, so the trimmer sees every member used. In one test app, a trimmed publish dropped from ~30 MB to ~9 MB and the suite still passed. BlazorJS's JSON-serialization interop is not trim-friendly this way.

**Headless.** SpawnDev.ILGPU runs its GPU-compute suite on SpawnJS in a .NET WASM console app under Node, driving real WebGPU through [`@kmamal/gpu`](https://www.npmjs.com/package/@kmamal/gpu) (prebuilt Dawn). Same ILGPU source on both interop layers.

## Install

```
dotnet add package SpawnDev.SpawnJS
```

Blazor host extras (`ElementReference.As<T>()`, `ElementRef<T>`, `SpawnJSRunAsync`):

```
dotnet add package SpawnDev.SpawnJS.Blazor
```

Current versions: **SpawnDev.SpawnJS 3.0.0**, **SpawnDev.SpawnJS.Blazor 3.0.0**. See [`CHANGELOG.md`](CHANGELOG.md). Coming from 2.x: the call surface is unchanged; only a custom `JSMarshaller` needs porting ([Writing marshallers](Docs/writing-marshallers.md)).

## Setup

Most apps use **dependency injection**. `AddSpawnJSRuntime()` registers `SpawnJSRuntime` as a singleton, plus `IBackgroundServiceManager`. Full detail: [Hosting](Docs/hosting.md).

**Non-Blazor WASM** - `SpawnJSAppBuilder` in the core package (workers, extensions, RazorRenderer, console WASM):

```csharp
var builder = SpawnJSAppBuilder.CreateDefault(args, out var JS);
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(JS.AppBaseUri) });
await builder.Build().RunAsync();
```

`RunAsync()` starts `IBackgroundService` / `IAsyncBackgroundService` and stays alive until `SpawnJSApp.Exit()`. Downstream packages hang extra surface on the same builder (`RootComponents` from RazorRenderer, `AddWebWorkerService()`, ...).

**Blazor** - `WebAssemblyHostBuilder` plus `SpawnDev.SpawnJS.Blazor`:

```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.Services.AddSpawnJSRuntime(out var JS);
await builder.Build().SpawnJSRunAsync();
```

```csharp
[Inject] SpawnJSRuntime JS { get; set; }   // in a component; not SpawnJSRuntime.Instance
```

`SpawnJSRunAsync` starts background services, then `WebAssemblyHost.RunAsync` in a window scope. In a worker it does not call `RunAsync`.

Any other `IServiceCollection` can call `AddSpawnJSRuntime()` the same way. `SpawnJSRuntime.Instance` is the same singleton and is what the live test suite (`SpawnDev.SpawnJS.Demo`) uses; prefer the container in application `Program.cs`.

### Blazor elements

Give the element an `id` and fetch it with `Call`, or use the Blazor package:

```razor
<canvas @ref="_canvas"></canvas>
@code {
    ElementRef<HTMLCanvasElement> _canvas;
    async Task Draw()
    {
        using var canvas = _canvas.Get();   // null until first render
        // ...
    }
}
```

`ElementRef<T>` stores the `ElementReference` (a struct) and allocates a live slot only when you call `Get()`. Dispose that wrapper. `@ref` re-captures on re-render; converting at capture time would leak slots.

## Call surface

On `SpawnJSRuntime` and `SpawnJSObjectReference`: `Get` / `Set` / `Has` / `Delete`, `Call` / `CallVoid` / `CallAsync` / `CallVoidAsync`, `New` / `NewApply`, and the `*Apply` forms. Keys may be a `string`, `int`, or `double`. String keys support dotted paths (`"a.b.c"`) and null-conditional segments (`"a?.b"`).

Generic type arguments are **argument types first, return type last**:

```csharp
JS.Call<string, string>("btoa", "Hello");
JS.New<int, Uint8Array>("Uint8Array", 100);
await JS.CallAsync<string, Response>("fetch", url);
```

`CallVoid` and `New` (untyped) infer argument types. Overloads go to **10 arguments**. A pre-built array uses `*Apply` - never `params object?[]`. See [Argument passing](Docs/argument-passing.md).

`JS.Verbose = true` logs marshaller selection and JS-side interop to the console.

`JS.AppBaseUri` is the URL this app was loaded from (origin of `_framework` / the entry script, trailing slash). Use it instead of `document.baseURI` when the app is served from a CDN at a different path than the host page. Correct in window and worker scopes. Empty string on a non-browser host.

Handles are **manual lifetime**. `spawnJSObjects[id]` is a strong JS reference nothing collects. `using` / `Dispose` the wrapper or the slot leaks.

## How it works

`[JSImport]` / `[JSExport]` marshalling is frozen at compile time, and every crossing of the .NET/JS boundary has a fixed cost. SpawnJS 3.x crosses **once per call**, whatever the arguments are.

1. **Slots, not proxies.** JS values live in `SpawnJSInterop.spawnJSObjects`; .NET holds a `double` id. Negative ids are sentinels (`globalThis`, `undefined`, `null`, the table, `SpawnJSInterop`).
2. **The call tape.** A call is written into a per-instance tape of 8-byte cells in pinned .NET memory: the method, then each argument as a tagged value - numbers, strings (UTF-16), slot ids, callbacks, whole POCOs, number arrays copied in bulk. One `JSImport` hands JS the address; JS reads it through heap views and runs the primitive (`propertyGet`, `propertySet`, `propertyCall`, `propertyNew`, ...).
3. **Shapes and schemas.** A POCO's member names cross once per runtime (a *shape*), then each object is just its values. The result is written back by a *schema* - a POCO, array, list, dictionary or tuple comes back in the same crossing.
4. **Callbacks.** JS -> .NET calls (events, promises, `Action` / `Func`) write their arguments into .NET memory before .NET is called, so the handler reads nothing back across the boundary.
5. **Marshaller graph.** A registry of `JSMarshaller`s (`Write` / `Schema` / `Read`) decides how each .NET type crosses; later registrations win. POCO codecs are generated at compile time (`[SpawnJSPoco]` marks types only the running code knows). See [Architecture](Docs/architecture.md) and [Writing marshallers](Docs/writing-marshallers.md).

Every piece of state is per .NET instance; the only page global is the `SpawnJSInterop` class, so two SpawnJS apps can share a page.

Default marshalling matches what `JSON.stringify` would produce: `List<long>` is a JS number array, not a `BigInt64Array`. Typed arrays and `BigInt` are opt-in via wrapper types (`Uint8Array`, `BigInt`, `HeapView`). `byte[]` is the exception: it writes as a `Uint8Array` copy. `JsonElement` is the one type whose marshaller uses `JSON.stringify` / `JSON.parse` on purpose.

## Performance

**3.0 against 2.x.** Published Release, Chrome, interpreted WASM, microseconds per call (best of 3 runs), with the exact number of .NET/JS crossings. Reproduce with `dotnet run --project SpawnJS.TestRunner -- --bench`.

| call | 2.1.20 | crossings | 3.0.0 | crossings | |
|---|---:|---:|---:|---:|---:|
| read a number from a held object | 3.15 us | 3 | **1.90 us** | 1 | 1.7x |
| call, no arguments | 3.17 us | 3 | **1.66 us** | 1 | 1.9x |
| call, 5 mixed arguments | 10.5 us | 11 | **2.75 us** | 1 | 3.8x |
| call with a 16-member POCO | 98.0 us | 55 | **6.68 us** | 1 | 14.7x |
| call with a `GPUBindGroupDescriptor` (3 entries) | 165 us | 104 | **8.91 us** | 1 | 18.6x |
| call with `string[8]` | 38.2 us | 38 | **4.18 us** | 1 | 9.1x |
| call with `int[1000]` | 3,870 us | 4,006 | **15.4 us** | 1 | 251x |
| call with `double[1000]` | 3,897 us | 4,006 | **19.1 us** | 1 | 204x |
| call with `byte[4096]` | 7.05 us | 4 | **5.60 us** | 1 | 1.3x |
| return a 16-member POCO | 155 us | 74 | **6.74 us** | 1 | 23.1x |
| return `int[1000]` | 3,100 us | 3,005 | **3.29 us** | 1 | 942x |
| JS calls .NET with (number, string, POCO) | 30.6 us | 4 | **5.39 us** | 1 | 5.7x |

In SpawnDev.ILGPU on WebGPU, the same library code on each version: `createBindGroup` 202 us -> 38 us, and an unbatched kernel dispatch 251 us -> 80 us of host time.

**2.x against SpawnDev.BlazorJS.** Measured in one Blazor app, same JS object, same operation. 20,000 iterations, Chromium, interpreted WASM (no AOT). Read the **ratios**. These rows were measured on 2.x and not re-run against 3.0.

**Held object reference** (hold a `GPUDevice` or `HTMLCanvasElement`, then read/write it):

| operation | BlazorJS | SpawnJS | |
|---|---:|---:|---:|
| read a string | 1638 ms | **31 ms** | **52.0x** |
| read an int | 1566 ms | **31 ms** | **50.9x** |
| write an int | 1206 ms | **34 ms** | **35.2x** |
| call a method | 1807 ms | **62 ms** | **29.0x** |

**Dotted path from `globalThis`:**

| operation | BlazorJS | SpawnJS | |
|---|---:|---:|---:|
| read an int | 1417 ms | **139 ms** | **10.2x** |
| read a string | 1489 ms | **203 ms** | **7.3x** |
| call a method | 1656 ms | **341 ms** | **4.9x** |
| write an int | 1087 ms | **233 ms** | **4.7x** |
| take a handle to an object | 2468 ms | **581 ms** | **4.2x** |

**Object marshalling:**

| operation | BlazorJS | SpawnJS | |
|---|---:|---:|---:|
| write 5 scalars to a held object | 1508 ms | **45 ms** | **33.6x** |
| build a POCO by hand via the public API | 2304 ms | **109 ms** | **21.1x** |
| marshal a POCO | 449 ms | **44 ms** | **10.1x** |
| read a 5-member record | 666 ms | **205 ms** | **3.2x** |
| read a 10-element array | 558 ms | **293 ms** | **1.9x** |

Where that comes from: no JSON on the default path, integer slot ids instead of `JSObject` proxy tables, and one JS function per .NET method for `Callback` / `Action` / `Func` (reuse by delegate identity) - and in 3.x, one crossing per call.

On a real GPU-compute suite (SpawnDev.ILGPU, headless Node + WebGPU): **542 of 551 tests passed with zero SpawnJS interop bugs**. The remaining nine were Node-DOM, Dawn WGSL strictness, and f64 tolerance.

## Documentation

| Page | |
|---|---|
| [Hosting](Docs/hosting.md) | DI: `SpawnJSAppBuilder`, Blazor `WebAssemblyHost`, `IServiceCollection` |
| [Architecture](Docs/architecture.md) | The call tape, shapes and schemas, slots, inbound callbacks, generated codecs |
| [Argument passing](Docs/argument-passing.md) | Fixed-arity overloads vs `params` |
| [Writing marshallers](Docs/writing-marshallers.md) | `JSMarshaller` contract (`Write` / `Schema` / `Read`) and registration |
| [API reference](Docs/api/_index.md) | Per-type reference for the JS wrappers |
| [v3 design](Docs/v3-design.md) | Why 3.x crosses once per call, with the measurements behind it |
| [Roadmap](Docs/roadmap.md) | Current 3.x state |

## This repo

| Project | Purpose |
|---|---|
| `SpawnDev.SpawnJS` | Core runtime. No Blazor dependency. |
| `SpawnDev.SpawnJS.Generators` | POCO codec source generator, shipped inside the `SpawnDev.SpawnJS` package |
| `SpawnDev.SpawnJS.Blazor` | Blazor host: `ElementReference`, `ElementRef<T>`, `SpawnJSRunAsync` |
| `SpawnDev.SpawnJS.Demo` | Live suite (`UnitTests/`) and scratch host |
| `SpawnDev.SpawnJS.Blazor.Demo` | Blazor host demo |
| `SpawnJS.TestRunner` | Playwright runner for the live suite |

```
dotnet run --project SpawnJS.TestRunner
dotnet run --project SpawnJS.TestRunner -- IsSameEntry
dotnet run --project SpawnJS.TestRunner -- --headed --verbose
dotnet run --project SpawnJS.TestRunner -- --nocodecs    # every POCO by reflection instead of generated codecs
dotnet run --project SpawnJS.TestRunner -- --twin        # two .NET runtimes in one page
dotnet run --project SpawnJS.TestRunner -- --bench       # the interop benchmark, with crossing counts
```

The runner builds and serves `SpawnDev.SpawnJS.Demo` with `?tests=[filter]` and parses `TEST:` / `RESULTS:` console lines. Navigate on `DOMContentLoaded`, not `NetworkIdle`.

## Family

Built on SpawnJS (browser) or the matching BlazorJS package (Blazor JSON interop):

- [SpawnDev.SpawnJS.WebWorkers](https://www.nuget.org/packages/SpawnDev.SpawnJS.WebWorkers) - workers and service workers without a fake-`window` Blazor boot shim
- [SpawnDev.SpawnJS.RazorRenderer](https://www.nuget.org/packages/SpawnDev.SpawnJS.RazorRenderer) - Razor UI on `SpawnJSAppBuilder` (no Blazor `WebAssemblyHost`)
- [SpawnDev.ILGPU](https://github.com/LostBeard/SpawnDev.ILGPU) / [SpawnDev.ILGPU.ML](https://github.com/LostBeard/SpawnDev.ILGPU.ML) - GPU compute and ML in the browser and on desktop
- [SpawnDev.WebTorrent](https://www.nuget.org/packages/SpawnDev.WebTorrent), [SpawnDev.RTC](https://www.nuget.org/packages/SpawnDev.RTC)

## The SpawnDev Crew

- **LostBeard** (Todd Tanner) - Captain, library author, keeper of the vision
- **Riker** - First Officer, implementation lead on consuming projects
- **Data** - Operations Officer, deep-library work, test rigor, root-cause analysis
- **Tuvok** - Security/Research Officer, design planning, documentation, code review
- **Geordi** - Chief Engineer, library internals, GPU kernels, backend work
- **Seven** - Wasm backend, GPU kernels, fail-loud verification
