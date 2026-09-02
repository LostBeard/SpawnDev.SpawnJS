# Changelog

All notable changes to SpawnDev.SpawnJS.

## [2.1.10] - 2026-09-02

### Fixed

- **The callback shim released the args slot on the straight line, not in a `finally` - so a throwing
  handler stranded the slot AND left a `once` callback able to fire again.** The JS shim held the args
  array, called into .NET, then released it:

  ```js
  var argsId = SpawnJSInterop.spawnJSObjectHold(args);
  handleCallback(callbackId, argsId, argsCnt);
  SpawnJSInterop.spawnJSObjectRelease(argsId);
  if (once) delete SpawnJSInterop._callbacks[callbackIdPair];
  ```

  .NET deliberately does not release that slot itself - doing so would cost an extra crossing per
  callback, and `Callback.HandleCallback` documents it (`FromID` with `preventDispose: true`). So the
  release above is the ONLY one.

  An exception from the user's handler propagates out of `HandleCallback` and back into JS here. ⚠️ This
  is NOT the runtime-killing case - the JS caller is usually a browser event dispatcher, which logs an
  uncaught error and carries on. The page survives and **both** cleanup lines are silently skipped, so the
  args array is stranded in `spawnJSObjects` for the life of the page, and a `once` callback stays
  registered JS-side and **can fire again**. The second of those is a correctness bug, not merely a leak.

  Both now run in a `finally`.

  MEASURED: a mid-run dump of `SpawnJSInterop.spawnJSObjects` from a consuming app showed ~30 stranded
  empty arrays - the exact shape of a zero-argument callback's args. After the fix the table holds 2
  arrays at rest and returns to 2 after a forced GC following repeated runs; they no longer accumulate.
  ⚠️ That is an after-the-fact comparison against the reported symptom, not a controlled A/B.

## [2.1.9] - 2026-08-28

### Fixed

- **The app-root resolver matched the framework folder BY NAME, so a renamed framework folder resolved
  `AppBaseUri` one level too deep** (`wwwroot/SpawnDev.SpawnJS.lib.module.js`).
  `SpawnJSInterop.#appRootFromLoadUrl` normalizes whatever URL a runtime artifact was loaded from back to
  the app root, and it did that by stripping a literal trailing `_framework/`. A published app can rename
  that folder - `SpawnDev.SpawnJS.WebWorkers`' `SpawnJSWebWorkersFrameworkFolderName` does exactly that,
  because a browser extension may not have a root folder whose name starts with `_`. The name match then
  failed silently and `AppBaseUri` came back as the framework folder ITSELF. Every URL built on it
  resolved one level too deep; worker entrypoints 404'd at `<root>/framework/main.classic.js`, which
  presents as a crashed renderer rather than a clean error.

  The folder is now identified by WHAT was loaded, never by what it is NAMED: the runtime entry
  (`dotnet.js` / `dotnet.<fingerprint>.js` / `dotnet.native.worker.<fingerprint>.mjs`) and every
  boot-manifest resource (`.wasm`/`.dll` assemblies, ICU `.dat`/`.blat`, `.pdb` symbols) live in the
  framework folder, so the app root is that folder's parent; anything else - a bundled entrypoint such as
  `main.classic.js` / `main.module.js` - already sits AT the app root and is not walked up. Behaviour for
  a normal `_framework` publish is unchanged.

### Added

- **`SpawnJSInterop.appRootFromLoadUrl(url)`** - the app-root normalizer, exposed so it is diagnosable and
  directly testable. 14 new `AppRoot.*` tests in the Demo suite drive that production function over every
  shape the resolver is handed (fingerprinted and unfingerprinted runtime entry, boot resources, a
  sub-path app, a RENAMED framework folder, a bundled entrypoint at the app root, query/fragment, and the
  rejected blob/empty/null inputs), plus a live `AppBaseUri` check in the running app.

## [2.1.7] - 2026-08-18

### Fixed

- **`HeapView`'s `ReadOnlyMemory<T>` constructor was dead** (`JSObjects/HeapView.cs`). It sized the
  view from `_memorySource` - the field only the `Memory<T>` constructor sets - so every call threw
  `InvalidOperationException: InvalidOperation_NoValue` before the view was ever created. That killed
  `HeapView.Create(ReadOnlyMemory<T>)`, `HeapView.CreateCopy(ReadOnlyMemory<T>)` and everything built
  on them; in SpawnDev.WebTorrent it took out every browser OPFS piece write
  (`OpfsChunkStore.PutAsync`). A `T[]` argument binds to the `Memory<T>` overload, so the whole
  library and its test suite went through the working lane and nothing noticed.

