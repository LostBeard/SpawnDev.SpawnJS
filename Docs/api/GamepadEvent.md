# GamepadEvent

**Namespace:** `SpawnDev.SpawnJS.JSObjects`  
**Inherits:** `Event`  
**Source:** `JSObjects/GamepadEvent.cs`  
**MDN Reference:** [GamepadEvent on MDN](https://developer.mozilla.org/en-US/docs/Web/API/GamepadEvent)

> The GamepadEvent interface of the Gamepad API contains references to gamepads connected to the system, which is what the gamepad events gamepadconnected and gamepaddisconnected are fired in response to. https://developer.mozilla.org/en-US/docs/Web/API/GamepadEvent

## Constructors

| Signature | Description |
|---|---|
| `GamepadEvent(SpawnJSObjectReference _ref)` | Deserialization constructor |

## Properties

| Property | Type | Access | Description |
|---|---|---|---|
| `Gamepad` | `Gamepad` | get | Returns a Gamepad object, providing access to the associated gamepad data for the event fired. |

## Examples

> **SpawnDev.SpawnJS Mapping Note:** The JavaScript examples below show the standard Web API usage.
> In SpawnDev.SpawnJS, the same API is available with C# conventions:
> - Properties and methods use **PascalCase** (e.g., `readyState` becomes `ReadyState`)
> - Events use **ActionEvent** with `+=`/`-=` (e.g., `addEventListener("click", fn)` becomes `OnClick += handler`)
> - Async methods return `Task<T>` instead of `Promise<T>`
> - Objects should be disposed with `using` statements
> - Access via `JS.Get<GamepadEvent>(...)` or constructor `new GamepadEvent(...)`

```js
window.addEventListener("gamepadconnected", (e) => {
  console.log(
    "Gamepad connected at index %d: %s. %d buttons, %d axes.",
    e.gamepad.index,
    e.gamepad.id,
    e.gamepad.buttons.length,
    e.gamepad.axes.length,
  );
});
```

```js
window.addEventListener("gamepaddisconnected", (e) => {
  console.log(
    "Gamepad disconnected from index %d: %s",
    e.gamepad.index,
    e.gamepad.id,
  );
});
```

*[See full example on MDN](https://developer.mozilla.org/en-US/docs/Web/API/GamepadEvent)*

