# Callback

**Namespace:** `SpawnDev.SpawnJS`  
**Inheritance:** Callback (abstract)  
**Concrete types:** `ActionCallback` and `ActionCallback<T1>` ... `ActionCallback<T1..T10>`; `FuncCallback<TResult>` and `FuncCallback<T1, TResult>` ... `FuncCallback<T1..T10, TResult>`  
**Source:** `Callback.cs`, `Callback.Create.cs`, `ActionCallback.cs`, `FuncCallback.cs`

> Callback makes a .NET method callable from JavaScript. JS holds a numeric callback id; invoking the function calls into .NET via the instance registered at startup (`handleCallback`). Arguments arrive as a held JS array and are read through the marshaller graph. Callbacks are `IDisposable`. Slot and callback tables are strong refs - dispose them.

Passing an `Action` / `Func` as an interop argument also works: `DelegateMarshaller` wraps it in a `Callback` and **reuses one JS function per delegate instance**.

## Static factory methods

`Create(...)` is reusable. `CreateOne(...)` sets `Once` and disposes after the first invocation (JS also drops `once` callbacks). Optional `CallbackGroup` on `Create` disposes the group together.

Action: 0 to 10 parameters. Func: 0 to 10 parameters plus `TResult`.

```csharp
using var cb = Callback.Create(() => Console.WriteLine("Called!"));
JS.CallVoid<Callback, int>("setTimeout", cb, 1000);

using var once = Callback.CreateOne<Event>(e => Console.WriteLine(e.Type));
JS.CallVoid<string, Callback>("addEventListener", "load", once);

JS.Set("_onTick", () => { /* DelegateMarshaller */ });
```

## Instance

| Member | Description |
|---|---|
| `Id` | Numeric id (never a `DotNetObjectReference`). |
| `Once` | True for `CreateOne` / `once: true` constructors. |
| `Sent` | True once the callback has been written to JS. Dispose notifies JS only if `Sent` and JS has not already dropped a `once` callback. |
| `CalledCount` / `HasBeenCalled` | Invocation count. |
| `RefCount` | Used by event `CallbackRef`. Setting it to 0 or less disposes. |
| `IsDisposed` | |
| `OnDisposed` | Fired from `Dispose()`. |
| `Dispose()` | Unregisters, notifies JS if needed. Does not decrement `RefCount`; it disposes now. |

`CallbackCount` is the number of live instances.

## CallbackGroup

```csharp
using var group = new CallbackGroup();
var onOpen = Callback.Create(() => { }, group);
var onClose = Callback.Create(() => { }, group);
group.Dispose(); // disposes both
```

## Notes

- **Always dispose** reusable callbacks. A held JS function plus a .NET dictionary entry will not go away on their own.
- `CreateOne` is for one-shot handlers (`setTimeout`, Promise `then`, a one-time event).
- Wrappers (`HTMLButtonElement.OnClick += ...`) typically go through `ActionEvent` / `CallbackRef` and take a `RefCount` so one method subscribed to several events shares one JS function.
- An **unhandled exception** from a callback can **exit the WASM runtime**. Catch inside the handler if the page must survive.