- **Disposing a `HeapView` whose constructor threw no longer takes down the runtime.** `Dispose(bool)`
  dereferenced `View`, which is null on an instance whose constructor threw before assigning it. The
  instance is still finalized, and an exception escaping a finalizer is fatal on the .NET WASM
  runtime: it exits with 255 and every later interop call fails with `Assert failed: .NET runtime
  already exited with 255`. So one broken constructor turned into an unrecoverable page. `View` is now
  null-checked, and the finalizer swallows anything that still escapes.

  The dispose flag was also inverted (`_disposeView = true` -> `_viewTaken`) so the safe behaviour is
  the field's DEFAULT value. Field initializers do not run when a constructor throws, so a flag that
  needed its initializer to be correct was wrong on exactly the instances that matter.

- **The non-generic `HeapView.CreateCopy(...)` overloads gave away a view they still owned.** All 22
  of them (11 `ReadOnlyMemory<T>`, 11 `Memory<T>`) were written as `=> Create<T, TView>(source, true)`
  and relied on the implicit `HeapView<T,TView>` -> `TView` conversion to produce the return value.
  That hands the caller the view while leaving the `HeapView` itself un-taken, undisposed and NOT
  finalization-suppressed - so when the temporary was collected its finalizer ran
  `View.Dispose()` and released the slot the caller was still using. They now route through
  `CreateCopy<T, TView>(source)`, which calls `TakeViewAndDispose()` - the same path the generic
  overload already used.

  Symptom in SpawnDev.WebTorrent: `AsyncFSChunkStore.PutAsync` builds the view, `await`s, then writes
  it, and the write failed with `Failed to execute 'write' on 'FileSystemWritableFileStream': The
  provided value is not of type 'WriteParams'` - JS receiving a released slot. Note this could not be
  reproduced from a synchronous test: in single-threaded WASM finalizers run when control yields to
  the JS event loop, not inside a synchronous `GC.Collect()` / `GC.WaitForPendingFinalizers()`.

### Added

- **9 `HeapView` tests** in the Demo suite (`UnitTests/MarshallerTests.cs`): six covering the
  `ReadOnlyMemory<T>` lane end to end (live view geometry, later .NET writes seen through a live view,
  multi-byte element sizing, copy independence, `CreateCopy`, empty source) and two covering disposal
  of a view whose constructor threw. All six `ReadOnlyMemory` tests fail against the previous code
  with the production exception, and the dispose test fails with the production
  `NullReferenceException`.

## [2.1.6] - 2026-08-18

### Added

- **`JsonElement` marshalling** (`Marshallers/JsonElementMarshaller.cs`, registered in
  `SpawnJSRuntime`). A `JsonElement` now crosses as a REAL Javascript value - an object becomes a JS
  object with live members, an array a JS Array, a primitive a JS primitive - rather than as a string
  containing JSON. Both directions move raw JSON text and let the other side parse it, using
  `JsonElement.GetRawText()` and `JsonDocument.Parse`, so nothing is serialized twice and the
  marshaller carries no reflection-based `System.Text.Json` dependency (no `IL2026`, and it works in
  an app built with `JsonSerializer.IsReflectionEnabledByDefault=false` - which the Blazor WASM SDK
  sets by default, and where the reflection-based serializer throws at runtime).

  A JS `undefined` reads back as `JsonValueKind.Undefined` and a JS `null` as `JsonValueKind.Null`;
  the two stay distinguishable, which is why there is no nullable companion marshaller -
  `JsonElement` models absence itself. `default(JsonElement)` writes JS `undefined`, so that round
  trips too.

- **`SpawnJSObjectReference.PropertySetRawJson(key, json)`** - writes pre-serialized JSON text
  straight through to the JS side's `JSON.parse`. `PropertySetJson` takes an `object` and runs the
  serializer on it, so handing it text that was already JSON encoded it a second time and landed a
  STRING on the JS side instead of an object. The raw form is what a caller holding JSON text wants,
  and it drops the reflection dependency with it.

- **16 `JsonElementMarshaller` tests** in the Demo suite (`UnitTests/MarshallerTests.cs`), covering
  both directions, both the int-key (call argument) and string-key (named member) write paths, the
  null/undefined distinction, nesting, and non-ASCII with JSON escapes. Suite is **176/176**.

### Fixed

