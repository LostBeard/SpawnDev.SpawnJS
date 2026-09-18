# Hosting and dependency injection

Most apps start SpawnJS through **dependency injection**. `AddSpawnJSRuntime()` registers the `SpawnJSRuntime` singleton, `IBackgroundServiceManager`, and `IGlobalScopeSource`. The runtime constructor is private; the singleton is created the first time `SpawnJSRuntime.Instance` is read, which `AddSpawnJSRuntime` does.

In a Blazor component, **inject** `SpawnJSRuntime`. Do not read `SpawnJSRuntime.Instance` inside a component. The static is for library internals, tests, and hosts that have no container.

## SpawnJSAppBuilder (non-Blazor WASM)

This is the host in the core package: a small `IServiceCollection` that already includes SpawnJS. Used by console WASM apps, workers, browser extensions, and [SpawnDev.SpawnJS.RazorRenderer](https://www.nuget.org/packages/SpawnDev.SpawnJS.RazorRenderer) (which adds `RootComponents` on the same builder).

```csharp
using SpawnDev.SpawnJS;

var builder = SpawnJSAppBuilder.CreateDefault(args, out var JS);

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(JS.AppBaseUri) });
// builder.Services.AddWebWorkerService();   // SpawnDev.SpawnJS.WebWorkers
// builder.RootComponents.Add<App>(...);     // SpawnDev.SpawnJS.RazorRenderer

await builder.Build().RunAsync();
```

`CreateDefault` overloads: `(args)`, `(out SpawnJSRuntime js)`, `(args, out js)`.

`SpawnJSApp.RunAsync()` starts `IBackgroundService` / `IAsyncBackgroundService` instances, then stays alive until `Exit()`. Overloads take `Action<SpawnJSApp>` or `Func<SpawnJSApp, Task>` and run that after services have started:

```csharp
await builder.Build().RunAsync(async app =>
{
    var JS = app.Services.GetRequiredService<SpawnJSRuntime>();
    JS.Log("ready");
});
```

## Blazor WebAssemblyHost

Package: `SpawnDev.SpawnJS.Blazor`. Extends Microsoft's host with `SpawnJSRunAsync` so background services start, and so a worker scope does not call `WebAssemblyHost.RunAsync` (that API is not scope-aware).

```csharp
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SpawnDev.SpawnJS;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSpawnJSRuntime(out var JS);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().SpawnJSRunAsync();
```

```csharp
[Inject] SpawnJSRuntime JS { get; set; }
```

`ElementReference.As<T>()` and `ElementRef<T>` live in this package. See the [README](../README.md).

## Any `IServiceCollection`

`AddSpawnJSRuntime()` is an `IServiceCollection` extension in the core package. Use it on a custom provider, a generic host, or any other builder that exposes `Services`:

```csharp
services.AddSpawnJSRuntime(out var JS);
var sp = services.BuildServiceProvider();
var JS2 = sp.GetRequiredService<SpawnJSRuntime>(); // same singleton
```

## `SpawnJSRuntime.Instance` without a container

Valid in a WASM console that has no DI, in library code that must not take a constructor parameter, and in `SpawnDev.SpawnJS.Demo` (the live test suite). It is the same object `AddSpawnJSRuntime` registers. Prefer the container in application `Program.cs`.
