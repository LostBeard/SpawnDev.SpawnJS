# SpawnDev.SpawnJS Documentation

Deeper documentation for **SpawnDev.SpawnJS** - JSON-free JavaScript interop for .NET WebAssembly. Start with the top-level [README](../README.md) for the overview; these pages go into the how and why.

## Contents

- **[Architecture](architecture.md)** - the core design: how every interop operation collapses to a few fixed `JSImport`/`JSExport` primitives, the `[ret]` array-wrap trick, and the managed marshaller graph that carries all the richness.
- **[Argument passing](argument-passing.md)** - why the call methods use explicit `0..N`-arg overloads plus an `Apply(object?[])` form instead of `params`, and the silent `params` array-collapse footgun that design avoids.
- **[Writing marshallers](writing-marshallers.md)** - the `JSMarshaller` contract, how the registry resolves a type to a marshaller, the two design laws (parity-by-default, bring-your-own), and how to register a custom marshaller.
- **[Roadmap](roadmap.md)** - current state and what is next (chiefly the JS -> .NET inbound dispatch).

## The one-paragraph version

Blazor's `IJSInProcessRuntime` routes every interop value through a JSON serialize/parse on both ends. That cost is invisible on bulk data but real on orchestration traffic, and worse, non-primitive return values (even a one-property object or a `Uint8Array`) cannot survive `JSON.stringify` on the JS side, which has no access to the .NET type system. SpawnJS removes JSON from the boundary entirely: values cross as live `JSObject` handles and the **.NET side** does all marshalling through a marshaller graph. No Blazor dependency, so it runs in any .NET WASM host (Blazor, Avalonia, headless console, Web Workers).
