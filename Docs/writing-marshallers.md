# Marshallers

A **marshaller** converts a single value between .NET and JavaScript by reading or writing it into a live `JSObject` (the transport). Marshallers are where all of SpawnJS's type-awareness lives - the registry of them is effectively the product.

## The contract

All marshallers derive from `JSMarshaller` (namespace `SpawnDev.SpawnJS.Marshallers`):

```csharp
public abstract class JSMarshaller
{
    // Can this marshaller handle the given .NET type? (type may be null when the value is null)
    public abstract bool CanMarshal(Type? typeToConvert);

    // Optional: return the actual marshaller instance to use (default: this)
    public virtual JSMarshaller GetMarshaller(Type? typeToConvert) => this;

    // Read: given a JS parent object and a key, produce the .NET value.
    // May return null (JS value was null/undefined, or the type's default is null).
    public abstract object? JSToNet(Type typeToConvert, JSObject jsParent, object jsKey, SpawnJSRuntime runtime);

    // Write: given a JS parent object, a key, and a .NET value, set the value.
    // type and value may be null when the value being marshalled is null.
    public abstract void NetToJS(Type? typeToConvert, JSObject jsParent, object jsKey, object? value, SpawnJSRuntime runtime);
}
```

`jsKey` is a JS property key - a string for named properties, or an int for array indices (the transport is often a JS array, so index `0` is the common read).

For a marshaller that handles exactly one type, derive from the generic base, which implements `CanMarshal` for you:

```csharp
public abstract class JSMarshaller<TType> : JSMarshaller
{
    public override bool CanMarshal(Type? typeToConvert) => typeof(TType) == typeToConvert;
}
```

## How a type resolves to a marshaller

`SpawnJSRuntime` holds an ordered `IList<JSMarshaller> Marshallers`. To marshal a value, the runtime scans that list **in reverse registration order** and picks the first marshaller whose `CanMarshal` returns true. Results are cached per type (and a dedicated slot handles the null-type case). Reverse order means **later registrations win** - the built-ins register first, so anything you add later takes precedence and can override a built-in.

The built-ins, in registration order (so `Default` is the last-resort fallback and `JSToNetInvoker` is checked first):

```
Default, Object, IEnumerable, ByteArray, String, Boolean, Number,
SpawnJSObjectReference, SpawnJSObject, JSObject, Struct, JSToNetInvoker
```

`Object` and `Struct` marshal by walking public JSON-serializable members and recursing through the runtime for each member's value - which is how nested objects cross without any JSON.

## Two design laws

1. **Parity by default, performance by opt-in.** The default graph mirrors what `JSON.stringify` would have produced, so existing code behaves identically. `List<long>` becomes an ordinary JS number array, **not** a `BigInt64Array`. TypedArrays and `BigInt` are opt-in via explicit wrapper types - never auto-selected because an element "happens to be" a `long` or a `float`. The caller chooses the fast lane by choosing the type.

2. **Bring your own.** Because the transport is a live `JSObject`, a custom marshaller can take whatever route is optimal for its type - including its own `JSImport`/`JSExport`/memory-view path - with no JSON middleman foreclosing an optimization.

## Registering a custom marshaller

Add it to the runtime's `Marshallers` list. Register **after** construction so it wins over the built-ins:

```csharp
var runtime = new SpawnJSRuntime();
runtime.Marshallers.Add(new MyThingMarshaller());   // last registered => first matched
```

Use the built-in marshallers (in `SpawnDev.SpawnJS/Marshallers/`) as reference implementations - `NumberMarshaller` shows type-cached reflection, `ObjectMarshaller` shows member-walking and recursion through the runtime, and `ByteArrayMarshaller` shows a direct typed path.
