# Writing marshallers

A `JSMarshaller` converts one .NET type to a JS value and back. The registry is the product: any type is marshalled by the first registered marshaller whose `CanMarshal` returns true, scanned in **reverse registration order** (later registrations win). Built-ins register first; yours register last. The resolved marshaller is cached per type.

## Two design laws

1. **Parity by default, performance by opt-in.** The default graph mirrors what `JSON.stringify` would produce. `List<long>` becomes a JS number array, not a `BigInt64Array`. Typed arrays and `BigInt` are wrapper types you ask for (`Uint8Array`, `BigInt`, `HeapView`). `byte[]` is the built-in exception: it copies to/from a `Uint8Array`.
2. **Any type, users bring their own.** A marshaller composes the tape's primitives, and can name a JS function (`JSTape.WriteRevived`) for a value that has no plain JS form.

## Contract

```csharp
public abstract class JSMarshaller
{
    public virtual ReturnType ReturnType { get; }
    public virtual JSSchema Schema { get; }                // default: the ReturnType's built-in schema
    public abstract bool CanMarshal(Type type);
    public virtual JSMarshaller<T> GetMarshaller<T>();
}

public abstract class JSMarshaller<TType> : JSMarshaller
{
    public abstract void Write(JSTape tape, TType value);  // .NET to JS: append to the call's tape
    public virtual TType Read(ref JSTapeReader reader);    // JS to .NET: read what JS wrote by Schema
    public virtual TType JSToNet(/* one of: bool, bool?, int, int?, double, double?, string, SpawnJSObjectReference */);
}
```

Nothing crosses in `Write` or `Read`: the whole call - every argument and everything nested in them - crosses once, and its result comes back in the same crossing.

### Write

Append exactly one value with the tape's primitives:

| `JSTape` | JS receives |
|---|---|
| `WriteNull` / `WriteUndefined` | `null` / `undefined` |
| `WriteNumber` / `WriteBoolean` / `WriteString` | number / boolean / string (UTF-16 code units exact) |
| `WriteRef(id)` | the held value (`SpawnJSObjectReference.Id`) |
| `WriteCallback` | the function that invokes a `Callback` |
| `WriteObject(JSShape)` then one value per name (`WriteAbsent` = not assigned) | a plain object |
| `WriteArray(count)` then values; `BeginArray` / `EndArray` when the count comes last | an Array |
| `WriteNumbers(span)` | a plain Array of numbers, in one copy |
| `WriteRecord(count)`, then `WriteKey` + value pairs | an object whose keys are data |
| `WriteHeapView` / `WriteArrayCopy` | a TypedArray / DataView over .NET memory, or a copy |
| `WriteRevived(name)` then one value | what the `SpawnJSInterop` function `name(key, value, true)` returns |
| `WriteValue(object?)` | the value, through its own runtime type's marshaller |

A value declared `object`, an interface or a base class is written by what it **is** (`WriteValue`). Create a `JSShape` once (per marshaller - marshallers are per runtime), not per value.

### Read

`Schema` says what JS writes back; `Read` reads it. The default reads the `ReturnType` form and calls the matching `JSToNet`, so a scalar or live-object marshaller only overrides `JSToNet`:

| `ReturnType` / built-in schema | JS writes | `JSToNet` overload |
|---|---|---|
| `Void` | nothing | `JSToNet()` |
| `Double` / `DoubleNullable` | number (null/undefined = 0 / null) | `double` / `double?` |
| `Boolean` / `BooleanNullable` | truthiness / null | `bool` / `bool?` |
| `Int32` / `Int32Nullable` | ToInt32 (null/undefined = 0 / null) | `int` / `int?` |
| `String` | string (a non-string converted) or null | `string` |
| `SpawnJSObjectReference` | held id, or null | `SpawnJSObjectReference` |
| `SpawnJSObjectReferenceNonNullable` | held id, even for null/undefined | `SpawnJSObjectReference` |
| `Json` | `JSON.stringify` | `string` |

Subclass the matching `JSMarshallerFrom*` helper (`JSMarshallerFromInt32<T>`, `JSMarshallerFromDouble<T>`, `JSMarshallerFromString<T>`, `JSMarshallerFromSpawnJSObjectReference<T>`, `JSMarshallerFromJson<T>`, and nullable variants).

A marshaller that reads a whole value overrides `Schema` with a composite (`JSSchema.Object(names)` + `Members`, `Array`, `Numbers`, `Record`, `Tuple`, `Bytes`) and `Read` with the matching `JSTapeReader` calls (`ReadObjectStart`, `ReadCount`, `ReadKey`, `ReadRaw<T>`, and the scalar reads). Build the schema once; for a type that can contain itself, create the schema before filling `Members` so the inner reference resolves to the same object.

