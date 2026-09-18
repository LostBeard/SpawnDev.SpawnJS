# Roadmap

SpawnJS **2.x** is the current line. The 1.x heap argument frame (`HEAPF64`, `(cmd, offset, length)`, `SlotInterop.FrameCall`) is gone. The live core is `JSImport` + `spawnJSObjects` + the `JSMarshaller` registry. Versions of record are in [`CHANGELOG.md`](../CHANGELOG.md) (core **2.1.17**, Blazor package **2.1.18** as of this writing).

## Done in 2.x

- Marshaller graph as the public extension point (`CanMarshal`, `NetToJS`, `JSToNet`, `ReturnType`).
- Microsoft `JSObject` avoided as a handle (speed, Symbol tagging, dispose aliasing). `JSHost.DotnetInstance` used once at register.
- Typed wrappers kept (`SpawnJSObject` / `JSRef`) so BlazorJS-style wrapper bodies port.
- Blazor extras split into `SpawnDev.SpawnJS.Blazor` (`ElementReference.As<T>()`, `ElementRef<T>`, `SpawnJSRunAsync`) so the core stays Blazor-free.
- Trim descriptor shipped in the package.
- Live suite in `SpawnDev.SpawnJS.Demo` + `SpawnJS.TestRunner` (Playwright). `TestsShared` / `WasmBrowserDemo` / `WasmConsoleDemo` are not in this repo.

## Standing constraints (not temporary)

- Slot lifetime is manual. A helper that "builds it JS-side and returns the slot" leaks unless the caller disposes.
- An unhandled exception on a JS-to-.NET callback can exit the WASM runtime. Handle or the process is gone.
- `Get` is a property lookup; `Call` is a method. Dotted paths are property paths, not expressions.
- Default marshalling is JSON-shaped (plain objects and number arrays). TypedArray / `BigInt` are explicit types.

## Follow the changelog

New work lands in `CHANGELOG.md`. This page is orientation, not a promise list.
