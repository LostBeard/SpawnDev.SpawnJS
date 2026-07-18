# Argument passing

## The pattern

The call and construct methods on `SpawnJSObjectReference`, `SpawnJSRuntime`, and the low-level `JSObjectExtensions` are provided as **explicit fixed-arity overloads** plus an **`Apply` form** that takes a pre-built array:

```csharp
// convenience overloads - one per argument count
obj.Call<int>("add", 1, 2);
obj.CallVoid("log", "hello", 42);
runtime.New("Uint8Array", 100);

// Apply form - when you already have an array
object?[] args = { 1, 2 };
obj.CallApply<int>("add", args);
obj.CallVoidApply("log", args);
```

- `SpawnJSObjectReference` / `SpawnJSRuntime`: `Call`, `CallVoid`, `CallAsync`, `CallVoidAsync`, `New`, `New<T>` each have overloads for `0..20` arguments, plus a `...Apply(string identifier, object?[] args)` method.
- `JSObjectExtensions` (low level, in `Extensions/JSObjectExtensions.Invoke.cs`): the `Invoke*` / `InvokeProperty*` families each have overloads for `0..10` arguments, plus a `...Apply(object?[] args)` method. There are more families here (one per return type: `Void`, `JSObject`, `Object`, `String`, `Int32`, `Double`, `Boolean`, the nullables, `ByteArray`, `Json<T>`), so the arity cap is 10 rather than 20 to keep the count sane.

This is deliberate. It is not repetition to be "simplified" into a single `params object?[]` method - doing so reintroduces a silent bug.

## The `params` footgun it avoids

With a `params object?[] args` method, the compiler first checks whether the single argument you passed is *itself* assignable to `object?[]`. If it is, the compiler binds it **as** the argument array rather than wrapping it:

```csharp
// Hypothetical single `params object?[] args` overload:
call("fn", "hello");        // args => [ "hello" ]      wrapped, as expected
call("fn", someObjectArray); // args => someObjectArray  SPREAD - not [ someObjectArray ]
call("fn", someStringArray); // args => spread too - string?[] is covariant to object?[]
```

So passing a single array argument silently spreads into N arguments, and the JS side receives the wrong argument shape - **no compiler warning, no exception**. It is even inconsistent by element type: a `string[]` spreads (reference-type array covariance), but an `int[]` wraps (value-type arrays are not covariant to `object?[]`). Identical-looking call sites, opposite behavior.

## How the overloads fix it

Each fixed-arity overload builds the array explicitly:

```csharp
public T Call<T>(string identifier, object? arg1)
    => CallApply<T>(identifier, new object?[] { arg1 });
```

So "one array argument" always means exactly one argument - the array is wrapped, never spread. The generic argument type is `object?`, and an array is a valid `object?`, so it lands in `arg1` and gets wrapped.

## When you genuinely have an array

Use the non-`params` `Apply` methods. They take the array as a single explicit parameter, so there is no collapse:

```csharp
object?[] args = new object?[] { myArray }; // one argument that happens to be an array
obj.CallApply<T>("fn", args);
obj.CallVoidApply("fn", args);
obj.NewApply("Thing", args);
obj.InvokeVoidApply(args);                  // low-level extension form
```

## Rule of thumb

- Passing a fixed, small number of arguments? Use the plain overload (`Call`, `CallVoid`, `New`, `Invoke*`, ...).
- Building arguments dynamically into an array? Use the `Apply` form (`CallApply`, `NewApply`, `Invoke*Apply`, ...).
- Never collapse the overloads into a single `params` method.
