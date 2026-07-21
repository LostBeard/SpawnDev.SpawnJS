# JsonConverter to Marshaller parity

Every `JsonConverter` in SpawnDev.BlazorJS, and what covers it in SpawnJS.

BlazorJS converts through `System.Text.Json`, so some of its converters exist **only to survive that
boundary**. SpawnJS never serializes - the value stays a live JS handle and the marshaller reads exactly
the caller's target type - so those converters have no counterpart and should not be ported. They are
marked *not applicable* below, with the reason.

Audited 2026-07-21.

## Covered

| BlazorJS JsonConverter | SpawnJS marshaller |
|---|---|
| `ActionConverterFactory` | `DelegateMarshaller` |
| `FuncConverterFactory` | `DelegateMarshaller` |
| `DateTimeEpochConverter` | `DateTimeMarshaller` |
| `EpochDateTimeConverter` | `EpochDateTimeMarshaller` |
| `DictionaryStringObject` | `DictionaryMarshaller` |
| `EnumStringConverter` | `EnumStringMarshaller` |
| `ITupleConverterFactory` | `TupleMarshaller` |
| `JSInProcessObjectReferenceConverterBase` | `SpawnJSObjectReferenceMarshaller` |
| `JSObjectConverterFactory` | `JSObjectMarshaller` / `SpawnJSObjectMarshaller` |
| `JSObjectReferenceArrayConverterFactory` | `ArrayMarshaller` |
| `JSObjectReferenceListConverterFactory` | `ListMarshaller` |
| `UnionConverterFactory` / `UnionJsonConverter` | `UnionMarshaller` |
| `HeapView`'s converter | the `heapView` JS primitive |

## Not applicable - artifacts of the Json boundary

| BlazorJS JsonConverter | Why nothing is needed |
|---|---|
| `HybridObjectConverterFactory` | Existed to pull a **nested** `JSObject` through a Json payload one type at a time, because a reference cannot be expressed in Json. `ObjectMarshaller` marshals each member through the graph, so a JSObject-typed property is just another member. |
| `JsonConverterCollection` | Registry plumbing for `JsonSerializerOptions`. The marshaller list replaces it. |
| `IServiceCollectionExtensions` | DI registration of the above. |

## Gaps

Ordered by usefulness. None of these are blocked - each is a marshaller plus tests.

| Missing | What it maps | Notes |
|---|---|---|
| **`Task` / `Task<T>`** | .NET `Task` to a JS `Promise` | ⚠️ **The dangerous one.** Nothing claims `Task`, so passing one as an argument or property falls to `ObjectMarshaller`, which property-walks it into `{result, id, status, ...}` - silently wrong, no exception. Same failure mode `Dictionary` had before `DictionaryMarshaller`. `Promise` already has a `Promise(Task)` constructor, so the mapping exists; it just is not wired into the graph. |
| **`Undefinable<T>`** | distinguishes JS `null` from `undefined` | The **type itself is not ported** - port `Undefinable`/`Undefinable<T>` first, then the marshaller. Needed for APIs where "absent" and "explicitly null" differ. |
| **`BigInteger`** | `System.Numerics.BigInteger` to JS `BigInt` | The `BigInt` JSObject wrapper IS ported; the .NET-side numeric mapping is not. Note the design law: TypedArray/BigInt fast paths are **opt-in via wrapper types**, never auto-selected, so this maps `BigInteger` only - it must not make `long` start crossing as `BigInt`. |
| **`Type`** | `System.Type` to its name string | Small; used for diagnostics and type-directed APIs. |
| **`DynamicJSObject`** | `DynamicObject` over a JS object | **Type not ported.** |
| **`JSObjectAsync`** | async-property JSObject base | **Type not ported.** Depends on how much of the async wrapper surface is wanted. |

## Rule of thumb when adding one

1. **Registration order is priority** - matching runs in **reverse** registration order, so a marshaller
   registered later wins. A new marshaller for a type that is a `struct` must register after
   `StructMarshaller`, and for a `class` after `ObjectMarshaller`, or those claim it first and
   property-walk the value. `TupleMarshaller` is registered last for exactly this reason.
2. **Assert what Javascript actually holds.** A property-walked value is still a well formed object, so a
   shape-only check passes while the value is wrong. Read the raw JS value first, then round trip.
3. **Test the READ, not just the write.** A write types itself from the boxed value; a read types itself
   from the declared type, and the two can resolve different marshallers.
