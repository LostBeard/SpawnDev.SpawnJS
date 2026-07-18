# Roadmap

SpawnDev.SpawnJS is early (version 0.0.1). This page tracks what works and what is next.

## Working now

- **.NET -> JS**, synchronous and asynchronous: get, set, has, delete, call, void-call, and construct, with strongly-typed generic returns.
- **Marshaller graph** with per-type caching, reverse-order priority, and null handling.
- **Transport hygiene** - the `JSObject` argument array and the `[ret]` return wrapper are disposed on every call.
- **Collapse-proof argument passing** - fixed-arity overloads plus `Apply` forms across the typed API and the low-level extensions (see [argument-passing.md](argument-passing.md)).
- **JS -> .NET bridge fires** - a managed method registered as a JS-callable function is reached from JavaScript through the single `_JSToNetCall` channel.

Verified via the `WasmBrowserDemo` project (a plain .NET WASM, non-Blazor console harness), and the whole solution builds warning-free and error-free across net8.0 / net9.0 / net10.0.

## In progress

- **JS -> .NET dispatch.** The inbound channel `_JSToNetCall(intent, argsArray)` is wired and reaches managed code; the remaining work is marshalling the inbound arguments back through the marshaller graph and dispatching on `intent`, then marshalling the result out - the exact mirror of the outbound path. The `intent` is a `string` today and can become an `int` (backed by a message-type `enum`) for a faster dispatch key without changing the pattern.

## Planned

- **Opt-in marshallers** for TypedArrays and `BigInt` (via explicit wrapper types, per the parity-by-default law).
- **Blazor adapters** - `SpawnJSRuntimeBlazor` / `SpawnJSObjectBlazor` implement the Blazor `IJSInProcessRuntime` / `IJSInProcessObjectReference` interfaces and are placeholder shells until the core proves out.
- **WebWorkers startup** - once libraries no longer need the Blazor interop layer, the SpawnDev.BlazorJS.WebWorkers fake-`window` startup shim can be dropped.
- **Avalonia / non-Blazor hosts** - bring SpawnDev.BlazorJS-style interop to .NET WASM apps that do not use Blazor (see [issue #28](https://github.com/LostBeard/SpawnDev.BlazorJS.WebWorkers/issues/28)).

## Design tell

Every capability added so far has made the design *simpler*, not bigger: async reused the sync path, the void path collapsed into the value path, and null handling fell out of the default marshaller. That convergence is the sign the core bend is correct - and the inbound path is expected to follow the same shape.
