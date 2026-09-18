# Writing marshallers

A `JSMarshaller` converts one .NET type to a JS value and back. The registry is the product: any type is marshalled by the first registered marshaller whose `CanMarshal` returns true, scanned in **reverse registration order** (later registrations win). Built-ins register first; yours register last. Resolved marshallers are cached per `Type`.

## Two design laws

1. **Parity by default, performance by opt-in.** The default graph mirrors what `JSON.stringify` would produce. `List<long>` becomes a JS number array, not a `BigInt64Array`. Typed arrays and `BigInt` are wrapper types you ask for (`Uint8Array`, `BigInt`, `HeapView`). `byte[]` is the built-in exception: it copies to/from a `Uint8Array`.
2. **Any type, users bring their own.** The transport is a live `SpawnJSObjectReference` (or a primitive `ReturnType`). A custom marshaller can drop to its own `JSImport` if it needs to.

## Contract

```csharp
public abstract class JSMarshaller
{
    public virtual ReturnType ReturnType { get; }
    public abstract bool CanMarshal(Type type);
    public virtual JSMarshaller<T> GetMarshaller<T>();
}

public abstract class JSMarshaller<TType> : JSMarshaller
{
    public virtual TType JSToNet(/* one of: bool, bool?, int, int?, double, double?, string, SpawnJSObjectReference */);
    public abstract void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TType value);
    public abstract void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TType value);
}
```

`ReturnType` selects which `[JSImport]` reads the JS result and how JS's `_serializeToNet` shapes it:

| `ReturnType` | JS prepares | .NET import |
|---|---|---|
| `Void` | nothing | void |
| `Double` / `DoubleNullable` | number | `double` / `double?` |
| `Boolean` / `BooleanNullable` | boolean | `bool` / `bool?` |
| `Int32` / `Int32Nullable` | number | `int` / `int?` |
| `String` | string (or `toString`) | `string` |
| `SpawnJSObjectReference` | `spawnJSObjectHold`, or null | id as `double` |
| `SpawnJSObjectReferenceNonNullable` | always hold, including null/undefined | id as `double` |
| `Json` | `JSON.stringify` | `string` |

Subclass the matching `JSMarshallerFrom*` helper (`JSMarshallerFromInt32<T>`, `JSMarshallerFromDouble<T>`, `JSMarshallerFromString<T>`, `JSMarshallerFromSpawnJSObjectReference<T>`, `JSMarshallerFromJson<T>`, and nullable variants). Override **only** the `JSToNet` overload that matches your `ReturnType`. The rest throw `NotImplementedException` by design.

`GetMarshaller<T>()` lets a factory specialize. `ArrayMarshaller<object>` is registered once; when asked for `int[]` it returns an `ArrayMarshaller<int>`. Return that specialization - the runtime caches whatever you return.

## Read vs write

- **Write (.NET to JS)** selects on the boxed **value's runtime type** (`GetMarshallerForWrite<T1>()` inside `InteropCallApply`).
- **Read (JS to .NET)** selects on the **declared** `T`.

`Nullable<int>` boxes as `int` on write, so write and read can resolve different marshallers if you forget to handle both. Built-in numeric marshallers register both `int` and `int?` (and an `INumber` / `INumberNullable` catch-all). `PocoMarshaller` unwraps `Nullable<TStruct>` itself. **Assert the round trip**, not only the write.

A JS class name cannot identify a derived type: `Object.prototype.toString.call(new TypeError())` is `"[object Error]"`. Walk the **prototype chain** (`ConstructorNames()`, most derived first). `UnionMarshaller` does this to pick an arm.

## Built-in registration order

Last registered wins. Approximate map of what is registered (see `SpawnJSRuntime` constructor for the exact list):

