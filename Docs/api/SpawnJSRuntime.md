# SpawnJSRuntime

**Namespace:** `SpawnDev.SpawnJS`  
**Inheritance:** `SpawnJSObjectReference` (id = `globalThis`)  
**Source:** `SpawnJSRuntime.cs` and partials (`Interop`, `Imports`, `KeyString`, `NewJSArray`, ...)

> Process-wide singleton for .NET to JavaScript interop. It does **not** wrap Blazor's `IJSInProcessRuntime`. Constructed on first `Instance` access (constructor is private). Application code gets it from DI (`AddSpawnJSRuntime` / `SpawnJSAppBuilder`). In a Blazor component, **inject** it. `Instance` is for library internals, tests, and hosts with no container.

## Setup

Most apps: [Hosting](../hosting.md). Short version:

```csharp
// Non-Blazor WASM
var builder = SpawnJSAppBuilder.CreateDefault(args, out var JS);
await builder.Build().RunAsync();

// Blazor (SpawnDev.SpawnJS.Blazor)
builder.Services.AddSpawnJSRuntime(out var JS);
await builder.Build().SpawnJSRunAsync();

[Inject] SpawnJSRuntime JS { get; set; }
```

## Properties

| Property | Type | Description |
|---|---|---|
| `Instance` | `SpawnJSRuntime` | Process-wide singleton. Created on first access. |
| `IsCreated` | `bool` | True after the singleton exists. |
| `Verbose` | `bool` | Instance flag. Also sets `SpawnJSInterop.verbose`. |
| `Marshallers` | `IList<JSMarshaller>` | Registry. Later entries win. See [writing-marshallers](../writing-marshallers.md). |
| `AppBaseUri` | `string` | URL this app was loaded from, trailing slash. Empty if unknown. |
| `InstanceId` | `string` | Hex id generated at startup. |
| `IsBrowser` | `bool` | `OperatingSystem.IsBrowser()` - true for Node WASM console too. Use `IsWindow` for a page. |
| `GlobalScopeName` | `string` | `constructor.name` of `globalThis`. |
| `GlobalScope` | `GlobalScope` | Enum for the current scope. |
| `GlobalThis` | `SpawnJSObject?` | Typed `globalThis`. |
| `WindowThis` / `DedicateWorkerThis` / `SharedWorkerThis` / `ServiceWorkerThis` | typed or null | Set when `globalThis` is that scope. |
| `IsWindow` / `IsWorker` / `IsDedicatedWorkerGlobalScope` / `IsSharedWorkerGlobalScope` / `IsServiceWorkerGlobalScope` | `bool` | |
| `DotnetInstance` | `SpawnJSObjectReference` | This app's runtime instance held on the JS side. |
| `HeapSize` | `long` | Last reported WASM heap size. |
| `OnHeapGrow` | `Action<long, long>?` | Fired on heap detach/grow. |

## Interop (inherited, keys on `globalThis`)

String keys support dotted paths and `?.`. Generic args are **argument types then return type**. Overloads: 0..10 args, plus `*Apply`.

| Method | Description |
|---|---|
| `Get<T>(key)` / `Get(key)` | Property. `Get` without `T` returns `SpawnJSObjectReference?`. |
| `Set<T>(key, value)` | Property write. |
| `Has(key)` / `Delete(key)` | `in` / `delete`. |
| `Call<...T>(key, ...)` / `CallVoid<...>(key, ...)` | Invoke a function. |
| `CallAsync<...T>` / `CallVoidAsync` | Same, await a Promise. |
| `New` / `New<T>` / `NewApply` | `new` a constructor. Untyped `New` returns `SpawnJSObjectReference`. |
| `TypeOf()` / `TypeOf(key)` / `ConstructorName()` / `ConstructorName(key)` / `ConstructorNames()` | JS type info; `ConstructorNames` is the prototype chain, most derived first. |

```csharp
var height = JS.Get<int>("window.innerHeight");
var userAgent = JS.Get<string>("navigator.userAgent");
var size = JS.Get<int?>("fruit.options?.size");
JS.Set("myApp.config.debug", true);

JS.CallVoid<string>("console.log", "Hello from .NET");
var encoded = JS.Call<string, string>("btoa", "Hello");
using var response = await JS.CallAsync<string, Response>("fetch", "/api/data");
using var audio = JS.New<string, Audio>("Audio", "song.mp3");
```

## Other methods on the runtime

| Method | Description |
|---|---|
| `GetMarshaller<T>()` / `GetMarshaller(Type)` | Resolve and cache a marshaller. |
| `GetDocument()` | `document` as `Document?`. |
| `Fetch(...)` | `fetch` overloads (`string`, `Request`, `FetchOptions`). |
| `Import` / `Import<T>` | Dynamic `import()`. Optional global name to cache the module. |
| `LoadScript` / `LoadScripts` | `<script>` in a window, `importScripts` in a worker. |
| `Log` / `LogError` | `console.log` / `console.error`, or `Console` when not browser. |
| `ReturnAs<TIn, T>(value)` | Identity round-trip through JS as type `T` (`returnMe`). |
| `ObjectEquals<T1, T2>(a, b, full = false)` | JS `==` or `===`. |
| `GetHeapSize()` / `GrowHeap()` | Heap diagnostics. |

## Scope

```csharp
if (JS.IsWindow)
{
    using var doc = JS.WindowThis!.Document;
}
else if (JS.IsDedicatedWorkerGlobalScope)
{
    JS.DedicateWorkerThis!.PostMessage("worker ready");
}
```

## See also

[Architecture](../architecture.md), [Argument passing](../argument-passing.md), [Writing marshallers](../writing-marshallers.md).
