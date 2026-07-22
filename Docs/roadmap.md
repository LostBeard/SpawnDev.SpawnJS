# Roadmap

SpawnDev.SpawnJS is **1.0.0**. This page tracks what works and what is next.

## Working now

- **.NET -> JS**, synchronous and asynchronous: get, set, has, delete, call, void-call, and construct, with strongly-typed generic returns.
- **JS -> .NET**: the single `_JSToNetCall(intent, argsArray)` channel carries typed inbound calls and callbacks back through the marshaller graph in reverse - the mirror of the outbound path. Exercised end to end by SpawnDev.ILGPU's GPU-compute suite (real WebGPU callbacks and dispatch) running headless on SpawnJS.
- **Marshaller graph** with per-type caching, reverse-order priority, and null handling.
- **Transport** - a shared flat buffer that unwinds as a stack, plus an argument frame in .NET memory the JavaScript side views directly, so no JavaScript object reference crosses per call.
- **Slot-backed references** - proxies are created lazily; slot keys are allocated monotonically and never reused.
- **Collapse-proof argument passing** - fixed-arity overloads plus `Apply` forms across the typed API and the low-level extensions (see [argument-passing.md](argument-passing.md)).

Verified across three suites - `WasmBrowserDemo` (browser), `WasmConsoleDemo` (headless Node), and the trimmed publish - and the solution builds warning- and error-free across net8.0 / net9.0 / net10.0.

## Next

- **Opt-in marshallers** for TypedArrays and `BigInt` (via explicit wrapper types, per the parity-by-default law), and the remaining `JsonConverter` parity gaps in [json-converter-parity.md](json-converter-parity.md).
- **SpawnDev.BlazorJS 4.0 on SpawnJS** - rebase the Blazor-compatible `IJSInProcessRuntime` / `IJSInProcessObjectReference` surface onto SpawnJS as one package, dropping the separate Blazor interop layer.
- **WebWorkers startup** - once libraries no longer need the Blazor interop layer, the SpawnDev.BlazorJS.WebWorkers fake-`window` startup shim can be dropped.
- **Non-Blazor hosts** - bring SpawnDev.BlazorJS-style interop to .NET WASM apps that do not use Blazor, such as Avalonia ([issue #28](https://github.com/LostBeard/SpawnDev.BlazorJS.WebWorkers/issues/28)).
- **Faster inbound dispatch key** - swap the `string` intent for an `int` (backed by a message-type `enum`) without changing the pattern.

## Design tell

Every capability added so far has made the design *simpler*, not bigger: async reused the sync path, the void path collapsed into the value path, null handling fell out of the default marshaller, and the inbound channel mirrored the outbound one. That convergence is the sign the core bend is correct.