`GetMarshaller<T>()` lets a factory specialize. `ArrayMarshaller<object>` is registered once; when asked for `int[]` it returns an `ArrayMarshaller<int>`. Return that specialization - the runtime caches whatever you return.

## Read vs write

- **Write (.NET to JS)** selects on the **value's runtime type**.
- **Read (JS to .NET)** selects on the **declared** `T`.

`Nullable<int>` boxes as `int` on write, so write and read can resolve different marshallers if you forget to handle both. Built-in numeric marshallers register both `int` and `int?` (and an `INumber` / `INumberNullable` catch-all). `PocoMarshaller` unwraps `Nullable<TStruct>` itself. **Assert the round trip**, not only the write.

A JS class name cannot identify a derived type: `Object.prototype.toString.call(new TypeError())` is `"[object Error]"`. Walk the **prototype chain** (`ConstructorNames()`, most derived first). `UnionMarshaller` does this to pick an arm.

## Built-in registration order

Last registered wins. Approximate map (see the `SpawnJSRuntime` constructor for the exact list):

| Marshaller | .NET | JS |
|---|---|---|
| `PocoMarshaller` (first = lowest priority) | class/struct POCO | plain object by shape; read by Object schema; honors `[JsonPropertyName]` / `[JsonIgnore]` / `[JsonInclude]` |
| `IEnumerableMarshaller` | `IEnumerable<T>` | Array |
| `VoidTypeMarshaller` | `VoidType` | nothing |
| `ObjectMarshaller` | `object` | by the value's runtime type; read as a held reference |
| `StringMarshaller` | `string` | string |
| `INumberMarshaller` / nullable | other `INumber<T>` | Number |
| `DoubleMarshaller` / `Int32Marshaller` / `BooleanMarshaller` (+ nullable) | those primitives | Number / Boolean |
| `ITupleMarshallerFactory` | `Tuple` / `ValueTuple` | Array |
| `SpawnJSObjectReferenceMarshaller` | `SpawnJSObjectReference` | any |
| `ArrayMarshaller` / `ListMarshaller` | `T[]` / `List<T>` | Array; numbers in one copy each way |
| `DictionaryMarshaller` | `IDictionary<K, V>` | plain object (record) |
| `HeapViewDescriptorMarshaller` | `HeapViewDescriptor` | TypedArray / DataView |
| `CallbackMarshaller` | `Callback` | Function |
| `ByteArrayMarshaller` | `byte[]` | Uint8Array (copy); reads any ArrayBuffer or view |
| `TaskMarshaller` | `Task` / `Task<T>` | Promise |
| `BigIntegerMarshaller` | `BigInteger` | BigInt |
| `UnionMarshallerFactory` | `Union<...>` | the held value; read picks the arm by prototype / typeof |
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

Add **before** the type is first marshalled. The winner is cached; a later add does not evict it.

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
    public override void Write(JSTape tape, Celsius value) => tape.WriteNumber(value.Value);
    public override Celsius JSToNet(double value) => new Celsius(value);
}
```

A live JS object uses `JSMarshallerFromSpawnJSObjectReference<T>`:

```csharp
public sealed class MyWrapperMarshaller : JSMarshallerFromSpawnJSObjectReference<MyWrapper?>
{
    public override bool CanMarshal(Type type) => typeof(MyWrapper).IsAssignableFrom(type);

    public override void Write(JSTape tape, MyWrapper? value)
    {
        if (value?.JSRef == null) tape.WriteNull();
        else tape.WriteRef(value.JSRef.Id);
    }

    public override MyWrapper? JSToNet(SpawnJSObjectReference? value)
        => value == null ? null : new MyWrapper(value);
}
```

If `MyWrapper : SpawnJSObject` with a public `.ctor(SpawnJSObjectReference)`, `SpawnJSObjectMarshaller` already handles it. You only write a marshaller when the default is wrong.

## Trimming

Interop return types carry `[DynamicallyAccessedMembers(PublicConstructors)]` so wrapper constructors survive. Built-in wrappers are also listed in the embedded `ILLink.Descriptors.xml`. A consumer POCO or custom wrapper in a trimmed app must preserve its own constructor and members (use them, `[DynamicDependency]`, or a descriptor). `PocoMarshaller` builds its plans by reflection: the same contract as any reflection mapper.

## Tests

Add a case in `SpawnDev.SpawnJS.Demo/UnitTests/` and list the class in `MarshallerTests.Run()`. A new class that is not in that sequence does not run. Round-trip the value; a write-only assert is how nullable properties once wrote fine and read back null. Anything touching per-runtime state also belongs in `TwinTests` (`SpawnJS.TestRunner --twin`).