| Marshaller | .NET | JS |
|---|---|---|
| `PocoMarshaller` (first = lowest priority) | class/struct POCO | plain object, member walk, honors `[JsonPropertyName]` / `[JsonIgnore]` / `[JsonInclude]` |
| `IEnumerableMarshaller` | `IEnumerable<T>` | Array |
| `VoidTypeMarshaller` | `VoidType` | nothing |
| `ObjectMarshaller` | `object` | re-dispatch on runtime type, or null |
| `StringMarshaller` | `string` | string |
| `INumberMarshaller` / nullable | other `INumber<T>` | Number |
| `DoubleMarshaller` / `Int32Marshaller` / `BooleanMarshaller` (+ nullable) | those primitives | Number / Boolean |
| `ITupleMarshallerFactory` | `Tuple` / `ValueTuple` | Array |
| `SpawnJSObjectReferenceMarshaller` | `SpawnJSObjectReference` | any |
| `ArrayMarshaller` / `ListMarshaller` | `T[]` / `List<T>` | Array |
| `DictionaryMarshaller` | `IDictionary<string, T>` | plain object |
| `HeapViewDescriptorMarshaller` | `HeapViewDescriptor` | TypedArray / DataView |
| `CallbackMarshaller` | `Callback` | Function |
| `ByteArrayMarshaller` | `byte[]` | Uint8Array (copy) |
| `TaskMarshaller` | `Task` / `Task<T>` | Promise |
| `BigIntegerMarshaller` | `BigInteger` | BigInt |
| `UnionMarshallerFactory` | `Union<...>` | any, arm by prototype / typeof |
| `DelegateMarshallerFactory` | `Action` / `Func` | Function (`Callback`, cached per delegate) |
| `SpawnJSObjectMarshaller` | `SpawnJSObject` wrappers | any, `Activator` + `.ctor(SpawnJSObjectReference)` |
| `EnumMarshallerFactory` | enum | Number |
| `EnumStringMarshallerFactory` | `EnumString<T>` | String |
| `EpochDateTimeMarshaller` | `EpochDateTime` | Number |
| `DateTimeMarshaller` | `DateTime` | String |
| `HeapViewMarshaller` | `HeapView` | TypedArray / DataView |
| `JsonElementMarshaller` | `JsonElement` | any via JSON text |
| `TypeMarshaller` / `RuntimeTypeMarshallerFactory` | `Type` | string |

`SpawnJSObjectMarshaller` is registered after `PocoMarshaller`, so wrappers never get cloned as POCOs.

## Registering your own

```csharp
var builder = SpawnJSAppBuilder.CreateDefault(args, out var JS);
JS.Marshallers.Add(new CelsiusMarshaller());
```

Add **before** the type is first marshalled. `GetMarshaller<T>` caches the winner; a later add does not evict that cache. The runtime already exists after `CreateDefault` / `AddSpawnJSRuntime`.

## Example

A value type that should cross as a JS number:

```csharp
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.Marshaller;

public readonly struct Celsius
{
    public Celsius(double value) => Value = value;
    public double Value { get; }
}

public sealed class CelsiusMarshaller : JSMarshallerFromDouble<Celsius>
{
    public override Celsius JSToNet(double value) => new Celsius(value);

    public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, Celsius value)
        => jsParent.Set(jsKey, value.Value);

    public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, Celsius value)
        => jsParent.Set(jsKey, value.Value);
}

// after CreateDefault / AddSpawnJSRuntime, before the type is used:
JS.Marshallers.Add(new CelsiusMarshaller());
```

A live JS object uses `JSMarshallerFromSpawnJSObjectReference<T>` and typically constructs a `SpawnJSObject` subclass:

```csharp
public sealed class MyWrapperMarshaller : JSMarshallerFromSpawnJSObjectReference<MyWrapper?>
{
    public override bool CanMarshal(Type type) => typeof(MyWrapper).IsAssignableFrom(type);

    public override MyWrapper? JSToNet(SpawnJSObjectReference? value)
        => value == null ? null : new MyWrapper(value);

    public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, MyWrapper? value)
        => jsParent.Set(jsKey, value?.JSRef);

    public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, MyWrapper? value)
        => jsParent.Set(jsKey, value?.JSRef);
}
```

If `MyWrapper : SpawnJSObject` with a public `.ctor(SpawnJSObjectReference)`, `SpawnJSObjectMarshaller` already handles it. You only write a marshaller when the default is wrong.

## Trimming

Interop return types carry `[DynamicallyAccessedMembers(PublicConstructors)]` so wrapper constructors survive. Built-in wrappers are also listed in the embedded `ILLink.Descriptors.xml`. A consumer POCO or custom wrapper in a trimmed app must preserve its own constructor and members (use them, `[DynamicDependency]`, or a descriptor). `PocoMarshaller` uses reflection; the same contract as any reflection mapper.

## Tests

Add a case in `SpawnDev.SpawnJS.Demo/UnitTests/` and list the class in `MarshallerTests.Run()`. A new class that is not in that sequence does not run. Round-trip the value; a write-only assert is how nullable properties once wrote fine and read back null.