- **`RTCStatsReport.Entries` / `Keys()` / `Values()` always came back EMPTY**
  (`JSObjects/WebRTC/RTCStatsReport.cs`). All three JS methods return a **Map Iterator**, not an
  Array, but the wrappers asked for `T[]` and so went through `ArrayMarshaller`, whose `JSToNet`
  sizes the result from `value.length`. An iterator has no `length`, so the read produced 0 and
  every caller got a zero-length array back - silently, with no error to explain it. `RTCStatsReport`
  was the only Map-like wrapper in the library still doing this; `Map`, `Set`, `Headers`,
  `URLSearchParams`, `DOMTokenList` and `TypedArray` all already declare these members as
  `Iterator<T>`. The three members now do the same and materialize via `Iterator<T>.ToArray()`
  inside `Using(...)` so the iterator's JS slot is released.

  Downstream: this is what made `SpawnDev.RTC`'s `IRTCStatsReport.Entries()` return nothing on the
  browser backend, so `getStats()` reported no candidate-pair and no peer-connection entry no matter
  how healthy the connection was.

## [2.1.5] - 2026-08-17

### Fixed

- **`HeapView.Dispose()` freed NOTHING** (`JSObjects/HeapView.cs`). The public `Dispose()` set
  `IsDisposed = true` *before* calling `Dispose(true)`, whose first statement is `if (IsDisposed) return;` -
  so the guard short-circuited the real work every time. Neither `ReleaseHandle()` nor `View.Dispose()`
  ever ran on the dispose path. Consequences: (1) the backing `TypedArray` view kept its JS slot held,
  released only if/when its own finalizer eventually ran, and (2) on the zero-copy path (`copy: false`)
  the pinned `MemoryHandle` / `GCHandle` stayed **pinned for the life of the app** - a managed-heap leak,
  not just a JS-slot one. `Dispose(bool)` now owns the flag. Every `using var heapView = ...` in the
  library (`TypedArray.Read`/`Write`/`Set`/`ReadBytes`, all 12+ call sites) was silently leaking.
- **`PocoMarshaller` / `DictionaryMarshaller` leaked one JS slot per marshalled argument.**
  `WriteToNewObject()` allocates a JS object via `JS.New<SpawnJSObjectReference>("Object")` - a strong
  slot-table entry with manual lifetime - and both `NetToJS` overloads did
  `jsParent.Set(jsKey, WriteToNewObject(value))`, dropping the returned handle on the floor. `Set` performs
  a real JS reference assignment (`parent[prop] = value`), so the temporary's slot is safe to release
  immediately; nothing did. Every sibling marshaller (`Array`, `IList`, `List`, `IEnumerable`, `ITuple`)
  already had this right via `using var outArray = JS.NewJSArray()`; these two were the outliers.
  Impact scaled with call rate: a WebGPU dispatch loop marshals a `GPUBindGroupDescriptor` **per dispatch**,
  so the slot table climbed into the tens of thousands and outran the WASM GC, ending in OOM. Measured on
  the SpawnDev.ILGPU demo suite: 544 undisposed references from a single dispatch call site over ~90s,
  reduced to zero.

## [1.1.9] - 2026-08-07

### Added

- **Trusted Types wrappers** (`JSObjects/TrustedTypes/`): `TrustedTypePolicyFactory` (`window.trustedTypes`),
  `TrustedTypePolicy`, `TrustedHTML`, `TrustedScript`, `TrustedScriptURL`, and `TrustedTypePolicyOptions`.
  These let code create a Trusted Type policy and produce approved values on a page whose CSP enforces
  `require-trusted-types-for 'script'` (e.g. YouTube, Gmail), where a plain string is refused at an
  injection sink. `CreatePolicy` takes `Callback`s (not `new Function`, which such a page's `unsafe-eval`
  block would also refuse) and builds the policy-options record explicitly so the JS keys are exact.
- **`DOMParser.ParseFromString(TrustedHTML, mimeType)`** overload - `parseFromString` is a Trusted Types
  sink and throws `This document requires 'TrustedHTML'` for a plain string under enforcement, so callers
  that may run on such a page pass a `TrustedHTML` produced by a policy. Kept in sync with the same addition
  in SpawnDev.BlazorJS.

## [1.1.4] - 2026-08-05

### Added

