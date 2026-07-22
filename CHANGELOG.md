# Changelog

All notable changes to SpawnDev.SpawnJS.

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
