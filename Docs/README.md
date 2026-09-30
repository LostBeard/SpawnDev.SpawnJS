# SpawnDev.SpawnJS Documentation

Deeper documentation for **SpawnDev.SpawnJS** - JSON-free JavaScript interop for .NET WebAssembly. Start with the top-level [README](../README.md).

## Contents

- **[Hosting](hosting.md)** - how apps start SpawnJS: `SpawnJSAppBuilder`, Blazor `WebAssemblyHost`, any `IServiceCollection`.
- **[Architecture](architecture.md)** - 3.x core: the per-instance call tape (one crossing per call), shapes and schemas, the `spawnJSObjects` table, inbound callbacks, marshaller graph.
- **[Argument passing](argument-passing.md)** - explicit 0..10-arg overloads plus `Apply(object?[])`, and why `params` silently spreads arrays.
- **[Writing marshallers](writing-marshallers.md)** - the `JSMarshaller` contract (`Write` / `Schema` / `Read`), the reverse-scan registry, custom registration.
- **[API reference](api/_index.md)** - per-type reference for the JS wrapper types.
- **[v3 design](v3-design.md)** - why 3.x crosses once per call, and the measurements behind it.
- **[Roadmap](roadmap.md)** - current 3.x state.

## The one-paragraph version

Blazor's `IJSInProcessRuntime` routes every interop value through JSON. The JS side has no .NET types, so non-primitive returns often cannot cross. SpawnJS keeps values live in a numeric JS table and lets **.NET** marshall through a `JSMarshaller` graph; since 3.0 a whole call, arguments and result included, crosses the .NET/JS boundary once. No Blazor dependency: Blazor, Avalonia, workers, or a headless .NET WASM console under Node.