- **`SpawnJSRuntime.AppBaseUri`** - the URL the app was loaded from (its own `main.*` / `_framework`
  origin), with a trailing slash. Unlike `document.baseURI` (the host page's base), this stays correct
  when the app is served from a CDN at a different path than the page, which is what worker entry scripts
  must resolve against. Determined per-runtime from this app's own dotnet runtime
  (`Module.mainScriptUrlOrBlob`, measured in both window and worker scopes; `getConfig().resources`
  resolvedUrl as backup), so two SpawnJS apps loaded from different origins on one page each report their
  own base - no `globalThis`, no page coupling. `AppBaseUriSource()` reports which shape resolved it,
  mirroring `WasmMemoryBufferSource()`. Guards: `AppBaseUriTests`.

## [1.1.2] - 2026-08-04

### Fixed

- **`DOMParser.ParseFromString`** was passing the input HTML string *as the method name*
  (`JSRef.Call<Document>(input, mimeType)` instead of `JSRef.Call<Document>("parseFromString", input, mimeType)`),
  so every call threw `TypeError: target[name] is not a function` at runtime while compiling clean. Added a
  `PortedDOMParserParseFromStringTest` regression guard.

### Changed

- Restored the test harness build: `ArgumentFrameTransportTests` needed `using SpawnDev.SpawnJS.Native`
  and `JSInteropTestsCore` needed `Reflect` qualified to `Native.Reflect` after the `Native.Reflect`
  namespace correction and the addition of `JSObjects.Reflect`.

## [1.1.0] - 2026-07-31

### Added

- **Fleshed-out `Window` wrapper.** Timers (`SetTimeout`/`SetInterval`/`ClearTimeout`/`ClearInterval`),
  `RequestAnimationFrame`/`CancelAnimationFrame` and `RequestIdleCallback`/`CancelIdleCallback` (each with
  `Callback`, `Action`, and `Func<Task>` overloads), `Fetch`, `CreateImageBitmap`, the File System Access
  pickers (`ShowOpenFilePicker`/`ShowSaveFilePicker`/`ShowDirectoryPicker` with support probes),
  `GetComputedStyle`, `MatchMedia`, `GetSelection`, `GetScreenDetails`, `Open`/`Close`/`Focus`/`Print`/
  `Stop`, `Alert`/`Confirm`/`Prompt`, `PostMessage`, `QueueMicrotask`, and the window geometry methods
  (`ResizeBy`/`ResizeTo`/`MoveBy`/`MoveTo`/`Scroll`/`ScrollBy`/`ScrollTo`).
- **`SpawnJSRuntime.Fetch(...)`** convenience overloads on the global scope, and `IGlobalScopeSource`.
- **`Toolbox.Async`** helper (`Run`/`RunAsync`) for firing async lambdas without a captured `async void`.
- **Parameterless `AddSpawnJSRuntime()`** extension overload.

### Fixed

- **Marshalling a Javascript array into `Array<T>` threw `MissingConstructor_Name`.** The generic
  `Array<TArrayItem>` deserialization constructor took `IJSInProcessObjectReference` (a leftover from the
  BlazorJS wrappers). `SpawnJSObjectReference` only *implicitly converts* to that interface, and
  `Activator.CreateInstance(type, new SpawnJSObjectReference(...))` - the path every wrapper is built
  through in `SpawnJSObjectMarshaller.JSToNet` - does not consider user-defined implicit conversions, so
  it could not bind the constructor and threw
  `MissingMethodException: MissingConstructor_Name, SpawnDev.SpawnJS.JSObjects.Array\`1[[...]]` on the
  first read of any JS array into `Array<T>`. It now takes `SpawnJSObjectReference`, matching the
  non-generic `Array` and every other wrapper.
- **Reading a Javascript number into a `Nullable<T>` over the frame path threw instead of returning the
  value.** `ReadFrameResult`'s primitive fast-path selected on the declared type without unwrapping
  `Nullable<T>`, so `Get<int?>` matched none of `double`/`int`/`float`/`long`/`bool` and fell through to a
  marshaller handle built over the scratch buffer at the call's own offset - where the call's first
  argument still sat. `JS.Get<int?>("navigator.hardwareConcurrency")` failed with
  `Value is not a Number: [object Window]` while `Get<int>` worked. The fast-path now selects on the
  underlying type, and the same-offset fallthrough stores the primitive payload into the scratch slot
  first, so any numeric type the fast-path does not name (`short`, `byte`, `Half`, a numeric enum) reads
  the returned value rather than the argument. Regression guards: `NullableNumberFromDottedPathTest`,
  `NonFastNumericFromDottedPathTest`.

## [1.0.0] - 2026-07-21

First release. SpawnJS is Javascript interop for .Net WebAssembly that does not serialise: references
are held as integer slots rather than `JSObject` proxies, and outbound call arguments are written into
.Net's own memory that Javascript views directly, so a call carries only a command name, an offset and
a length.

### Removed

- The argument-transport experiments the shipped frame beat, so 1.0.0 exposes only the winner:
  `HeapArgBuffer` (structure-of-arrays) and `HeapArgFrame3` (stride-24 string frame);
  `HeapArgFrame.WriteTaggedByte` / `ReadTagByte` (a byte tag, beaten by an f64 tag in the slot's
  padding); and the `SpawnJSRuntime.UseArgFrame` switch with the old Javascript-side argument array it
  selected. The probe surface these left behind goes with them - `SlotInterop.BindArgBuffer`,
  `HeapSum`, `HeapTaggedSum`, `FrameTaggedSum`, `FrameStringLength`, `SlotStringLength`, `SlotSum` and
  their Javascript counterparts. `SlotInterop` stays public with the production-shape probes.
- `__sjsBuildObject` and `SlotInterop.BuildObject`. Building a value into a slot and handing the slot
  back leaks by construction; it is deleted rather than left as a trap. `__sjsBuildObjectInto`, which
  assigns without allocating a temporary, stays.

### Fixed

- **Two slot-table leaks, both found only under a real WebGPU dispatch.** Replacing the `JSObject`
  proxy with slot ids moved lifetime from the runtime's GC to the library: `__sjsSlots[n]` is a strong
  reference nothing collects, so a slot owned by nobody leaks the entry and the Javascript value it
  names. WebAssembly's GC is too lazy for finalizers to cover it.
  - An object passed as a call **argument** was built into a slot whose id was handed over, and nothing
    freed it - two entries per GPU dispatch, 4495 live slots after 2000 launches. Object arguments are
    now carried as `ArgTag.InlineObject`: the payload packs the member region's own absolute heap index
    and pair count, so Javascript builds the object in place while reading the argument list. No slot,
    nothing to free, still one crossing, and nesting composes through the same path.
  - **Interning treated every string as a repeated literal.** A string that never recurs took a slot
    nothing would read again. `Callback` names itself `cb_{n}`, unique per instance, twice per awaited
    promise: 402 dead slots over 200 awaits. A string is now interned on its **second** sighting.

### Added

- `ArgTag.InlineObject` and `ArgTag.PackInline`, with the matching `SJS_TAG_OBJECT` on the Javascript
  side.
- A numeric inbound callback path - `registerCallbackById` / `registerCallbackVoidById`,
  `_JSToNetCallById`, and the `_jsToNetById` table - so an anonymous callback's generated id crosses as
  a number instead of a marshalled string on every invocation. Named handlers keep their string key,
  which is public API.
- `SlotInterop.SlotTableCount()` - the size of the actual slot table. `SpawnJSHandle.LiveSlotCount`
  counts only handle-owned slots and read zero through both leaks above, so a guard needed something
  that can observe the failure.
- `ArgumentSlotLeakTests` - asserts the slot table does not grow across repeated calls that pass
  objects, nested objects and unique strings, and that a repeated string still interns exactly once.
  Both leak guards were verified to fail against the reintroduced bugs.

### Performance

Measured on a real GPU adapter (NVIDIA Lovelace, `fallback=False`), SpawnDev.ILGPU WebGPU dispatch:

| | before | after |
|---|---:|---:|
| kernel launch (queue only) | 207.2 us | **154.4 us** |
| dispatch + `SynchronizeAsync` | 695.4 us | 782.8 us |

The launch path is 25% cheaper and the slot table no longer grows. The synchronising round trip is
still slower than the pre-transport baseline - see Known issues.

### Known issues

- **Dispatch + `SynchronizeAsync` is ~12% slower than baseline (780us vs 695us) and the cause is not
  yet known.** The callback id was the stated suspect and has been **ruled out by measurement**: making
  anonymous callbacks numeric end to end left the figure unchanged (780 +/-24 against 783 +/-17). Both
  slot leaks are fixed and the table no longer grows, so it is not accumulation either. Next candidates
  are the promise-to-task conversion itself (two `Callback`s plus a `CallbackGroup` per await) and the
  `Promise.ThenCatch` round trip - neither measured yet.

## Earlier work

The transport this changelog starts from - slot-native reads with no `JSObject` proxy outside startup,
the argument frame in .Net memory read through the runtime's own `HEAPF64`/`HEAPU8` views, one-crossing
descriptor marshalling, typed inbound invocation, and instance-scoped per-runtime state - landed before
this file existed. See the git history from `b231f76` onward.
