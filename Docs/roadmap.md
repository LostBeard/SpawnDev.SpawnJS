# Roadmap

SpawnJS **3.x** is the current line. Versions of record are in [`CHANGELOG.md`](../CHANGELOG.md) (core and Blazor package **3.0.0** as of this writing).

## Done in 3.x

- **One crossing per call.** Arguments, POCOs, arrays and the method travel in a per-instance call tape in .NET memory; Javascript reads it in one `JSImport` and writes the result back by a `JSSchema` in the same crossing ([Architecture](architecture.md), [v3 design](v3-design.md)).
- **Callback arguments in .NET memory before the call.** A JS -> .NET handler reads nothing back across the boundary.
- **Marshaller contract:** `Write(JSTape, T)` / `Schema` / `Read(ref JSTapeReader)` ([Writing marshallers](writing-marshallers.md)). `NetToJS` is gone.
- **Generated POCO codecs** (`SpawnDev.SpawnJS.Generators`, shipped in the package) with `[SpawnJSPoco]` for runtime-only types; the reflection plan stays for anything the generator cannot mirror exactly.
- Multi-instance kept: all state per .NET instance, tested with two runtimes in one page.

## Carried from 2.x

- Microsoft `JSObject` avoided as a handle (speed, Symbol tagging, dispose aliasing). `JSHost.DotnetInstance` used once at register.
- Typed wrappers (`SpawnJSObject` / `JSRef`) so BlazorJS-style wrapper bodies port.
- Blazor extras in `SpawnDev.SpawnJS.Blazor` (`ElementReference.As<T>()`, `ElementRef<T>`, `SpawnJSRunAsync`) so the core stays Blazor-free.
- Trim descriptor shipped in the package.
- Live suite in `SpawnDev.SpawnJS.Demo` + `SpawnJS.TestRunner` (Playwright).

## Open

- A `Func` callback's return value costs one extra call (a deliberate trade: the return value's pins must outlive the handler in threaded builds).
- Generated codecs leave readonly fields, `override` / `new` members and types without a public parameterless constructor to the reflection plan.

## Standing constraints (not temporary)

- Slot lifetime is manual. A helper that "builds it JS-side and returns the slot" leaks unless the caller disposes.
- An unhandled exception on a JS-to-.NET callback can exit the WASM runtime. Handle or the process is gone.
- `Get` is a property lookup; `Call` is a method. Dotted paths are property paths, not expressions.
- Default marshalling is JSON-shaped (plain objects and number arrays). TypedArray / `BigInt` are explicit types.

## Follow the changelog

New work lands in `CHANGELOG.md`. This page is orientation, not a promise list.
