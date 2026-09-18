# Argument passing

## The pattern

`Call`, `CallVoid`, `CallAsync`, `CallVoidAsync`, `New`, and `New<T>` on `SpawnJSObjectReference` / `SpawnJSRuntime` are **explicit fixed-arity overloads** (0 through 10 arguments) plus an **`Apply` form** that takes a pre-built `object?[]`.

Generic type arguments are **argument types first, return type last**:

```csharp
obj.Call<int, int, int>("add", 1, 2);                 // T1, T2, TReturn
obj.CallVoid<string, int>("log", "hello", 42);        // args only
runtime.New<int, Uint8Array>("Uint8Array", 100);
runtime.New("Uint8Array", 100);                       // returns SpawnJSObjectReference

object?[] args = { 1, 2 };
obj.CallApply<int>("add", args);
```

This is deliberate. Collapsing to `params object?[]` reintroduces a silent bug.

## The `params` footgun

With `params object?[] args`, the compiler first checks whether the single argument is *itself* assignable to `object?[]`. If it is, that argument **is** the array rather than wrapped as one element:

```csharp
// Hypothetical single params object?[] overload:
call("fn", "hello");         // args => [ "hello" ]
call("fn", someObjectArray); // args => someObjectArray  SPREAD
call("fn", someStringArray); // spread too - string[] is covariant to object?[]
```

A `string[]` spreads (reference-type array covariance). An `int[]` wraps (value-type arrays are not covariant to `object?[]`). Same-looking call sites, opposite behavior, no warning.

## How the overloads fix it

Each fixed-arity overload names the argument as `object?` (or a typed `T1`):

```csharp
public T Call<T1, T>(string identifier, T1 arg1)
    => JS.InteropCall<double, string, T1, T>("propertyCall", Id, identifier, arg1);
```

One array argument is one argument. The `Apply` methods take `object?[]` as a **named** parameter, so there is no collapse:

```csharp
object?[] args = new object?[] { myArray }; // one argument that happens to be an array
obj.CallApply<T>("fn", args);
obj.NewApply("Thing", args);
```

## Rule of thumb

- Fixed, small argument list: `Call`, `CallVoid`, `New`, `CallAsync`.
- Arguments already in an array: `CallApply`, `NewApply`, `CallApplyVoid`, `CallApplyAsync`.
- Never collapse the overloads into `params`.
