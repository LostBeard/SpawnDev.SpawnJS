# SpawnJS v3 - one crossing per call

Status: design approved by TJ 2026-09-30. Implemented on branch `v3` (local only): every call direction is one
crossing - arguments, results, and callback arguments. Remaining: a Func callback's return value (one extra call),
generated POCO plans, and a consumer trial.

## The problem, measured

v2 crosses the .NET/JS boundary once per VALUE. Every marshaller implements
`NetToJS(jsParent, key, value)` - "write this value into a JS object at this key" - and that signature can only
be satisfied by a crossing. Published Release, Chrome, one crossing is ~1.1-1.4 us and time tracks the count:

| case | us/op | crossings |
|---|---|---|
| call void, 0 args | 4.0 | 3 |
| call void, 5 mixed args | 14.0 | 11 |
| 16 member POCO argument | 152 | 55 |
| GPUBindGroupDescriptor, 3 entries | 256 | 104 |
| int[1000] argument | 5,447 | 4,006 |
| POCO returned from JS | 215 | 74 |

Reproduce: `?bench=` in SpawnDev.SpawnJS.Demo, `SpawnJS.TestRunner --bench` (see `Bench/InteropBench.cs`).

## The design

**One contract, one path.** No second transport kept beside the old one, no fast paths bolted on:

```csharp
public abstract class JSMarshaller<T>
{
    public abstract void Write(JSTape tape, T value);         // .NET -> JS: append to the tape, no crossing
    public abstract JSSchema Schema { get; }                  // what JS must produce when .NET reads a T
    public abstract T Read(ref TapeReader r);                 // JS -> .NET: read what JS wrote, no crossing
}
```

The writer is the runtime's `JSTape` object, not a `ref` struct: a marshaller writing an argument can itself make
an interop call (a v2 marshaller calls `JS.New("Object")`), so the write position has to live on the tape for the
nested call to stack above it.

**The tape.** A buffer in .NET memory, owned by the SpawnJSRuntime instance. .NET fills it with plain stores;
JS reads it through the WASM heap views. A call is: fill the tape, ONE JSImport, read the result off the tape.
The existing dispatcher shape (`InteropCall(method, args)`) stays; only its transport changes, and the
`NewJSArray` / per-value `propertySet` fill layer is deleted rather than given a fast path.

**Tape primitives** (tagged, values in order, nested values follow their parent): number, bool, null,
undefined, string (UTF-16 in the tape), ref (slot id), callback, object (shape id + member values), array
(count + values), bulk typed array (kind + span). A marshaller that needs special JS construction names a JS
function registered through `SpawnJSInterop.registerReviver`, so users still bring their own marshallers.

**Shapes and strings travel inline.** The first time a type (or a repeated string such as a method name) is
written, its definition rides on the same tape just before its first use, and JS caches it. No registration
crossings, and once warm a call carries no property names or method names at all.

**Schemas generalise ReturnType.** v2 already has JS shape a result to what .NET asked for (11 scalar kinds).
A `JSSchema` lets that request describe a POCO, an array, a nullable, a union (arms selected JS side by
prototype chain, where the live value is) or "any". "any" - a read typed `object` - is a held ref, exactly
what v2's ObjectMarshaller returns today.

**The value decides on write.** A member or argument declared `object`, `object[]`, an interface or an
abstract base is written by its RUNTIME type. The declared type only matters for reads. Guarded by the
`RuntimeTyped.*` tests (16). v2 failed two of them (custom interface, abstract base) until 2.1.20-local.3.

**Per instance, always.** Tape address, shape cache, string table: on the `instanceInfo` that
`_registerInstance` creates, keyed by `dotnetId`. The only page global is the `SpawnJSInterop` class itself.
Slot ids stay minted JS side as in v2. Several .NET runtimes on one page must keep working - v1 broke that by
hanging state on globalThis.

**Not in v3:** deferring or batching several calls into one crossing. It changes when errors surface, and
TJ does not want the interop band-aided. One call is one crossing, with normal error timing.

## Hazards to design in

- WASM memory growth replaces the heap buffer: views are refreshed per call (v2's `getHeap()` tracks detach).
- Threaded builds put the heap in a SharedArrayBuffer, which `TextDecoder` refuses.
- The browser extension's content security policy blocks JS code generation: no `new Function`.
- A call can re-enter .NET through a callback, which calls JS again: the tape is used as a stack.
- Trimming: reflection-walked POCOs lose getters in a trimmed publish (measured: `Arg_GetMethNotFnd`).
  POCO encoders are generated per type, which fixes it and removes per-member reflection.

## Order of work, and the gates

1. Core: tape, one JSImport, numbers / strings / refs, call on a held object.
2. Two-instance test (two .NET runtimes, one page). Bench against the v2 baseline.
3. Port the marshallers, one at a time, the existing suite green after each.
4. Generated POCO encoders. Done: `SpawnDev.SpawnJS.Generators`, see [architecture.md](architecture.md#6a-generated-poco-codecs). 354 codecs in the suite (327 of them the library's own descriptors). On a trimmed Release build, A/B against the reflection plan: return Dto16 10.0-10.4 -> 6.9-7.1 us, GPUBindGroupDescriptor 11.7-12.6 -> 9.4-10.0 us, Dto16 argument 8.5-10.2 -> 7.2-8.3 us. Still one crossing.

Every step: full suite green (v2 baseline 228/228 as of 2.1.20-local.3), benchmark numbers in the commit message.
